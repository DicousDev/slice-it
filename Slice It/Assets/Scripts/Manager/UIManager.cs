using UnityEngine;
using TMPro;
using SliceIt.ScriptableObjects.Utils.Events;

namespace SliceIt.Manager
{
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onStartGame = default;
        [SerializeField] private TextMeshProUGUI tapToPlayText;

        private void OnEnable()
        {
            onStartGame.onGameEvent += DisableTapToPlay;
        }

        private void OnDisable()
        {
            onStartGame.onGameEvent -= DisableTapToPlay;
        }

        private void DisableTapToPlay()
        {
            tapToPlayText.enabled = false;
        }
    }
}
