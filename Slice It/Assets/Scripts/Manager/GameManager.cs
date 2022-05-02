using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SliceIt.ScriptableObjects.Utils.Events;
using SliceIt.Enum;
using SliceIt.GameInput;

namespace SliceIt.Manager
{
    public sealed class GameManager : MonoBehaviour
    {
        public static event Action<int> onAddedPoint;
        [SerializeField] private GameState gameState = GameState.START;
        [SerializeField] private GameEvent onStartGame = default;
        [SerializeField] private GameEvent onGameWinner = default;
        [SerializeField] private GameEvent onGameOver = default;
        [SerializeField] private GameEvent onAddPoints = default;
        [SerializeField] private int slicesToWinner = 5;
        private int points;
        private StartAndEndGameInput gameInput;

        private void OnEnable()
        {
            gameInput.Enable();
            onAddPoints.onGameEvent += AddPoint;
            onGameOver.onGameEvent += GameOver;
        }

        private void OnDisable()
        {
            gameInput.Disable();
            onAddPoints.onGameEvent -= AddPoint;
            onGameOver.onGameEvent -= GameOver;
        }

        private void Awake()
        {
            gameInput = new StartAndEndGameInput();
        }

        private void Update()
        {
            CheckToStartGame();
            CheckInWinnerToReloadLevel();
            CheckInGameOverToReloadLevel();
        }

        private void AddPoint()
        {
            points++;
            onAddedPoint?.Invoke(points);
            CheckWinner();
        }

        private void CheckWinner()
        {
            if (points < slicesToWinner) return;

            Winner();
        }

        private void Winner()
        {
            gameState = GameState.WINNER;
            onGameWinner.Raise();
        }

        private void GameOver()
        {
            gameState = GameState.GAMEOVER;
            Time.timeScale = 0;
        }

        private void CheckToStartGame()
        {
            if (gameState == GameState.START && gameInput.StartEnd.StartEnd.triggered)
            {
                gameState = GameState.GAME;
                onStartGame.Raise();
            }
        }

        private void CheckInWinnerToReloadLevel()
        {
            if (gameState == GameState.WINNER && gameInput.StartEnd.StartEnd.triggered)
            {
                ReloadLevel();
            }
        }

        private void CheckInGameOverToReloadLevel()
        {
            if(gameState == GameState.GAMEOVER && gameInput.StartEnd.StartEnd.triggered)
            {
                ReloadLevel();
            }
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
    }
}
