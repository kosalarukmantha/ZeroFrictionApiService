using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.DataAccessLayer.Model
{
    public class SQLConnectionString
    {
        public string Server { get; set; }
        public string DataBase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IntegratedSecurity { get; set; }
        public string ConnectTimeout { get; set; }
        public string ConnectionMinPoolSize { get; set; }
    }
}
