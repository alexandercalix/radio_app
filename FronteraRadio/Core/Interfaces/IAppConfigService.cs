using System;
using RadioApp.Core.Models;

namespace FronteraRadio.Core.Interfaces;

public interface IAppConfigService
{
    Task<AppConfig> GetConfigAsync();
}