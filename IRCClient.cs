using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace IRClient
{
    public class IrcClient
    {
        public string Name { get; set; }
        public string Nick { get; set; }
        public string Channel { get; set; }
        public string Host { get; set; }
        public bool IsConnected { get; protected set;}

        protected readonly string Server;
        protected readonly int Port;
        protected readonly TcpClient Irc;

        public StreamWriter Writer { get; protected set; }
        public StreamReader Reader { get; protected set; }

        public IrcClient(string server, int port, string nick, string name, string host)
        {
            Server = server;
            Port = port;
            Nick = nick;
            Name = name;
            Host = host;
            Irc = new TcpClient(Server,Port);
        }

        public void Connect()
        {
            var stream = Irc.GetStream();
            Writer = new StreamWriter(stream, Encoding.Default);
            Reader = new StreamReader(stream, Encoding.Default);
            Write("USER " + Nick + " " + Host + " " + Host + " :" + Name);
            Write("NICK " + Nick);
            Write("JOIN " + Channel);
            IsConnected = true;
        }

        public void Disconnect()
        {
            Writer.Close();
            Reader.Close();
            Irc.Close();
            IsConnected = false;
        }

        public void Write(string cmd)
        {
            string[] cparam = cmd.Split(new char[]{' '},2);
            string command = cparam[0];
            var param = cparam.Length > 1 ? cparam[1] : "";
            var parameters = param.Split(' ');

            if (Command.Commands.ContainsKey(command)) cmd = Command.Commands[command].Apply(cmd, parameters, this);
            else
            {
                cmd = "PRIVMSG " + Channel + " :" + cmd;
            }

            Writer.WriteLine(cmd);
            Writer.Flush();
            Console.WriteLine(cmd);

        }

        public void Start()
        {
            while (IsConnected)
            {
                var data = Reader.ReadLine();
                Console.WriteLine(data);
                var ex = data.Split(new char[] { ' ' }, 5);

                if (ex[0] == "PING")
                {
                    Write("PONG " + ex[1]);
                }
            }

        }
    }
}
