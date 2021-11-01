using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace backgroundworker.ScopedService
{
    public interface IScopeSettingsService
    {
        Task WriteSetting(CancellationToken cancellationToken);
    }
}
