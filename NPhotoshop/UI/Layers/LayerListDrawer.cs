using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using NPhotoshop.Core;
using NPhotoshop.Core.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;

using Windows.UI;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;

namespace NPhotoshop.UI.Layers
{
    public class LayerListDrawer
    {
        List<LayerListElementDrawer> LDrawers;
        LayerController Target;
        ListView LayerContainer;
        public void Refresh()
        {
            for (int i = Target.LayerCount - 1; i >= 0; i--)
            {
                LDrawers[i].Refresh();
            }
        }
        public LayerListDrawer(LayerController target, ListView view)
        {
            Target = target;
            LayerContainer = view;
            LDrawers = new List<LayerListElementDrawer>();

            Target.OnAddLayer += (l, idx) =>
            {
                LDrawers.Add(CreateLayerElementDrawer(l));
                LayerContainer.UpdateLayout();
            };
            Target.OnSelectLayer += (l, idx) =>
            {
                LDrawers[idx].Set2ClickedState();
            };
            Target.OnUnSelectLayer += (l, idx) =>
            {
                LDrawers[idx].Set2UnClickedState();
            };
            Target.OnRemoveLayer += (l, idx) =>
            {
                LayerContainer.Items.RemoveAt(idx);
                LayerContainer.UpdateLayout();
                LDrawers.RemoveAt(idx);
            };
            Target.OnMoveLayer += (l, from, to) =>
            {
                LayerListElementDrawer temp = LDrawers[from];
                LDrawers.RemoveAt(from);
                LDrawers.Insert(to, temp);

                LayerContainer.DispatcherQueue.TryEnqueue(() =>
                {
                    var t1 = LayerContainer.Items[from];
                    LayerContainer.Items.RemoveAt(from);
                    LayerContainer.Items.Insert(to, t1);

                });
            };

            for (int i = 0; i < Target.LayerCount; i++)
            {
                LDrawers.Add(CreateLayerElementDrawer(target.GetLayer(i)));
                LayerContainer.UpdateLayout();
            }
        }

        LayerListElementDrawer CreateLayerElementDrawer(Layer l)
        {
            Grid g = new Grid();
            g.Margin = new Thickness(-20, 0, -20, 0);
            g.Height = 60;
            g.Width = 300;
            g.BorderThickness = new Thickness(1);
            g.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
            g.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            g.CanDrag = true;
            g.AllowDrop = true;
            // 드래그 시작 이벤트
            g.DragStarting += (s, e) =>
            {
                // 레이어 인덱스를 설정하여 드래그 데이터로 전달
                e.Data.SetText(Target.GetLayerIndex(l).ToString());
                e.Data.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move; // 이동 설정
            };

            // 드래그 중일 때 발생하는 이벤트 (시각적 힌트 제공)
            g.DragOver += (s, e) =>
            {
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
            };

            // 드롭 이벤트 처리
            g.Drop += (s, e) =>
            {
                //var deferral = e.GetDeferral();  // 비동기 작업을 위한 Deferral 생성

                // 드래그한 데이터에서 레이어 인덱스를 가져옴
                if (e.DataView.Contains(StandardDataFormats.Text))
                {
                    e.DataView.GetTextAsync().AsTask().ContinueWith(task =>
                    {
                        if (task.Result != null)
                        {
                            int sourceIndex = int.Parse(task.Result);
                            int targetIndex = Target.GetLayerIndex(l);

                            if (sourceIndex != targetIndex)
                            {
                                System.Diagnostics.Debug.WriteLine($"{sourceIndex} - {targetIndex}");
                                Target.MoveLayer(sourceIndex, targetIndex);
                            }
                        }
                    });
                }
            };
            //g.allow

            Button b = new Button();
            b.Margin = new Thickness(60, 0, 0, 0);
            b.Width = 300;
            b.Height = 60;
            b.FontSize = 15;
            b.HorizontalContentAlignment = HorizontalAlignment.Left;

            Button rb = new Button();
            rb.Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
            rb.Width = 35;
            rb.Height = 35;
            rb.Margin = new Thickness(0, 0, 10, 0);
            rb.HorizontalAlignment = HorizontalAlignment.Right;

            Image rbImage = new Image();
            rbImage.Margin = new Thickness(-10, 0, -10, 0);
            rbImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/EditorIcons/icon_trash_2_white.png"));
            rb.Content = rbImage;

            Microsoft.UI.Xaml.Controls.Image img = new Microsoft.UI.Xaml.Controls.Image();
            img.Width = 60;
            img.Height = 60;
            img.HorizontalAlignment = HorizontalAlignment.Left;

            g.Children.Add(b);
            g.Children.Add(img);
            g.Children.Add(rb);

            LayerContainer.Items.Add(g);    

            return new LayerListElementDrawer(g, b, img, rb, l, Target.SelectLayer, Target.RemoveLayer);
        }

    }
}
