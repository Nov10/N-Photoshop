using NPhotoshop.Core.Image;
using NPhotoshop.Core.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Layers
{
    public class NormalLayer : Layer
    {
        NBitmap Data;

        public void SetImage(NBitmap image)
        {
            Data = image;
            OnModify();
        }
        public void SetPixel(int x, int y, Color c)
        {
            Data.SetPixel(x, y, c);
            OnModify();
        }

        public NormalLayer(int x, int y) : base(x, y)
        {
            Data = new NBitmap(x, y);
        }

        public override NBitmap Render()
        {
            return Data;
        }
    }
}
