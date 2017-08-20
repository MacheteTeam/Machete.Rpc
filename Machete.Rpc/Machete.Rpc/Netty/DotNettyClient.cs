using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Machete.Rpc.Enum;
using Machete.Rpc.Proxy;
using Machete.Rpc.Transport;

namespace Machete.Rpc.Netty
{
    public class DotNettyClient : ITransportClient
    {
        public static IChannel Channel { set; get; }

        public static IEventLoopGroup Group { set; get; }

        public ClientMessageHandler ClientMessageHandler = new ClientMessageHandler();

        public async void Connect(string host, int port)
        {
            Group = new MultithreadEventLoopGroup();

            var bootstrap = new Bootstrap();
            bootstrap
                .Group(Group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new LengthFieldPrepender(4));
                    pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                    pipeline.AddLast(ClientMessageHandler);
                }));

            Channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(host), port));
        }

        public async void Close()
        {
            await Channel.CloseAsync();
            await Group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public TransportMessage SendMessage(string message)
        {
            if (message == null)
            {
                return null;
            }
            IByteBuffer buffer = Unpooled.Buffer(256);
            TransportMessage transportMessage = new TransportMessage
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = message,
                TransoprtType = TransoprtType.Request
            };
            //注册结果回调
            var callbackTask = ClientMessageHandler.RegisterResultCallbackAsync(transportMessage.Id);

            //Task.Run(() =>
            //{
            //    Thread.Sleep(5000);
            //    TaskCompletionSource<TransportMessage> task;
            //    ClientMessageHandler._resultDictionary.TryGetValue(transportMessage.Id, out task);
            //    task.SetResult(transportMessage);
            //});


            try
            {
                message = Newtonsoft.Json.JsonConvert.SerializeObject(transportMessage);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                buffer.WriteBytes(messageBytes);
                Channel.WriteAndFlushAsync(buffer);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            TransportMessage transport = callbackTask.Result;
            return transport;
        }


        /// <summary>
        /// 创建一个调用传输消息。
        /// </summary>
        /// <param name="invokeMessage">调用实例。</param>
        /// <returns>调用传输消息。</returns>
        //public static TransportMessage CreateInvokeMessage(RemoteInvokeMessage invokeMessage)
        //{
        //    return new TransportMessage(invokeMessage)
        //    {
        //        Id = Guid.NewGuid().ToString("N")
        //    };
        //}
    }
}
