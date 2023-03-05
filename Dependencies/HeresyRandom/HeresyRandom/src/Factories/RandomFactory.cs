namespace HereticalSolutions.RandomGeneration.Factories
{
    public static partial class RandomFactory
    {
        public static SystemRandomGenerator BuildSystemRandomGenerator()
        {
            return new SystemRandomGenerator();
        }
    }
}