using System.Windows;

namespace DLExt.OutlookAddin.Converters
{
    public class BoolToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BoolToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
