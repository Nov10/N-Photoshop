using NPhotoshop.Core.Layers;
using NPhotoshop.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Editor
{
    public abstract class Tool
    {
        public abstract bool Apply(Vector2Int preMousePoint, Vector2Int nowMousePoint, Layer target);
    }
}
