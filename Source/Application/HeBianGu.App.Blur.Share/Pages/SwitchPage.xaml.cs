﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HeBianGu.Base.WpfBase;

namespace HeBianGu.Applications.ControlBase.Demo.Pages
{
    /// <summary>
    /// SwitchPage.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchPage : Page
    {
        public SwitchPage()
        {
            InitializeComponent();
        }
    }



    public class SwitchViewModel : NotifyPropertyChanged
    {

        private object _currentContent;
        /// <summary> 说明  </summary>
        public object CurrentContent
        {
            get { return _currentContent; }
            set
            {
                _currentContent = value;
                RaisePropertyChanged("CurrentContent");
            }
        }

        List<UIElement> controls = new List<UIElement>();

        int value = 1;

        protected override void RelayMethod(object obj)
        {
            string command = obj.ToString();

            //  Do：应用
            if (command == "init")
            {



            }
            //  Do：取消
            else if (command == "GroupExpander.SelectChanged")
            {


            }

        }

        public RelayCommand<string> LoadedCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(Loaded, CanLoaded)).Value;


        protected virtual bool CanLoaded(string args)
        {
            return true;
        }

        protected void Loaded(string args)
        {
            SolidColorBrush[] solids = new SolidColorBrush[] {
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Green),
                new SolidColorBrush(Colors.Yellow),
                new SolidColorBrush(Colors.Purple),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.DarkBlue),
                new SolidColorBrush(Colors.DarkGray),
                new SolidColorBrush(Colors.DarkGreen),
                new SolidColorBrush(Colors.Orange),
            };

            Random random = new Random();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Grid grid = new Grid();
                    grid.Width = 1000;
                    grid.Height = 1000;
                    grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    grid.VerticalAlignment = VerticalAlignment.Stretch;
                    grid.Background = solids[i % 9];
                    controls.Add(grid);
                }
            });

        }

        public RelayCommand<string> SelectChangedCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(SelectChanged, CanSelectChanged)).Value;

        private void SelectChanged(string args)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                value = value >= controls.Count ? 1 : value;

                this.CurrentContent = controls[value - 1];
                value++;
            });

        }


        private bool CanSelectChanged(string args)
        {
            return true;
        }
    }
}
