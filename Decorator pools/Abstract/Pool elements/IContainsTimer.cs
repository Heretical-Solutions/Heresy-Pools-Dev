using HereticalSolutions.Timers;

namespace HereticalSolutions.Pools
{
	public interface IContainsTimer
	{
		ITimer Timer { get; }

		ITimerExpiredNotifier Callback { get; set; }
	}
}