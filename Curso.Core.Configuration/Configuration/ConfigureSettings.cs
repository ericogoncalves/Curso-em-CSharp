using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Curso.Core.Configuration
{
    internal static class GlobalSettings
    {
        public static ConfigureSettings Configuration { get; set; }
    }

    public class ConfigureSettings
    {
        public const string CursoService = "Curso.Core";
        public const string AuthService = "Auth";
        public const string TaklkService = "Talk";
        private static string GetFileName()
        {
#if RELEASE
			return "Configuration.PROD.json";
#endif
#if DEBUG
            return "Configuration.DEV.json";
#endif
        }

        public ConfigureSettings()
        {
            Services = new List<Service>();
        }

        public List<Service> Services { get; set; }
        public JwtConfig Jwt { set; get; }
        public FileDataSettings FileDataSettings { get; set; }
        public SmtpData SmtpData { get; set; }
        public string NewPasswordLink { get; set; }
        public EmailConfiguartion EmailConfiguration { get; set; }

        public static string GetNewPasswordLink()
        {
            return LoadJson().NewPasswordLink;
        }

        public static JwtConfig GetJwtConfig()
        {
            return LoadJson()?.Jwt;
        }

        public static FileDataSettings GetFileDataSettings()
        {
            return LoadJson()?.FileDataSettings;
        }

        public static EmailConfiguartion GetEmailConfig()
        {
            return LoadJson()?.EmailConfiguration;
        }

        public static SmtpData GetSmtpData()
        {
            return LoadJson()?.SmtpData;
        }

        public static ConfigureSettings GetSettings()
        {
            return LoadJson();
        }

        public static Service GetService(string serviceName)
        {
            var settings = LoadJson();
            return settings.Services.FirstOrDefault(x => x.ServiceName == serviceName);
        }

        private static ConfigureSettings LoadJson()
        {
            try
            {
                ConfigureSettings config = null;
                if (GlobalSettings.Configuration != null) return GlobalSettings.Configuration;

                var file = GetFileName();

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var configPath = Path.Combine(path, file);

                if (!File.Exists(configPath))
                    configPath = Path.Combine(path, "Configuration", file);

                var reader =
                    new JsonTextReader(new StringReader(File.ReadAllText(configPath)));
                var serializer = new JsonSerializer();
                config = serializer.Deserialize<ConfigureSettings>(reader);
                if (config == null)
                    throw new Exception("No configuration was found");
                if (config.Services == null || config.Services.Count == 0)
                    throw new Exception("curso settings was not parametrized");

                GlobalSettings.Configuration = config;

                return GlobalSettings.Configuration;
            }
            catch (Exception ex)
            {
                throw new Exception("A problem occured when loading DevSnapCoreSettings.json. Error: " + ex.Message);
            }
        }

    }

    public enum EFileDataStorage : byte
    {
        Local = 0,
        AWS = 1,
        Azure = 2,
        CGP = 3
    }

    public class JwtConfig
    {
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
    }

    public class EmailConfiguartion
    {
        public string Sender { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public string Host { get; set; }
        public string ApiKey { get; set; }
        public string NoReply { get; set; }
    }

    public class FileDataSettings
    {
        public string Comment { get; set; }
        public string StorageBucket { get; set; }
        public EFileDataStorage FileDataStorage { get; set; }
        public string AwsProfileName { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AzureAccessKey { get; set; }
        public string GoogleCredentialFile { get; set; }
    }

    public class SmtpData
    {
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string ServiceProtocol { get; set; }
        public string ServiceUser { get; set; }
        public string ServicePassword { get; set; }
        public string ServiceDefaultSender { get; set; }
    }

    public class Service
    {
        public string ServiceName { get; set; }
        public string ServiceUri { get; set; }
        public Repository Repository { get; set; }
    }

    public class Repository
    {
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
    }

}
