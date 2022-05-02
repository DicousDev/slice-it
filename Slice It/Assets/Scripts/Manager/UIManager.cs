using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SliceIt.ScriptableObjects.Utils.Events;

namespace SliceIt.Manager
{
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onStartGame = default;
        [SerializeField] private GameEvent onGameOver = default;
        [SerializeField] private GameEvent onAddPoint = default;
        [SerializeField] private TextMeshProUGUI tapToPlayText;
        [SerializeField] private TextMeshProUGUI tapToPlayAgainText;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private TextMeshProUGUI pointsAddedRecentlyText;
        [SerializeField] private float pointsFontSizeFinalAnimation = 50;
        [SerializeField] private float pointsFontSpeedAnimation = 0.7f;
        [SerializeField] private float delaySpeedPointsFont = 0.01f;
        private int pointsRecent = 0;
        [SerializeField] private float delayToDisableRecentlyAddedPointsInSeconds = 1.5f;


        private void OnEnable()
        {
            onStartGame.onGameEvent += DisableTapToPlay;
            onStartGame.onGameEvent += EnabledPointsText;
            onGameOver.onGameEvent += GameOver;
            onAddPoint.onGameEvent += AddPointsRecently;
            GameManager.onAddedPoint += UpdatePoints;
        }

        private void OnDisable()
        {
            onStartGame.onGameEvent -= DisableTapToPlay;
            onStartGame.onGameEvent -= EnabledPointsText;
            onGameOver.onGameEvent -= GameOver;
            onAddPoint.onGameEvent -= AddPointsRecently;
            GameManager.onAddedPoint -= UpdatePoints;
        }

        private void AddPointsRecently()
        {
            pointsRecent++;
            UpdateRecentPoints();
            pointsAddedRecentlyText.enabled = true;
            StopCoroutine(nameof(DisableRecentlyAddedPoints));
            StartCoroutine(nameof(DisableRecentlyAddedPoints));
        }

        private void UpdateRecentPoints()
        {
            pointsAddedRecentlyText.text = "+" + pointsRecent.ToString();
        }

        private IEnumerator DisableRecentlyAddedPoints()
        {
            yield return new WaitForSeconds(delayToDisableRecentlyAddedPointsInSeconds);
            pointsAddedRecentlyText.enabled = false;
            resetPointsRecent();
        }

        private void resetPointsRecent()
        {
            pointsRecent = 0;
        }

        private void GameOver()
        {
            tapToPlayAgainText.enabled = true;
        }

        private void UpdatePoints(int points)
        {
            StartCoroutine(AnimatePointsWhenReceiving());
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

        private IEnumerator AnimatePointsWhenReceiving()
        {
            float fontSizeStart = pointsText.fontSizeMax;
            while (pointsText.fontSize < pointsFontSizeFinalAnimation)
            {
                pointsText.fontSizeMax += pointsFontSpeedAnimation;
                yield return new WaitForSeconds(delaySpeedPointsFont);
            }

            while(fontSizeStart < pointsText.fontSize)
            {
                pointsText.fontSizeMax -= pointsFontSpeedAnimation;
                yield return new WaitForSeconds(delaySpeedPointsFont);
            }
        }
    }
}
