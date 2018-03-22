using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace MyWarServer.Controller
{
    abstract class BaseController
    {
        RequestCode requestCode = RequestCode.None;
        /// <summary>
        /// 默认requestCode
        /// </summary>
        public RequestCode RequestCode
        {
            get
            {
                return requestCode;
            }
        }
        /// <summary>
        /// 当ActionCode未指定的默认处理方法
        /// </summary>
        /// <param name="data">客户端发送的数据</param>
        /// <returns>默认方法不返回数据</returns>
        public virtual string DefaultHandle(string data)
        {
            return null;
        }
    }
}
