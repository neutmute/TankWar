namespace TankWar.Engine.Interfaces
{
    public interface IViewPortClients
    {
        void InitGame(ViewPortGameState viewPortGameState);

        void Tick(ViewPortGameState viewPortGameState);
    }
}