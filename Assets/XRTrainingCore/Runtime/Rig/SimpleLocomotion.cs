using UnityEngine;

namespace XRTrainingCore
{
    /// <summary>
    /// Continuous locomotion that works in both modes by reading IInputProvider.
    /// Put on the rig root (next to the input provider).
    /// Desktop: enableMouseLook = true. VR: false (head tracking does the looking).
    /// </summary>
    public class SimpleLocomotion : MonoBehaviour
    {
        [SerializeField] Transform rigRoot;
        [SerializeField] Transform head;   // camera
        [SerializeField] float speed = 2f;
        [SerializeField] bool enableMouseLook;

        IInputProvider input;
        float pitch;

        void Awake() => input = GetComponentInParent<IInputProvider>();

        void Update()
        {
            if (input == null) return;

            if (enableMouseLook)
            {
                Vector2 look = input.Look;
                rigRoot.Rotate(0f, look.x, 0f, Space.World);
                pitch = Mathf.Clamp(pitch - look.y, -80f, 80f);
                head.localEulerAngles = new Vector3(pitch, 0f, 0f);
            }

            Vector2 m = input.Move;
            Vector3 fwd = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;
            rigRoot.position += (fwd * m.y + right * m.x) * (speed * Time.deltaTime);
        }
    }
}
