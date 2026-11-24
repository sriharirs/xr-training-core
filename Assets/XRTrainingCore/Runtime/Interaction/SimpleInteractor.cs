using UnityEngine;

namespace XRTrainingCore
{
    /// <summary>
    /// Ray-based select and grab that works identically in VR and desktop mode.
    /// Put on the rig root. holdPoint: child of the camera (desktop) or of the right controller (VR).
    /// </summary>
    public class SimpleInteractor : MonoBehaviour
    {
        [SerializeField] Transform holdPoint;
        [SerializeField] float maxDistance = 6f;
        [SerializeField] LayerMask mask = ~0;
        [SerializeField] float followSpeed = 20f;

        IInputProvider input;
        Grabbable held;
        Rigidbody heldRb;

        void Awake() => input = GetComponentInParent<IInputProvider>();

        void Update()
        {
            if (input == null) return;
            Ray ray = input.AimRay;

            if (input.SelectDown && Physics.Raycast(ray, out RaycastHit selHit, maxDistance, mask))
            {
                var interactable = selHit.collider.GetComponentInParent<Interactable>();
                if (interactable != null) interactable.Select();
            }

            if (held == null)
            {
                if (input.GrabDown && Physics.Raycast(ray, out RaycastHit hit, maxDistance, mask))
                {
                    var g = hit.collider.GetComponentInParent<Grabbable>();
                    if (g != null) Grab(g);
                }
            }
            else if (input.GrabUp)
            {
                Release();
            }
        }

        void LateUpdate()
        {
            if (held != null)
                held.transform.localPosition = Vector3.Lerp(
                    held.transform.localPosition, Vector3.zero, followSpeed * Time.deltaTime);
        }

        void Grab(Grabbable g)
        {
            held = g;
            heldRb = g.GetComponent<Rigidbody>();
            heldRb.isKinematic = true;
            g.transform.SetParent(holdPoint, true);
        }

        void Release()
        {
            held.transform.SetParent(null, true);
            heldRb.isKinematic = false;
            held = null;
            heldRb = null;
        }
    }
}
