﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
//
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

namespace GroomWise.Models.Interfaces.Service;

public interface ISessionManagerService
{
    public IEmployee? SessionUser { get; set; }
    void StartSession(IEmployee account);
    void EndSession();
}