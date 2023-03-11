namespace HereticalSolutions.Pools.Factories
{
    public interface IDecoratorBuilder<TValue>
    {
        TValue Build(); //(AssemblyTicket<TValue> ticket, int level);
    }
}