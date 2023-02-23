namespace HereticalSolutions.Timers
{
	public interface ITimerExpiredNotifiable
	{
		void HandleTimerExpired(ITimer timer);
	}
}