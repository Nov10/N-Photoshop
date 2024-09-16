
using NPhotoshop.Core.Image;
using NPhotoshop.Maths;

namespace NPhotoshop.Core.Layers
{
    public abstract class Layer
    {
        public string Name;

        protected NBitmap CachedBitmap;
        public bool IsUpdatedCahceBitmap;
        public bool IsChangeApplied;

        public int Width { get { return CachedBitmap.Width; } }
        public int Height { get { return CachedBitmap.Height; } }

        public Layer(int x, int y)
        {
            CachedBitmap = new NBitmap(x, y);
            IsUpdatedCahceBitmap = false;
        }
        
        public void OnModify()
        {
            IsUpdatedCahceBitmap = false;
            IsChangeApplied = false;
        }

        public abstract NBitmap Render();

        public NBitmap CachedRender()
        {
            if(IsUpdatedCahceBitmap == false)
            {
                CachedBitmap = Render();
                IsUpdatedCahceBitmap = true;
            }
            return CachedBitmap;
        }
    }
}