using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluckin_Bot.Commands.Modules
{
    public class MealModule : ModuleBase
    {
        //TODO: Change the chat message to dynamically say how many were sold/cooked
        [Command("Sold")]
        public async Task SellMeal([Remainder] int amount = 0)
        {
            IUser user = Context.User;

            var sb = new StringBuilder();

            if (amount > 0)
            {
                string total = UpdateData("SALES", user, amount);

                sb.AppendLine($"{user.Mention} just sold a {Program.Config["MealName"]} for a total of {total}!");

            }

            await ReplyAsync(sb.ToString());
        }

        [Command("Made")]
        public async Task MakeMeal([Remainder] int amount = 0)
        {
            IUser user = Context.User;

            var sb = new StringBuilder();

            if (amount > 0)
            {
                string total = UpdateData("COOKS", user, amount);

                sb.AppendLine($"{user.Mention} just cooked a {Program.Config["MealName"]} for a total of {total}!");

            }

            await ReplyAsync(sb.ToString());
        }

        [Command("Both")]
        public async Task BothCommand([Remainder] int amount = 0)
        {
            IUser user = Context.User;

            var sb = new StringBuilder();

            if (amount > 0)
            {
                //TODO: Clean up this segment, make the chat a bit better

                string total = UpdateData("SALES", user, amount);
                sb.AppendLine($"{user.Mention} just sold a {Program.Config["MealName"]} for a total of {total}!");

                total = UpdateData("COOKS", user, amount);
                sb.AppendLine($"{user.Mention} just cooked a {Program.Config["MealName"]} for a total of {total}!");

                
            }

            await ReplyAsync(sb.ToString());
        }




        private string UpdateData(string parent, IUser user, int amount)
        {
            string json = File.ReadAllText("data.json");

            Console.WriteLine(json);

            JObject pObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            JArray sellers = (JArray)pObj[parent];

            var seller = (sellers.Where(s => s["id"].ToString() == user.ToString())
                .Select(s => s)).FirstOrDefault();

            if (seller == null) seller = new JObject();

            if (seller["id"] == null)
            {
                Console.WriteLine("Seller was not found, making a new one");

                seller["id"] = user.ToString();
                seller["value"] = amount;

                sellers.Add(seller);
            }
            else
            {
                int amt = int.Parse(seller["value"].ToString()) + amount;
                seller["value"] = amt.ToString();

            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(pObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("data.json", output);

            return seller["value"].ToString();
        }
    }
}
