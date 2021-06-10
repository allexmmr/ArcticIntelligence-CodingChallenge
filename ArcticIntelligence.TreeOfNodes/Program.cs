using ArcticIntelligence.TreeOfNodes.Models;
using ArcticIntelligence.TreeOfNodes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ArcticIntelligence.TreeOfNodes
{
    public class Program
    {
        public static void Main()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            NodeService nodeService = serviceProvider.GetService<NodeService>();

            try
            {
                Node node = nodeService.Create(1);
                node.Left = nodeService.Create(2);
                node.Right = nodeService.Create(3);
                node.Left.Left = nodeService.Create(4);
                node.Left.Right = nodeService.Create(5);
                node.Right.Left = nodeService.Create(6);
                node.Right.Right = nodeService.Create(7);
                node.Right.Left.Right = nodeService.Create(8);

                int sum = nodeService.Sum(node);

                Console.WriteLine($"The sum of all the elements is {sum}.");
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
                .AddTransient<NodeService>();
        }
    }
}