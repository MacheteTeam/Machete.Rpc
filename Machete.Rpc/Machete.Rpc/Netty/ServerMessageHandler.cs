using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Machete.Rpc.Enum;
using Machete.Rpc.Proxy;

namespace Machete.Rpc.Netty
{
    public class ServerMessageHandler : ChannelHandlerAdapter
    {
        public event RequestHandler Handle;

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                string receiveData = buffer.ToString(Encoding.UTF8);
                TransportMessage transportMessage =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<TransportMessage>(receiveData);
                if (transportMessage.TransoprtType == TransoprtType.Init)
                {
                    string responseData = "ok";
                    transportMessage.TransoprtType = TransoprtType.Ans;
                    transportMessage.Message = responseData;
                }
                if (transportMessage.TransoprtType == TransoprtType.Request)
                {
                    string responseData = Handle?.Invoke(transportMessage.Message);
                    transportMessage.TransoprtType = TransoprtType.Response;
                    transportMessage.Message = responseData;
                }
                else
                {
                    string responseData = "不需要处理的";
                    transportMessage.TransoprtType = TransoprtType.Response;
                    transportMessage.Message = responseData;
                }

                string responseMessage = Newtonsoft.Json.JsonConvert.SerializeObject(transportMessage);
                byte[] messageBytes = Encoding.UTF8.GetBytes(responseMessage);
                IByteBuffer byteBuffer = Unpooled.Buffer(messageBytes.Length);
                byteBuffer.WriteBytes(messageBytes);
                context.WriteAndFlushAsync(byteBuffer);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
        }
    }
}
