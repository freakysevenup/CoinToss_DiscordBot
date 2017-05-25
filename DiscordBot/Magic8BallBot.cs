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
                .Do(async (e) =>
                {
                    string message = "";
                    message += "Commands:\n";
                    message += "'-' is the prefix character.\n";
                    message += "Type '-=' To Flip the coin\n";
                    message += "Type '-= <n>' To Flip the coin 'n' times\n";
                    message += "Type '-heads' To Guess that Heads will be flipped.\n";
                    message += "Type '-tails' To Guess that Tails will be flipped.\n";
                    await (e.Channel.SendMessage(message));
                });

            commands.CreateCommand("=")
                .Parameter("i", ParameterType.Optional)
                .Do(async (e) =>
                {
                    int numberOfFlips = 0;
                    int heads = 0;
                    int tails = 0;
                    string message = "";
                    if (!(e.GetArg("i").Equals("")))
                    {
                        numberOfFlips = Convert.ToInt32(e.GetArg("i"));
                    }

                    if ((e.GetArg("i").Equals("")))
                    {
                        if (getResponse(getRand()).Equals("Heads"))
                        {
                            await (e.Channel.SendMessage(e.User.Name + " flipped Heads"));
                        }
                        else
                        {
                            await (e.Channel.SendMessage(e.User.Name + " flipped Tails"));
                        }
                    }
                    else
                    {
                        if (numberOfFlips < 1001)
                        {
                            for (int i = numberOfFlips; i > 0; i--)
                            {
                                if (flippedHeads(getResponse(getRand())))
                                {
                                    heads++;
                                }
                                else
                                {
                                    tails++;
                                }
                            }

                            message += e.User.Name + "\n";
                            message += "You flipped : \n";
                            message += "Heads " + heads + " times.\n";
                            message += "Tails " + tails + " times.\n";

                            await (e.Channel.SendMessage(message));
                        }
                        else {
                            await (e.Channel.SendMessage("Please choose a number between 1 and 1000"));
                        }
                    }

                });

            commands.CreateCommand("heads")
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

        private bool flippedHeads(string value)
        {
            if (value.Equals("Heads"))
            {
                return true;
            }
            return false;
        }
    }
}
