﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Image
{
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}
