using ZeroFriction.DataAccessLayer.DataStucture;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ZeroFriction.DataAccessLayer.Model
{
    public class ExecuteQueryObj
    {
        public String Sql { get; set; }
        public List<SqlParameter> SqlParams { get; set; }
        public DmlType DmlType { get; set; }
        public bool IsAudited { get; set; }
        public int? CreatedUser { get; set; }
        public String TableName { get; set; }
        public String CaptureText { get; set; }
        public String NameSpace { get; set; }
        public string note { get; set; }
        public string tableName { get; set; }
        public DataTable dataTable { get; set; }
    }
}
