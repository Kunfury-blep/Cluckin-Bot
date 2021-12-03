using Discord;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cluckin_Bot.Public_Classes
{
    public class DataHandler
    {
        private string OldContestFolder = "Old Contests";


        public string UpdateData(string parent, IUser user, int amount)
        {
            string json = File.ReadAllText("data.json");

            JObject pObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            JArray sellers = (JArray)pObj[parent];

            var seller = (sellers.Where(s => ulong.Parse(s["id"].ToString()) == user.Id)
                .Select(s => s)).FirstOrDefault();

            if (seller == null) seller = new JObject();

            if (seller["id"] == null)
            {
                Console.WriteLine("Seller was not found, making a new one");

                seller["id"] = user.Id;
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
    
        public string GetData(string contestName)
        {
            if (String.IsNullOrEmpty(contestName)) contestName = "data";
            string fileName = contestName + ".json";
            JObject pObj = null;

            string json = "No Data Found!";
            if(fileName != "data.json") fileName = ($"{OldContestFolder}/{fileName}");

            if(File.Exists(fileName))
            {
                json = File.ReadAllText(fileName);
                pObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            }

            return json;
        }
    
    }
}
