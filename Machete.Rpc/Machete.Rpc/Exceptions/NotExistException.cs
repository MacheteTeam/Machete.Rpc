/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 9:43:23 
* clrversion :	4.0.30319.42000
******************************************************/

using System;

namespace Machete.Rpc.Exceptions
{
    public class NotExistException : Exception
    {
        //三个共有构造器
        public NotExistException() : base()//调用基类的构造器
        {

        }

        public NotExistException(string message) : base(message)//调用基类的构造器
        {
        }
    }
}
