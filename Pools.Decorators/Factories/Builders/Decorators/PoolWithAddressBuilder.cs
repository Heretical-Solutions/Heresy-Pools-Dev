using System;

using HereticalSolutions.Pools.Factories.Internal;
using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Pools.Factories
{
    public class PoolWithAddressBuilder<T>
    {
	    private AddressTreeNode<T> root;
	    
	    public void Initialize()
	    {
		    root = new AddressTreeNode<T>
		    {
			    CurrentAddress = string.Empty,
			    
			    FullAddress = string.Empty,
			    
			    CurrentAddressHash = -1,
			    
			    FullAddressHashes = new int[0],

			    Level = 0,
			    
			    Pool = null
		    };
	    }

		public void Parse(
			string address,
			INonAllocDecoratedPool<T> pool)
		{
			string[] addressParts = address.SplitAddressBySeparator();
			
			int[] addressHashes = address.AddressToHashes();

			AddressTreeNode<T> currentNode = root;

			for (int i = 0; i < addressHashes.Length; i++)
			{
				bool traversed = TraverseToChildNode(
					ref currentNode,
					addressHashes[i]);

				if (!traversed)
					CreateAndTraverse(
						addressParts,
						addressHashes,
						ref currentNode,
						addressHashes[i]);
			}

			currentNode.Pool = pool;
		}

		private bool TraverseToChildNode(
			ref AddressTreeNode<T> currentNode,
			int targetAddressHash)
		{
			for (int i = 0; i < currentNode.Children.Count; i++)
			{
				if (currentNode.Children[i].CurrentAddressHash == targetAddressHash)
				{
					currentNode = currentNode.Children[i];

					return true;
				}
			}

			return false;
		}

		private void CreateAndTraverse(
			string[] addressParts,
			int[] addressHashes,
			ref AddressTreeNode<T> currentNode,
			int targetAddressHash)
		{
			int currentNodeLevel = currentNode.Level;
			
			AddressTreeNode<T> child = new AddressTreeNode<T>
			{
				CurrentAddress = addressParts.AddressAtDepth(currentNodeLevel),
				
				FullAddress = addressParts.PartialAddress(currentNodeLevel),
				
				CurrentAddressHash = targetAddressHash,
				
				FullAddressHashes = addressHashes.PartialAddressHashes(currentNodeLevel),

				Level = currentNodeLevel + 1,
				
				Pool = null
			};

			currentNode.Children.Add(child);

			currentNode = child;
		}

		public INonAllocDecoratedPool<T> Build()
		{
			if (root == null)
				throw new Exception("[PoolWithAddressBuilder] BUILDER NOT INITIALIZED");

			var result = BuildPoolWithAddress(root);

			root = null;

			return result;
		}

		private INonAllocDecoratedPool<T> BuildPoolWithAddress(
			AddressTreeNode<T> node)
		{
			IRepository<int, INonAllocDecoratedPool<T>> repository =
				RepositoriesFactory.BuildDictionaryRepository<int, INonAllocDecoratedPool<T>>();

			foreach (var child in node.Children)
			{
				repository.Add(child.CurrentAddressHash, BuildPoolWithAddress(child));
			}

			if (node.Pool != null)
			{
				repository.Add(0, node.Pool);
			}

			return PoolsFactory.BuildNonAllocPoolWithIdAddress(repository, node.Level);
		}
    }
}