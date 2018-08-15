using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DineConnect.Common
{
    public class ServerMessageProcessingHandler : IProcessingHandler<ProcessingMessage>
    {
        protected static Logger logger = LogManager.GetLogger("dbLoggerInfo");

        private readonly IConfiguration configuration;

        public ServerMessageProcessingHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }   
        
        public async Task ProcessingMessageAsync(ProcessingMessage processingObject, params string[] topicMessages)
        {
            logger.Info("Processing Message at server side.");
            bool isSucceed = true;
            try
            {
                await ArchiveFileMessage(processingObject);
                // DO something to handle file.
            }
            catch (Exception ex)
            {
                logger.Error<Exception>($"Exception happened when processing message. {processingObject.Id}. Filename: {processingObject.Name}. ", ex);
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {                    
                    MoveFileToProcessedFolder(processingObject);
                }
            }
        }

        private void MoveFileToProcessedFolder(ProcessingMessage processingObject)
        {
            var processingFile = GetSafeFileName(configuration.GetProcessingFolder(), processingObject);
            var processedFile = GetSafeFileName(configuration.GetProcessedFolder(), processingObject);             

            File.Move(processingFile, processedFile);
        }

        private async Task ArchiveFileMessage(ProcessingMessage processingObject)
        {
            var processingFile = GetSafeFileName(configuration.GetProcessingFolder(), processingObject);            
            using (var stream = new StreamWriter(processingFile))
            {
                await stream.WriteAsync(processingObject.ContentFile);
            }
        }        

        private string GetSafeFileName(string path, ProcessingMessage processingObject)
        {
            var safePath = path;
            if (!path.EndsWith("\\"))
            {
                safePath = $"{safePath}\\{processingObject.Id}_{processingObject.Name}";
            }
            else
            {
                safePath = $"{safePath}{processingObject.Id}_{processingObject.Name}";
            }

            return safePath;
        }
    }
}
