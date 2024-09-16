using NPhotoshop.Core.Image;
using NPhotoshop.Core.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace NPhotoshop.Core.Editor.Brushes
{
    public class SolidCircleBrushType : BrushType
    {
        public SolidCircleBrushType(int radius, float smoothness, Color c) : base(radius, smoothness, c)
        {
        }

        public override void GetBrushTargetPixel(int centerX, int centerY, Layers.Layer target, Action<int, int> loop)
        {
            int y_down = (int)MathF.Max(0, centerY - Radius);
            int y_up = (int)MathF.Min(target.Height, centerY + Radius);


            int x_left = (int)MathF.Max(0, centerX - Radius);
            int x_right = (int)MathF.Min(target.Width, centerX + Radius);

            // 원 범위 내의 픽셀들에 접근하여 색을 설정
            for (int p_y = y_down; p_y <= y_up; p_y++)
            //System.Threading.Tasks.Parallel.For(y_down, y_up, (p_y) =>
            {
                for (int p_x = x_left; p_x <= x_right; p_x++)
                {
                    // 해당 픽셀이 원 안에 있는지 확인
                    int dx = p_x - centerX;
                    int dy = p_y - centerY;
                    if (dx * dx + dy * dy <= Radius * Radius)
                    {
                        loop(p_x, p_y);
                    }
                }
            }
        }

     
    }
}
