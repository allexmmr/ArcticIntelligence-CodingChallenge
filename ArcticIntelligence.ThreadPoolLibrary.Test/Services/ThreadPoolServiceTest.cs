using ArcticIntelligence.ThreadPoolLibrary.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace ArcticIntelligence.ThreadPoolLibrary.Test.Services
{
    public class ThreadPoolServiceTest
    {
        private readonly ThreadPoolService _threadPoolService;

        public ThreadPoolServiceTest()
        {
            // Arrange
            Mock<ILogger<ThreadPoolService>> mockLogger = new Mock<ILogger<ThreadPoolService>>();
            _threadPoolService = new ThreadPoolService(mockLogger.Object);
        }

        [Fact]
        public void ExecuteOneTask_ReturnsTrue()
        {
            TaskService task1 = new TaskService(null, () =>
            {
                Thread.Sleep(1000);
            });

            bool actual = _threadPoolService.Execute(task1);

            Assert.True(actual);
        }

        [Fact]
        public void ExecuteThreeTasks_ReturnsTrue()
        {
            // Task 1
            TaskService task1 = new TaskService(null, () =>
            {
                Thread.Sleep(1000);
            });

            bool actual1 = _threadPoolService.Execute(task1);

            Assert.True(actual1);

            // Task 2
            TaskService task2 = new TaskService(null, () =>
            {
                Thread.Sleep(1000);
            });

            bool actual2 = _threadPoolService.Execute(task2);

            Assert.True(actual2);

            // Task 3
            TaskService task3 = new TaskService(null, () =>
            {
                Thread.Sleep(1000);
            });

            bool actual3 = _threadPoolService.Execute(task3);

            Assert.True(actual3);
        }
    }
}