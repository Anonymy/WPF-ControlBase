﻿using HeBianGu.Service.Mvc;
using System;

namespace HeBianGu.App.Chart
{
    [ViewModel("K")]
    internal class KViewModel : MvcViewModelBase
    {
        protected override void Init()
        {

        }

        private Random random = new Random();

        protected override void Loaded(string args)
        {

        }

        /// <summary> 命令通用方法 </summary>
        protected override async void RelayMethod(object obj)

        {
            string command = obj?.ToString();

            //  Do：对话消息
            if (command == "Sumit")
            {

            }

            //  Do：等待消息
            else if (command == "Cancel")
            {

            }
        }

    }
}
