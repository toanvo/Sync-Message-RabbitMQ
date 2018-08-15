using NLog;
using DineConnect.Common;
using System;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using Topshelf;
using EasyNetQ;

namespace DineConnect.ClientSync
{
    class Program
    {
        private static Logger logger = LogManager.GetLogger("dbLoggerInfo");
        
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            var clientName = RabbitMqConfiguration.Instance.ClientServiceName();
            try
            {
                var busFactory = new BusFactory(RabbitMqConfiguration.Instance.GetConnectionString());

                HostFactory.Run(x =>
                {
                    x.Service<QueueWatcher>(sc =>
                    {
                        sc.ConstructUsing(() => new ContainerRegistration(busFactory).GetClientQueueWatcher());
                        sc.WhenStarted(tc => tc.Start());
                        sc.WhenStopped(tc => tc.Stop());
                    });

                    x.RunAsLocalSystem();
                    x.StartAutomatically();
                    x.SetDescription("Processing Client Queue service");
                    x.SetDisplayName(clientName);
                    x.SetServiceName(clientName);
                });
            }
            catch (Exception ex)
            {
                logger.Error<Exception>("Error", ex);
            }
            finally
            {
                logger.Info("Service closing");
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ProcessUnhandledException((Exception)e.ExceptionObject);
        }

        private static void ProcessUnhandledException(Exception ex)
        {
            if (ex is TargetInvocationException)
            {
                ProcessUnhandledException(ex.InnerException);
                return;
            }

            logger.Error<Exception>("Error when initiate application", ex);
        }
    }
}
