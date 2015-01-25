using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChessProjectFinal.Converters
{
    public class HideOnNullConverter : IValueConverter
    {
        /// <summary>
        /// Converts to the target value
        /// </summary>
        /// <param name="value">The original value</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The parameter given</param>
        /// <param name="culture">The current culture information for localization</param>
        /// <returns>A converted value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility convertedValue;
            if (value is string)
            {
                convertedValue = !string.IsNullOrWhiteSpace((string)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is IList)
            {
                convertedValue = (value as IList).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is ICollection)
            {
                convertedValue = (value as ICollection).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is ICollectionView)
            {
                convertedValue = !(value as ICollectionView).IsEmpty ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                convertedValue = value != null ? Visibility.Visible : Visibility.Collapsed;
            }

            return convertedValue;
        }

        /// <summary>
        /// Converts back to the original value
        /// </summary>
        /// <param name="value">The converted value</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The parameter given</param>
        /// <param name="culture">The current culture information for localization</param>
        /// <returns>The original object</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
