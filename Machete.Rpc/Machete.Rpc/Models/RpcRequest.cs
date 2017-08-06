/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:02:28 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXY.Rpc.Models
{
    public class RpcRequest
    {
        public string NameSpace { set; get; }

        public string Method { set; get; }

        public string Parameter { set; get; }

        /// <summary>
        /// 0 正常访问 -1 关闭连接请求 1心跳检测
        /// </summary>
        public int Type { set; get; }

        public static RpcRequest BuildRequest(string nameSpace, string method, string parameter)
        {
            return new RpcRequest()
            {
                NameSpace = nameSpace,
                Method = method,
                Parameter = parameter,
                Type = 0
            };
        }

        public static RpcRequest BuildCloseRequest()
        {
            return new RpcRequest()
            {
                Type = -1,
            };
        }
    }
}
