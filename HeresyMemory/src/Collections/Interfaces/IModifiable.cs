namespace HereticalSolutions.Collections
{
	public interface IModifiable<TCollection>
	{
		TCollection Contents { get; }
		
		void UpdateContents(TCollection newContents);
	}
}