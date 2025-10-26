/*
  LostSkiesDataDump: A mod for outputting data from the game Lost Skies.
  Copyright (C) 2025  DocW

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using UnityEngine;
using WildSkies.Service;

namespace LostSkiesDataDump;

[Serializable]
public class GameVersionInfo
{
    internal SteamPlatform SteamPlatform { get; set; }
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA1822 // Mark members as static
    public string absoluteURL => Application.absoluteURL;
    public string buildGUID => Application.buildGUID;
    public string cloudProjectId => Application.cloudProjectId;
    public string companyName => Application.companyName;
    public bool genuine => Application.genuine;
    public bool genuineCheckAvailable => Application.genuineCheckAvailable;
    public string identifier => Application.identifier;
    public string installerName => Application.installerName;
    public ApplicationInstallMode installMode => Application.installMode;
    public bool isConsolePlatform => Application.isConsolePlatform;
    public bool isEditor => Application.isEditor;
    public bool isMobilePlatform => Application.isMobilePlatform;
    public RuntimePlatform platform => Application.platform;
    public string productName => Application.productName;
    public string unityVersion => Application.unityVersion;
    public string version => Application.version;
    public string buildNumber => PlatformService.buildNumber;
    public string tag => PlatformService.tag;
    public string commitSha => PlatformService.commitSha;
    public string releaseBranch => PlatformService.releaseBranch;
    public string sourceBranch => PlatformService.sourceBranch;
    public string commitShaShort => PlatformService.commitShaShort;
#pragma warning restore CA1822 // Mark members as static
    public uint _appId => SteamPlatform._appId;
#pragma warning restore IDE1006 // Naming Styles
    public string PlatformName => SteamPlatform.PlatformName;
    public string GetPlatformEnvironment => SteamPlatform.GetPlatformEnvironment();
    public string GetBuildInfoLabel => SteamPlatform.GetBuildInfoLabel();
}
