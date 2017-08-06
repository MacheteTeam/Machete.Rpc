using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Rpc.Attributes;
using Machete.Rpc.Sample.Service;

namespace Machete.Rpc.Sample.Implement
{
    [RpcService]
    public class ChatService : IChatService
    {
        public string Hi(string name)
        {
            return name + ":你好 世界";
        }

        public string Hi(string name, string content)
        {
            return name + ":" + content;
        }
    }
}
