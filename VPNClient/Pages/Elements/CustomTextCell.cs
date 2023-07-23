namespace VPNClient.Pages.Elements
{
    public class CustomTextCell : TextCell
    {

        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(
            nameof(SelectedBackgroundColor), typeof(Color), typeof(CustomTextCell), Colors.White
        );

        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }

        public CustomTextCell()
        {

        }
    }
}
