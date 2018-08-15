using System;

namespace DineConnect.Common
{
    public class ProcessingMessage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ExtensionFileType { get; set; }
        public string ContentFile { get; set; }
    }
}
