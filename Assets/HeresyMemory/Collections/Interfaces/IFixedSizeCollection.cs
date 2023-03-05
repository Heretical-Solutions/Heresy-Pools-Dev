namespace HereticalSolutions.Collections
{
    public interface IFixedSizeCollection<T>
    {
        int Capacity { get; }

        T ElementAt(int index);
    }
}