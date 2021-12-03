using Cluckin_Bot.Public_Classes;
using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace Cluckin_Bot.Commands.Modules
{
    public class MealModule : ModuleBase
    {
        private DataHandler _dh;
        MealModule()
        {
            _dh = new DataHandler();
        }


        //TODO: Change the chat message to dynamically say how many were sold/cooked
        [Command("Sold")]
        public async Task SellMeal([Remainder] int amount = 1)
        {
            IUser user = Context.User;

            var sb = new StringBuilder();

            string total = _dh.UpdateData("SALES", user, amount);

            sb.AppendLine($"{user.Mention} just sold {amount} {Program.Config["MealName"]} for a total of {total}!");

            await ReplyAsync(sb.ToString());
        }

        [Command("Made")]
        public async Task MakeMeal([Remainder] int amount = 1)
        {
            IUser user = Context.User;

            var sb = new StringBuilder();

            string total = _dh.UpdateData("COOKS", user, amount);

            sb.AppendLine($"{user.Mention} just cooked {amount} {Program.Config["MealName"]} for a total of {total}!");


            await ReplyAsync(sb.ToString());
        }

        [Command("Both")]
        public async Task BothCommand([Remainder] int amount = 1)
        {
            IUser user = Context.User;

            var sb = new StringBuilder();
            //TODO: Clean up this segment, make the chat a bit better

            string total = _dh.UpdateData("SALES", user, amount);
            sb.AppendLine($"{user.Mention} just sold {amount} {Program.Config["MealName"]} for a total of {total}!");

            total = _dh.UpdateData("COOKS", user, amount);
            sb.AppendLine($"{user.Mention} just cooked {amount} {Program.Config["MealName"]} for a total of {total}!");

            await ReplyAsync(sb.ToString());
        }




        
    }
}
