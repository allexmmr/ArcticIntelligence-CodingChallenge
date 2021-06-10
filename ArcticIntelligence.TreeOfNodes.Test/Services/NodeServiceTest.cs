using ArcticIntelligence.TreeOfNodes.Models;
using ArcticIntelligence.TreeOfNodes.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ArcticIntelligence.TreeOfNodes.Test.Services
{
    public class NodeServiceTest
    {
        private readonly NodeService _nodeService;

        public NodeServiceTest()
        {
            // Arrange
            Mock<ILogger<NodeService>> mockLogger = new Mock<ILogger<NodeService>>();
            _nodeService = new NodeService(mockLogger.Object);
        }

        [Fact]
        public void CreateNode_Returns36()
        {
            Node node = _nodeService.Create(1);
            node.Left = _nodeService.Create(2);
            node.Right = _nodeService.Create(3);
            node.Left.Left = _nodeService.Create(4);
            node.Left.Right = _nodeService.Create(5);
            node.Right.Left = _nodeService.Create(6);
            node.Right.Right = _nodeService.Create(7);
            node.Right.Left.Right = _nodeService.Create(8);

            int actual = _nodeService.Sum(node);

            // Assert
            Assert.True(actual > 0);
            Assert.Equal(36, actual);
        }

        [Fact]
        public void CreateNode_Returns6()
        {
            Node node = _nodeService.Create(1);
            node.Left = _nodeService.Create(2);
            node.Right = _nodeService.Create(3);

            int actual = _nodeService.Sum(node);

            // Assert
            Assert.True(actual > 0);
            Assert.Equal(6, actual);
        }

        [Fact]
        public void CreateNode_Returns1()
        {
            Node node = _nodeService.Create(1);

            int actual = _nodeService.Sum(node);

            // Assert
            Assert.True(actual > 0);
            Assert.Equal(1, actual);
        }
    }
}