using System.Collections.Generic;

namespace RocketLandingPlatform
{
    public class LastCheckedPoint
    {
        public LastCheckedPoint(int x, int y)
        {
            LastCheckedCoordinate = new Coordinate(x, y);
            CalculateNeighborCoordinates();
        }
        private Coordinate LastCheckedCoordinate { get; }
        public List<Coordinate> NeighborCoordinates { get; set; } = new();

        private void CalculateNeighborCoordinates()
        {
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X - 1, LastCheckedCoordinate.Y - 1));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X - 1, LastCheckedCoordinate.Y));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X - 1, LastCheckedCoordinate.Y + 1));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X, LastCheckedCoordinate.Y - 1));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X, LastCheckedCoordinate.Y));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X, LastCheckedCoordinate.Y + 1));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X + 1, LastCheckedCoordinate.Y - 1));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X + 1, LastCheckedCoordinate.Y));
            NeighborCoordinates.Add(new Coordinate(LastCheckedCoordinate.X + 1, LastCheckedCoordinate.Y + 1));

        }
    }
}
