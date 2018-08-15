namespace DineConnect.Common
{
    public interface IConfiguration
    {
        string GetConnectionString();

        string GetFileWatcher();

        string GetProcessingFolder();

        string GetProcessedFolder();

        string GetPatternFile();

        string[] GetTopicConsumers();
    }
}
