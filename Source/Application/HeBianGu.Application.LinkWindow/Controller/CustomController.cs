﻿using HeBianGu.Base.WpfBase;
using HeBianGu.General.WpfControlLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeBianGu.Application.LinkWindow
{
    [Controller("Custom")]
    class CustomController : Controller
    {
        public async Task<IActionResult> Custom()
        {
            return await ViewAsync();
        }
    }
}
