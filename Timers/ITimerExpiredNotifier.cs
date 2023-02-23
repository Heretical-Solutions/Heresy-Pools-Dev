namespace HereticalSolutions.Pools
{
	public interface ITimerExpiredNotifier
	{
		void NotifyTimerExpired(IContainsTimer containsTimer);
	}
}