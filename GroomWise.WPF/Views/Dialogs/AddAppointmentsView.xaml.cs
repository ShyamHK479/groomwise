﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

using System.ComponentModel;
using System.Linq;
using GroomWise.Infrastructure.Navigation.Interfaces;

namespace GroomWise.Views.Dialogs;

public partial class AddAppointmentsView : IWindow
{
    public AddAppointmentsView(object viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        var parent = App.Current.Windows.OfType<MainView>().FirstOrDefault();
        parent.Focus();
        base.OnClosing(e);
    }
}
