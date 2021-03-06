﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IMixer
    {
        void Mix(Individual[] ind1, Individual[] ind2);

        Individual[] Individuals1 { get; }
        Individual[] Individuals2 { get; }
    }
}
