using HereticalSolutions.Pools.AllocationCallbacks;

namespace HereticalSolutions.Pools.Factories
{
    public static partial class PoolsFactory
    {
        #region Allocation callbacks

        public static RenameByStringAndIndexCallback BuildRenameByStringAndIndexCallback(string name)
        {
            return new RenameByStringAndIndexCallback(name);
        }

        #endregion
    }
}