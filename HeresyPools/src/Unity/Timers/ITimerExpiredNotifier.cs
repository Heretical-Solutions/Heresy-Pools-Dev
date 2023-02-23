namespace HereticalSolutions.Pools
{
	public interface ITimerExpiredNotifier
	{
		void NotifyTimerExpired(ITimerContainable timerContainable);
	}
}