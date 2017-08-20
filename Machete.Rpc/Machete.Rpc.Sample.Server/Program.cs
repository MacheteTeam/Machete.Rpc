using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Machete.Rpc.Netty;


namespace Machete.Rpc.Sample.Server
{
    class Program
    {

        static async Task RunServerAsync()
        {
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["rpc.server.port"].ToString());
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        pipeline.AddLast("echo", new EchoServerHandler());
                    }));

                IChannel boundChannel = await bootstrap.BindAsync(port);

                Console.ReadLine();

                await boundChannel.CloseAsync();
            }
            finally
            {
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        static async Task Start()
        {
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["rpc.server.port"].ToString());
            RpcHub hub = new RpcHub();
            hub.Start(port);

            //DotNettyServer server = new DotNettyServer();
            //await server.Listen(port);

            Console.WriteLine("服务已经启动 端口：" + port);
        }

        static void Main(string[] args)
        {
            Start().Wait();
            Console.ReadKey();
            //Test();
        }

        public static void Test()
        {
            var source = new TaskCompletionSource<int>();

            new Thread(() => { Thread.Sleep(5000); source.SetResult(123); })
                .Start();

            Task<int> task = source.Task;      // 我们的“奴隶”任务
            Console.WriteLine(task.Result);   // 123
        }
    }
}
