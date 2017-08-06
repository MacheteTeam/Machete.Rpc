/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 9:41:33 
* clrversion :	4.0.30319.42000
******************************************************/

using System;

namespace Machete.Rpc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class RpcServiceAttribute : Attribute
    {
    }
}
