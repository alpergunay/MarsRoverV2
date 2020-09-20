using Hb.MarsRover.Domain.Types;

namespace MarsRover.Domain.DomainModels
{
    public class Direction : Enumeration
    {
        public static Direction E = new Direction(1, "E", "East");
        public static Direction W = new Direction(2, "W", "West");
        public static Direction N = new Direction(3, "N", "North");
        public static Direction S = new Direction(4, "S", "South");
        public Direction(int enumId, string code, string name) : base(enumId, code, name)
        {
            
        }
    }
}
