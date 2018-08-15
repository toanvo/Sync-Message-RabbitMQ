using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using NLog;

namespace DineConnect.Common.Handlers
{
    public class ServerFileInfoProcessingHandler : IProcessingHandler<FileInfo>
    {
        protected static Logger logger = LogManager.GetLogger("dbLoggerInfo");

        private readonly IConfiguration configuration;
        private readonly ISendContentFileToQueueHandler sendContentFileToQueueHandler;

        public ServerFileInfoProcessingHandler(IConfiguration configuration, ISendContentFileToQueueHandler sendContentFileToQueueHandler)
        {
            this.configuration = configuration;
            this.sendContentFileToQueueHandler = sendContentFileToQueueHandler;
        }

        public async Task ProcessingMessageAsync(FileInfo configFileInfo, params string[] topicMessages)
        {
            logger.Info($"Processing file {configFileInfo.FullName} at client side.");
            await ProcessFileToDispatch(configFileInfo, topicMessages);
        }

        private async Task ProcessFileToDispatch(FileInfo configFileInfo, string[] topicMessages)
        {
            StreamReader streamReader = null;
            try
            {
                var patternProcessedFiles = RabbitMqConfiguration.Instance.GetPatternFile();
                foreach (var file in GetFiles(configFileInfo.FullName, patternProcessedFiles, SearchOption.TopDirectoryOnly))
                {
                    logger.Info($"Send file info {file} into Queue");
                    using (streamReader = new StreamReader(file))
                    {
                        var content = await streamReader.ReadToEndAsync();
                        streamReader.Close();
                        await sendContentFileToQueueHandler.SendContentFileToQueueAsync(new FileInfo(file), content, topicMessages);
                    }

                    DeleteFile(file);
                }
            }
            catch (Exception ex)
            {
                logger.Error<Exception>("Error encountered attempting to ready data for sending", ex);
            }
            finally
            {
                streamReader?.SafeDispose();
            }
        }

        private static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

        private static string[] GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
        }
    }
}
