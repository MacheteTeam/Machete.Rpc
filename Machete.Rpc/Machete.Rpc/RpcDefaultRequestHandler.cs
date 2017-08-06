﻿/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:00:19 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
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
                var methodInfo = obj.GetType().GetMethod(request.Method);
                if (methodInfo != null)
                {
                    List<string> requestParamList = JsonConvert.DeserializeObject<List<string>>(request.Parameter);

                    ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                    List<object> parameters = new List<object>();
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        Type type = parameterInfo.ParameterType;
                        parameters.Add(JsonConvert.DeserializeObject(requestParamList[i], type));
                    }
                    var result = methodInfo.Invoke(obj, parameters.ToArray());
                    response.Code = 0;
                    response.Message = "成功";
                    response.Response = JsonConvert.SerializeObject(result);
                }
            }
            {
                response.Message = "未发现服务，请检查！";
            }
            return response;
        }
    }
}