using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools.Factories
{
	public class ArgumentBuilderNonAlloc
	{
		private readonly IPoolDecoratorArgument[] argumentChain;

		private int count;

		public ArgumentBuilderNonAlloc(IPoolDecoratorArgument[] argumentChain)
		{
			this.argumentChain = argumentChain;

			count = 0;
		}

		public ArgumentBuilderNonAlloc Clean()
		{
			for (int i = 0; i < count; i++)
				argumentChain[i] = null;
			
			count = 0;

			return this;
		}

		public ArgumentBuilderNonAlloc Add(IPoolDecoratorArgument argument)
		{
			argumentChain[count] = argument;

			count++;

			return this;
		}

		public IPoolDecoratorArgument[] Build()
		{
			return argumentChain;
		}
	}
}