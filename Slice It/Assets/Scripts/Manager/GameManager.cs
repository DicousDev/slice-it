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
        [SerializeField] private GameEvent onGameOver = default;
        [SerializeField] private GameEvent onAddPoints = default;
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
            CheckInGameOverToReloadLevel();
        }

        private void AddPoint()
        {
            points++;
            onAddedPoint?.Invoke(points);
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

        private void CheckInGameOverToReloadLevel()
        {
            if(gameState == GameState.GAMEOVER && gameInput.StartEnd.StartEnd.triggered)
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 1;
            }
        }
    }
}
