﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

namespace GroomWise.Services.Converter;

[ValueConversion(typeof(DateTime), typeof(string))]
public class DateTimeToMonthConverter : IValueConverter
{
    public static DateTimeToMonthConverter Instance = new DateTimeToMonthConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = value as DateTime? ?? default;
        return date.ToString("MMMM yyyy");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
