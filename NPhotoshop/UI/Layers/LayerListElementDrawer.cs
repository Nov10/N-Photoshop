using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using NPhotoshop.Core.Image;
using NPhotoshop.Core.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace NPhotoshop.UI.Layers
{
    public class LayerListElementDrawer
    {
        Grid Backgroudn;
        Button Btn;
        Button RemoveBtn;
        Image Img;

        Layer Target;

        public void Refresh()
        {
            Btn.Content = Target.Name;
            Img.Source = Target.CachedRender().ConvertToBitmap();
        }

        public LayerListElementDrawer(Grid backgroudn, Button btn, Image img, Button removeBtn, Layer layer, Action<Layer> onClick, Action<Layer> onRemoveClick)
        {
            Backgroudn = backgroudn;
            Btn = btn;
            Btn.Content = layer.Name;
            Img = img;
            RemoveBtn = removeBtn;
            Target = layer; 
            Refresh();

            Btn.Click += (s, e) =>
            {
                onClick(Target);
            };

            removeBtn.Click += (s, e) =>
            {
                onRemoveClick(Target);
            };
        }

        public void Set2ClickedState()
        {
            Backgroudn.BorderBrush = new SolidColorBrush(Colors.White);
            Backgroudn.BorderThickness = new Microsoft.UI.Xaml.Thickness(4);
        }
        public void Set2UnClickedState()
        {
            Backgroudn.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            Backgroudn.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
        }
    }
}
