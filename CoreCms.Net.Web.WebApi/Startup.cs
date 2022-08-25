using Autofac;
using AutoMapper;
using CoreCms.Net.Auth;
using CoreCms.Net.Configuration;
using CoreCms.Net.Core.AutoFac;
using CoreCms.Net.Core.Config;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.Mapping;
using CoreCms.Net.Middlewares;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.Sms;
using CoreCms.Net.Services.Mediator;
using CoreCms.Net.Swagger;
using CoreCms.Net.Task;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using InitQ;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Qc.YilianyunSdk;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.WxOpen;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreCms.Net.RedisMQ.Subscribe;
using CoreCms.Net.Utility.Extensions;
using Essensoft.Paylink.Alipay;
using Essensoft.Paylink.WeChatPay;

namespace CoreCms.Net.Web.WebApi
{
    /// <summary>
    /// ��������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// web����
        /// </summary>
        public IWebHostEnvironment Env { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //��ӱ���·����ȡ֧��
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
            services.AddSingleton(new LogLockHelper(Env.ContentRootPath));

            //Memory����
            services.AddMemoryCacheSetup();
            //Redis����
            services.AddRedisCacheSetup();

            //������ݿ�����SqlSugarע��֧��
            services.AddSqlSugarSetup();
            //���ÿ���CORS��
            services.AddCorsSetup();


            //���session֧��(session������cache���д洢)
            services.AddSession();
            // AutoMapper֧��
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            //MediatR
            services.AddMediatR(typeof(OrderPayedCommand).Assembly);

            //ʹ�� SignalR
            services.AddSignalR();

            //Redis��Ϣ����
            services.AddRedisMessageQueueSetup();

            // ����Payment ����ע��(֧����֧��/΢��֧��)
            services.AddAlipay();
            services.AddWeChatPay();

            // �� appsettings.json �� ����ѡ��
            services.Configure<WeChatPayOptions>(Configuration.GetSection("WeChatPay"));
            services.Configure<AlipayOptions>(Configuration.GetSection("Alipay"));

            //Swagger�ӿ��ĵ�ע��
            services.AddClientSwaggerSetup();

            //���������ƴ�ӡ��
            services.AddYiLianYunSetup();

            //ע��Hangfire��ʱ����
            services.AddHangFireSetup();


            //��Ȩ֧��ע��
            services.AddAuthorizationSetupForClient();
            //������ע��
            services.AddHttpContextSetup();
            //΢��ע��
            services.AddSenparcWeixinServices(Configuration);

            //���������м���AutoFac�������滻����
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //ע��mvc��ע��razor������ͼ
            services.AddMvc(options =>
                {
                    //ʵ����֤
                    options.Filters.Add<RequiredErrorForClent>();
                    //�쳣����
                    options.Filters.Add<GlobalExceptionsFilterForClent>();
                    //Swagger�޳�����Ҫ����apiչʾ���б�
                    options.Conventions.Add(new ApiExplorerIgnores());
                })
                .AddNewtonsoftJson(p =>
                {
                    //���ݸ�ʽ����ĸСд ��ʹ���շ�
                    p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //��ʹ���շ���ʽ��key
                    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //����ѭ������
                    p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //����ʱ���ʽ������ʹ��yyyy/MM/dd��ʽ����Ϊiosϵͳ��֧��2018-03-29��ʽ��ʱ�䣬ֻʶ��2018/03/09���ָ�ʽ����
                    p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
                });

           

        }

        /// <summary>
        ///     Autofac��������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��ȡ���п��������Ͳ�ʹ������ע��
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            builder.RegisterModule(new AutofacModuleRegister());

        }

        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="senparcSetting"></param>
        /// <param name="senparcWeixinSetting"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            // ��¼�����뷵������ (ע�⿪��Ȩ�ޣ���Ȼ�����޷�д��)
            app.UseReuestResponseLog();
            // �û����ʼ�¼(����ŵ���㣬��Ȼ��������쳣���ᱨ����Ϊ���ܷ�����)(ע�⿪��Ȩ�ޣ���Ȼ�����޷�д��)
            app.UseRecordAccessLogsMildd();
            // ��¼ip���� (ע�⿪��Ȩ�ޣ���Ȼ�����޷�д��)
            app.UseIpLogMildd();


            //ǿ����ʾ����
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

            app.UseSwagger().UseSwaggerUI(c =>
            {
                //���ݰ汾���Ƶ��� ����չʾ
                typeof(CustomApiVersion.ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(
                    version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Doc {version}");
                    });
                //����Ĭ����ת��swagger-ui
                c.RoutePrefix = "doc";
                //c.RoutePrefix = string.Empty;
            });


            #region Hangfire��ʱ����

            var queues = new string[] { GlobalEnumVars.HangFireQueuesConfig.@default.ToString(), GlobalEnumVars.HangFireQueuesConfig.apis.ToString(), GlobalEnumVars.HangFireQueuesConfig.web.ToString(), GlobalEnumVars.HangFireQueuesConfig.recurring.ToString() };
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerTimeout = TimeSpan.FromMinutes(4),
                SchedulePollingInterval = TimeSpan.FromSeconds(15),//�뼶������Ҫ���ö̵㣬һ�������������Ĭ��ʱ�䣬Ĭ��15��
                ShutdownTimeout = TimeSpan.FromMinutes(30),//��ʱʱ��
                Queues = queues,//����
                WorkerCount = Math.Max(Environment.ProcessorCount, 20)//�����߳�������ǰ���������̣߳�Ĭ��20
            });

            //��Ȩ
            var filter = new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    SslRedirect = false,
                    // Require secure connection for dashboard
                    RequireSsl = false,
                    // Case sensitive login checking
                    LoginCaseSensitive = false,
                    // Users
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = AppSettingsConstVars.HangFireLogin,
                            PasswordClear = AppSettingsConstVars.HangFirePassWord
                        }
                    }
                });
            var options = new DashboardOptions
            {
                AppPath = "/",//����ʱ��ת�ĵ�ַ
                DisplayStorageConnectionString = false,//�Ƿ���ʾ���ݿ�������Ϣ
                Authorization = new[]
                {
                    filter
                },
                IsReadOnlyFunc = Context =>
                {
                    return false;//�Ƿ�ֻ�����
                }
            };

            app.UseHangfireDashboard("/job", options); //���Ըı�Dashboard��url
            HangfireDispose.HangfireService();

            #endregion


            #region ʢ��΢��ע��
            // ���� CO2NET ȫ��ע�ᣬ���룡
            var registerService = app.UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
                {
                    #region CO2NET ȫ������
                    #endregion
                }, true)
                //ʹ�� Senparc.Weixin SDK
                .UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
                {
                    #region ΢���������

                    /* ΢�����ÿ�ʼ
                    * 
                    * ���鰴������˳�����ע�ᣬ�����뽫������ڵ�һλ��
                    */
                    #region ע�ṫ�ںŻ�С���򣨰��裩

                    weixinRegister
                        //ע�ṫ�ں�
                        //.RegisterMpAccount(senparcWeixinSetting.Value, "���ں�")

                        //ע�������ںŻ�С����
                        .RegisterWxOpenAccount(senparcWeixinSetting.Value, "С����")

                        //AccessTokenContainer.Register(appId, appSecret, name);//�����ռ䣺Senparc.Weixin.MP.Containers
                    #endregion
                        ;
                    /* ΢�����ý��� */

                    #endregion
                });

            #endregion


            //ʹ�� Session
            app.UseSession();

            if (env.IsDevelopment())
            {
                // �ڿ��������У�ʹ���쳣ҳ�棬�������Ա�¶�����ջ��Ϣ�����Բ�Ҫ��������������
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // CORS����
            app.UseCors(AppSettingsConstVars.CorsPolicyName);

            // Routing
            app.UseRouting();

            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // �ȿ�����֤
            app.UseAuthentication();
            // Ȼ������Ȩ�м��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areas",
                    "{area:exists}/{controller=Default}/{action=Index}/{id?}"
                );

                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Default}/{action=Index}/{id?}");
            });



            //����Ĭ����ʼҳ����default.html��
            //�˴���·���������wwwroot�ļ��е����·��
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();

        }


    }
}