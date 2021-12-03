using Cluckin_Bot.Public_Classes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Cluckin_Bot.Commands.Modules
{
    public class LeaderboardModule : ModuleBase
    {
        private DataHandler _dh;
        LeaderboardModule()
        {
            _dh = new DataHandler();
        }

        [Command("leaderboard")]
        [Alias("ranking",  "lb", "rankings")]
        public async Task DisplayRankings([Remainder] string mealName = "")
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();
            // let's use an embed for this one!
            var embed = new EmbedBuilder();


            // time to add some options to the embed (like color and title)
            embed.WithColor(new Color(0, 255, 0));
            embed.Title = $"{Program.Config["MealName"]} Rankings!";

            string name = (Context.Message.Author as SocketGuildUser).Nickname;
            if (name == null) name = Context.User.Username;

            sb.Append(GetLeaderboard(0));

            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }





        private StringBuilder GetLeaderboard(int pos)
        {
            string json = _dh.GetData("");
            JObject jObj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder sb = new StringBuilder();

            //Gets the seperate JArrays from the single JSON
            JArray sellers = (JArray)jObj["SALES"];
            JArray cookers = (JArray)jObj["COOKS"];

            //Converts the JArrays to lists, possibly unneeded?
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(sellers.ToString());
            List<Sale> cooks = JsonConvert.DeserializeObject<List<Sale>>(cookers.ToString());

            //Combines the lists into a single dictionary, using ID as key
            Dictionary<UInt64, int> totalDict = new Dictionary<UInt64, int>();
            foreach(var s in sales)
            {
                totalDict.Add(s.id, s.value);
            }
            foreach (var c in cooks)
            {
                if (totalDict.ContainsKey(c.id)) totalDict[c.id] += c.value;
                else totalDict.Add(c.id, c.value);
            }

            var sortedDict = from entry in totalDict orderby entry.Value descending select entry;


            int i = 1;
            foreach (var s in sortedDict)
            {
                //IUser user = Context.Client.GetUserAsync(i.Key).Result;
                IGuildUser gUser = Context.Guild.GetUserAsync(s.Key).Result;
                if(gUser != null) sb.AppendLine($"{i}. {gUser.Nickname} - {s.Value}");
                i++;
            }

            return sb;
        }
    }
    

}

public class Sale
{
    public UInt64 id;
    public int value;
}