using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private string _url;

        #region private members 	
        // TCPListener to listen for incomming TCP connection requests. 
        private TcpListener tcpListener;
        // Background thread for TcpServer workload. 	
        private Thread tcpListenerThread;
        // Create handle to connected tcp client. 	
        private TcpClient connectedTcpClient;
        #endregion

        public Server(string url)
        {
            _url = url;
            tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();
        }

        //void Loop() // last name is Update
        //{
        //    SendMessage();
        //    Thread.Sleep(18);
        //}

        /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
        private void ListenForIncommingRequests()
        {
            try
            {
                // Create listener on localhost port 8052. 			
                tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
                tcpListener.Start();
                Console.WriteLine(_url);
                Byte[] bytes = new Byte[1024];
                while (true)
                {
                    using (connectedTcpClient = tcpListener.AcceptTcpClient())
                    {
                        // Get a stream object for reading 					
                        using (var stream = connectedTcpClient.GetStream())
                        {
                            int length;
                            // Read incomming stream into byte arrary. 						
                            while ( (length = stream.Read(bytes, 0, bytes.Length) ) != 0)
                            {
                                var incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);
                                // Convert byte array to string message. 							
                                string clientMessage = Encoding.ASCII.GetString(incommingData);
                                Console.WriteLine($"Client message: {clientMessage}");
                            }
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Console.Write("SocketException " + socketException.ToString());
            }
        }

        /// Send message to client using socket connection. 	
        public void SendMessage(string s)
        {
            if (connectedTcpClient == null)
            {
                return;
            }

            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = connectedTcpClient.GetStream();
                if (stream.CanWrite)
                {
                    string serverMessage = s;
                    // Convert string message to byte array.                 
                    byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                    // Write byte array to socketConnection stream.               
                    stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                    Console.WriteLine("Server sent his message - should be received by client");
                }
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Socket exception: " + socketException);
            }
        }
    }
}
