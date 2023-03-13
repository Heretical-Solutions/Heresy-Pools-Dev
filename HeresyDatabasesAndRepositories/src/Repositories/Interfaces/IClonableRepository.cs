namespace HereticalSolutions.Repositories
{
    public interface IClonableRepository<TKey, TValue>
    {
        IRepository<TKey, TValue> Clone();
    }
}