namespace MarsRover.Domain.DomainModels
{
    public class Coordinate
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }

        public Coordinate(int xCoordinate, int yCoordinate)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }
        public override string ToString()
        {
            return $"{XCoordinate} {YCoordinate}";
        }
    }
}
