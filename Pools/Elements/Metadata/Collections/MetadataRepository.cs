using System;

using HereticalSolutions.Repositories;

namespace HereticalSolutions.Pools.Metadata
{
    public class MetadataRepository : IMetadataCollection
    {
        private readonly IReadOnlyRepository<Type, object> repository;

        public MetadataRepository(IReadOnlyRepository<Type, object> repository)
        {
            this.repository = repository;
        }

        public bool Has<TValue>()
        {
            return repository.Has(typeof(TValue));
        }

        public TValue Get<TValue>()
        {
            return (TValue)repository.Get(typeof(TValue));
        }
    }
}