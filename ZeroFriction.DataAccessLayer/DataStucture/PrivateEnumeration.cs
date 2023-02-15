using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.DataAccessLayer.DataStucture
{
    //Execution Mode enumeration
    public enum Execution
    {
        Single,
        Multiple
    }

    //The Database Mode enumeration.
    public enum DBMode
    {
        DB1,
        DB2,
        DB3,
        DB4,
        DB5
    }

    //The Database DataType enumeration.
    public enum DataType
    {
        Binary = 0, Bit = 1, VarChar = 2, DateTime = 3, Decimal = 4, Double = 5, Float = 6, Int = 7, BigInt = 8, Image = 9
    }

    //Timeout peroid in milisecond
    public enum Timeout
    {
        Yes,
        No
    }

    //The Database Type enumeration.
    public enum DBType
    {
        MsSQL,
        MySQL,
        MongoDB,
        CosmosDB
    }
    public enum DmlType
    {
        Select = 0,
        Insert = 1,
        Update = 2,
        Delete = 3,
        Create = 4,
        Truncate = 5
    }
}
