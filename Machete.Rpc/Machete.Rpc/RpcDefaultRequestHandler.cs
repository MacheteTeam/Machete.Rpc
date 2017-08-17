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
using Machete.Rpc.Models;
using Newtonsoft.Json;

namespace Machete.Rpc
{
    public class RpcDefaultRequestHandler : IRpcRequestHandler
    {
        public string Handle(string requestData)
        {
            RpcRequest request = JsonConvert.DeserializeObject<RpcRequest>(requestData);
            string nameSpace = request.NameSpace;
            RpcResponse response = new RpcResponse() { Code = 1, Message = "未知错误" };
            object obj;
            bool flag = RpcConatiner.ServiceContainer.TryGetValue(nameSpace, out obj);
            if (flag)
            {
                List<string> requestParamList = JsonConvert.DeserializeObject<List<string>>(request.Parameter);
                List<string> requestParamTypeList = JsonConvert.DeserializeObject<List<string>>(request.ParameterType);
                List<Type> types = new List<Type>();
                try
                {
                    foreach (var paramType in requestParamTypeList)
                    {
                        Type type = GetType(paramType);
                        if (type == null)
                        {
                            response.Code = 3;
                            response.Message = "未加载目标方法";
                            return JsonConvert.SerializeObject(response);
                        }
                        types.Add(type);
                    }

                    MethodInfo method = obj.GetType().GetMethod(request.Method, types.ToArray());

                    if (method == null)
                    {
                        response.Code = 2;
                        response.Message = "请求方法不存在";
                        return JsonConvert.SerializeObject(response);
                    }

                    List<object> parameters = new List<object>();
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        Type type = parameterInfo.ParameterType;
                        parameters.Add(JsonConvert.DeserializeObject(requestParamList[i], type));
                    }

                    var result = method.Invoke(obj, parameters.ToArray());
                    response.Code = 0;
                    response.Message = "成功";
                    response.Response = JsonConvert.SerializeObject(result);
                    return JsonConvert.SerializeObject(response);
                }
                catch (System.Exception e)
                {
                    response.Code = -1;
                    response.Message = e.Message;
                    return JsonConvert.SerializeObject(response);
                }
            }
            response.Message = "未找到远程接口服务";
            return JsonConvert.SerializeObject(response);
        }

        private Type GetType(string typeFullName)
        {
            //搜索当前域中已加载的程序集
            Assembly[] asses = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly ass in asses)
            {
                Type type = ass.GetType(typeFullName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
