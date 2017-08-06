/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 17:53:13 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXY.Rpc.Helpers
{
    public class Log4NetHelper
    {
        private static readonly log4net.ILog LogInfo = log4net.LogManager.GetLogger("LogInfo");
        private static readonly log4net.ILog LogError = log4net.LogManager.GetLogger("LogError");
        private static readonly log4net.ILog LogMonitor = log4net.LogManager.GetLogger("LogTrace");
        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        public static void WriteLog(string info)
        {
            if (LogInfo.IsInfoEnabled)
            {
                LogInfo.Info(info);
            }
        }
        public static void WriteLog(string info, Exception ex)
        {
            if (LogError.IsErrorEnabled)
            {
                LogError.Error(info, ex);
            }
        }
    }
}
