namespace Job;


public struct JobHandle
{
    internal readonly LinkedListNode<Scheduler.Job>? jobNode;

    internal JobHandle(LinkedListNode<Scheduler.Job> node)
    {
        this.jobNode = node;
    }
}