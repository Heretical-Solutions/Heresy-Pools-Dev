using System;

namespace HereticalSolutions.RandomGeneration
{
    public class SystemRandomGenerator : IRandomGenerator
    {
        private Random random;

        public SystemRandomGenerator()
        {
            //Courtesy of https://stackoverflow.com/questions/1785744/how-do-i-seed-a-random-class-to-avoid-getting-duplicate-random-values
            random = new Random(Guid.NewGuid().GetHashCode());
        }

        public float Random(float min, float max)
        {
            //Courtesy of https://stackoverflow.com/questions/3365337/best-way-to-generate-a-random-float-in-c-sharp

            // Perform arithmetic in double type to avoid overflowing
            double range = (double) max - (double) min;
            double sample = random.NextDouble();
            double scaled = (sample * range) + min;
            float result = (float) scaled;

            return result;
        }
    }
}