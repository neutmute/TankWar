namespace TankWar.Engine
{
    public class Turret
    {
        public int Angle { get; set; }

        public int Power { get; set; }

        public override string ToString()
        {
            return string.Format("A={0}, P={1}", Angle, Power);
        }
    }
}