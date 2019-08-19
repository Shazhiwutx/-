using Newtonsoft.Json;
using System;
using System.Text;

namespace SZWNet
{
    public class MsgHelper
    {
        /// <summary>
        /// 将收到的缓存消息字节数组转换成NetMsg消息对象
        /// </summary>
        /// <param name="buffer">缓存的消息字节数组</param>
        /// <returns>NetMsg消息对象</returns>
        public static NetMsg DeSerialize(byte[] buffer)
        {
            if (buffer == null)
                return null;

            string json = System.Text.Encoding.Default.GetString(buffer);
            NetMsg netMsg = new NetMsg();

            try
            {
                netMsg = JsonConvert.DeserializeObject<NetMsg>(json);
            }
            catch
            {
                Log.WriteLine(string.Format("无法解析当前数据，消息内容 = {0}", json), Log.LogType.Error);

            }
            return netMsg;
        }


        /// <summary>
        /// 将NetMsg消息对象序列化成消息字节数组
        /// </summary>
        /// <param name="netMsg">序列化的消息对象</param>
        /// <returns>消息字节数组</returns>
        public static byte[] Serialize(NetMsg netMsg)
        {
            string json = JsonConvert.SerializeObject(netMsg);

            byte[] body = Encoding.Default.GetBytes(json);
            byte[] head = BitConverter.GetBytes(body.Length);
            byte[] buffer = new byte[head.Length + body.Length];

            Buffer.BlockCopy(head, 0, buffer, 0, head.Length);
            Buffer.BlockCopy(body, 0, buffer, head.Length, body.Length);

            return buffer;
        }
    }
}
