using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SliceIt.ScriptableObjects.Utils.Events;
using SliceIt.Enum;

namespace SliceIt.Manager
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameState gameState = GameState.START;
        [SerializeField] private GameEvent onStartGame = default;
        [SerializeField] private GameEvent onGameOver = default;
        [SerializeField] private GameEvent onAddPoints = default;
        public static event Action<int> onAddedPoint;
        private int points;

        private void OnEnable()
        {
            onAddPoints.onGameEvent += AddPoint;
            onGameOver.onGameEvent += GameOver;
        }

        private void OnDisable()
        {
            onAddPoints.onGameEvent -= AddPoint;
            onGameOver.onGameEvent -= GameOver;
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
            if (gameState == GameState.START && Input.GetKeyDown(KeyCode.Space))
            {
                gameState = GameState.GAME;
                onStartGame.Raise();
            }
        }

        private void CheckInGameOverToReloadLevel()
        {
            if(gameState == GameState.GAMEOVER && Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 1;
            }
        }
    }
}
