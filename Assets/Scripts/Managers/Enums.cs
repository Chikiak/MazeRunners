namespace Managers
{
    public enum Direction
    {
        Up, 
        Down, 
        Left, 
        Right
    }
    public enum TrapType
    {
        Nothing,
        Spikes
    }

    public enum PieceType
    {
        Healer,
        Destroyer,
        Lancer,
        Gladiator,
        Thief,
        Explorer
    }

    public enum StatusEffect
    {
        None,
    }

    public enum PlayerID
    {
        Player1,
        Player2,
    }

    public enum RangeType
    {
        Square,
        Path,
        Diamond,
        Line
    }

    public enum ActionType
    {
        Move,
        UseAbility,
    }
    public enum GameStates
    {
        Starting,
        SelectInitialPiece,
        PutingInitialPiece,
        PieceOnBoardSelection,
        CellSelection,
        SelectAction,
    }
}