
using System.Globalization;

namespace LifeManagementApp.Converters;

public class BoolToTextConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is bool isDone)
        {
            return isDone ? "Completed" : "Pending";
        }

        return "Pending";
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException(
            "This converter supports one-way binding only.");
    }
}
