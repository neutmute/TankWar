using TankWar.Engine.Objects;

namespace TankWar.Engine.Interfaces
{
    public interface IGamepadClients
    {
        void NotifyGameStatus(GameStatus gameStatus, int countdown);

        void PushPlayerStatus(Player player);
    }
}