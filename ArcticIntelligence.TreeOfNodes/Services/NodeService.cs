using ArcticIntelligence.TreeOfNodes.Interfaces;
using ArcticIntelligence.TreeOfNodes.Models;
using Microsoft.Extensions.Logging;

namespace ArcticIntelligence.TreeOfNodes.Services
{
    /// <summary>
    /// Node Service.
    /// </summary>
    public class NodeService : INodeCreator, INodeCalculator
    {
        private readonly ILogger<NodeService> _logger;

        /// <summary>
        /// Node Service Constructor.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public NodeService(ILogger<NodeService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Create a new Node with the given key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Returns the node.</returns>
        public Node Create(int key)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Create));

            Node node = new Node
            {
                Key = key,
                Left = null,
                Right = null
            };

            return node;
        }

        /// <summary>
        /// Sum of all the elements.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Returns an int containing the sum of all the elements.</returns>
        public int Sum(Node node)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Sum));

            if (node == null)
            {
                return 0;
            }

            return node.Key + Sum(node.Left) + Sum(node.Right);
        }
    }
}