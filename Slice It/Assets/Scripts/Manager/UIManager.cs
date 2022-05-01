using UnityEngine;
using TMPro;
using SliceIt.ScriptableObjects.Utils.Events;

namespace SliceIt.Manager
{
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onStartGame = default;
        [SerializeField] private TextMeshProUGUI tapToPlayText;
        [SerializeField] private TextMeshProUGUI pointsText;

        private void OnEnable()
        {
            onStartGame.onGameEvent += DisableTapToPlay;
            onStartGame.onGameEvent += EnabledPointsText;
            GameManager.onAddedPoint += UpdatePoints;
        }

        private void OnDisable()
        {
            onStartGame.onGameEvent -= DisableTapToPlay;
            onStartGame.onGameEvent -= EnabledPointsText;
            GameManager.onAddedPoint -= UpdatePoints;
        }

        private void UpdatePoints(int points)
        {
            pointsText.text = "Points: " + points.ToString();
        }

        private void EnabledPointsText()
        {
            pointsText.text = "Points: 0";
            pointsText.enabled = true;
        }

        private void DisableTapToPlay()
        {
            tapToPlayText.enabled = false;
        }
    }
}
