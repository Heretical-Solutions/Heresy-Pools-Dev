using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
    public class IndexedMetadata : IIndexed
    {
        public int Index { get; set; }

        public IndexedMetadata()
        {
            Index = -1;
        }
    }
}