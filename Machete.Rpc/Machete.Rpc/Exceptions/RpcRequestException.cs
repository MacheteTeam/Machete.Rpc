/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 17:20:20 
* clrversion :	4.0.30319.42000
******************************************************/

using System;

namespace Machete.Rpc.Exceptions
{
    public class RpcRequestException : Exception
    {
        public RpcRequestException()
        {
        }

        public RpcRequestException(string message) : base(message)
        {

        }

        public RpcRequestException(string message, Exception ex):base(message,ex)
        {
           
        }
    }
}
