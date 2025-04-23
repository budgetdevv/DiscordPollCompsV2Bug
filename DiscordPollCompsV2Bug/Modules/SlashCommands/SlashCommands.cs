using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DiscordPollCompsV2Bug.Modules.SlashCommands
{
    public class SlashCommands(GatewayClient client): ApplicationCommandModule<SlashCommandContext>
    {
        [SlashCommand("make-bug", "Make bug")]
        public async Task MakeBug()
        {
            await RespondAsync(InteractionCallback.DeferredMessage());

            var textDisplay = new TextDisplayProperties("Hi!");

            var container = new ComponentContainerProperties([ textDisplay ]);

            static MessagePollMediaProperties CreatePollText(string text)
            {
                return new MessagePollMediaProperties().WithText(text);
            }

            var poll = new MessagePollProperties(
                question: CreatePollText("Do you like bugs?"),
                answers:
                [
                    new(CreatePollText("Yes")),
                    new(CreatePollText("No")),
                ]
            ).WithDurationInHours(1);

            var message = await FollowupAsync(new()
            {
                Components = [ container ],
                Poll = poll,
                Flags = MessageFlags.IsComponentsV2,
            });

            textDisplay.WithContent("Bye!");

            try
            {
                await message.ModifyAsync(x => x.Components = [ container ]);
            }

            catch (Exception)
            {
                await FollowupAsync("We bugged! An exception was thrown");
            }
        }
    }
}
