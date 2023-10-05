using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;

namespace Job;


public class Scheduler : IDisposable
{
    internal struct Job
    {
        public readonly Action execute;


        public Job(Action execute)
        {
            this.execute = execute;
        }
    }

    private readonly LinkedList<Job> jobs;
    private readonly object jobsLock;
    private readonly Thread thread;
    private bool isRunning;


    public Scheduler()
    {
        jobs = new LinkedList<Job>();
        jobsLock = new object();
        isRunning = true;
        thread = new Thread(ThreadProc_ProcessJob);
    }

    void IDisposable.Dispose()
    {
        isRunning = false;
        thread.Join();
    }

    private void ThreadProc_ProcessJob()
    {
        while (isRunning)
        {
            Job currentJob;

            lock (jobsLock)
            {
                if (jobs.Count == 0)
                    continue;

                currentJob = jobs.First();
            }
        }
    }

    internal JobHandle Schedule(IJob job, JobHandle dependOn = default)
    {
        return Schedule(job.Execute, dependOn);
    }

    internal JobHandle Schedule(IJobParallelFor job, int count, JobHandle dependOn = default)
    {
        void execute() => Parallel.For(0, count, job.Execute);

        return Schedule(execute, dependOn);
    }

    private JobHandle Schedule(Action execute, JobHandle dependOn)
    {
        LinkedListNode<Job> newJobNode = new LinkedListNode<Job>(new Job(execute));
        JobHandle handle = new JobHandle(newJobNode);

        lock (jobsLock)
        {
            if (dependOn.jobNode != null)
                jobs.AddAfter(dependOn.jobNode, newJobNode);
            else
                jobs.AddLast(newJobNode);
        }

        return handle;
    }
}