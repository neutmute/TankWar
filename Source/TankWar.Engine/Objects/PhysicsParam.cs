namespace TankWar.Engine.Objects
{
    public class PhysicsParamScreenTuneTransform : PhysicsParam
    {
        public override double Time { get { return base.Time * 5 * _gameLoopIntervalMs / 1000; } }

        private readonly int _gameLoopIntervalMs;

        public PhysicsParamScreenTuneTransform(int angle, int power, int time, int gameLoopIntervalMs)
            : base(angle, power, time)
        {
            _gameLoopIntervalMs = gameLoopIntervalMs;
        }
    }

    public class PhysicsParam
    {
        public virtual int Angle { get; private set; }
        public virtual double Power { get; private set; }
        public virtual double Time { get; private set; }

        public PhysicsParam(int angle, int power, int time)
        {
            Angle = angle;
            Power = power;
            Time = time;
        }

        public override string ToString()
        {
            return string.Format("[A={0}, P={1}, t={2}]", Angle, Power, Time);
        }
    }
}