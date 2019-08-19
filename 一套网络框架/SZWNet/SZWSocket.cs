using System;
using System.Net;
using System.Net.Sockets;

namespace SZWNet
{
    public class SZWSocket<T> where T: SZWSession,new()
    {
        public T session = null;
        private Socket socket;

        public SZWSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #region 服务器
        public void StartAsServer(string address, int port)
        {
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
                socket.Listen(0);
                socket.BeginAccept(ClientConnectCallBack, socket);
                Log.WriteLine("服务器启动成功!");
                Log.WriteLine("等待客户端连入......");
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message, Log.LogType.Error);
            }
        }

        private void ClientConnectCallBack(IAsyncResult result)
        {
            try
            {
                Socket client = socket.EndAccept(result);
                Log.WriteLine(string.Format("客户端：{0}连入",client.RemoteEndPoint.ToString()));
                T session = new T();
                session.StartReceive(client);

            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message, Log.LogType.Error);
            }
            socket.BeginAccept(new AsyncCallback(ClientConnectCallBack), socket);
        }
        #endregion

        #region 客户端

        public void StatrAsClient(string address, int port)
        {
            try
            {
                socket.BeginConnect(new IPEndPoint(IPAddress.Parse(address), port), ConnectServerCallBack, socket);
                Log.WriteLine("客户端启动成功!");
                Log.WriteLine("等待连入服务器......");
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message,Log.LogType.Error);             
            }
        }

        private void ConnectServerCallBack(IAsyncResult result)
        {
            try
            {
                socket.EndConnect(result);
                session = new T();
                session.StartReceive(socket);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message, Log.LogType.Error);
            }
        }
        #endregion
    }
}
