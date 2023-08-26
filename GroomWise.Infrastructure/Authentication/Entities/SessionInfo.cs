﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

using GroomWise.Domain.Entities;
using GroomWise.Domain.Interfaces;
using LiteDB;

namespace GroomWise.Infrastructure.Session.Entities;

public record SessionInfo
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public IList<Role> Roles { get; set; }
}