using System;
using System.Collections.Concurrent;
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
    public class ClientMessageHandler : ChannelHandlerAdapter
    {
        public static readonly ConcurrentDictionary<string, TaskCompletionSource<TransportMessage>> _resultDictionary =
            new ConcurrentDictionary<string, TaskCompletionSource<TransportMessage>>();


        public static int Size = 256;

        readonly IByteBuffer initialMessage;

        public ClientMessageHandler()
        {
            this.initialMessage = Unpooled.Buffer(256);
            TransportMessage message = new TransportMessage()
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "init",
                TransoprtType = TransoprtType.Init
            };
            byte[] messageBytes = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(message));
            this.initialMessage.WriteBytes(messageBytes);
        }

        public override void ChannelActive(IChannelHandlerContext context) => context.WriteAndFlushAsync(initialMessage);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;

            if (byteBuffer != null)
            {
                TransportMessage _message = Newtonsoft.Json.JsonConvert.DeserializeObject<TransportMessage>(byteBuffer.ToString(Encoding.UTF8));
                if (_message.TransoprtType == TransoprtType.Response)
                {
                    Receive(_message);
                }
            }

            // Task.FromResult<object>(null);  // for .NET45, emulate the functionality
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        /// <summary>
        /// 注册指定消息的回调任务。
        /// </summary>
        /// <param name="id">消息Id。</param>
        /// <returns>远程调用结果消息模型。</returns>
        public Task<TransportMessage> RegisterResultCallbackAsync(string id)
        {

            var task = new TaskCompletionSource<TransportMessage>();
            _resultDictionary.TryAdd(id, task);
            return task.Task;
        }

        private void Receive(TransportMessage message)
        {
            TaskCompletionSource<TransportMessage> task;
            if (!_resultDictionary.TryGetValue(message.Id, out task))
                return;
            task.SetResult(message);
        }
    }
}
