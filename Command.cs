using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRClient
{
    public sealed class Command
    {
        public static Dictionary<string,Command> Commands = new Dictionary<string, Command>()
            {
                {"PRIVMSG", new Command("PRIVMSG",(x,y,z) => x)},
                {"USER", new Command("USER",(x,y,z) => x)},
                {"NICK", new Command("NICK",(x, y, z) =>
                    {
                        z.Nick = y[0];
                        return x;
                    })},
                {"JOIN", new Command("JOIN",(x, y, z) =>
                    {
                        z.Channel = y[0];
                        return x;
                    })},
                {"PING", new Command("PING",(x,y,z) => x)},
                {"HELP", new Command("HELP",(x,y,z) => x)},
                {"QUIT", new Command("QUIT",(x,y,z) => x)},
                {"ME", new Command("ME",(x, y, z) => "PRIVMSG " + z.Channel + " :\u0001" + "ACTION " + string.Join(" ",y) + "\u0001")},
            };

        public static void Register(string name, Command cmd)
        {
            Commands.Add(name,cmd);
        }

        private readonly Func<string, string[], IrcClient, string> _applier = null;
        private string _name = null;
        public Command(string name, Func<string,string[],IrcClient,string> applier )
        {
            _name = name;
            _applier = applier;

        }

        public string Apply(string cmd, string[] param, IrcClient client)
        {
            return _applier(cmd, param, client);
        }
    }
}
