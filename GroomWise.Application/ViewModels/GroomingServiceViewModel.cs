﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

using GroomWise.Application.Events;
using GroomWise.Application.Mappers;
using GroomWise.Application.Observables;
using GroomWise.Domain.Enums;
using GroomWise.Infrastructure.Database;
using GroomWise.Infrastructure.Navigation.Interfaces;
using Injectio.Attributes;
using MvvmGen;
using MvvmGen.Events;
using Swordfish.NET.Collections;

namespace GroomWise.Application.ViewModels;

[ViewModel]
[RegisterSingleton]
[Inject(typeof(GroomWiseDbContext))]
[Inject(typeof(IDialogService))]
[Inject(typeof(INavigationService))]
[Inject(typeof(IEventAggregator))]
public partial class GroomingServiceViewModel
{
    [Property]
    private ObservableGroomingService _activeGroomingService = new();

    [Property]
    private ConcurrentObservableCollection<ObservableGroomingService> _groomingServices = new();

    partial void OnInitialize()
    {
        PopulateCollections();
    }

    private async void PopulateCollections()
    {
        await Task.Run(() =>
        {
            var services = GroomWiseDbContext.GroomingServices
                .GetAll()
                .Select(GroomingServiceMapper.ToObservable);

            GroomingServices = new ConcurrentObservableCollection<ObservableGroomingService>(
                services
            );
        });
    }

    [Command]
    private async Task CreateService()
    {
        await Task.Run(() =>
        {
            DialogService.CreateAddServicesDialog(this, NavigationService);
        });
    }

    [Command]
    private async Task SaveService()
    {
        await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(ActiveGroomingService.Type))
            {
                EventAggregator.Publish(
                    new PublishNotificationEvent(
                        $"Service name cannot be blank",
                        NotificationType.Danger
                    )
                );
                return;
            }
            var dialogResult = DialogService.Create(
                "GroomWise",
                "Create Service?",
                NavigationService
            );
            if (dialogResult is true)
            {
                var service = ActiveGroomingService.ToEntity();
                GroomWiseDbContext.GroomingServices.Insert(service);
                DialogService.CloseDialogs(NavigationService);
                EventAggregator.Publish(
                    new PublishNotificationEvent(
                        $"Service {ActiveGroomingService.Type} saved",
                        NotificationType.Success
                    )
                );
                EventAggregator.Publish(new CreateGroomingServiceEvent());
                ActiveGroomingService = new();
                PopulateCollections();
            }
        });
    }
}
