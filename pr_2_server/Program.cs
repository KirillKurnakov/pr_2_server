using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using pr_2_server.Models;
using pr_2_server.Workers;

class Server
{
    private static ServerInfo serverInfo;
    private readonly Mutex mutexObj = new Mutex();
    public static int activeClients  = 0;
    public static object lockObject = new object();

    static void Main()
    {
        serverInfo = new ServerInfo();

        TcpListener listener = new TcpListener(IPAddress.Any, serverInfo.Port);
        listener.Start(); // запуск сервера
        Console.WriteLine("Сервер запущен на порту: " + serverInfo.Port);

        while (true)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient(); // ожидание подключения
                lock (lockObject) { activeClients++; }
                Console.WriteLine("Клиент подключился. Активных подключений: " + activeClients);

                Thread thread = new Thread(HandleClient); // новый поток для обработки клиента
                Console.WriteLine("Поток запускается...");

                thread.Start(client);
                Console.WriteLine("Поток запускается для" + activeClients);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при подключении: " + ex.Message);
            }
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj; // obj - общий тип, приводим к tcpclient
        NetworkStream stream = client.GetStream();

        try
        {
            FIleAnalysis fIleAnalysis = new FIleAnalysis();
            // проверка создания папки хранения, если ее нет, то создаем
            if (!Directory.Exists(fIleAnalysis.StoragePath))
                Directory.CreateDirectory(fIleAnalysis.StoragePath);


            byte[] buffer = new byte[512];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // Presentation: дешифруем байты в строку
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead); 
            fIleAnalysis.fileContent = data;

            // Генерируем путь для сохранения
            fIleAnalysis.uniqueFileName = Guid.NewGuid() + "_" + fIleAnalysis.FileName;
            fIleAnalysis.filePath = Path.Combine(fIleAnalysis.StoragePath, fIleAnalysis.uniqueFileName);

            // Application
            //Console.WriteLine("[Application] Получено сообщение от клиента.");

            fIleAnalysis.AnalysisFile = Path.Combine(fIleAnalysis.StoragePath, "result.txt");

            // Сохраняем файл на диск
            File.WriteAllText(fIleAnalysis.filePath, fIleAnalysis.fileContent, Encoding.UTF8);

            string analysis = AnalyzeFile(fIleAnalysis.filePath);

            Console.WriteLine($"Файл {fIleAnalysis.FileName} обработан и сохранен как {fIleAnalysis.uniqueFileName}");

            // Presentation: кодируем сообщение в байты
            byte[] dataToClient = Encoding.UTF8.GetBytes(analysis);

            // Application: отправка данных
            stream.Write(dataToClient, 0, dataToClient.Length);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обработке файла: " + ex.Message);
        }
        finally
        {
            client.Close(); // закрываем соединение
            lock (lockObject)
            {
                activeClients--;
            }
            Console.WriteLine("Клиент отключился. Активных подключений: " + activeClients);
        }
    }
    private static string AnalyzeFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        FIleAnalysis fileAnalysis = new FIleAnalysis();
        fileAnalysis.lineCount = lines.Count();

        char[] checkSym = { ' ' };

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line)) // строка не пустая
            {
                fileAnalysis.wordCount += line.Split(checkSym, StringSplitOptions.RemoveEmptyEntries).Length; // количество слов
                fileAnalysis.charCount += line.Length; // считаем символы
            }
        }
        return " Строк: " + fileAnalysis.lineCount + " Слов: " + fileAnalysis.wordCount + " Символов: " + fileAnalysis.charCount; // формируем результат анализа

        //return "Файл: " + Path.GetFileName(filePath) + "\n" + "\n" + " Строк: " + fileAnalysis.lineCount + " Слов: " + fileAnalysis.wordCount + " Символов: " + fileAnalysis.charCount; // формируем результат анализа
    }
}