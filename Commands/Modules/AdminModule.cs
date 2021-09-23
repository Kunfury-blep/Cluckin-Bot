using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluckin_Bot.Commands.Modules
{
    public class AdminModule : ModuleBase
    {
        [Command("setName")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task ChangeMeal([Remainder] string args = null)
        {
            var sb = new StringBuilder();

            Console.WriteLine(args);

            if (args == null) sb.AppendLine("Please enter a name for the current meal");
            else
            {
                string json = File.ReadAllText("config.json");
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["MealName"] = args;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("config.json", output);
                Console.WriteLine($"config.json changed to: {jsonObj["MealName"]}");
                sb.AppendLine($"Meal Name successfully changed to: {jsonObj["MealName"]}");
                Program.Config["MealName"] = args.ToString();
            }

            await ReplyAsync(sb.ToString());

        }
    }
}
