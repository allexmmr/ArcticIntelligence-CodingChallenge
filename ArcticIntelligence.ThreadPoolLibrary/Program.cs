using ArcticIntelligence.ThreadPoolLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace ArcticIntelligence.ThreadPoolLibrary
{
    public class Program
    {
        public static void Main()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                TaskService task1 = new TaskService(null, () =>
                {
                    Thread.Sleep(1000);
                });

                TaskService task2 = new TaskService(null, () =>
                {
                    Thread.Sleep(1000);
                });

                TaskService task3 = new TaskService(null, () =>
                {
                    Thread.Sleep(1000);
                });

                ThreadPoolService threadPoolService = serviceProvider.GetService<ThreadPoolService>();

                Console.WriteLine("Execute Task 1.");
                threadPoolService.Execute(task1);

                Console.WriteLine("Execute Task 2.");
                threadPoolService.Execute(task2);

                Console.WriteLine("Execute Task 3.");
                threadPoolService.Execute(task3);

                Thread.Sleep(1000);

                Console.WriteLine("Execute all tasks.");
                threadPoolService.ExecuteRange(new[] { task1, task2, task3 });
                threadPoolService.Stop();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<ThreadPoolService>();

            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<TaskService>();
        }
    }
}