namespace HereticalSolutions.Pools
{
    public interface IPushable<T>
    {
        EPoolElementStatus Status { set; }

        void UpdatePushBehaviour(IPushBehaviourHandler<T> pushBehaviourHandler);
    }
}