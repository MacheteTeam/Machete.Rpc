/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:15:42 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Machete.Rpc.Attributes;
using Machete.Rpc.Exceptions;

namespace Machete.Rpc
{
    public class RpcConatiner
    {
        public static Dictionary<string, Object> ServiceContainer = new Dictionary<string, Object>();

        public static void Initialize()
        {
            try
            {
                string bastPath = AppDomain.CurrentDomain.BaseDirectory;
                var serviceDlls = ConfigurationManager.AppSettings["rpc.service"].Split(',').ToList();
                if (serviceDlls.Count == 0)
                {
                    throw new NotExistException("没有找到服务实现");
                }

                foreach (var dllPath in serviceDlls)
                {
                    Assembly assembly = Assembly.LoadFrom(bastPath + "/" + dllPath);
                    List<Type> types = assembly.GetTypes().ToList();
                    foreach (var type in types)
                    {
                        RpcServiceAttribute attribute = type.GetCustomAttribute<RpcServiceAttribute>();
                        if (attribute != null)
                        {
                            List<Type> interfaces = type.GetInterfaces().ToList();
                            foreach (var iInterface in interfaces)
                            {
                                RpcServiceAttribute attribute1 = iInterface.GetCustomAttribute<RpcServiceAttribute>();
                                if (attribute1 != null)
                                {
                                    ServiceContainer.Add(iInterface.FullName, Activator.CreateInstance(type));
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
