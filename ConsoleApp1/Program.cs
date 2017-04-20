using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    class Program
    {
        Encoding utf8 = Encoding.UTF8;
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost"); //Адреса звідки буде здійснена відправка
            IPAddress ipAddr = ipHost.AddressList[0]; //Отримання Адреси
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 65535); //Задання порту серверу
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Створення сокету
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                while (true) //Управління 
                {
                    Console.WriteLine("Очiкування з'єднання {0}", ipEndPoint);
                    Socket handler = sListener.Accept();
                    string data = null;
                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    Console.Write("Відповідь: " + data + "\n\n");
                    if (data.IndexOf("Кiнець") > -1)
                    {
                        Console.WriteLine("До побачення.");
                        break;
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}