using NPhotoshop.Core.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.Core.Layers
{
    public class LayerController
    {
        public int NowSelctedLayer { get; private set; }
        public int LayerCount { get { return Layers.Count; } }
        public LayerController(int wid, int hei)
        {
            Layers = new List<Layer>();
            RenderTarget = new NBitmap(wid, hei);
        }

        public void MoveLayer(int sourceIndex, int targetIndex)
        {
            if (sourceIndex < 0 || targetIndex < 0 || sourceIndex >= Layers.Count || targetIndex >= Layers.Count)
                return;

            // 레이어를 리스트에서 이동
            Layer temp = Layers[sourceIndex];
            if (targetIndex == NowSelctedLayer)
                OnUnSelectLayer?.Invoke(Layers[targetIndex], targetIndex);
            Layers.RemoveAt(sourceIndex);
            Layers.Insert(targetIndex, temp);

            OnMoveLayer?.Invoke(temp, sourceIndex, targetIndex); //UI처리 이후 재선택
            if(targetIndex == NowSelctedLayer)
            {
                SelectLayer(targetIndex);
            }
            Layers[targetIndex].OnModify();

        }

        public Layer GetNowSelectedLayer()
        {
            return GetLayer(NowSelctedLayer);
        }

        public Layer GetLayer(int index)
        {
            return Layers[index];
        }

        public int GetLayerIndex(Layer l)
        {
            return Layers.IndexOf(l);
        }

        List<Layer> Layers;
        public NBitmap RenderTarget { get; private set; }

        public void AddLayer(Layer layer)
        {
            Layers.Add(layer);
            OnAddLayer?.Invoke(layer, Layers.Count - 1);
            Layers[Layers.Count - 1].OnModify();
        }

        public void SelectLayer(int index)
        {
            SelectLayer(Layers[(index)]);
        }
        public void SelectLayer(Layer target)
        {
            if (Layers.Contains(target) == false)
                return;
            int index = Layers.IndexOf(target);
            OnUnSelectLayer?.Invoke(target, NowSelctedLayer);
            NowSelctedLayer = index;
            OnSelectLayer?.Invoke(target, NowSelctedLayer);
        }
        public void RemoveLayer(Layer target)
        {
            if (Layers.Contains(target) == false)
                return;
            int index = Layers.IndexOf(target);

            OnRemoveLayer?.Invoke(target, index);
            Layers.RemoveAt(index);

            if (NowSelctedLayer >= Layers.Count && Layers.Count >= 1)
            {
                NowSelctedLayer = Layers.Count - 1;
                SelectLayer(Layers[Layers.Count - 1]);
                Layers[Layers.Count - 1].OnModify();
            }
        }
        public System.Action<Layer, int> OnSelectLayer;
        public System.Action<Layer, int> OnUnSelectLayer;
        public System.Action<Layer, int> OnAddLayer;
        public System.Action<Layer, int> OnRemoveLayer;
        public System.Action<Layer, int, int> OnMoveLayer;

        public bool IsChanged
        {
            get
            {
                for (int i = 0; i < Layers.Count; i++)
                {
                    if (Layers[i].IsChangeApplied == false)
                        return true;
                }
                return false;
            }
        }

        public NBitmap Render()
        {
            RenderTarget.Clear();
            for (int i = Layers.Count - 1; i >= 0; i--)
            {
                Layers[i].IsChangeApplied = true;
                RenderTarget = LerpByAlpha(RenderTarget, Layers[i].Render());
            }
            return RenderTarget;
        }

        NBitmap LerpByAlpha(NBitmap back, NBitmap fore)
        {
            NBitmap result = new NBitmap(back.Width, back.Height);

            result.LoopForPixel((x, y) =>
            {
                result.SetPixel(x, y, ColorMixer.LerpByAlpha(back.GetPixel(x, y), fore.GetPixel(x, y)));
            });

            return result;
        }
    }
}
