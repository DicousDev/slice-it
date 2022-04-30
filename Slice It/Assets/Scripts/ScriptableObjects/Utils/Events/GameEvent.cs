using System;
using UnityEngine;

namespace SliceIt.ScriptableObjects.Utils.Events
{
    [CreateAssetMenu(fileName="Game Event", menuName="ScriptableObjects/Utils/Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        public event Action onGameEvent;

        public void Raise()
        {
            onGameEvent?.Invoke();
        }
    }
}