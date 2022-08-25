using System;
using System.IO;
using System.Linq;
using Autofac;
using CoreCms.Net.Auth;
using CoreCms.Net.Configuration;
using CoreCms.Net.Core.AutoFac;
using CoreCms.Net.Core.Config;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.Mapping;
using CoreCms.Net.Middlewares;
using CoreCms.Net.Services.Mediator;
using CoreCms.Net.Swagger;
using Essensoft.Paylink.Alipay;
using Essensoft.Paylink.WeChatPay;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.WxOpen;
using SqlSugar;

namespace CoreCms.Net.Web.Admin
{
    /// <summary>
    ///     ��������
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// </summary>
        public IWebHostEnvironment Env { get; }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
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

            //MediatR��ֻ��Ҫע��һ��,ͬ��Ŀ������¾Ͳ���Ҫע������
            services.AddMediatR(typeof(OrderPayedCommand).Assembly);

            //ʹ�� SignalR
            services.AddSignalR();

            // ����Payment ����ע��(֧����֧��/΢��֧��)
            services.AddAlipay();
            services.AddWeChatPay();

            // �� appsettings.json �� ����ѡ��
            services.Configure<WeChatPayOptions>(Configuration.GetSection("WeChatPay"));
            services.Configure<AlipayOptions>(Configuration.GetSection("Alipay"));

            //Swagger�ӿ��ĵ�ע��
            services.AddAdminSwaggerSetup();

            //���������ƴ�ӡ��
            services.AddYiLianYunSetup();

            //jwt��Ȩ֧��ע��
            services.AddAuthorizationSetupForAdmin();
            //������ע��
            services.AddHttpContextSetup();

            //���������м���AutoFac�������滻����
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //΢��ע��
            services.AddSenparcWeixinServices(Configuration);

            //ע��mvc��ע��razor������ͼ
            services.AddMvc(options =>
                {
                    //ʵ����֤
                    options.Filters.Add<RequiredErrorForAdmin>();
                    //�쳣����
                    options.Filters.Add<GlobalExceptionsFilterForAdmin>();
                    //Swagger�޳�����Ҫ����apiչʾ���б�
                    options.Conventions.Add(new ApiExplorerIgnores());

                    options.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson(p =>
                {
                    //���ݸ�ʽ����ĸСд ��ʹ���շ�
                    p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //��ʹ���շ���ʽ��key
                    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //����ѭ������
                    p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //����ʱ���ʽ
                    p.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

        }

        /// <summary>
        ///     Autofac��������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());

            //��ȡ���п��������Ͳ�ʹ������ע��
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            // signalr
            app.UseSignalRSendMildd();


            app.UseSwagger().UseSwaggerUI(c =>
            {
                //���ݰ汾���Ƶ��� ����չʾ
                typeof(CustomApiVersion.ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(
                    version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Doc {version}");
                    });
                c.RoutePrefix = "doc";
            });


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
            // ��תhttps
            //app.UseHttpsRedirection();
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ʹ��cookie
            app.UseCookiePolicy();
            // ���ش�����
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
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

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
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