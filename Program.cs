using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace distributed_lock
{
    class Program
    {
        private static List<int> numbers;

        static async Task Main(string[] args)
        {
            SemaphoreSlim _semaphoregate = new SemaphoreSlim(10);

            numbers = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                numbers.Add(i);
            }

            Parallel.ForEach(numbers, async number =>
            {
                await _semaphoregate.WaitAsync();
                await Console.Out.WriteLineAsync(number + " - Hello World: " + DateTime.Now.Second);
                await Task.Delay(1000);
                _semaphoregate.Release();
            });

            Console.ReadLine();

        }
    }
}
