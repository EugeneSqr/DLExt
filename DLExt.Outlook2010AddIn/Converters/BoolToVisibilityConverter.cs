using System.Windows;

namespace DLExt.Outlook2010AddIn.Converters
{
    public class BoolToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BoolToVisibilityConverter()
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
