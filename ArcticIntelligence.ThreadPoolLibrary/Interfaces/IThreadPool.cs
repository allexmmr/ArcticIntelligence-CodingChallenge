using ArcticIntelligence.ThreadPoolLibrary.Services;
using System.Collections.Generic;

namespace ArcticIntelligence.ThreadPoolLibrary.Interfaces
{
    public interface IThreadPool
    {
        /// <summary>
        /// Create a task.
        /// </summary>
        /// <param name="task">Task Service.</param>
        void AddTask(TaskService task);

        /// <summary>
        /// Add a range of tasks.
        /// </summary>
        /// <param name="tasks">IEnumerable of Task Service.</param>
        void AddTaskRange(IEnumerable<TaskService> tasks);

        /// <summary>
        /// Delete a task.
        /// </summary>
        /// <param name="task">Task Service.</param>
        void DeleteTask(TaskService task);

        /// <summary>
        /// Execute a task.
        /// </summary>
        /// <param name="task">Task Service.</param>
        /// <returns>Returns whether has executed it or not.</returns>
        bool Execute(TaskService task);

        /// <summary>
        /// Execute a range of tasks.
        /// </summary>
        /// <param name="task">IEnumerable of Task Service.</param>
        /// <returns>Returns whether has executed it or not.</returns>
        bool ExecuteRange(IEnumerable<TaskService> tasks);

        /// <summary>
        /// Stop a task.
        /// </summary>
        void Stop();

        /// <summary>
        /// Dispose a task.
        /// </summary>
        void Dispose();
    }
}