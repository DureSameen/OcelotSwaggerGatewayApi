using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace IdentityServer.Helpers.Localization
{
    public interface IGenericControllerLocalizer<T> : IStringLocalizer<T>
    {

    }
}
