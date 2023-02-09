using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpClient1
{
    class Program
    {
       
        static void Main(string[] args)
        {
          

            Client.Connect("127.0.0.1", 2000);

            for (int i = 0; i < 10; i++)
            {
                Thread th = new Thread(TestWriterThreads);
                th.Start(); 
            }


            Console.Read();
        }

        static void TestWriterThreads()
        {
            int count = 0;

            while (count <1)
            {
                string uuid = Guid.NewGuid().ToString();
                Client.WriterThread(uuid);

                Console.WriteLine(Client.getAXS(uuid));
                count++;

                Thread.Sleep(200);
            }
        }
    }
}
