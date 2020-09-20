using System;
using Hb.MarsRover.Domain.Types;
using MarsRover.Domain.Exceptions;

namespace MarsRover.Domain.DomainModels
{
    public class Plateau : Entity, IAggregateRoot<Guid>
    {
        private int _xCoordinate;
        private int _yCoordinate;
        public Coordinate Coordinate { get; set; }


        public Plateau(Guid id, int xCoordinate, int yCoordinate)
        {
            if (xCoordinate <= 0 || yCoordinate <= 0)
                throw new PlateauDomainException("Invalid Coordinates");
            Id = id;
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            InitializePlateau(_xCoordinate, _yCoordinate);

        }
        public void InitializePlateau(int x, int y)
        {
            Coordinate = new Coordinate(x, y);
        }
        public override string ToString()
        {
            return $"{Coordinate.XCoordinate} {Coordinate.YCoordinate}";
        }
    }
}
