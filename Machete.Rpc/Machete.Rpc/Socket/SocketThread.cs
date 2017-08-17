/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/17 22:54:16 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DXY.Rpc.Helpers;

namespace Machete.Rpc.Socket
{
    public class SocketThread
    {
        //connections变量表示连接数  
        public static int Connections = 0;
        public TcpClient Client { set; get; }

        public event RequestHandler Handle;

        //构造函数  
        public SocketThread(TcpClient clientsocket)
        {
            //service对象接管对消息的控制  
            this.Client = clientsocket;
            Connections++;
        }

        public void Execute()
        {
            //[4]取得从客户端发来的数据  
            NetworkStream networkStream = Client.GetStream();  //这是一个网络流，从这个网络流可以去的从客户端发来的数据  
            BinaryReader br = new BinaryReader(networkStream);
            BinaryWriter bw = new BinaryWriter(networkStream);

            while (true)
            {
                try
                {
                    string receiveData = br.ReadString(); //接收消息  
                    string responseData = Handle?.Invoke(receiveData);
                    bw.Write(responseData);   //向对方发送消息  
                }
                catch (System.Exception ex)
                {
                    Log4NetHelper.WriteLog("server 接收消息失败", ex);
                    break;
                }
            }
            br.Close();
            bw.Close();
            Client.Close();
            Connections--;
        }
    }
}
