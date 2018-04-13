using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWarCommon;

namespace MyWarServer.Servers
{
    class Message
    {
        private byte[] data = new byte[1024]; // 数据缓存
        private int startIndex = 0; // 标记已经存取了多少数据长度
        private int requestCodeStartIndex = 4; // 请求code解析开始位置
        private int actionCodeStartIndex = 8; // 动作code解析开始位置
        private int resultStartIndex = 0; // 正式数据开始位置

        public Message()
        {
            this.resultStartIndex = this.requestCodeStartIndex + this.actionCodeStartIndex;
        }

        public byte[] Data
        {
            get { return data; }
        }
        public int StartIndex
        {
            get { return startIndex; }
        }
        // 获取剩余空间
        public int RemainSize
        {
            get
            {
                // 返回总长度减去已经存储的长度
                return data.Length - startIndex;
            }
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="newAmount">数据长度</param>
        /// <param name="processMessageCallBack">解析消息回调</param>
        public void ReadMessage(int newAmount, Action<RequestCode, ActionCode, string> processMessageCallBack)
        {
            // 更新数据存储位置
            startIndex += newAmount;
            while (true)
            {
                // 数据长度还不完整
                if (startIndex <= 4) { return; }
                // 开始解析数据, 解析出标识的数据长度
                int count = BitConverter.ToInt32(data, 0);
                // startIndex - 4得出除去标识数据长度，剩余的真实数据长度
                // 如果真实数据长度大于标识的长度，说明数据完整
                if (startIndex - 4 >= count)
                {
                    // 解析请求code
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, requestCodeStartIndex);
                    // 解析动作方法code
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, actionCodeStartIndex);
                    // 开始读取数据，从4开始代表0到3为标识数据长度所占用
                    // 开始读取数据，从4开始代表0到3为标识数据长度所占用
                    string result = Encoding.UTF8.GetString(data, this.resultStartIndex, count - actionCodeStartIndex);
                    processMessageCallBack(requestCode, actionCode, result);
                    // Console.WriteLine("解析出一条数据：" + result);
                    // 移动数据 count+4表示上面已经读取了， startIndex-4-count表示剩余长度
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= count + 4; // 读取后，更新存储数据长度
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 打包响应数据
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PickResponseData(ActionCode actionCode, string data)
        {
            // 转换方法code
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            // 转换数据
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            // 数据总长度
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            // 转换数据长度
            byte[] amountBytes = BitConverter.GetBytes(dataAmount);
            // 返回顺序，总长度 -> 请求code -> 数据
            return amountBytes.Concat(requestCodeBytes).Concat(dataBytes).ToArray();
        }
    }
}
