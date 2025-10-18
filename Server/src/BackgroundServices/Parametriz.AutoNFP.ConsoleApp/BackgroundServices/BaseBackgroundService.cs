using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.BackgroundServices
{
    public abstract class BaseBackgroundService : IHostedService, IDisposable
    {
        protected readonly Task _completedTask = Task.CompletedTask;
        protected Timer? _timer;

        protected readonly IServiceProvider _serviceProvider;

        protected BaseBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected virtual TimeSpan ObterDelay()
        {
            var agora = DateTime.Now;

            var primeiraExecucao = new DateTime(agora.AddHours(1).Year, agora.AddHours(1).Month, agora.AddHours(1).Day, agora.AddHours(1).Hour, 0, 0);

            return primeiraExecucao - agora;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, ObterDelay(), TimeSpan.FromHours(1));

            await _completedTask;
        }

        protected abstract void DoWork(object? state);

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();

            await _completedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
