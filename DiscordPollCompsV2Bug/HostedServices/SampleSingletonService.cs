using Microsoft.Extensions.DependencyInjection;

namespace DiscordPollCompsV2Bug.HostedServices
{
    public sealed class SampleSingletonService: ICustomService
    {
        public static ValueTask Register(IServiceCollection services)
        {
            services.AddSingleton<SampleSingletonService>();
            return ValueTask.CompletedTask;
        }
    }
}
