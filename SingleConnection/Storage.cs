using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace TCPServer
{
    public class Storage
    {
        public static ConcurrentDictionary<string, TcpClient> connectedClients = new ConcurrentDictionary<string, TcpClient>();

        public static ConcurrentQueue<string> WorkQueue = new ConcurrentQueue<string>();

        public static void init()
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() => work());
            }
        }

        public static void work()
        {
            while (true)
            {
                try
                {
                    string uuid = string.Empty;

                    bool dequeued = WorkQueue.TryDequeue(out uuid);

                    if (dequeued)
                    {
                        TcpClient client = null;

                        bool removed = connectedClients.TryRemove(uuid, out client);

                        if (removed)
                        {
                            BinaryWriter writer = new BinaryWriter(client.GetStream());
                            writer.Write(BitConverter.GetBytes(uuid.Length).Concat(Encoding.ASCII.GetBytes(uuid)).ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                Thread.Sleep(500);
            }
        }

    }
}
