using Hb.MarsRover.Domain.Types;

namespace MarsRover.Domain.DomainModels
{
    public class Command : Enumeration
    {
        public static Command Left = new Command(1,"L", "Left");
        public static Command Right = new Command(2,"R", "Right");
        public static Command Move = new Command(3,"M", "Move");
        public static Command UnRecognizedCommand = new Command(4,"U", "Unrecognized Command");
        public Command(int enumId, string code, string name) : base(enumId, code, name)
        {
        }
    }
}
