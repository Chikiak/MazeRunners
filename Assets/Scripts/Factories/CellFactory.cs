using System;
using Controllers;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Core.Models;
using Core.Models.Traps;
using UnityEngine;

public static class CellFactory
{
    public static SOsManager sosManager;

    public static ICell NewCell(TrapTypes type, (int, int) cellPosition)
    {
        ICell newCell = type switch
        {
            TrapTypes.NoTrap => new Cell(cellPosition),
            TrapTypes.Spikes => new SpikeTrap(cellPosition),
            _ => throw new Exception("Factory: Invalid trap type 1")
        };
        return newCell;
    }

    public static ICell NewCell(ICell cell)
    {
        ICell newCell = cell.Type switch
        {
            TrapTypes.NoTrap => new Cell(cell),
            TrapTypes.Spikes => new SpikeTrap(cell),
            _ => throw new Exception("Factory: Invalid cell type 2")
        };

        return newCell;
    }
}