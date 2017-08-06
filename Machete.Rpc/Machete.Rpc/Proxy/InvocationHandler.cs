/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/6 15:08:11 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Rpc.Proxy
{
    public interface InvocationHandler
    {
        object InvokeMember(object obj, int rid, string name, params object[] args);
    }
}
