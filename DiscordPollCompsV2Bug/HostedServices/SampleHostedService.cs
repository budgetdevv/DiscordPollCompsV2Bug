using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordPollCompsV2Bug.HostedServices
{
    public sealed class SampleHostedService: ICustomService, IHostedService
    {
        public static ValueTask Register(IServiceCollection services)
        {
            services.AddHostedService<SampleHostedService>();
            return ValueTask.CompletedTask;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(SampleHostedService));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
