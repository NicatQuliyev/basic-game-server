using System;


namespace TcpListenerDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server";

            Server.Start(20, 25965);

            Console.ReadKey();
        }
    }
}
