/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 17:16:28 
* clrversion :	4.0.30319.42000
******************************************************/

using System;

namespace Machete.Rpc.Exceptions
{
    public class RpcArgumentException : Exception
    {
        public RpcArgumentException()
        {

        }

        public RpcArgumentException(string message) : base(message)
        {

        }
    }
}
