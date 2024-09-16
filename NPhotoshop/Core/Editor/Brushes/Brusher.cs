using NPhotoshop.Core.Image;
using NPhotoshop.Core.Layers;
using NPhotoshop.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Editor.Brushes
{
    public class Brusher : Tool
    {
        public BrushType NowType;
        public void SetColor(Color c)
        {
            NowType.Color = c;  
        }

        public void SetBrushType(BrushType type)
        {
            NowType = type;
        }

        public void LoopForPixel(int x, int y, Layers.Layer target, System.Action<int, int> loop)
        {
            NowType.GetBrushTargetPixel(x, y, target, loop);
        }

        public void LinearInterpolationLoop(int x0, int y0, int x1, int y1, Layers.Layer target, System.Action<int, int> loop)
        {
            int dx = (int)MathF.Abs(x1 - x0);
            int dy = (int)MathF.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                if (x0 >= 0 && x0 < target.Width && y0 >= 0 && y0 < target.Height)
                {
                    //int index = (y0 * width + x0) * 4;

                    NowType.GetBrushTargetPixel(x0, y0, target, loop);
                }

                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = err * 2;

                if (e2 > -dy) {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx) {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public override bool Apply(Vector2Int preMousePoint, Vector2Int nowMousePoint, Layer target)
        {
            NormalLayer t = (NormalLayer)target;
            if (t == null)
                return false;

            LinearInterpolationLoop(preMousePoint.x, preMousePoint.y, nowMousePoint.x, nowMousePoint.y, target, (x, y) =>
            {
                t.SetPixel(x, y, NowType.Color);
                //((NormalLayer)psd.ThisLayerController.GetNowSelectedLayer()).SetPixel(x, y, new NPhotoshop.Core.Image.Color(MainColorPicker.Color.R, MainColorPicker.Color.G, MainColorPicker.Color.B, MainColorPicker.Color.A));
            });
            return true;
        }
    }
}
