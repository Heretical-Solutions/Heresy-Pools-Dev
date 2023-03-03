namespace HereticalSolutions.Pools
{
    public interface IMetadata
    {
        bool Has<TValue>();

        TValue Get<TValue>();
    }
}