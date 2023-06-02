﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

namespace GroomWise;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// Application Entry for GroomWise.Desktop
    /// </summary>
    public App()
    {
        InitializeComponent();

        DarkObservableCollectionSettings.RegisterSynchronizer<DarkWpfSynchronizer>();

        BuilderServices.BuildWithContainer(ServiceContainer.ConfigureServices());
        BuilderServices.Resolve<IHotkeyListenerService>().Start();
        BuilderServices.Resolve<IMigrationService>().RunMigrations();
        BuilderServices.Resolve<ILoginView>().Show();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Set the priority to above normal
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;

        // Set the maximum amount of memory that the application can use to 2 GB for x86 and 4 GB for x64.
        IntPtr maxWorkingSet = new IntPtr(
            (IntPtr.Size == 4) ? (int)(1.5 * 1024 * 1024 * 1024) : 4L * 1024L * 1024L * 1024L
        );
        Process.GetCurrentProcess().MaxWorkingSet = maxWorkingSet;

        // Set the minimum amount of memory that the application can use to 250 MB for x86 and 500 MB for x64.
        IntPtr minWorkingSet = new IntPtr(
            (IntPtr.Size == 4) ? 250L * 1024L * 1024L : 500L * 1024L * 1024L
        );
        Process.GetCurrentProcess().MinWorkingSet = minWorkingSet;

        // Set the thread pool minimum and maximum threads to the number of
        // processor cores to improve the performance of parallel tasks.
        int processorCount = Environment.ProcessorCount;
        ThreadPool.SetMinThreads(processorCount, processorCount);
        ThreadPool.SetMaxThreads(processorCount, processorCount);
    }
}
