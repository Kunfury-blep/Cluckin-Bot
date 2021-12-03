using Cluckin_Bot.Commands;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cluckin_Bot
{
    class Program
    {
        public static IConfiguration Config;

        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }

}
