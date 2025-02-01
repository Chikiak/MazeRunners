using System.Collections.Generic;

namespace Managers
{
    public static class GetCellsManager
    {
        public static List<(int x, int y)> GetReachablePositions(RangeType range, int distance, (int x, int y) Position, int size)
        {
            List<(int x, int y)> positions;
            List<(int x, int y)> finalPositions = new List<(int x, int y)>();
            switch (range)
            {
                case RangeType.Square:
                    positions = GetReachableSquare(distance, Position);
                    break;
                case RangeType.Diamond:
                    positions = GetReachableRombus(distance, Position);
                    break;
                case RangeType.Line:
                    positions = GetReachableLineal(distance, Position);
                    break;
                default:
                    positions = new List<(int x, int y)>();
                    break;
            }
            foreach ((int x, int y) position in positions)
            {
                if (ValidatePosition(position,size)) finalPositions.Add(position);
            }
            return finalPositions;
        }

        static List<(int x, int y)> GetReachableSquare(int distance, (int x, int y) Position)
        {
            List<(int x, int y)> positions = new List<(int, int)>();

            for(int x = -distance/2; x <= distance/2; x++) 
            {                                             
                for(int y = -distance/2; y <= distance/2; y++)
                {
                    positions.Add((Position.x + x, Position.y + y));
                }
            }

            if (GameManager.GameState == GameStates.PutingInitialPiece)
            {
                positions.Remove(((Position.x, Position.y)));
            }
            return positions;
        }

        static List<(int x, int y)> GetReachableRombus(int distance, (int x, int y) Position)
        {
            List<(int x, int y)> positions = new List<(int, int)>();
            for(int x = 1; x <= distance; x++)
            {
                positions.AddRange(GetPositionsAtDistance(x, Position));
            }
            return positions;
        }

        static List<(int x, int y)> GetPositionsAtDistance (int distance, (int x, int y) Position)
        {
            List<(int x, int y)> positions = new List<(int, int)>();
            int y;
            for (int x = 0; x <= distance; x++)
            {
                y = distance - x;
                positions.Add((Position.x + x, Position.y + y));
                if (y > 0) {positions.Add((Position.x + x, Position.y - y)); }
                if (x > 0) {positions.Add((Position.x - x, Position.y + y)); }
                if (x > 0 && y > 0) {positions.Add((Position.x - x, Position.y - y)); }
            }
            return positions;

        }

        static List<(int x, int y)> GetReachableLineal(int distance, (int x, int y) Position)
        {
            List<(int x, int y)> positions = new List<(int, int)>();

            for(int x = 1; x <= distance; x++)
            {
                positions.Add((Position.x + x, Position.y));
                positions.Add((Position.x - x, Position.y));
                positions.Add((Position.x, Position.y + x));
                positions.Add((Position.x, Position.y - x));
            }
            return positions;
        }

        static bool ValidatePosition((int x, int y) position, int size)
        {
            if(position.x < 0 || position.y < 0) return false;
            if(position.x >= size || position.y >= size) return false;
            return true;
        }
    }
}