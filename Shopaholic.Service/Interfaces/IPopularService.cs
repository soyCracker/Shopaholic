﻿using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IPopularService
    {
        List<ProductTopDTO> GetProductByMonthFlowTop();

        List<ProductTopDTO> GetProductByMonthOrderTop();
    }
}
