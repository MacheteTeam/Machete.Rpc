/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/3 10:01:06 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DXY.Rpc;

namespace Machete.Rpc
{
    public class RpcNet
    {
        public static Encoding Encode = Encoding.UTF8;

        public static event RequestHandler Handle;

        /// <summary>
        /// 监听请求
        /// </summary>
        /// <param name="port"></param>
        public static void Listen(int port)
        {
            //[1]TcpListener对Socket进行了封装，这各类会自己创建Socket对象  
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            //[2]开始进行监听  
            tcpListener.Start();
            while (true)
            {
                //[3]等待客户端连接过来  
                TcpClient client = tcpListener.AcceptTcpClient();
                //创建消息服务线程对象  
                RpcNetThread newclient = new RpcNetThread(client);
                newclient.Handle += Handle;
                //把RpcNetThread类的ClientService方法委托给线程  
                Thread newthread = new Thread(newclient.ClientService);
                //启动消息服务线程  
                newthread.Start();
            }
        }

        /// <summary>
        /// 发送连接
        /// 支持中文
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message">发送信息</param>
        /// <param name="close"></param>
        public static string SendMessage(TcpClient client, string message, bool close = false)
        {
            NetworkStream stream = client.GetStream();
            BinaryReader br = new BinaryReader(stream);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(message);  //向服务器发送字符串 
            string result = null;
            if (!close)
            {
                while (true)
                {
                    try
                    {
                        result = br.ReadString(); //接收服务器发送的数据  
                        break;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return result;
        }
    }
}
