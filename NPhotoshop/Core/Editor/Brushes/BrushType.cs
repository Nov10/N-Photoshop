using NPhotoshop.Core.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Editor.Brushes
{
    public abstract class BrushType
    {
        public int Radius;
        public float Smoothness;
        public Color Color;

        protected BrushType(int radius, float smoothness, Color c)
        {
            Radius = radius;
            Smoothness = smoothness;
            Color = c;  
        }

        public abstract void GetBrushTargetPixel(int centerX, int centerY, Layers.Layer target, System.Action<int, int> loop);
    }
}
