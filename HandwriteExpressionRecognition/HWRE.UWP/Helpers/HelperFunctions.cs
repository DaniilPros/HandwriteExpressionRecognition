using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace HWRE.UWP.Helpers
{
    internal static class HelperFunctions
    {
        public static void UpdateCanvasSize(FrameworkElement root, FrameworkElement output, FrameworkElement inkCanvas)
        {
            output.Width = root.ActualWidth;
            output.Height = root.ActualHeight / 2;
            inkCanvas.Width = root.ActualWidth;
            inkCanvas.Height = root.ActualHeight / 2;
        }
    }
}
