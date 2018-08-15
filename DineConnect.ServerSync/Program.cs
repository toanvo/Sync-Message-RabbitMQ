using NLog;
using DineConnect.Common;
using System;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using Topshelf;
using EasyNetQ;

namespace DineConnect.ServerSync
{
    class Program
    {
        private static Logger logger = LogManager.GetLogger("dbLoggerInfo");

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            
            try
            {
                var busFactory = new BusFactory(RabbitMqConfiguration.Instance.GetConnectionString());

                HostFactory.Run(x =>
                {
                    x.Service<QueueWatcher>(sc =>
                    {
                        sc.ConstructUsing(() =>
                        new ContainerRegistration(busFactory).GetServerQueueWatcher());

                        sc.WhenStarted(tc => tc.Start());
                        sc.WhenStopped(tc => tc.Stop());
                    });

                    x.RunAsLocalSystem();

                    x.SetDescription("Processing File Queue Server service");
                    x.SetDisplayName("Dine Connect Server Sync");
                    x.SetServiceName("DineConnect.ServerSync");
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

            logger.Error("Error");
        }
    }
}
