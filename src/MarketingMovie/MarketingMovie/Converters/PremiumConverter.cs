using System;
using System.Globalization;
using Xamarin.Forms;
namespace MarketingMovie
{
    public class PremiumConverter : IValueConverter
    {
        public PremiumConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPremium = (bool)value;

            return isPremium ? Color.Red : Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
