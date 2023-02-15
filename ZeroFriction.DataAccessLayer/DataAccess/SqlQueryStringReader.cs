using ZeroFriction.DataAccessLayer.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace ZeroFriction.DataAccessLayer.DataAccess
{
    public static class SqlQueryStringReader
    {
        private const int maxRetryCount = 3;
        private const int retrySleepTimeMilliseconds = 2000;

        private static string ReadQueryStringFromFile(string id, string xmlfilename)
        {
            int retryCount = 0;
            try
            {
                ConfigWrapper config = new ConfigWrapper();
                var filePath = config.GetAppSettingsItem("XmlQueryPath") + xmlfilename + ".xml";

                using (var reader = new XmlTextReader(filePath))
                {
                    reader.WhitespaceHandling = WhitespaceHandling.None;
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SqlQueryString")
                        {
                            string currentId = reader.GetAttribute("Id");
                            if (currentId == id)
                            {
                                reader.Read();
                                return reader.Value;
                            }
                        }
                    }
                }

            }

            catch (IOException e)
            {

                if (++retryCount > maxRetryCount)
                {
                    throw;
                }

                Thread.Sleep(retrySleepTimeMilliseconds);
            }


            return null;
        }

        public static string GetQueryStringById(string id, string xmlfilename)
        {
            return ReadQueryStringFromFile(id, xmlfilename);
        }
    }
}
