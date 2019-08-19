using System;

namespace SZWNet
{
    class Program
    {
        static void Main(string[] args)
        {
            SZWSocket<SZWSession> socket = new SZWSocket<SZWSession>();
            socket.StartAsServer("127.0.0.1", 5055);
            Console.Read();
        }
    }
}
