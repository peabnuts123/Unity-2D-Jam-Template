using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ThreadedCoroutine : MonoBehaviour
{
    // Delegates
    public delegate void WorkFinished(ThreadContainer threadContainer);


    // Public config
    public float tickIntervalSeconds;

    // Internal state
    private Coroutine coroutine;
    private IList<ThreadContainer> threadContainers;

    // Lifecycle
    void OnEnable()
    {
        // Setup list of containers
        threadContainers = new List<ThreadContainer>();
        // Start coroutine tick function
        coroutine = StartCoroutine(Tick());
    }

    void OnDisable()
    {
        Stop();
    }

    // Add work to be execute on the thread pool
    public void AddWork(Action<ThreadContainer> workFunction, WorkFinished onWorkFinished)
    {
        // Create struct for passing args into ThreadFunction
        ThreadFunctionArgs args = new ThreadFunctionArgs
        {
            work = workFunction,
            container = new ThreadContainer
            {
                OnWorkFinished = onWorkFinished,
            },
        };

        // Queue work item on ThreadPool
        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadFunction), args);

        // Add container to set of currently running containers
        threadContainers.Add(args.container);

    }

    // Coroutine tick function
    private IEnumerator Tick()
    {
        bool haveAnyContainersFinished = false;
        while (true)
        {
            haveAnyContainersFinished = false;

            // Check if any containers are finished processing
            // @NOTE we are wording this carefully to allow `OnWorkFinished` callbacks
            //  to add more work without upsetting this for-loop
            //  We definitely do NOT remove any containers within this loop, however.
            // Store reference to number of containers so that if this count changes our loop won't process them
            int numContainers = threadContainers.Count;
            ThreadContainer threadContainer;
            for (int containerIndex = 0; containerIndex < numContainers; containerIndex++)
            {
                threadContainer = threadContainers[containerIndex];
                if (threadContainer.HasFinished)
                {
                    // Invoke callbacks for when this work has finished
                    //  with the finished container
                    if (threadContainer.OnWorkFinished != null)
                    {
                        threadContainer.OnWorkFinished(threadContainer);
                    }

                    // Mark container to be deleted
                    threadContainer.MarkForDeletion();
                    haveAnyContainersFinished = true;
                }
                else if (threadContainer.HasError)
                {
                    Debug.LogError(threadContainer.Error.ToString());

                    // Mark container to be deleted
                    threadContainer.MarkForDeletion();
                    haveAnyContainersFinished = true;
                }
            }

            // Only filter / reassign list if any containers have actually finished
            //  as we don't want to do this too often
            if (haveAnyContainersFinished)
            {
                threadContainers = threadContainers.Where((ThreadContainer container) => !container.MarkedForDeletion).ToList();
            }

            // Check back later
            yield return new WaitForSecondsRealtime(tickIntervalSeconds);
        }
    }

    // Function invoked be every thread
    static void ThreadFunction(object inputObject)
    {
        // Get actual arguments from Object parameter
        ThreadFunctionArgs args = (ThreadFunctionArgs)inputObject;

        try
        {

            // Call work function with container reference
            args.work(args.container);

            // Mark thread as finished
            args.container.HasFinished = true;
        }
        catch (Exception e)
        {
            args.container.HasError = true;
            args.container.Error = e;
        }
    }

    // Stop calling Tick
    //  Active threads will continue running but will not call back
    public void Stop()
    {
        StopCoroutine(coroutine);
    }

    // Struct for passing 2 arguments into ThreadFunction
    private struct ThreadFunctionArgs
    {
        public Action<ThreadContainer> work;
        public ThreadContainer container;
    }
}

public class ThreadContainer
{
    // Used to identify container
    //  For debugging purposes
    public readonly int Id;
    // Whether the associated work for this container
    //  has finished processing
    public bool HasFinished;
    // Whether the associated work for this container
    //  failed to process and threw an exception
    public bool HasError;
    // Error instance, stored when any error occurs
    public Exception Error;
    // String-keyed collection of results, populated
    //  by thread-run work function, and read from
    //  main Unity thread
    private IDictionary<string, object> Results;
    private bool markedForDeletion;


    public ThreadedCoroutine.WorkFinished OnWorkFinished;

    public ThreadContainer()
    {
        this.Id = UnityEngine.Random.Range(0, 10000);
        Results = new Dictionary<string, object>();
    }

    // Abstractions over accessing the Results dictionary
    public void StoreResult(string name, object value)
    {
        Results.Add(name, value);
    }
    public T GetResult<T>(string name)
    {
        return (T)Results[name];
    }
    public void MarkForDeletion()
    {
        this.markedForDeletion = true;
    }
    public bool MarkedForDeletion
    {
        get { return this.markedForDeletion; }
    }
}