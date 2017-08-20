using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Rpc.Enum;

namespace Machete.Rpc.Netty
{
    /// <summary>
    /// 数据传输
    /// </summary>
    public class TransportMessage
    {
        public string Id { set; get; }

        public string Message { set; get; }

        /// <summary>
        /// 传输类型 1.init 2.ans  2.request 3.response
        /// </summary>
        public TransoprtType TransoprtType { set; get; }
    }

     
}
