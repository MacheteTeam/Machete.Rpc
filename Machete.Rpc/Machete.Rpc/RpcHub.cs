/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:24:47 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using DXY.Rpc;
using DXY.Rpc.Helpers;
using DXY.Rpc.Models;
using Machete.Rpc.Exceptions;
using Machete.Rpc.Models;
using Machete.Rpc.Netty;
using Machete.Rpc.Socket;
using Newtonsoft.Json;

namespace Machete.Rpc
{
    public class RpcHub
    {
        /// <summary>
        ///server 启动端口连接
        /// </summary>
        /// <param name="port"></param>
        public async void Start(int port)
        {
            RpcConatiner.Initialize();
            DotNettyServer server = new DotNettyServer();
            await server.Listen(port);
            await Task.Factory.StartNew(async () =>
             {
                 await server.Listen(port);
             });
        }


        /// <summary>
        /// 创建TCP连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public TcpClient Connect(string host, int port)
        {
            try
            {
                TcpClient client = new TcpClient(host, port);
                return client;
            }
            catch (System.Exception e)
            {
                Log4NetHelper.WriteLog(e.StackTrace);
                throw new NotConnectionException("rpc服务出现异常，请查看服务器状态！");
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            RpcRequest request = RpcRequest.BuildCloseRequest();
            SyncTcpClient.SendMessage(ClientContainer.Client, JsonConvert.SerializeObject(request), true); //关闭服务端链接
            ClientContainer.Client.Close();
        }
    }
}
