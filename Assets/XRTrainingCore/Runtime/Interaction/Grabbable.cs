using UnityEngine;

namespace XRTrainingCore
{
    /// <summary>Marks an object that SimpleInteractor can pick up.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : MonoBehaviour { }
}
