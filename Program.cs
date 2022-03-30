using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading.Redis;
using StackExchange.Redis;

namespace distributed_lock
{
    class Program
    {
        private static List<int> numbers;

        static async Task Main(string[] args)
        {
            numbers = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                numbers.Add(i);
            }

            var redis = ConnectionMultiplexer.Connect("localhost");
            var db = redis.GetDatabase();

            var semaphore = new RedisDistributedSemaphore("MySemaphoreID", maxCount: 10, database: db);

            Parallel.ForEach(numbers, async number =>
            {
                await using (await semaphore.AcquireAsync())
                {
                    var now = DateTime.Now;

                    await Console.Out.WriteLineAsync(number + " - Hello World: " + now.Second);

                    await Task.Delay(1000);
                }
            });

            Console.ReadLine();
        }
    }
}
