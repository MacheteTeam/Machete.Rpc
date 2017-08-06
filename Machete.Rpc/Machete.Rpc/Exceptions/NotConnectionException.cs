/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/4 21:04:36 
* clrversion :	4.0.30319.42000
******************************************************/

using System;

namespace Machete.Rpc.Exceptions
{
    public class NotConnectionException : Exception
    {
        public NotConnectionException() { }

        public NotConnectionException(string message) : base(message)
        {

        }
    }
}
