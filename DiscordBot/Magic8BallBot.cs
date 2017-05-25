using Discord;
using Discord.Commands;
using System;

namespace DiscordBot
{
    public class CoinToss
    {
        DiscordClient client;
        CommandService commands;
        Random rand;

        public CoinToss()
        {
            client = new DiscordClient(input =>
            {
                input.LogLevel = LogSeverity.Info;
                input.LogHandler = Log;
            } );

            client.UsingCommands(input =>
            {
                input.PrefixChar = '-';
                input.AllowMentionPrefix = true;
            });

            commands = client.GetService<CommandService>();

            // Commands
            commands.CreateCommand("help")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string message = "";
                    message += "Commands:\n";
                    message += "'-' is the prefix character.\n";
                    message += "Type '-=' To Flip the coin\n";
                    message += "Type '-heads' To Guess that Heads will be flipped.\n";
                    message += "Type '-tails' To Guess that Tails will be flipped.\n";
                    await (e.Channel.SendMessage(message));
                });

            commands.CreateCommand("=")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    if (getResponse(getRand()).Equals("Heads"))
                    {
                        await (e.Channel.SendMessage(e.User.Name + " flipped Heads"));
                    }
                    else
                    {
                        await (e.Channel.SendMessage(e.User.Name + " flipped Tails"));
                    }
                });

            commands.CreateCommand("heads")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    if (getResponse(getRand()).Equals("Heads"))
                    {
                        await (e.Channel.SendMessage("Heads!! " + e.User.Name + " guessed Correctly!!"));
                    }
                    else
                    {
                        await (e.Channel.SendMessage("Tails!! Sorry " + e.User.Name + " you lose this time!"));
                    }
                });

            commands.CreateCommand("tails")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    if (getResponse(getRand()).Equals("Tails"))
                    {
                        await (e.Channel.SendMessage("Tails!! " + e.User.Name + " guessed Correctly!!"));
                    }
                    else 
                    {
                        await (e.Channel.SendMessage("Heads!! Sorry " + e.User.Name + " you lose this time!"));
                    }
                });

            client.ExecuteAndWait(async () =>
            {
                await (client.Connect("MzE3MzM5ODAxNjYzNzAwOTky.DAiZSw.rwH-8nJGTAGu2D3WF7_JEm4y2qw", TokenType.Bot));
            });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private int getRand()
        {
            rand = new Random(DateTime.Now.Millisecond);
            return rand.Next(1, 3);
        }

        private string getResponse(int randomNumber)
        {
            switch (randomNumber)
            {
                case 1:
                    return "Heads";
                case 2:
                    return "Tails";
                default:
                    return "Something went horribly wrong... please re-check the code";
            }
        }
    }
}
