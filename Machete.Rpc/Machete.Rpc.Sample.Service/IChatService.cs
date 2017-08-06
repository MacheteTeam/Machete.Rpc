/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/6 16:03:36 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Rpc.Attributes;

namespace Machete.Rpc.Sample.Service
{
    [RpcService]
    public interface IChatService
    {
        string Hi(string name);

        string Hi(string name, string content);

        string Hello(int age);

        string Hello(double age);
    }
}
