﻿// Copyright © 2022 By HeBianGu(QQ:908293466) https://github.com/HeBianGu/WPF-ControlBase

using System;
using System.IO;
using System.Windows.Input;

namespace HeBianGu.Base.WpfBase
{
    public class ProcessCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            if (parameter == null) return false;

            bool result = File.Exists(parameter?.ToString()) || Directory.Exists(parameter?.ToString());

            if (parameter.ToString().ToUpper().StartsWith("http") == true) return true;

            return result;
        }

        public void Execute(object parameter)
        {
            System.Diagnostics.Process.Start(parameter?.ToString());
        }


        public event EventHandler CanExecuteChanged;
    }
}