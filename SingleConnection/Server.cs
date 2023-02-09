using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TCPServer
{
    public class Server
    {
        static int Number = 0 ;
       
        public void start()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 2000);
            listener.Start();

            Console.WriteLine("Server Listeing on 127.0.0.1:2000");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Factory.StartNew(() => ReaderThread(client)); 
            }
        }

        public void WriterThread(BinaryWriter writer)
        {
            //while (true)
            {
                try
                {
                    string message = "Random Number" + Number++.ToString();
                    writer.Write(BitConverter.GetBytes(message.Length).Concat(Encoding.ASCII.GetBytes(message)).ToArray());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                Thread.Sleep(100);
            }
        }

        public void ReaderThread(TcpClient client)
        {
            while (true)
            {
                try
                {   
                    BinaryReader reader = new BinaryReader(client.GetStream());

                    int expectedLength = reader.ReadInt32();

                    if (expectedLength > 0)
                    {
                        String message = Encoding.ASCII.GetString(reader.ReadBytes(expectedLength));

                        Storage.connectedClients.TryAdd(message, client);

                        Storage.WorkQueue.Enqueue(message);

                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                //Thread.Sleep(1000);
            }
        }

    }
}
