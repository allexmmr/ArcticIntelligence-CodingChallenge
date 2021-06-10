using ArcticIntelligence.TreeOfNodes.Models;

namespace ArcticIntelligence.TreeOfNodes.Interfaces
{
    /// <summary>
    /// Interface of INodeCalculator.
    /// </summary>
    public interface INodeCalculator
    {
        /// <summary>
        /// Sum of all the elements.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Returns an int containing the sum of all the elements.</returns>
        int Sum(Node node);
    }
}