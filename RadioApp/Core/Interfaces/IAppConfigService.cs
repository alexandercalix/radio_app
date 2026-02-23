using System;
using RadioApp.Core.Models;

namespace RadioApp.Core.Interfaces;

public interface IAppConfigService
{
    Task<AppConfig> GetConfigAsync();
}