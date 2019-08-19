using System;
using System.Net.Sockets;

namespace SZWNet
{
    /// <summary>
    /// 网络会话，数据收发，和对收到的数据进行处理，可以继承该类，并对相关方法重写
    /// </summary>
    public class SZWSession
    {
        private Socket socket;
        private ByteBuffer byteBuffer = new ByteBuffer();

        #region 接受数据
        public void StartReceive(Socket socket)
        {
            this.socket = socket;
            try
            {
                socket.BeginReceive(byteBuffer.headBuffer, byteBuffer.headIndex, byteBuffer.headLength- byteBuffer.headIndex, SocketFlags.None, ReceiveHead, socket);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message, Log.LogType.Error);
            }
        }

        public void ReceiveHead(IAsyncResult result)
        {
            try
            {
                int length = socket.EndReceive(result);
                if (length > 0)
                {
                    byteBuffer.headIndex += length;
                    if (byteBuffer.headIndex < byteBuffer.headLength)
                    {
                        socket.BeginReceive(byteBuffer.headBuffer, byteBuffer.headIndex, byteBuffer.headLength - byteBuffer.headIndex, SocketFlags.None, ReceiveHead, socket);
                    }
                    else
                    {
                        byteBuffer.InitBodyBuff();
                        socket.BeginReceive(byteBuffer.bodyBuffer, byteBuffer.bodyIndex, byteBuffer.bodyLength - byteBuffer.bodyIndex, SocketFlags.None, ReceiveBody, socket);
                    }
                }
                else
                {
                    OnDisConnected();
                    Close();
                }
            }
            catch(Exception e)
            {
                Log.WriteLine("接受数据错误：" + e.Message, Log.LogType.Error);
            }
        }

        public void ReceiveBody(IAsyncResult result)
        {
            try
            {
                int length = socket.EndReceive(result);
                if (length > 0)
                {
                    byteBuffer.bodyIndex += length;
                    if (byteBuffer.bodyIndex < byteBuffer.bodyLength)
                    {
                        socket.BeginReceive(byteBuffer.bodyBuffer, byteBuffer.bodyIndex, byteBuffer.bodyLength - byteBuffer.bodyIndex, SocketFlags.None, ReceiveBody, socket);
                    }
                    else
                    {
                        NetMsg netMsg = MsgHelper.DeSerialize(byteBuffer.bodyBuffer);
                        OnRecMsg(netMsg);
                        byteBuffer.Reset();
                        socket.BeginReceive(byteBuffer.headBuffer, byteBuffer.headIndex, byteBuffer.headLength - byteBuffer.headIndex, SocketFlags.None, ReceiveHead, socket);
                    }
                }
                else
                {
                    OnDisConnected();
                    Close();
                }
            }
            catch (Exception e)
            {
                Log.WriteLine("接受数据错误：" + e.Message, Log.LogType.Error);
            }
        }
        #endregion

        #region 发送数据

        public void Send(NetMsg netMsg)
        {
            socket.Send(MsgHelper.Serialize(netMsg));
        }
        #endregion

        public virtual void OnRecMsg(NetMsg netMsg)
        {
            Log.WriteLine("请重写该方法以处理接收到的数据");
            Log.WriteLine("当前使用默认方法");
            Send(netMsg);
        }
        public virtual void OnConnected()
        {
            Log.WriteLine("新会话建立连接");
        }

        public virtual void OnDisConnected()
        {
            Log.WriteLine("会话断开连接");
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }
    

}
