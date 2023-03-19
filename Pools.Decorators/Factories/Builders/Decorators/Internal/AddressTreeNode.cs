using System.Collections.Generic;

namespace HereticalSolutions.Pools.Factories.Internal
{
    public class AddressTreeNode<T>
    {
        public int Level = 0;

        public List<AddressTreeNode<T>> Children = new List<AddressTreeNode<T>>();
        
        public string CurrentAddress;

        public string FullAddress;
        
        public int CurrentAddressHash = -1;

        public int[] FullAddressHashes = null;

        public INonAllocDecoratedPool<T> Pool;
    }
}