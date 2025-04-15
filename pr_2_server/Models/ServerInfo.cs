using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pr_2_server.Models
{
    internal class ServerInfo
    {
        public int Port { get; set; } = 5555;
        public string StoragePath { get; set; } = "received_files";

        public int activeClients { get; set; } = 0;

        public object lockObject { get; set; } = new object();

        public string AnalysisFile { get; set; }

        public void HandleClient(object obj)
        {

            AnalysisFile = Path.Combine(StoragePath, "result.txt");
            TcpClient client = (TcpClient)obj; // obj - общий тип, приводим к tcpclient
            NetworkStream stream = client.GetStream(); // сетевой поток
            StreamReader reader = new StreamReader(stream, Encoding.UTF8); // объект для чтения данных из потока
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true }; // объект для записи данных в поток

            try
            {
                string fileName = reader.ReadLine();

                string filePath = StoragePath + Guid.NewGuid() + "_" + fileName; // генерируем уникальное имя файла

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter fileWriter = new StreamWriter(fs, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != "EOF") // читаем строки до "EOF"
                    {
                        fileWriter.WriteLine(line); // записываем текущую строку в файл
                    }
                    fileWriter.Flush(); // пишем все в файл
                }

                string analysis = AnalyzeFile(filePath);
                File.AppendAllText(AnalysisFile, analysis + "\n"); // пишем результаты в файл result
                writer.WriteLine(analysis);
                Console.WriteLine("Файл " + fileName + " обработан и сохранен");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке файла: " + ex.Message);
                writer.WriteLine("Ошибка при обработке файла.");
            }
            finally
            {
                client.Close(); // закрываем соединение с клиентом
                lock (lockObject) { activeClients--; } // уменьшаем счетчик подключений
                Console.WriteLine("Клиент отключился. Активных подключений: " + activeClients);
            }
        }

        private string AnalyzeFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);


            int lineCount = lines.Count(); // cчитаем строки
            int wordCount = 0;
            int charCount = 0;
            char[] checkSym = { ' ' };

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line)) // строка не пустая
                {
                    wordCount += line.Split(checkSym, StringSplitOptions.RemoveEmptyEntries).Length; // количество слов
                    charCount += line.Length; // считаем символы
                }
            }
            return "Файл: " + Path.GetFileName(filePath) + "\n" + "\n" + " Строк: " + lineCount + " Слов: " + wordCount + " Символов: " + charCount; // формируем результат анализа
        }
    }
}
