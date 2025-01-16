public enum Directions
{
    Up,
    Down,
    Left,
    Right
}
public enum TokensStates
{
    Idle,
    Special,
    Damaged,
    Dead
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
public enum ActionTypes
{
    StartTurn,
    Move,
    UseAbility,
}
public enum Players
{
    Player1,
    Player2
}
public enum RangesType
{
    Square,
    Path
}
public enum TokensNames
{
    Healer,
    Destroyer,
}

public enum TrapTypes
{
    NoTrap,
    Spikes,
}