/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:03:00 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXY.Rpc.Models
{
    public class RpcResponse
    {
        /// <summary>
        /// 返回代码 0 表示成功
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// 返回的提示信息
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        ///  返回值
        /// </summary>
        public string Response { set; get; }
    }
}
