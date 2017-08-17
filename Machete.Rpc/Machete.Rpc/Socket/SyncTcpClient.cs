/******************************************************
* author :  Lenny
* email  :  niel@dxy.cn 
* function: 
* time:	2017/8/17 22:51:46 
* clrversion :	4.0.30319.42000
******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Rpc.Socket
{
    public class SyncTcpClient
    {
        /// <summary>
        /// 发送
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
                    catch (System.Exception)
                    {
                        throw;
                    }
                }
            }
            return result;
        }

    }
}
