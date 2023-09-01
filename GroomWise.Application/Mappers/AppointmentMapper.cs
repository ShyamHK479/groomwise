﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

using GroomWise.Application.Observables;
using GroomWise.Domain.Entities;
using Mapster;

namespace GroomWise.Application.Mappers;

public static class AppointmentMapper
{
    public static ObservableAppointment ToObservable(this Appointment appointment)
    {
        TypeAdapterConfig<Appointment, ObservableAppointment>
            .NewConfig()
            .Map(
                dest => dest.Services,
                src =>
                    src.Services
                        .Select(
                            groomingService =>
                                new ObservableAppointmentService
                                {
                                    GroomingService = groomingService.ToObservable()
                                }
                        )
                        .ToList()
            )
            .Map(
                dest => dest.Employees,
                src => src.Employees.Select(employee => employee.ToObservable()).ToList()
            )
            .Map(dest => dest.Customer, src => src.Customer.ToObservable())
            .Map(dest => dest.Pet, src => src.Pet.ToObservable());
        return appointment.Adapt<ObservableAppointment>();
    }

    public static Appointment ToEntity(this ObservableAppointment observableAppointment)
    {
        TypeAdapterConfig<ObservableAppointment, Appointment>
            .NewConfig()
            .Map(
                dest => dest.Services,
                src =>
                    src.Services
                        .Select(appointmentService => appointmentService.GroomingService)
                        .ToList()
            )
            .Map(
                dest => dest.Employees,
                src =>
                    src.Employees
                        .Select(observableEmployee => observableEmployee.ToEntity())
                        .ToList()
            )
            .Map(dest => dest.Customer, src => src.Customer.ToEntity())
            .Map(dest => dest.Pet, src => src.Pet.ToEntity());
        return observableAppointment.Adapt<Appointment>();
    }
}
