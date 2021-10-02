﻿using HeBianGu.Base.WpfBase;
using HeBianGu.General.WpfControlLib;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HeBianGu.Application.ChartWindow
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : ApplicationBase
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShellWindow shellWindow = new ShellWindow();

            StartWindow startWindow = new StartWindow();

            Task.Run(() =>
            {
                Thread.Sleep(3000);

                this.Dispatcher.Invoke(() =>
                {
                    startWindow.Close();
                });
            });

            startWindow.ShowDialog();

            shellWindow.Show(); 

            Debug.WriteLine(string.Join(",", Enumerable.Range(1, 300).ToArray()));

            Random random = new Random();

            Trace.WriteLine(string.Join(",", Enumerable.Range(1, 300).Select(l => random.Next(1, 100)).ToArray()));

            Debug.WriteLine(string.Join(",", Enumerable.Range(1, 360).ToArray()));

            Debug.WriteLine(string.Join(",", Enumerable.Range(1, 360).Select(l => Math.Round(Math.Sin(Convert.ToDouble(l.ToString()) / 4) * 5.0 + 5, 2)).ToArray()));

            Debug.WriteLine(string.Join(",", Enumerable.Range(1, 360).Select(l => Math.Round(Math.Sin(Convert.ToDouble(l.ToString()) / 10) * 5.0 + 5, 2)).ToArray()));


            Debug.WriteLine(string.Join(",", Enumerable.Range(1, 360).Select(l => l * 3).ToArray()));

            Debug.WriteLine(string.Join(",", Enumerable.Range(1, 360).Select(l => Math.Round((10.0 / 360.0) * l, 2)).ToArray()));


            int[] sss = { 1, 2, 3, 4, 5, 6 };

            int[] sss1 = { 1};

           var sssss=  sss1.Zip(sss, (l, k) =>
             {
                 return l;
             }).ToList();

        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            //  Do：注册Mvc模式
            services.UseMvc();

            //  Do ：注册本地化配置读写服务
            services.AddSingleton<IThemeLocalizeService, LocalizeService>();

            //  Do ：注入领域模型服务
            services.AddSingleton<IAssemblyDomain, AssemblyDomain>();

            ////  Do ：注册日志服务
            //services.AddSingleton<ILogService, AssemblyDomain>();

        }

        protected override void Configure(IApplicationBuilder app)
        {
            //  Do：设置默认主题
            app.UseLocalTheme(l =>
            {
                l.AccentColor = (Color)ColorConverter.ConvertFromString("#FF003D99");
                //l.ForegroundColor = (Color)ColorConverter.ConvertFromString("#727272");

                l.SmallFontSize = 15D;
                l.LargeFontSize = 18D;
                l.FontSize = FontSize.Small;

                l.ItemHeight = 35;
                //l.ItemWidth = 120;
                l.ItemCornerRadius = 5;

                l.AnimalSpeed = 5000;

                l.AccentColorSelectType = 0;

                l.IsUseAnimal = false;

                l.ThemeType = ThemeType.Light;

                l.Language = Language.Chinese;

                l.AccentBrushType = AccentBrushType.LinearGradientBrush;
            });
        }

    }
}
