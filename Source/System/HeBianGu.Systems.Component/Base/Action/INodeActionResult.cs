﻿// Copyright © 2022 By HeBianGu(QQ:908293466) https://github.com/HeBianGu/WPF-ControlBase

using System.Collections.Generic;

namespace HeBianGu.Systems.Component
{
    public interface INodeActionResult : IActionResult
    {
        List<IComponentPort> Ports { get; set; }
    }
}
