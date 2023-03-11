namespace HereticalSolutions.Repositories
{
    public interface ICloneableObjectRepository
    {
        IObjectRepository Clone();
    }
}