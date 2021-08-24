using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluckin_Bot.Commands.Modules
{
    public class LeaderboardModule : ModuleBase
    {
        [Command("leaderboard")]
        [Alias("ranking, lb, rankings")]
        public async Task DisplayRankings()
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();
            // let's use an embed for this one!
            var embed = new EmbedBuilder();


            // time to add some options to the embed (like color and title)
            embed.WithColor(new Color(0, 255, 0));
            embed.Title = "<Current Meal> Rankings!";

            string name = (Context.Message.Author as SocketGuildUser).Nickname;
            if (name == null) name = Context.User.Username;


            // we can get lots of information from the Context that is passed into the commands
            // here I'm setting up the preface with the user's name and a comma
            sb.AppendLine($"{name},");
            sb.AppendLine();

            sb.AppendLine("This is a mockup for the leaderboard system of current meals");

            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
    }
}
