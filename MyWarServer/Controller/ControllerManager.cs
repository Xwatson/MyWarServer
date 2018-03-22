﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;

// 管理服务器端controller
namespace MyWarServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();

        public ControllerManager()
        {
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            // 向字典添加默认controller
            DefaultController defaultController = new DefaultController();
            controllerDic.Add(defaultController.RequestCode, defaultController);
        }
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="requestCode">请求</param>
        /// <param name="actionCode">动作</param>
        public void RequestHandle(RequestCode requestCode, ActionCode actionCode, string data)
        {
            BaseController baseController;
            // 获取controller
            bool isController = controllerDic.TryGetValue(requestCode, out baseController);
            if (!isController)
            {
                Console.WriteLine("[错误]：未能找到[" + Enum.GetName(typeof(RequestCode), requestCode) + "]对应的controller，无法处理请求。");
                return;
            }
            // 获取方法名
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            // 反射获取到方法信息
            MethodInfo mi = baseController.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[错误]：未能在Controller[" + baseController.GetType() + "]中没有找到[" + methodName + "]方法。");
                return;
            }
            object[] datas = new object[] { data };
            // 调用方法
            object result = mi.Invoke(baseController, datas);
        }
    }
}