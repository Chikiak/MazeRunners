using System;
using TMPro;
using UnityEngine;

namespace Views
{
    public class InstructionsText : MonoBehaviour
    {
        private TMP_Text _text;
        private void Awake()
        {
            GameManager.OnStateChanged += HandleStateChanged;
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            HandleStateChanged(GameStates.SelectInitialPiece);
        }

        private void HandleStateChanged(GameStates newState)
        {
            if (_text == null) return;
            if (newState == GameStates.SelectInitialPiece || newState == GameStates.PutingInitialPiece)
            {
                _text.text = "Select a piece and place it in one of the available squares on the board.";
            }

            if (newState == GameStates.PieceOnBoardSelection)
            {
                _text.text = "Select one of your pieces \nOR\n change the maze";
            }
        }
    }
}