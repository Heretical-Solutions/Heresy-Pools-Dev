namespace HereticalSolutions.Timers
{
    public interface ITimer
    {
        float Countdown { get; }

        float DefaultDuration { get; }

        ITimerExpiredNotifiable Callback { get; set; }

        void Start();

        void Start(float duration);
    }
}