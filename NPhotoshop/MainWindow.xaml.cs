using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using NPhotoshop.Core;
using NPhotoshop.Core.Editor;
using NPhotoshop.Core.Editor.Brushes;
using NPhotoshop.Core.Image;
using NPhotoshop.Core.Layers;
using NPhotoshop.Maths;
using NPhotoshop.UI.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using System.Timers;
using Windows.Devices.Radios;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NPhotoshop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static int GetDropIndex(ListView listView, DragEventArgs e)
        {
            var position = e.GetPosition(listView);

            foreach (var item in listView.Items)
            {
                var container = listView.ContainerFromItem(item) as ListViewItem;
                if (container != null)
                {
                    var bounds = container.TransformToVisual(listView).TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));

                    // Check to see if the bounds of the item's container intersects the drop point
                    if (bounds.Top > position.Y)
                    {
                        // Get the list view to tell us the index of the hit container
                        return listView.IndexFromContainer(container);
                    }
                }
            }

            // The drop occurred after the last item in the list
            return listView.Items.Count;
        }
        public void SwapLayer(int a, int b)
        {
            //var l1 = LayerContainer.Items[a];
            //var l2 = LayerContainer.Items[b];

            //LayerContainer.Items[b] = l1;
            //LayerContainer.Items[a] = l2;

            //LayerContainer.UpdateLayout();

            //var pl1 = psd.Layers[a];
            //var pl2 = psd.Layers[b];
            //psd.Layers[b] = pl1;
            //psd.Layers[a] = pl2;

            //var ld1 = LDrawers[a];
            //var ld2 = LDrawers[b];
            //LDrawers[b] = ld1;
            //LDrawers[a] = ld2;
        }

        Brusher SolidColorBrush;
        Brusher Eraser;

        NPsdUIDrawer UIDrawer;
        public MainWindow()
        {
            this.InitializeComponent();

            psd = new NPsd(600, 600);

            SolidColorBrush = new Brusher();
            SolidColorBrush.SetBrushType(new SolidCircleBrushType(5, 0, new Core.Image.Color(10, 10, 10, 255)));
            Eraser = new Brusher();
            Eraser.SetBrushType(new SolidCircleBrushType(5, 0, new Core.Image.Color(0, 0, 0, 0)));

            NormalLayer l = new NormalLayer(psd.Width, psd.Height);
            l.SetImage(NBitmap.CreateWhiteNBitmap(psd.Width, psd.Height));
            l.Name = "새 레이어 1";


            NormalLayer l2 = new NormalLayer(psd.Width, psd.Height);
            l2.Name = "새 레이어 2";
            psd.ThisLayerController.AddLayer(l2);
            psd.ThisLayerController.AddLayer(l);

            NPsdDrawer.Width = psd.Width;
            NPsdDrawer.Height = psd.Height;
            bmap = new WriteableBitmap(psd.Width, psd.Height);

            UIDrawer = new NPsdUIDrawer(psd, LayerContainer);

            RefreshImage();

            t.Interval = TimeSpan.FromMilliseconds(10);
            t.Tick += T_Tick1;
            t.Start();
            psd.ThisLayerController.SelectLayer(l);

            BrushSelectButton_Click(null, null);
        }

        private void G_Drop(object sender, DragEventArgs e)
        {
        }

        private void T_Tick1(object sender, object e)
        {
            RefreshImage();

            UIDrawer.Refresh();
        }

        WriteableBitmap bmap;
        NPsd psd;

        void RefreshImage(bool force = false)
        {
            if(psd.IsChanged == true || force == true)
            {
                NPsdDrawer.Source = ConvertToBitmap(psd.Render());
            }
        }
        public WriteableBitmap ConvertToBitmap(NBitmap image)
        {
            WriteableBitmap b = new WriteableBitmap(image.Width, image.Height);
            // Bitmap 버퍼에 접근
            using (var stream = b.PixelBuffer.AsStream())
            {
                // 버퍼는 BGRA 순서로 데이터를 가짐
                byte[] pixelData = new byte[image.Width * image.Height * 4];

                for (int i = 0; i < image.Pixels.Length; i++)
                {
                    pixelData[i * 4] = image.Pixels[i].B; // Blue
                    pixelData[i * 4 + 1] = image.Pixels[i].G; // Green
                    pixelData[i * 4 + 2] = image.Pixels[i].R; // Red
                    pixelData[i * 4 + 3] = image.Pixels[i].A; // Alpha
                }

                stream.Write(pixelData, 0, pixelData.Length);
            }
            return b;
        }
        DispatcherTimer t = new DispatcherTimer();
        private System.Timers.Timer aTimer;
        private readonly double cycleTime = 1000; // 1초

        bool IsNewInput = false;

        Tool NowSelectedTool;

        private void NPsdDrawer_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(NPsdDrawer).Properties.IsLeftButtonPressed == false)
            {
                IsNewInput = true;
                return;
            }


            double e_x = e.GetCurrentPoint(NPsdDrawer).Position.X;
            double e_y = e.GetCurrentPoint(NPsdDrawer).Position.Y;

            int p_x = Convert2PixelCoord(e_x, psd.Width);
            int p_y = Convert2PixelCoord(e_y, psd.Height);
            if(IsNewInput == true)
            {
                prev_x = p_x;
                prev_y = p_y;
                IsNewInput = false;
            }

            NowSelectedTool.Apply(new Math.Vector2Int(prev_x, prev_y), new Math.Vector2Int(p_x, p_y), psd.ThisLayerController.GetNowSelectedLayer());
            //b.LinearInterpolationLoop(prev_x, prev_y, p_x, p_y, psd.ThisLayerController.GetNowSelectedLayer(), (x, y) =>
            //{
            //    ((NormalLayer)psd.ThisLayerController.GetNowSelectedLayer()).SetPixel(x, y, new NPhotoshop.Core.Image.Color(MainColorPicker.Color.R, MainColorPicker.Color.G, MainColorPicker.Color.B, MainColorPicker.Color.A));
            //});
            prev_x = p_x;
            prev_y = p_y;
        }

        int prev_x;
        int prev_y;

        int Convert2PixelCoord(double x, int r)
        {
            int p = (int)x;
            if(0 <= p && p <= r)
            {
                return p;
            }
            return 0;
        }

        Vector2 ViewScale = new Vector2(1, 1);

        private void BackgroundColorGrid_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            int delta = e.GetCurrentPoint(NPsdDrawer).Properties.MouseWheelDelta;
            if (delta > 0)
            {
                ViewScale += new Vector2(1, 1) * 0.1f;
            }
            else if (delta < 0)
            {
                ViewScale -= new Vector2(1, 1) * 0.1f;
            }
        }

        private void Button_Click_AddNewLayer(object sender, RoutedEventArgs e)
        {
            NormalLayer l = new NormalLayer(psd.Width, psd.Height);
            l.Name = "새 레이어";

            psd.ThisLayerController.AddLayer(l);
            psd.ThisLayerController.MoveLayer(psd.ThisLayerController.LayerCount - 1, psd.ThisLayerController.NowSelctedLayer);
            RefreshImage();
        }

        private void EraserSelectButton_Click(object sender, RoutedEventArgs e)
        {
            EraserSelectButton.BorderThickness = new Thickness(2);
            BrushSelectButton.BorderThickness = new Thickness(0);
            NowSelectedTool = Eraser;
        }

        private void BrushSelectButton_Click(object sender, RoutedEventArgs e)
        {

            EraserSelectButton.BorderThickness = new Thickness(0);
            BrushSelectButton.BorderThickness = new Thickness(2);
            NowSelectedTool = SolidColorBrush;
        }

        private void MainColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            SolidColorBrush.SetColor(new NPhotoshop.Core.Image.Color(MainColorPicker.Color.R, MainColorPicker.Color.G, MainColorPicker.Color.B, MainColorPicker.Color.A));
        }
    }
}
