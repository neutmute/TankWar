namespace TankWar.Engine.Interfaces
{
    public interface IViewPortClients
    {
        void StartGame(ViewPortState viewPortState);

        void Tick(ViewPortState viewPortState);
    }
}