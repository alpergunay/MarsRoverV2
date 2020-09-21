using System;
using System.Runtime.Serialization;
using Hb.MarsRover.Domain.Types;
using MarsRover.Domain.Exceptions;

namespace MarsRover.Domain.DomainModels
{
    public class Plateau : Entity, IAggregateRoot<Guid>
    {
        [DataMember]
        //private int _xCoordinate;
        public int XCoordinate { get; set; }
        [DataMember]
        //private int _yCoordinate;
        public int YCoordinate { get; set; }
        [IgnoreDataMember]
        public Coordinate Coordinate { get; set; }


        public Plateau(Guid id, int xCoordinate, int yCoordinate)
        {
            if (xCoordinate <= 0 || yCoordinate <= 0)
                throw new PlateauDomainException("Invalid Coordinates");
            Id = id;
            //_xCoordinate = xCoordinate;
            //_yCoordinate = yCoordinate;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            InitializePlateau(XCoordinate, YCoordinate);

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
