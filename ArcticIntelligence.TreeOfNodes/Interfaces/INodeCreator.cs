using ArcticIntelligence.TreeOfNodes.Models;

namespace ArcticIntelligence.TreeOfNodes.Interfaces
{
    /// <summary>
    /// Interface of INodeCreator.
    /// </summary>
    public interface INodeCreator
    {
        /// <summary>
        /// Create a new Node with the given key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Returns the node.</returns>
        Node Create(int key);
    }
}