﻿// Copyright © 2022 By HeBianGu(QQ:908293466) https://github.com/HeBianGu/WPF-ControlBase

using HeBianGu.Base.WpfBase;
using HeBianGu.Control.Guide;
using HeBianGu.Service.Mvp;

namespace System
{
    public static class PropertyGridExtention
    {
        ///// <summary>
        ///// 注册
        ///// </summary>
        ///// <param name="service"></param>
        //public static void AddPropertyGrid(this IServiceCollection service)
        //{
        //    service.AddSingleton<IService, Service>();
        //}

        ///// <summary>
        ///// 配置
        ///// </summary>
        ///// <param name="service"></param>
        //public static void UsePropertyGrid(this IApplicationBuilder service, Action<Setting> action)
        //{
        //    action?.Invoke(Setting.Instance);
        //}

        /// <summary>
        /// 设置显示新手向导
        /// </summary>  
        public static IServiceCollection AddGuideViewPresenter(this IServiceCollection services)
        {
            services.AddSingleton<IGuideViewPresenter, GuideViewPresenter>();

            return services;
        }
    }


}
