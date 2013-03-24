using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRClient
{
    class IrcBot : IrcClient
    {
        public IrcBot(string server, int port, string nick, string name) : base(server, port, nick, name)
        {
        }

        public new void Start()
        {

            while (IsConnected)
            {
                var data = Reader.ReadLine();
                Console.WriteLine(data);
                var charSeparator = new char[] { ' ' };
                var ex = data.Split(charSeparator, 5);

                if (ex[0] == "PING")
                {
                    Write("PONG " + ex[1]);
                }

                if (ex.Length > 4) //is the command received long enough to be a bot command?
                {
                    string command = ex[3]; //grab the command sent

                    switch (command.Substring(1))
                    {
                        case "!join":
                            Write("JOIN " + ex[4]); //if the command is !join send the "JOIN" command to the server with the parameters set by the user
                            break;
                        case "!say":
                            Write(ex[4]);
                            break;
                        case "!nick":
                            Write("NICK " + ex[4]);
                            break;
                        case "!quit":
                            Write("QUIT " + ex[4]);
                            Disconnect();
                            break;

                    }
                }
            }
        }
    }
}
