using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Media.Imaging;
using NPhotoshop.Core.Image;
using NPhotoshop.Core.Layers;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace NPhotoshop.Core
{
    public class NPsd
    {
        //public List<Layer> Layers;
        public LayerController ThisLayerController { get; private set; }
        public int Width;
        public int Height;


        public bool IsChanged
        {
            get
            {
                if(ThisLayerController.IsChanged == true)
                {
                    return true;
                }
                return false;
            }
        }

        public NBitmap ThisBitmap { get; private set; }

        public NPsd(int width, int height)
        {
            Width = width;
            Height = height;
            ThisLayerController = new LayerController(width, height);
            ThisBitmap = new NBitmap(Width, Height);
        }


        public NBitmap Render()
        {
            return ThisLayerController.Render();
        }
    }
}