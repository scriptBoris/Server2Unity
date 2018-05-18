using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            try
            {
                var server = new Server("127.0.0.1");
                Console.WriteLine("Server is run");

                while (input != "qq")
                {
                    server.SendMessage(input);
                    input = Console.ReadLine();
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error launch server: {ex.Message}");
            }
            Thread.Sleep(800);
            Console.WriteLine("Shutdown server...");
        }
    }
}
