using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroFriction.DataAccessLayer.Configuration;
using ZeroFriction.DataAccessLayer.DataStucture;

namespace ZeroFriction.DataAccessLayer.StandardData.Databases
{
    public class CosmosDatabase
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory loggerFactory;

        private ConfigWrapper config = new ConfigWrapper();
        protected string conString;

        public CosmosDatabase(ILoggerFactory logger)
        {
            loggerFactory = logger;
            _logger = logger.CreateLogger<MsSQLDatabase>();

        }


        // <CreateDatabaseAsync>
        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        public  string GetDatabaseAsync()
        {
            // Create a new database
            try
            {
                return config.GetAppSettingsItem("DatabaseId");
            }
            catch (Exception ex)
            {
                _logger.LogError("GetDatabaseAsync error thrown:{0}", ex);
                throw ex;
            }
            
        }

        // <CreateContainerAsync>
        /// <summary>
        /// Create the container if it does not exist. 
        /// Specifiy "/partitionKey" as the partition key path since we're storing family information, to ensure good distribution of requests and storage.
        /// </summary>
        /// <returns></returns>
        public string GetContainerAsync()
        {
            // Create a new container
            try
            {
                return config.GetAppSettingsItem("ContainerId");
            }
            catch (Exception ex)
            {
                _logger.LogError("GetDatabaseAsync error thrown:{0}", ex);
                throw ex;
            }
        }

        public string GetEndPointAsync()
        {
            // Create a new container
            try
            {
                return config.GetAppSettingsItem("EndpointUri");
            }
            catch (Exception ex)
            {
                _logger.LogError("GetDatabaseAsync error thrown:{0}", ex);
                throw ex;
            }
        }

        public string GetPrimaryKeyAsync()
        {
            // Create a new container
            try
            {
                return config.GetAppSettingsItem("PrimaryKey");
            }
            catch (Exception ex)
            {
                _logger.LogError("GetDatabaseAsync error thrown:{0}", ex);
                throw ex;
            }
        }

    }
}
