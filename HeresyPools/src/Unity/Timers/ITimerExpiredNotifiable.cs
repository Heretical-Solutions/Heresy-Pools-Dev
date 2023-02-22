namespace HereticalSolutions.Timers
{
	public interface ITimerExpiredNotifiable
	{
		void HandleTimerExpired(Timer timer);
	}
}