﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.NPattern
{
    public interface IClone<T>
    {
        T Clone();
    }
}