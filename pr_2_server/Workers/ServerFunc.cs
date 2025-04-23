using pr_2_server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pr_2_server.Workers
{
    internal class ServerFunc
    {
        //Console.WriteLine("[Application] Сообщение отправлено.");

        //writer.WriteLine(analysis);

        //mutexObj.ReleaseMutex();

        //stream.Close();

        //client.Close();

        //fIleAnalysis.AnalysisFile = Path.Combine(fIleAnalysis.StoragePath, "result.txt");
        //client = (TcpClient)obj; // obj - общий тип, приводим к tcpclient
        //stream = client.GetStream(); // сетевой поток
        //reader = new StreamReader(stream, Encoding.UTF8); // объект для чтения данных из потока
        //writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true }; // объект для записи данных в поток

        // Оборачиваем входной поток в StreamReader
        //reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        //writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

        // Считываем имя файла
        //fIleAnalysis.FileName = reader.ReadLine();
        //if (string.IsNullOrWhiteSpace(fIleAnalysis.FileName))
        //    throw new Exception("Не удалось получить имя файла");

        //mutexObj.WaitOne();

        // Генерируем путь для сохранения
        //fIleAnalysis.uniqueFileName = Guid.NewGuid() + "_" + fIleAnalysis.FileName;
        //fIleAnalysis.filePath = Path.Combine(fIleAnalysis.StoragePath, fIleAnalysis.uniqueFileName);

        // Читаем оставшееся содержимое как весь файл (текст)
        //fIleAnalysis.fileContent = reader.ReadToEnd();

        //Console.WriteLine();
        //Console.WriteLine(fIleAnalysis.fileContent);
        //Console.WriteLine();

        // Сохраняем файл на диск
        //File.WriteAllText(fIleAnalysis.filePath, fIleAnalysis.fileContent, Encoding.UTF8);

        // Анализируем файл и отправляем результат клиенту
        //string analysis = AnalyzeFile(fIleAnalysis.filePath);
    }
}
