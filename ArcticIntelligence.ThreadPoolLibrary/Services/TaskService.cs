using ArcticIntelligence.ThreadPoolLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace ArcticIntelligence.ThreadPoolLibrary.Services
{
    /// <summary>
    /// Task Service.
    /// </summary>
    public class TaskService : ITaskExecuter
    {
        private readonly ILogger<TaskService> _logger;

        private readonly Action _action;
        public Action Action => _action;

        private bool _isInUse;
        public bool IsInUse
        {
            get
            {
                return _isInUse;
            }
        }

        /// <summary>
        /// Task Service Constructor.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="action">Action.</param>
        public TaskService(ILogger<TaskService> logger, Action action)
        {
            _logger = logger;
            _action = action;
        }

        /// <summary>
        /// Execute a task.
        /// </summary>
        public void Execute()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Execute));

            lock (this)
            {
                _isInUse = true;
            }

            Action();
        }
    }
}