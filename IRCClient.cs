using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace IRClient
{
    class IrcClient
    {
        public string Name { get; set; }
        public string Nick { get; set; }
        public string Channel { get; set; }
        public bool IsConnected { get; protected set;}

        protected readonly string _server;
        protected readonly int _port;
        protected readonly TcpClient _irc;

        public StreamWriter Writer { get; protected set; }
        public StreamReader Reader { get; protected set; }

        protected readonly string[] _commands = new string[]{"PRIVMSG","USER", "NICK", "JOIN", "PING", "PONG", "HELP", "QUIT", "ME"};

        public IrcClient(string server, int port, string nick, string name)
        {
            _server = server;
            _port = port;
            Nick = nick;
            Name = name;
            _irc = new TcpClient(_server,_port);
        }

        public void Connect()
        {
            var stream = _irc.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
            Write("USER " + Nick + " tal.is" + " tal.is" + " :" + Name);
            Write("NICK " + Nick);
            Write("JOIN " + Channel);
            IsConnected = true;
        }

        public void Disconnect()
        {
            Writer.Close();
            Reader.Close();
            _irc.Close();
            IsConnected = false;
        }

        public void Write(string cmd)
        {
            string[] cparam = cmd.Split(new char[]{' '},2);
            string command = cparam[0];
            var param = cparam.Length > 1 ? cparam[1] : "";
            var parameters = param.Split(' ');
            //Save the channel
            if (command == "JOIN") Channel = parameters[0];
            else if (command == "NICK") Nick = parameters[0];
            else if (command == "QUIT") Disconnect();
            else if (command == "ME") cmd = "PRIVMSG " + Channel + " :\u0001" + "ACTION " + param + "\u0001";
            else if (!_commands.Contains(command)) cmd = "PRIVMSG " + Channel + " :" + cmd;
            
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
                var charSeparator = new char[] { ' ' };
                var ex = data.Split(charSeparator, 5);

                if (ex[0] == "PING")
                {
                    Write("PONG " + ex[1]);
                }
            }

        }
    }
}
