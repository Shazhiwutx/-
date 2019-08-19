using System.Collections.Generic;

namespace SZWNet
{

    //使用帮助
    //1.指定通讯CMD
    //2.增加消息内容
    [System.Serializable]
    public class NetMsg
    {
        public string cmd;
        public Dictionary<string, string> msgDic = new Dictionary<string, string>();

        /// <summary>
        /// 向消息对象中加入一个消息键值对
        /// </summary>
        /// <param name="key">消息键值</param>
        /// <param name="value">消息内容</param>
        public void AddMsg(string key, string value)
        {
            if (msgDic.ContainsKey(key))
            {
                Log.WriteLine(string.Format("当前增加的消息Key值已经存在，Key = {0}", key), Log.LogType.Warning);
                return;
            }
            msgDic.Add(key, value);
        }

        /// <summary>
        /// 从消息对象中移除一个键值为key的对象
        /// </summary>
        /// <param name="key">要移除的消息key值</param>
        public void RemoveMsg(string key)
        {
            if (msgDic.ContainsKey(key))
            {
                msgDic.Remove(key);
            }

        }
    }
}
