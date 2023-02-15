using ZeroFriction.DataAccessLayer.DataStucture;
using ZeroFriction.DataAccessLayer.StandardData.Databases;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.DataAccessLayer.StandardData
{
    public class DatabaseManager
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory loggerFactory;
        private DBMode Mode = DBMode.DB1;

        public DatabaseManager(ILoggerFactory logger, DBMode mode)
        {
            this.Mode = mode;

            loggerFactory = logger;
            _logger = logger.CreateLogger<DatabaseManager>();

        }

        public AbstractDatabase GetDatabase()
        {

            try
            {
                return DBFactory.CreateDB(loggerFactory).GetDB(DBType.MsSQL, this.Mode);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
