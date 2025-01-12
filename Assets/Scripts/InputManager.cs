using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action OnGenerateNewMaze;
    public static Action<int> OnShowFace;
    public static Action OnDesarmMaze;
    public static Action OnGameStateChanged;
    
    [SerializeField] private KeyCode generateNewMazeKey = KeyCode.Space;
    [SerializeField] private KeyCode desarmMazeKey = KeyCode.A;
    [SerializeField] private int maxFaceIndex = 5;

    private void Update()
    {
        HandleMazeGeneration();
        HandleFaceSelection();
        HandleDesarmMaze();
    }

    private void HandleMazeGeneration()
    {
        if (Input.GetKeyUp(generateNewMazeKey))
        {
            OnGenerateNewMaze?.Invoke();
        }
    }

    private void HandleFaceSelection()
    {
        for (int i = 0; i <= maxFaceIndex; i++)
        {
            if (Input.GetKeyUp(KeyCode.Alpha0 + i))
            {
                OnShowFace?.Invoke(i);
            }
        }
    }

    private void HandleDesarmMaze()
    {
        if (Input.GetKeyUp(desarmMazeKey))
        {
            OnDesarmMaze?.Invoke();
        }
    }
}