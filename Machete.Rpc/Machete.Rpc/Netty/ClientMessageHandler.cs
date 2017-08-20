using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
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

        readonly IByteBuffer _initialMessage;

        public ClientMessageHandler()
        {
            TransportMessage message = new TransportMessage()
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "init",
                TransoprtType = TransoprtType.Init
            };
            byte[] messageBytes = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(message));
            this._initialMessage = Unpooled.Buffer(messageBytes.Length);
            this._initialMessage.WriteBytes(messageBytes);
        }

        public override void ChannelActive(IChannelHandlerContext context) => context.WriteAndFlushAsync(_initialMessage);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;

            if (byteBuffer != null)
            {
                TransportMessage _message = Newtonsoft.Json.JsonConvert.DeserializeObject<TransportMessage>(byteBuffer.ToString(Encoding.UTF8));
                if (_message.TransoprtType == TransoprtType.Response)
                {
                    ResultCallback(_message);
                }
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent)
            {
                var eventState = evt as IdleStateEvent;
                SendHeartbeatAsync(context, eventState);
            }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task SendHeartbeatAsync(IChannelHandlerContext ctx, IdleStateEvent state)
        {
            //获取心跳包的打包内容
            TransportMessage message = new TransportMessage()
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "heartbeat",
                TransoprtType = TransoprtType.Heartbeat,
            };

            string heartbeatStr = Newtonsoft.Json.JsonConvert.SerializeObject(message);

            var heartbeatBuff = ctx.Allocator.Buffer(heartbeatStr.Length);
            byte[] messageBytes = Encoding.UTF8.GetBytes(heartbeatStr);
            heartbeatBuff.WriteBytes(messageBytes);

            return ctx.WriteAndFlushAsync(heartbeatBuff);
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

        /// <summary>
        /// 清楚回调过的回调函数
        /// </summary>
        /// <param name="id"></param>
        public void ClearResultCallback(string id)
        {
            //删除回调任务
            TaskCompletionSource<TransportMessage> value;
            _resultDictionary.TryRemove(id, out value);
        }

        /// <summary>
        ///  回调
        /// </summary>
        /// <param name="message"></param>
        private void ResultCallback(TransportMessage message)
        {
            TaskCompletionSource<TransportMessage> task;
            if (!_resultDictionary.TryGetValue(message.Id, out task))
                return;
            task.SetResult(message);
        }
    }
}
