/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:01:37 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.IO;
using System.Net.Sockets;
using DXY.Rpc;
using DXY.Rpc.Helpers;
using DXY.Rpc.Models;
using Machete.Rpc.Models;
using Newtonsoft.Json;

namespace Machete.Rpc
{
    public class RpcNetThread
    {
        //connections变量表示连接数  
        public static int Connections = 0;
        public TcpClient Client { set; get; }

        public event RequestHandler Handle;

        //构造函数  
        public RpcNetThread(TcpClient clientsocket)
        {
            //service对象接管对消息的控制  
            this.Client = clientsocket;
            Connections++;
        }

        public void ClientService()
        {
            //[4]取得从客户端发来的数据  
            NetworkStream stream = Client.GetStream();//这是一个网络流，从这个网络流可以去的从客户端发来的数据  
            NetworkStream networkStream = Client.GetStream();
            BinaryReader br = new BinaryReader(networkStream);
            BinaryWriter bw = new BinaryWriter(networkStream);

            while (true)
            {
                try
                {
                    string receiveData = br.ReadString(); //接收消息  
                    RpcRequest request = JsonConvert.DeserializeObject<RpcRequest>(receiveData);
                    if (request.Type == -1)
                    {
                        //TODO 做些事情 来处理退出的用户
                        break;
                    }
                    RpcResponse response = Handle?.Invoke(request);
                    string responseData = JsonConvert.SerializeObject(response);
                    bw.Write(responseData);   //向对方发送消息  
                }
                catch (Exception ex)
                {
                    Log4NetHelper.WriteLog("server 接收消息失败", ex);
                    break;
                }
            }
            br.Close();
            bw.Close();
            stream.Close();
            Client.Close();
            Connections--;
        }
    }
}
