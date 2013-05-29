using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public class CartesianMapper
    {
        private readonly Point _viewPortSize;

        public CartesianMapper(Point viewPortSize)
        {
            _viewPortSize = viewPortSize;
        }

        public Point CartesianToScreen(Point point)
        {
            var newY = _viewPortSize.Y - point.Y;
            var mappedPoint = new Point(point.X, newY, PointType.Screen);
            return mappedPoint;
        }
    }
}