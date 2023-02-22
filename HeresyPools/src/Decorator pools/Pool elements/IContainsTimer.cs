using HereticalSolutions.Timers;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Pools
{
	public interface ITimerContainable
	{
		Timer Timer { get; }

		ITimerContainableTimerExpiredNotifiable Callback { get; set; }
	}
}