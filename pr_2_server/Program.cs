using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using pr_2_server.Models;

class Server
{

    public static ServerInfo serverInfo;

    static void Main()
    {
        serverInfo = new ServerInfo();
        // проверка создания папки хранения, если ее нет, то создаем
        if (!Directory.Exists(serverInfo.StoragePath))
            Directory.CreateDirectory(serverInfo.StoragePath);

        TcpListener listener = new TcpListener(IPAddress.Any, serverInfo.Port); // создаем сокет, принимающий входящие соединения от клиентов
        listener.Start(); // запуск сервера
        Console.WriteLine("Сервер запущен на порту: " + serverInfo.Port);

        while (true)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient(); // ожидание подключения
                lock (serverInfo.lockObject) { serverInfo.activeClients++; }
                Console.WriteLine("Клиент подключился. Активных подключений: " + serverInfo.activeClients);
                Server class1 = new Server();

                Thread thread = new Thread(serverInfo.HandleClient); // новый поток для обработки клиента
                thread.Start(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при подключении: " + ex.Message);
            }
        }
    }
}