/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:00:19 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DXY.Rpc;
using DXY.Rpc.Models;
using Newtonsoft.Json;

namespace Machete.Rpc
{
    public class RpcDefaultRequestHandler : IRpcRequestHandler
    {
        public RpcResponse Handle(RpcRequest request)
        {
            string nameSpace = request.NameSpace;
            RpcResponse response = new RpcResponse() { Code = 1, Message = "未知错误" };
            object obj;
            bool flag = RpcConatiner.ServiceContainer.TryGetValue(nameSpace, out obj);
            if (flag)
            {
                List<string> requestParamList = JsonConvert.DeserializeObject<List<string>>(request.Parameter);

                List<MethodInfo> methodInfos = obj.GetType().GetMethods().Where(x => x.Name == request.Method).ToList();

                List<object> parameters = new List<object>();

                if (methodInfos.Count == 0)
                {
                    response.Message = "没有可调用的方法";
                    return response;
                }
                if (methodInfos.Count == 1)
                {
                    var methodInfo = methodInfos.First();
                    ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        Type type = parameterInfo.ParameterType;
                        parameters.Add(JsonConvert.DeserializeObject(requestParamList[i], type));
                    }
                    // var attributes = methodInfo.GetCustomAttributes();
                    var result = methodInfo.Invoke(obj, parameters.ToArray());
                    response.Code = 0;
                    response.Message = "成功";
                    response.Response = JsonConvert.SerializeObject(result);
                    return response;
                }
                if (methodInfos.Count > 1)
                {
                    foreach (var methodInfo in methodInfos)
                    {
                        bool match = true;
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        if (parameterInfos.Length != requestParamList.Count)
                        {
                            continue;
                        }
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            string paramterValue = requestParamList[i];
                            ParameterInfo parameterInfo = parameterInfos[i];
                            Type type = parameterInfo.ParameterType;
                            try
                            {
                                JsonConvert.DeserializeObject(paramterValue, type);
                            }
                            catch
                            {
                                match = false;
                                break;
                            }
                            parameters.Add(JsonConvert.DeserializeObject(requestParamList[i], type));
                        }
                        if (match)
                        {
                            //var attributes = methodInfo.GetCustomAttributes();
                            var result = methodInfo.Invoke(obj, parameters.ToArray());
                            response.Code = 0;
                            response.Message = "成功";
                            response.Response = JsonConvert.SerializeObject(result);
                            return response;
                        }
                    }
                }
            }
            return response;
        }
    }
}
