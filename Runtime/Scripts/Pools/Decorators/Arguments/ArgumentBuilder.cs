using System;
using System.Collections.Generic;
using UnityEngine;

namespace HereticalSolutions.Pools.Arguments
{
	public class ArgumentBuilder
	{
		private List<IPoolDecoratorArgument> argumentChain = new List<IPoolDecoratorArgument>();

		public ArgumentBuilder Add<TArgument>(out TArgument instance) where TArgument : IPoolDecoratorArgument
		{
			instance = (TArgument)Activator.CreateInstance(typeof(TArgument));

			argumentChain.Add(instance);

			return this;
		}

		public IPoolDecoratorArgument[] Build()
		{
			return argumentChain.ToArray();
		}
	}
}