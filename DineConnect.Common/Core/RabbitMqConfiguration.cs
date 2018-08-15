using System;
using System.Configuration;

namespace DineConnect.Common
{
    public sealed class RabbitMqConfiguration : IConfiguration
    {
        private static readonly Lazy<RabbitMqConfiguration> lazy =
            new Lazy<RabbitMqConfiguration>(() => new RabbitMqConfiguration());

        public static RabbitMqConfiguration Instance => lazy.Value;

        public string GetConnectionString()
        {
            var rabbitMqsetting = ConfigurationManager.GetSection("RabbitMqSettings") as RabbitMqSettings;
            return $"host={rabbitMqsetting.Host}:{rabbitMqsetting.Port},username={rabbitMqsetting.UserName};password={rabbitMqsetting.Password};persistentMessages={rabbitMqsetting.IsPersistence}";
        }

        public string GetFileWatcher()
        {
            return ConfigurationManager.AppSettings["FileWatcher"];
        }

        public string GetPatternFile()
        {
            return ConfigurationManager.AppSettings["PatternFile"];
        }

        public string GetProcessedFolder()
        {
            return ConfigurationManager.AppSettings["ProcessedFolder"];
        }

        public string GetProcessingFolder()
        {
            return ConfigurationManager.AppSettings["ProcessingFolder"];
        }

        public string GetSubcriptionId()
        {
            return ConfigurationManager.AppSettings["SubcriptionId"];
        }

        public string[] GetTopicConsumers()
        {
            var topicValues = ConfigurationManager.AppSettings["ReceivedTopicMessagePattern"];
            if (!string.IsNullOrEmpty(topicValues))
            {
                return topicValues.Split(',');
            }

            return new string[] { };
        }

        public string[] GetTopicsPublisher()
        {
            var topicValues = ConfigurationManager.AppSettings["TopicMessagePattern"];
            if (!string.IsNullOrEmpty(topicValues))
            {
                return topicValues.Split(',');
            }

            return new string[] { };
        }

        public string ClientServiceName()
        {
            return ConfigurationManager.AppSettings["ClientServiceName"];
        }
    }
}
