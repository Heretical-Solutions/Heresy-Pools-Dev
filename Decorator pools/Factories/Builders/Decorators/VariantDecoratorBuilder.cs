using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;
using UnityEngine;

namespace HereticalSolutions.Pools.Factories
{
    public class VariantDecoratorBuilder
    {
        private const string KEY_DESCRIPTOR = "Descriptor";

		private const string KEY_DESCRIPTORS = "Descriptors";

		private const string KEY_PREFAB = "Prefab";

		private const string KEY_VARIANT = "Variant";


		public INonAllocDecoratedPool<GameObject> Assemble(
			AssemblyTicket<INonAllocDecoratedPool<GameObject>> ticket,
			int level)
		{
			//Retrieve arguments
			var descriptors = ticket.Arguments.Get(KEY_DESCRIPTORS);


			//Next shop
			var nextShop = ticket.AssemblyLine[++level];


			//Create repository
			IRepository<int, VariantContainer<GameObject>> repository = RepositoriesFactory.BuildDictionaryRepository<int, VariantContainer<GameObject>>();


			//Fill repository
			var variantDescriptors = //(RuntimeGameObjectPoolWithVariantsSample.VariantDescriptor[])descriptors;

			for (int i = 0; i < variantDescriptors.Length; i++)
			{
				var currentVariant = variantDescriptors[i];

				/*
				var nextShopTicket = TicketFactory.BuildNextShopTicket<INonAllocDecoratedPool<GameObject>>(ticket);

				nextShopTicket.Arguments.Remove(KEY_DESCRIPTORS);

				nextShopTicket.Arguments.Add(KEY_PREFAB, currentVariant.Prefab);

				nextShopTicket.Arguments.Add(KEY_VARIANT, i);
				*/
				
				repository.Add(
					i,
					new VariantContainer<GameObject>
					{
						Chance = currentVariant.Chance,

						Pool = nextShop.Assemble(nextShopTicket, level)
					});
			}


			//Return
			var repository = new DictionaryRepository<int, VariantContainer<GameObject>>(repository);

			var chain = new NonAllocDecoratorPoolChain<GameObject>();

			chain
				.Add(new PoolWithVariants<GameObject>(repository));

			return chain.TopWrapper;
		}
    }
}