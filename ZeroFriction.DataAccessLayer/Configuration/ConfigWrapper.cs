using ZeroFriction.DataAccessLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace ZeroFriction.DataAccessLayer.Configuration
{
    public class ConfigWrapper
    {
        public ConfigWrapper()
        {
            var envname = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
           // var pathVariable = Path.Combine(Directory.GetCurrentDirectory(), "appsettings." + envname + ".json");

            configurationBuilder.AddJsonFile(path, false);
           // configurationBuilder.AddJsonFile(pathVariable, optional: true);
            RootConfig = configurationBuilder.Build();
        }

        public IConfigurationRoot RootConfig { get; set; }


        public string GetAppSettingsItem(string name)
        {

            try
            {
                return RootConfig.GetSection("Configurations").GetSection("AppSettings").GetSection(name)?.Value;
            }
            catch (Exception)
            {
                return "";
            }
        }



        public NameValueCollection GetDBCollection(string key)
        {
            SQLConnectionString constr = new SQLConnectionString();

            constr = RootConfig.GetSection("Configurations").GetSection("MsSQL").GetSection(key).Get<SQLConnectionString>();

            NameValueCollection sqlDBString = new NameValueCollection();
            sqlDBString.Add("integratedSecurity", constr.IntegratedSecurity);

            sqlDBString.Add("server", constr.Server);
            sqlDBString.Add("database", constr.DataBase);
            sqlDBString.Add("connecttimeout", constr.ConnectTimeout);
            sqlDBString.Add("connectionminpoolsize", constr.ConnectionMinPoolSize);
            sqlDBString.Add("username", constr.UserName);
            sqlDBString.Add("password", constr.Password);

            return sqlDBString;
        }

        public NameValueCollection GetSecureAppSetting()
        {
            RootSecureAppSettings constr = new RootSecureAppSettings();

            constr = RootConfig.GetSection("Configurations").GetSection("secureAppSettings").Get<RootSecureAppSettings>();

            NameValueCollection appSettingString = new NameValueCollection();
            // can read keys on SecureAppSetting
            return appSettingString;
        }

       
    }
}
