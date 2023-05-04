﻿// Copyright (C) 2023 Russell Camo (Russkyc).- All Rights Reserved
// 
// Unauthorized copying or redistribution of all files, in source and binary forms via any medium
// without written, signed consent from the author is strictly prohibited.

#region

using Ini.Net;

#endregion

namespace GroomWise.Models.Interfaces;

public interface IConfigurationService
{
    string Key { get; set; }
    IniFile Config { get; set; }
}