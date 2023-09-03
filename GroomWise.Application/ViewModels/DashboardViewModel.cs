﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

using GroomWise.Application.Events;
using GroomWise.Application.Mappers;
using GroomWise.Application.Observables;
using GroomWise.Infrastructure.Authentication.Interfaces;
using GroomWise.Infrastructure.Database;
using Injectio.Attributes;
using MvvmGen;
using MvvmGen.Events;
using Swordfish.NET.Collections;

namespace GroomWise.Application.ViewModels;

[ViewModel]
[RegisterSingleton]
[Inject(typeof(IAuthenticationService))]
[Inject(typeof(GroomWiseDbContext))]
public partial class DashboardViewModel
    : IEventSubscriber<LoginEvent>,
        IEventSubscriber<CreateAppointmentEvent>,
        IEventSubscriber<DeleteAppointmentEvent>
{
    [Property]
    private ConcurrentObservableCollection<ObservableAppointment> _appointments = new();

    [Property]
    private ConcurrentObservableCollection<ObservableAppointment> _upcomingAppointments = new();

    [Property]
    private string _user;

    partial void OnInitialize()
    {
        PopulateCollections();
    }

    private async void PopulateCollections()
    {
        await Task.Run(() =>
        {
            var appointments = GroomWiseDbContext.Appointments
                .GetMultiple(
                    appointment =>
                        appointment.Date == DateTime.Today
                )
                .Select(AppointmentMapper.ToObservable)
                .OrderBy(appointment => appointment.Date);

            var upcomingApointments = GroomWiseDbContext.Appointments
                .GetMultiple(
                    appointment =>
                        appointment.Date >= DateTime.Today
                        && appointment.Date < DateTime.Today.AddDays(7)
                )
                .Select(AppointmentMapper.ToObservable)
                .OrderBy(appointment => appointment.Date)
                .ThenBy(appointment => appointment.StartTime);

            Appointments = new ConcurrentObservableCollection<ObservableAppointment>(appointments);
            UpcomingAppointments = new ConcurrentObservableCollection<ObservableAppointment>(
                upcomingApointments
            );
        });
    }

    public void OnEvent(LoginEvent eventData)
    {
        User = AuthenticationService.GetSession()?.FirstName!;
    }

    public void OnEvent(CreateAppointmentEvent eventData)
    {
        PopulateCollections();
    }

    public void OnEvent(DeleteAppointmentEvent eventData)
    {
        PopulateCollections();
    }
}
