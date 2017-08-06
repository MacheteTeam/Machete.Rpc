/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:02:28 
* clrversion :	4.0.30319.42000
******************************************************/

namespace Machete.Rpc.Models
{
    public class RpcRequest
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { set; get; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { set; get; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { set; get; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string ParameterType { set; get; }

        /// <summary>
        /// 0 正常访问 -1 关闭连接请求 1心跳检测
        /// </summary>
        public int Type { set; get; }

        public static RpcRequest BuildRequest(string nameSpace, string method, string parameter, string parameterType)
        {
            return new RpcRequest()
            {
                NameSpace = nameSpace,
                Method = method,
                Parameter = parameter,
                Type = 0,
                ParameterType = parameterType
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
