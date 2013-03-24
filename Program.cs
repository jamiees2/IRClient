using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRClient
{
    class Program
    {
        private static TcpClient client = null;

        static void Main(string[] args)
        {
            const string server = "irc.rizon.net";
            const int port = 6670;
            var ircClient = new IrcBot(server, port, "finalC_fake", "finalC") {Channel = "#TS"};
            ircClient.Connect();
            new Thread(ircClient.Start).Start();
            new Thread(delegate ()
                {
                    while (ircClient.IsConnected)
                    {
                        var line = Console.ReadLine();
                        ircClient.Write(line);
                    }
                }).Start();
        }
    }
}
