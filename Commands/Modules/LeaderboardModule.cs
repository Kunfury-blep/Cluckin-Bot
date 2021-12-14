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
using System.Diagnostics.CodeAnalysis;

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

            List<User> users = GetLeaderboard();

            //TODO: Add count for sales and cooks alongside the total
            int i = 1;
            foreach (var u in users)
            {
                sb.AppendLine($"{i}. {u.GetName(Context.Guild)}: {u.total}");
                i++;
            }

            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }

 
        [Command("leaderboardSign")]
        [Alias("lbs")]
        public async Task LeaderboardSign()
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();

            List<User> users = GetLeaderboard();

            if(users.Count > 3) users.RemoveRange(3, users.Count - 3);

            if (users.Count <= 0) sb.AppendLine("There are no sales so far");
            else
            {
                //sb.Append($"{Program.Config["MealName"]} Rankings!");
                sb.AppendLine("**Copy below to a sign.**");
                sb.AppendLine("```");
                sb.Append("<b>Specials Cooked/Sold</b>");
                sb.Append(GetSign(users[0], 1));
                if(users.Count >= 2) sb.Append(GetSign(users[1], 2));
                if (users.Count >= 3) sb.Append(GetSign(users[2], 3));
                sb.AppendLine("```");
            }


            await ReplyAsync(sb.ToString());
        }

        private StringBuilder GetSign(User user, int num)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"~n~{num}. {user.GetName(Context.Guild)}: {user.total}");

            return sb;
        }

        private List<User> GetLeaderboard()
        {
            List<User> users = new List<User>();
            //Gets the full JSON Object
            JObject jObj = (JObject)JsonConvert.DeserializeObject(_dh.GetData(""));


            //Gets the specified segment of the JSON as a JArray
            JArray sellers = (JArray)jObj["SALES"];
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(sellers.ToString());

            //Iterates through the sales, adding them to the users.
            foreach(var s in sales)
            {
                User u = new User()
                {
                    id = s.id,
                    sales = s.value
                };
                users.Add(u);
            };

            sellers = (JArray)jObj["COOKS"];
            sales = JsonConvert.DeserializeObject<List<Sale>>(sellers.ToString());

            //Iterates through the cooks, adding the values to the users, creating a new user if none exist
            foreach (var s in sales)
            {
                bool exists = false;
                foreach(var i in users)
                {
                    if (i.id == s.id)
                    {
                        i.cooks = s.value;
                        i.total = i.cooks + i.sales;
                        exists = true;
                        break;
                    }
                    
                }

                if (!exists)
                {
                    User u = new User()
                    {
                        id = s.id,
                        cooks = s.value,
                        total = s.value
                    };
                    users.Add(u);
                }
            };

            users.Sort();
            users.Reverse();
            return users;
        }

    }

    public class User : IComparable<User>
    {
        public UInt64 id;
        public int cooks;
        public int sales;

        public int total;

        //Custom sorter based on the total values
        public int CompareTo(User other)
        {

            return total.CompareTo(other.total);
        }

        /// <summary>
        /// Gets the name of the specified user
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        public string GetName(IGuild guild)
        {
            string result = "*N/A*";
            if (guild != null)
            {
                var user = guild.GetUserAsync(id);
                if (user.Result != null)
                {
                    result = user.Result.Nickname;
                    if (String.IsNullOrEmpty(result)) result = user.Result.Username;
                }
                    
            }
            
               
            return result;

        }
    }

}

public class Sale
{
    public UInt64 id;
    public int value;
}

