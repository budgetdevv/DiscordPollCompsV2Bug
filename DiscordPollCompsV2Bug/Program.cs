using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

using DiscordPollCompsV2Bug;
using DiscordPollCompsV2Bug.Helpers;
using DiscordPollCompsV2Bug.HostedServices;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;

services
    .AddDiscordGateway(options =>
    {
        options.Intents = GatewayIntents.All;
    })
    .AddApplicationCommands<SlashCommandInteraction, SlashCommandContext>(OptionsHelper.ConfigureApplicationCommandService)
    .AddApplicationCommands<UserCommandInteraction, UserCommandContext>(OptionsHelper.ConfigureApplicationCommandService)
    .AddApplicationCommands<MessageCommandInteraction, MessageCommandContext>(OptionsHelper.ConfigureApplicationCommandService)
    .AddComponentInteractions<ButtonInteraction, ButtonInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddComponentInteractions<UserMenuInteraction, UserMenuInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddComponentInteractions<ModalInteraction, ModalInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddHttpClient()
    .AddGatewayEventHandlers(typeof(Program).Assembly)
    .AddOptions<Configuration>()
    .BindConfiguration(string.Empty);

await ICustomService.RegisterServices(services);

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

await host.RunAsync();
