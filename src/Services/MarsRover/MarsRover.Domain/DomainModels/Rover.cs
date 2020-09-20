using Hb.MarsRover.Domain.Types;
using MarsRover.Domain.Exceptions;
using System;
using MarsRover.Domain.Events;

namespace MarsRover.Domain.DomainModels
{
    public class Rover : Entity, IAggregateRoot<Guid>
    {
        public Direction CurrentDirection { get; set; }
        public Coordinate CurrentCoordinate { get; private set; }
        private Guid _plateauId;
        public Plateau Plateau { get; private set; }
        private readonly Guid _roverId;
        private int _xCoordinate;
        private int _yCoordinate;
        private int _directionId;

        public Rover(Guid id, int xCoordinate, int yCoordinate, int directionId, Guid plateauId)
        {
            _plateauId = plateauId;
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            _directionId = directionId;
            _roverId = id;
            InitializeCurrentCoordinate(xCoordinate, yCoordinate);
            InitializeCurrentDirection(directionId);
        }

        public void InitializeCurrentDirection(int directionId)
        {
            CurrentDirection = Enumeration.FromValue<Direction>(directionId);
        }

        public void InitializeCurrentCoordinate(int x, int y)
        {
            CurrentCoordinate = new Coordinate(x, y);
        }

        public void SetPosition(int x, int y, int direction)
        {
            CurrentCoordinate.XCoordinate = x;
            CurrentCoordinate.YCoordinate = y;
            _xCoordinate = x;
            _yCoordinate = y;
            _directionId = direction;
        }
        public void InitializePlateau(Guid id, int x, int y)
        {
            Plateau = new Plateau(id, x, y);
        }
        public bool Move()
        {
            if(!CanRoverMove())
                throw new MarsRoverDomainException("Rover cannot move to outside of the Plateau");
            var isMoved = true;
            if (CurrentDirection == Direction.E)
            {
                this.CurrentCoordinate.XCoordinate++;
            }
            else if (CurrentDirection == Direction.W)
            {
                this.CurrentCoordinate.XCoordinate--;
            }
            else if (CurrentDirection == Direction.N)
            {
                this.CurrentCoordinate.YCoordinate++;
            }
            else if (CurrentDirection == Direction.S)
            {
                this.CurrentCoordinate.YCoordinate--;
            }
            else
            {
                isMoved = false;
            }

            if (isMoved)
            {
                AddDomainEvent(new RoverMovedDomainEvent(this));
                return true;
            }
            throw new MarsRoverDomainException("Rover could not be moved! Please check direction data");
        }

        public void ProcessCommand(Command command)
        {
            if (command == Command.Left)
            {
                RotateLeft();
            }
            else if (command == Command.Right)
            {
                RotateRight();
            }
            else if (command == Command.Move)
            {
                Move();
            }
            else
            {
                throw new MarsRoverDomainException($"Invalid Command. Command Type {command.Name}");
            }
        }

        private bool RotateLeft()
        {
            var isRotated = true;
            if (CurrentDirection == Direction.N)
            {
                CurrentDirection = Direction.W;
            }
            else if (CurrentDirection == Direction.W)
            {
                CurrentDirection = Direction.S;
            }
            else if(CurrentDirection == Direction.S)
            {
                CurrentDirection = Direction.E;
            }
            else if(CurrentDirection == Direction.E)
            {
                CurrentDirection = Direction.N;
            }
            else
            {
                isRotated = false;
            }
            if(isRotated)
                AddDomainEvent(new RoverRotatedDomainEvent(this));
            return isRotated;
        }

        private bool RotateRight()
        {
            var isRotated = true;
            if (CurrentDirection == Direction.N)
            {
                CurrentDirection = Direction.E;
            }
            else if(CurrentDirection == Direction.E)
            {
                CurrentDirection = Direction.S;
            }
            else if(CurrentDirection == Direction.S)
            {
                CurrentDirection = Direction.W;
            }
            else if(CurrentDirection == Direction.W)
            {
                CurrentDirection = Direction.N;
            }
            else
            {
                isRotated = false;
            }
            if (isRotated)
                AddDomainEvent(new RoverRotatedDomainEvent(this));
            return isRotated;

        }
        public bool CanRoverMove()
        {
            return (CurrentDirection != Direction.N ||
                    CurrentCoordinate.YCoordinate + 1 <= Plateau.Coordinate.YCoordinate) &&
                   (CurrentDirection != Direction.W || CurrentCoordinate.XCoordinate - 1 >= 0) &&
                   (CurrentDirection != Direction.S || CurrentCoordinate.YCoordinate - 1 >= 0) &&
                   (CurrentDirection != Direction.E ||
                    CurrentCoordinate.XCoordinate + 1 <= Plateau.Coordinate.XCoordinate);
        }

        public Command[] DecodeInstruction(string instruction)
        {
            var instructionList = instruction.Split(' ');
            Command[] commands;
            try
            {
                commands = Array.ConvertAll(instructionList, Enumeration.FromCode<Command>);
            }
            catch (Exception e)
            {
                throw new MarsRoverDomainException(string.Concat("Unable to decode instructions. Please check the commands", e.Message));
            }
            return commands;
        }

        public void ProcessInstruction(string instruction)
        {
            var commands = DecodeInstruction(instruction);

            foreach (var command in commands)
            {
                ProcessCommand(command);
            }
            AddDomainEvent(new InstructionProcessedDomainEvent(this));
        }
        public string DisplayPosition()
        {
            return $"{CurrentCoordinate.ToString()} {CurrentDirection}";
        }
    }
}
