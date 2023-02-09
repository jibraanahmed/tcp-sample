using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;

namespace TcpClient1
{
    public class Client
    {
        static int requestCount = 0;
        static int recievedCount = 0;
        static TcpClient client = null;
        static BinaryWriter writer = null;
        static int Number = 0;

        static ConcurrentDictionary<string, string> MessagesColletion = new ConcurrentDictionary<string, string>();

        public static void Connect(string ipaddress, int port)
        {
            try
            {
                client = new TcpClient(ipaddress, port);
                writer = new BinaryWriter(client.GetStream());
                BinaryReader reader = new BinaryReader(client.GetStream());

                //Task.Factory.StartNew(() => WriterThread(writer));
                Task.Factory.StartNew(() => ReaderThread(reader));
            }
            catch (Exception ex)
            {
            }

        }

        public static void WriterThread(string message)
        {
           // while (true)
            {
                try
                {
                    writer.Write(BitConverter.GetBytes(message.Length).Concat(Encoding.ASCII.GetBytes(message)).ToArray());
                    requestCount++;

                    Console.Title = requestCount + "|" + recievedCount;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                //Thread.Sleep(100);
            }
        }

        private static void ReaderThread(BinaryReader reader)
        {
            while (true)
            {
                try
                {

                    int expectedLength = reader.ReadInt32();

                    if (expectedLength > 0)
                    {
                        String message = Encoding.ASCII.GetString(reader.ReadBytes(expectedLength));

                        MessagesColletion.TryAdd(message, message);

                      
                        //Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }

        public static string getAXS(string uuid)
        {
            string result = string.Empty;
            int count =0;

            try
            {
                do
                {
                    bool removed = false;

                    removed = MessagesColletion.TryRemove(uuid, out result);

                    if (removed)
                    {
                        recievedCount++;
                        Console.Title = requestCount + "|" + recievedCount;
                        return result;
                    }
                    count++;
                    Thread.Sleep(1000);

                } while (string.IsNullOrEmpty(result) && count < 30);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

    }
}
