using System;
using Core.Interfaces.Entities;

namespace Controllers
{
    public partial class MazeController
    {
        #region Selecting

        public void PutInitialPiece()
        {
            var tokenController = GameManager.SelectedToken;
            if (tokenController == null) return;

            var cell = GetMazeFace(0).Cells[GameManager.SelectedCell.Item1, GameManager.SelectedCell.Item2];
            if (cell == null) return;
            tokenController.Model.SetPosition((GameManager.SelectedCell.Item1, GameManager.SelectedCell.Item2));
            cell.AddToken(tokenController);
            _maze.TokensMaze[GameManager.SelectedCell.Item1, GameManager.SelectedCell.Item2].Add(tokenController);
            OnMazeChanged();
        }

        private void SetSelectableCells(RangesType rangeType, int size, (int, int) pos)
        {
            int center = (_maze.Size - 1) / 2;

            var cells = GetCellsInRange(rangeType, size, pos);
            if (GameManager.GameState == GameStates.PutingInitialPiece)
                cells.Remove(GetMazeFace(0).Cells[center, center]);

            foreach (var cell in cells)
            {
                if (GameManager.GameState == GameStates.PutingInitialPiece && cell.Tokens.Count > 0) continue;

                _selectableCells.Add(cell);
                cell.SetSelectable(true);
                (int, int) thisPos = cell.Position;
                GetMazeFace(0).SetCell(thisPos.Item1, thisPos.Item2, cell);
            }

            _view.UpdateMaze(GetMazeFace(0));
        }

        private void SetSelectableCells()
        {
            if (GameManager.GameState == GameStates.PutingInitialPiece)
            {
                SetSelectableCells(RangesType.Square, 3, ((_maze.Size - 1) / 2, (_maze.Size - 1) / 2));
            }
            else if (GameManager.GameState == GameStates.CellSelection)
            {
                SetSelectableCells(RangesType.Path, GameManager.SelectedToken.Model.RemainingMovs,
                    GameManager.SelectedToken.Model.Position);
            }
        }

        private void HandleSelectedCell((int, int) position)
        {
            foreach (var cell in _selectableCells)
            {
                cell.SetSelectable(false);
                GetMazeFace(0).SetCell(cell.Position.Item1, cell.Position.Item2, cell);
            }

            _view.UpdateMaze(GetMazeFace(0));
            OnCellSelected?.Invoke(position);
        }

        public IToken GetToken(TokensNames tokenName)
        {
            foreach (var tokenList in _maze.TokensMaze)
            {
                foreach (var tokenController in tokenList)
                {
                    if (tokenController.Model.Name == tokenName)
                    {
                        return tokenController.Model;
                    }
                }
            }

            throw new Exception($"Token not found: {tokenName}");
        }

        #endregion
    }
}