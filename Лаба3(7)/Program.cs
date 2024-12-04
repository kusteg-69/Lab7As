using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Лаба3_7
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string ipAddress = "0.0.0.0";
            int port = 1314;

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://{ipAddress}:{port}/");

            Console.Title = "Сервер";
            Console.WriteLine("Статус: Запущен!");
            Console.WriteLine($"Адрес сервера: http://{ipAddress}:{port}/");

            listener.Start();

            string textContent = "Добро пожаловать на сервер!\n" +
                                "Это пример текста, созданного сервером.\n" +
                                "Изображение доступно по следующему пути: EeJhL8Lm4KY.jpg";

            string textFilePath = @"..\Kust_69\info.txt";
            //string imageFilePath = @"C:\Users\user\Desktop\Сыркин\EeJhL8Lm4KY.jpg";

            File.WriteAllText(textFilePath, textContent);


            string browserPath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

            if (File.Exists(browserPath))
            {
                string url = $"http://{ipAddress}:{port}/";
                Console.Write("Открываю браузер");

                Process.Start(new ProcessStartInfo
                {
                    FileName = browserPath,
                    Arguments = url,
                    UseShellExecute = true
                });


            }

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();

                _ = HandleRequestAsync(context);
            }
        }

        static async Task HandleRequestAsync(HttpListenerContext context)
        {
            Environment.CurrentDirectory = @"C:\Users\user\Desktop\Сыркин"; // Путь в директорий
            HttpListenerRequest request = context.Request;//получаем данные запроса
            HttpListenerResponse response = context.Response;//получаем объект для установки ответа

            string requestUrl = request.RawUrl;
            string contentType = "text/html";

            if (requestUrl.EndsWith(".jpg") || requestUrl.EndsWith(".jpeg"))
            {
                contentType = "image/jpeg";
            }
            else if (requestUrl.EndsWith(".css"))
            {
                contentType = "text/css";
            }
            else if (requestUrl.EndsWith(".txt"))
            {
                contentType = "text/plain; charset=utf-8";
            }

            response.ContentType = contentType;

            string filePath = Path.Combine(Environment.CurrentDirectory, requestUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                // Читаем и отправляем файл, если он существует.
                byte[] fileBytes = File.ReadAllBytes(filePath);
                await response.OutputStream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            response.Close();
        }
    }
}
