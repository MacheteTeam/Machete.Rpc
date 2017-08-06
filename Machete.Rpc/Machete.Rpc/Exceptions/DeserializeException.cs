/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 17:13:33 
* clrversion :	4.0.30319.42000
******************************************************/

using System;

namespace Machete.Rpc.Exceptions
{
    public class DeserializeException : Exception
    {
        public DeserializeException()
        {
        }

        public DeserializeException(string message) : base(message)
        {

        }

        public DeserializeException(string message, Exception ex):base(message,ex)
        {

        }
    }
}
