﻿using HeBianGu.Base.WpfBase;
using HeBianGu.Control.ThemeSet;
using HeBianGu.General.WpfControlLib;
using System;
using System.Windows;
using System.Windows.Media;

namespace HeBianGu.App.Above
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : ApplicationBase
    {
        //  Do ：配置要启动的主窗口
        protected override System.Windows.Window CreateMainWindow(StartupEventArgs e)
        {
            return new ShellWindow();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            //  Do ：启用窗口加载和隐藏动画 需要引用 HeBianGu.Service.Animation
            services.AddWindowAnimation();

            //  Do ：启用消息提示 需要引用 HeBianGu.Control.Message
            services.AddMessage();

            //  Do ：启用对话框 需要引用HeBianGu.Window.MessageDialog
            services.AddMessageDialog();

            //  Do ：启用和显示右上角主题设置
            services.AddTheme();

            //  Do ：启用右上见配置按钮 需要添加引用HeBianGu.Systems.Setting
            services.AddSetting();
            services.AddSettingViewPrenter();

            //  Do ：注入领域模型服务
            services.AddSingleton<IAssemblyDomain, AssemblyDomain>();

            services.AddMvc();

        }

        protected override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            //  Do：应用Mvc 需要引用HeBianGu.Service.Mvc
            app.UseMvc();

            //  Do：设置默认主题
            app.UseLocalTheme(l =>
            {
                l.AccentColor = (Color)ColorConverter.ConvertFromString("#FF0093FF");

                l.SmallFontSize = 15D;
                l.LargeFontSize = 18D;
                l.FontSize = FontSize.Small;

                l.ItemHeight = 35;
                //l.ItemWidth = 120;
                l.ItemCornerRadius = 2;

                l.AnimalSpeed = 5000;

                l.AccentColorSelectType = 0;

                l.IsUseAnimal = false;

                l.ThemeType = ThemeType.Light;

                l.Language = Language.Chinese;

                l.AccentBrushType = AccentBrushType.RadialGradientBrushReverse;
            });
        }

    }

}
