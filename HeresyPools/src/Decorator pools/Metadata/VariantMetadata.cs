namespace HereticalSolutions.Pools
{
    public class VariantMetadata : IContainsVariant
    {
        public int Variant { get; set; }

        public VariantMetadata()
        {
            Variant = -1;
        }
    }
}