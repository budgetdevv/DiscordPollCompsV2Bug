using System.Net;

using Microsoft.Extensions.Logging;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

namespace DiscordPollCompsV2Bug.Helpers
{
    internal static class OptionsHelper
    {
        private sealed class ApplicationCommandResultHandler<ContextT>: IApplicationCommandResultHandler<ContextT>
            where ContextT: IApplicationCommandContext
        {
            public ValueTask HandleResultAsync(IExecutionResult result, ContextT context, GatewayClient? client, ILogger logger, IServiceProvider services)
            {
                return HandleResultCoreAsync(result, context, client, logger, services);
            }
        }
        
        private sealed class ComponentInteractionResultHandler<ContextT>: IComponentInteractionResultHandler<ContextT>
            where ContextT: IComponentInteractionContext
        {
            public ValueTask HandleResultAsync(IExecutionResult result, ContextT context, GatewayClient? client, ILogger logger, IServiceProvider services)
            {
                return HandleResultCoreAsync(result, context, client, logger, services);
            }
        }
        
        public static void ConfigureApplicationCommandService<InteractionT, ContextT>(ApplicationCommandServiceOptions<InteractionT, ContextT> options)
            where InteractionT: ApplicationCommandInteraction where ContextT : IApplicationCommandContext
        {
            options.DefaultDMPermission = false;
            options.ResultHandler = new ApplicationCommandResultHandler<ContextT>();
        }

        public static void ConfigureInteractionService<InteractionT, ContextT>(ComponentInteractionServiceOptions<InteractionT, ContextT> options)
            where InteractionT: Interaction
            where ContextT: IComponentInteractionContext
        {
            options.ResultHandler = new ComponentInteractionResultHandler<ContextT>();
        }

        private static async ValueTask HandleResultCoreAsync<ContextT>(IExecutionResult result, ContextT context, GatewayClient? client, ILogger logger, IServiceProvider services) where ContextT : IInteractionContext
        {
            if (result is not IFailResult failResult)
            {
                return;
            }

            var interaction = context.Interaction;
                
            InteractionMessageProperties message = new()
            {
                Content = $"**{failResult.Message}**",
                Flags = MessageFlags.Ephemeral,
            };
            
            try
            {
                await interaction.SendResponseAsync(InteractionCallback.Message(message));
            }
            
            catch (RestException restException) when (restException.StatusCode == HttpStatusCode.BadRequest)
            {
                if (restException.Error is { Code: 40060 })
                {
                    await interaction.SendFollowupMessageAsync(message);
                }
            }
            
            catch
            {
                // ignored
            }
        }
    }
}
