using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cluckin_Bot.Commands.Modules
{
    public class AdminModule : ModuleBase
    {
        [Command("setName")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task RenameMeal([Remainder] string args = null)
        {
            var sb = new StringBuilder();

            if (args == null) sb.AppendLine("Please enter a name for the current meal");
            else sb.AppendLine(RenameContest(args));

            await ReplyAsync(sb.ToString());

        }

        [Command("SetupMeal")]
        [Alias("sm")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task SetupNewMeal([Remainder] string args = null)
        {
            var sb = new StringBuilder();

            Console.WriteLine(args);
            if (args == null) sb.AppendLine("Please enter a name for the new meal");
            else
            {
                Directory.CreateDirectory(Program.Config["MealFolder"]);

                string mealName = Program.Config["MealName"];

                //Strips any illegal values from the file name
                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                mealName = r.Replace(mealName, "");

                //Creates a backup of the current contest, sets the name to the contest name
                string copyPath = $"{Program.Config["MealFolder"]}/{mealName}.json";
                File.Copy("data.json", copyPath, true);

                //Reverts the data.json file to a blank state
                File.Copy("baselineData.json", "data.json", true);

                sb.AppendLine("Successfully backed up the current Contest.");
                sb.AppendLine(RenameContest(args));
            }

            await ReplyAsync(sb.ToString());

        }



        private string RenameContest(string name)
        {
            string json = File.ReadAllText("config.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["MealName"] = name;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("config.json", output);
            Console.WriteLine($"config.json changed to: {jsonObj["MealName"]}");
            
            Program.Config["MealName"] = name.ToString();

            return ($"Meal Name successfully changed to: {jsonObj["MealName"]}");
        }
    }
}
