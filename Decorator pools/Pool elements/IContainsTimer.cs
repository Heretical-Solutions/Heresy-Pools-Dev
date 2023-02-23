using HereticalSolutions.Timers;

namespace HereticalSolutions.Pools
{
	public interface ITimerContainable
	{
		Timer Timer { get; }

		ITimerExpiredNotifier Callback { get; set; }
	}
}