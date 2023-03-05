namespace HereticalSolutions.Pools.Arguments
{
	public static class ArgumentExtensions
	{
		public static bool TryGetArgument<TArgument>(this IPoolDecoratorArgument[] args, out TArgument value) where TArgument : IPoolDecoratorArgument
		{
			value = default(TArgument);

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] is TArgument)
				{
					value = (TArgument)args[i];

					return true;
				}
			}

			return false;
		}
	}
}