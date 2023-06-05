﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

namespace GroomWise.Services.App;

public partial class ThemeManagerService : ObservableObject, IThemeManagerService
{
    private readonly ILogger _logger;
    private readonly IConfigProvider _configProvider;

    [ObservableProperty]
    private bool _darkMode;

    [ObservableProperty]
    private string? _colorTheme;

    public ThemeManagerService(ILogger logger, IConfigProvider configProvider)
    {
        _logger = logger;
        _configProvider = configProvider;
        UseDarkTheme(_configProvider.DarkMode);
        UseColorTheme(_configProvider.ColorTheme);
    }

    public void UseDarkTheme(bool night)
    {
        Task.Run(async () =>
        {
            SaveBaseTheme(night);
            await Application.Current.Dispatcher.InvokeAsync(
                () => ThemeManager.Instance.SetBaseTheme(night ? "Dark" : "Light")
            );
            _logger.Log(this, $"Set dark mode {night}");
            DarkMode = night;
        });
    }

    public void UseColorTheme(string color)
    {
        Task.Run(async () =>
        {
            SaveColorTheme(color);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ColorTheme = color;
                ThemeManager.Instance.SetColorTheme(color);
            });
            _logger.Log(this, $"Set color theme {color}");
        });
    }

    public IList<string> GetColorThemes()
    {
        return ThemeManager.Instance.GetColorThemes().ToList();
    }

    public IList<string> GetBaseThemes()
    {
        return ThemeManager.Instance.GetColorThemes().ToList();
    }

    public void Reset()
    {
        SaveBaseTheme(false);
        UseDarkTheme(false);
        UseColorTheme("Default");
        SaveColorTheme("Default");
        _logger.Log(this, "Reset color theme");
    }

    private void SaveColorTheme(string color)
    {
        _configProvider.ColorTheme = color;
        _logger.Log(this, $"Save color theme {color}");
    }

    private void SaveBaseTheme(bool dark)
    {
        _configProvider.DarkMode = dark;
        _logger.Log(this, $"Save base dark theme {dark}");
    }
}
