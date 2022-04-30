using UnityEngine;
using SliceIt.ScriptableObjects.Utils.Events;

namespace SliceIt.Manager
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onStartGame = default;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                onStartGame.Raise();
            }
        }
    }
}
