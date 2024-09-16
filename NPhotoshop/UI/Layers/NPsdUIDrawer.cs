using Microsoft.UI.Xaml.Controls;
using NPhotoshop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPhotoshop.UI.Layers
{
    public class NPsdUIDrawer
    {
        NPsd Target;
        public NPsdUIDrawer(NPsd psd, ListView LayerContainer)
        {
            Target = psd;
            LayerDrawer = new LayerListDrawer(Target.ThisLayerController, LayerContainer);
        }
        LayerListDrawer LayerDrawer;

        public void Refresh()
        {
            LayerDrawer.Refresh();
        }
    }
}
