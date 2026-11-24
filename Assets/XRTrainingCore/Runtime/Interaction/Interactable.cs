using UnityEngine;
using UnityEngine.Events;

namespace XRTrainingCore
{
    /// <summary>Raises an event when selected (trigger in VR, left click on desktop).</summary>
    public class Interactable : MonoBehaviour
    {
        public UnityEvent onSelected;
        public void Select() => onSelected?.Invoke();
    }
}
