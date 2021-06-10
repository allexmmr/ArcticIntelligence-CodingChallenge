using ArcticIntelligence.ThreadPoolLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArcticIntelligence.ThreadPoolLibrary.Services
{
    /// <summary>
    /// Thread Pool Service.
    /// </summary>
    public class ThreadPoolService : IThreadPool
    {
        #region Declarations

        private readonly ILogger<ThreadPoolService> _logger;
        private readonly List<Task> _taskList = new List<Task>();
        private readonly List<TaskService> _taskServiceList = new List<TaskService>();
        private readonly Dictionary<int?, ManualResetEvent> _threadsEvent = new Dictionary<int?, ManualResetEvent>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private int _numberOfThreads;
        private int _countActiveThreads;
        private bool _isStopped;
        private bool _isDisposed;

        private readonly object _lockObject = new object();
        private readonly object _lockObjectCount = new object();

        #endregion

        /// <summary>
        /// Thread Pool Constructor.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="numberOfThreads">Number maximum of threads.</param>
        public ThreadPoolService(ILogger<ThreadPoolService> logger, int numberOfThreads = 5)
        {
            _logger = logger;
            _numberOfThreads = numberOfThreads;

            for (int i = 0; i < _numberOfThreads; i++)
            {
                CreateThread();
            }
        }

        /// <summary>
        /// Create a task.
        /// </summary>
        /// <param name="task">Task Service.</param>
        public void AddTask(TaskService task)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(AddTask));

            lock (_taskServiceList)
            {
                _taskServiceList.Add(task);
            }
        }

        /// <summary>
        /// Add a range of tasks.
        /// </summary>
        /// <param name="tasks">IEnumerable of Task Service.</param>
        public void AddTaskRange(IEnumerable<TaskService> tasks)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(AddTaskRange));

            lock (_taskServiceList)
            {
                _taskServiceList.AddRange(tasks);
            }
        }

        /// <summary>
        /// Delete a task.
        /// </summary>
        /// <param name="task">Task Service.</param>
        public void DeleteTask(TaskService task)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(DeleteTask));

            lock (_taskServiceList)
            {
                _taskServiceList.Remove(task);
            }
        }

        /// <summary>
        /// Execute a task.
        /// </summary>
        /// <param name="task">Task Service.</param>
        /// <returns>Returns whether has executed it or not.</returns>
        public bool Execute(TaskService task)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Execute));

            lock (_lockObject)
            {
                if (!_isStopped)
                {
                    _logger?.LogInformation("Start a new task.");
                    AddTask(task);
                    StartTask();

                    return true;
                }
                else
                {
                    _logger?.LogError("Failed to execute the task.");

                    return false;
                }
            }
        }

        /// <summary>
        /// Execute a range of tasks.
        /// </summary>
        /// <param name="task">IEnumerable of Task Service.</param>
        /// <returns>Returns whether has executed it or not.</returns>
        public bool ExecuteRange(IEnumerable<TaskService> tasks)
        {
            lock (_lockObject)
            {
                if (!_isStopped)
                {
                    _logger?.LogInformation("Start {0} new tasks.", tasks.Count());

                    AddTaskRange(tasks);

                    for (int i = 0; i < tasks.Count(); i++)
                    {
                        StartTask();
                    }

                    return true;
                }
                else
                {
                    _logger?.LogError("Failed to execute the task.");

                    return false;
                }
            }
        }

        /// <summary>
        /// Stop a task.
        /// </summary>
        public void Stop()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Stop));

            lock (_lockObject)
            {
                _isStopped = true;
            }

            int count = 0;

            do
            {
                lock (_lockObjectCount)
                {
                    count = _countActiveThreads;
                }
            }
            while (count > 0);

            Dispose();
        }

        /// <summary>
        /// Dispose a task.
        /// </summary>
        public void Dispose()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Dispose));

            if (!_isDisposed)
            {
                _tokenSource.Cancel();

                foreach (Task task in _taskList)
                {
                    _threadsEvent[task.Id].Set();
                    _threadsEvent[task.Id].Dispose();
                }

                try
                {
                    Task.WaitAll(_taskList.ToArray());
                }
                catch (AggregateException ex)
                {
                    _logger?.LogError(ex.Message);
                }
                finally
                {
                    _tokenSource.Dispose();
                }

                _isDisposed = true;
            }
        }

        private void StartTask()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(StartTask));

            lock (_taskServiceList)
            {
                bool isAvailable = false;

                foreach (Task task in _taskList)
                {
                    if (_threadsEvent[task.Id].WaitOne(0) == false)
                    {
                        isAvailable = true;
                        _threadsEvent[task.Id].Set();

                        lock (_lockObjectCount)
                        {
                            _countActiveThreads++;
                        }

                        break;
                    }
                }

                if (!isAvailable)
                {
                    lock (_lockObjectCount)
                    {
                        _countActiveThreads++;
                    }

                    _threadsEvent[CreateThread()].Set();
                }
            }
        }

        private int CreateThread()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(CreateThread));

            _countActiveThreads++;

            if (_countActiveThreads > _numberOfThreads)
            {
                _logger?.LogWarning("The allowed number of threads has been exceeded.");

                _numberOfThreads++;
            }

            Task task = new Task(ThreadWork, _tokenSource.Token, TaskCreationOptions.LongRunning);
            _threadsEvent.Add(task.Id, new ManualResetEvent(false));
            task.Start();
            _taskList.Add(task);

            return task.Id;
        }

        private void ThreadWork()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(ThreadWork));

            while (!_tokenSource.IsCancellationRequested)
            {
                _threadsEvent[Task.CurrentId].WaitOne();
                TaskService task = SelectTask();

                if (task != null)
                {
                    try
                    {
                        _logger?.LogInformation("Start executing Task ID {0}.", Task.CurrentId);

                        task.Execute();

                        _logger?.LogInformation("End executing Task ID {0}.", Task.CurrentId);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex.Message);
                    }
                    finally
                    {
                        _threadsEvent[Task.CurrentId].Reset();

                        lock (_lockObjectCount)
                        {
                            _countActiveThreads--;
                        }
                    }
                }
            }
        }

        private TaskService SelectTask()
        {
            TaskService task = null;

            lock (_taskServiceList)
            {
                IEnumerable<TaskService> waitingTask = _taskServiceList.Where(x => !x.IsInUse);

                if (waitingTask.Count() > 0)
                {
                    task = waitingTask.FirstOrDefault();

                    DeleteTask(task);
                }
            }

            return task;
        }
    }
}