namespace HereticalSolutions.Pools
{
    public interface IMetadataCollection
    {
        bool Has<TValue>();

        TValue Get<TValue>();
    }
}