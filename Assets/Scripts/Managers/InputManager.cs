using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private KeyCode generateNewMazeKey = KeyCode.Space;
        [SerializeField] private KeyCode desarmMazeKey = KeyCode.A;
        [SerializeField] private KeyCode newTurnKey = KeyCode.S;
        [SerializeField] private int maxFaceIndex = 5;

        private void Update()
        {
            HandleMazeGeneration();
            HandleFaceSelection();
            HandleDesarmMaze();
            HandleNewTurn();
        }
        private void HandleMazeGeneration()
        {
            if (Input.GetKeyUp(generateNewMazeKey))
            {
                GameManager.OnGenerateNewMaze?.Invoke();
            }
        }

        private void HandleFaceSelection()
        {
            for (int i = 0; i <= maxFaceIndex; i++)
            {
                if (Input.GetKeyUp(KeyCode.Alpha0 + i))
                {
                    GameManager.OnShowFace?.Invoke(i);
                }
            }
        }

        private void HandleDesarmMaze()
        {
            if (Input.GetKeyUp(desarmMazeKey))
            {
                GameManager.OnDesarmMaze?.Invoke();
            }
        }
        private void HandleNewTurn()
        {
            if (Input.GetKeyUp(newTurnKey))
            {
                GameManager.OnNewTurn?.Invoke();
            }
        }
    }
}