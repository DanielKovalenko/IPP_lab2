using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static string[] board = new string[64]; // Шахова дошка
    static int[] positions = new int[2]; // Позиції фігур
    static object lockObject = new object(); // Об'єкт для блокування доступу до даних
    

    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        // Ініціалізація шахової дошки
        InitializeBoard();

        // Запуск сервера
        Task serverTask = ServerThreadAsync();

        // Запуск двох клієнтів
        Task client1Task = ClientThreadAsync(0);
        Task client2Task = ClientThreadAsync(1);

        // Очікування завершення роботи клієнтів
        await Task.WhenAll(client1Task, client2Task);

        Console.WriteLine("Гра завершена.");
        Console.ReadLine();
    }

    static void InitializeBoard()
    {
        for (int i = 0; i < 64; i++)
        {
            board[i] = "-";
        }
    }

    static async Task ServerThreadAsync()
    {
        Random rand = new Random();
        while (true)
        {
            // Зміна позицій фігур
            lock (lockObject)
            {
                positions[0] = rand.Next(0, 64);
                positions[1] = rand.Next(0, 64);
            }

            // Вивід нових координат фігур на екран
            Console.WriteLine($"Нові координати: X - {positions[0]}, O - {positions[1]}");

            await Task.Delay(3000); // Затримка 3 секунди
        }
    }

    static async Task ClientThreadAsync(object playerId)
    {
        int id = (int)playerId;
        //Console.WriteLine($"Гравець {id + 1} приєднався до гри.");

        while (true)
        {
            
            // Відображення шахової дошки
            lock (lockObject)
            {
                Console.Clear();
                for (int i = 0; i < 64; i++)
                {
                    if (i == positions[0])
                        Console.Write("X "); // Позиція першої фігури
                    else if (i == positions[1])
                        Console.Write("O "); // Позиція другої фігури
                    else
                        Console.Write(board[i] + " ");
                    if (i % 8 == 7)
                        Console.WriteLine();
                }
                Console.WriteLine();
                // Вивід координат фігур на екран
                Console.WriteLine($"Поточні координати: X - {positions[0]}, O - {positions[1]}");
            }

            await Task.Delay(1000); // Затримка 1 секунда
        }
    }
}