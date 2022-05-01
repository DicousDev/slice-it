using System;
using UnityEngine;
using SliceIt.ScriptableObjects.Utils.Events;

namespace SliceIt.Manager
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onStartGame = default;
        [SerializeField] private GameEvent onAddPoints = default;
        public static event Action<int> onAddedPoint;
        private int points;
        private bool startedGame = false; 

        private void OnEnable()
        {
            onAddPoints.onGameEvent += AddPoint;
        }

        private void OnDisable()
        {
            onAddPoints.onGameEvent -= AddPoint;
        }

        private void Update()
        {
            CheckToStartGame();
        }

        private void AddPoint()
        {
            points++;
            onAddedPoint?.Invoke(points);
        }

        private void CheckToStartGame()
        {
            if (!startedGame && Input.GetKeyDown(KeyCode.Space))
            {
                startedGame = true;
                onStartGame.Raise();
            }
        }
    }
}
