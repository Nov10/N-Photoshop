using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Image
{
    public static class ColorMixer
    {
        public static Color LerpByAlpha(Color back, Color fore)
        {
            return new Color(
                LerpByAlpha(back.R, fore.R, fore.A),
                LerpByAlpha(back.G, fore.G, fore.A),
                LerpByAlpha(back.B, fore.B, fore.A),
                (byte)((back.A + fore.A)));
        }

        static byte LerpByAlpha(byte back, byte fore, byte alpha)
        {
            float a = (alpha) / 255.0f;
            return (byte)( (1-a)*back + a*fore);
        }
    }
}
