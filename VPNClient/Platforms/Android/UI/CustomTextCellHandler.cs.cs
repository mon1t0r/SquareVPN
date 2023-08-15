using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using VPNClient.Pages.Elements;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace VPNClient.Platforms.Android.UI
{
    public class CustomTextCellHandler : Microsoft.Maui.Controls.Handlers.Compatibility.TextCellRenderer
    {
        private AView pCellCore;
        private bool pSelected;
        private Drawable pUnselectedBackground;

        protected override AView GetCellCore(Cell item, AView convertView, AViewGroup parent, AContext context)
        {
            pCellCore = base.GetCellCore(item, convertView, parent, context);

            pSelected = true;

            return pCellCore;
        }

        protected override void OnCellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnCellPropertyChanged(sender, e);

            if (e.PropertyName == "IsSelected")
            {
                if (!pSelected)
                    pUnselectedBackground = pCellCore.Background;

                pSelected = !pSelected;
                if (pSelected)
                    pCellCore.SetBackgroundColor(((CustomTextCell)sender).SelectedBackgroundColor.ToAndroid());
                else
                    pCellCore.SetBackground(pUnselectedBackground);
            }
        }
    }
}
