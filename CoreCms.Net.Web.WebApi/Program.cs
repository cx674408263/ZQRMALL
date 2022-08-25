using System;
using Autofac.Extensions.DependencyInjection;
using CoreCms.Net.Loging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using LogLevel = NLog.LogLevel;

namespace CoreCms.Net.Web.WebApi
{
    /// <summary>
    /// ������
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                //ȷ��NLog.config�������ַ�����appsettings.json��ͬ��
                NLogUtil.EnsureNlogConfig("nLog.config");
                //������Ŀ����ʱ��Ҫ��������
                NLogUtil.WriteAll(LogLevel.Trace, LogType.Web, "�ӿ�����", "�ӿ������ɹ�");

                host.Run();
            }
            catch (Exception ex)
            {
                //ʹ��nlogд��������־�ļ�����һ���ݿ�û����/���ӳɹ���
                NLogUtil.WriteFileLog(LogLevel.Error, LogType.ApiRequest, "�ӿ�����", "��ʼ�������쳣", ex);
                throw;
            }
        }

        /// <summary>
        ///     ��������֧��
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־�������
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //������С����־����
                })
                .UseNLog() //NLog: Setup NLog for Dependency injection
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.AllowSynchronousIO = true; //����ͬ�� IO
                        })
                        .UseStartup<Startup>();
                });
        }
    }
}