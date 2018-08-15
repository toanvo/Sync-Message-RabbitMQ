using System.Configuration;

namespace DineConnect.Common
{
    public class RabbitMqSettings : ConfigurationSection
    {
        [ConfigurationProperty("host", DefaultValue = "localhost", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = "5672", IsRequired = false)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("username", DefaultValue = "guest", IsRequired = false)]
        public string UserName
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password", DefaultValue = "guest", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("isPersistence", DefaultValue = "true", IsRequired = false)]
        public string IsPersistence
        {
            get
            {
                return (string)this["isPersistence"];
            }
            set
            {
                this["isPersistence"] = value;
            }
        }
    }
}
