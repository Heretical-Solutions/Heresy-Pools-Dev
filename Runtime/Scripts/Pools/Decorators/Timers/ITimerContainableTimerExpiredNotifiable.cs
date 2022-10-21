namespace HereticalSolutions.Pools
{
	public interface ITimerContainableTimerExpiredNotifiable
	{
		void HandleTimerContainableTimerExpired(ITimerContainable timerContainable);
	}
}