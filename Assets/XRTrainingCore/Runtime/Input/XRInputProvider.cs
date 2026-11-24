using UnityEngine;
using UnityEngine.XR;

namespace XRTrainingCore
{
    /// <summary>
    /// Headset controller implementation of IInputProvider (Quest 3, Vive, etc. via OpenXR).
    /// Uses the UnityEngine.XR InputDevices API so it does not depend on a specific
    /// XR Interaction Toolkit version. Assign the right-hand controller transform as aimTransform.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class XRInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] Transform aimTransform;

        InputDevice left, right;
        Vector2 move;
        bool select, grab, prevSelect, prevGrab;

        public Vector2 Move => move;
        public Vector2 Look => Vector2.zero; // in VR the head does the looking
        public bool SelectDown => select && !prevSelect;
        public bool GrabDown => grab && !prevGrab;
        public bool GrabHeld => grab;
        public bool GrabUp => !grab && prevGrab;
        public Ray AimRay => aimTransform != null
            ? new Ray(aimTransform.position, aimTransform.forward)
            : new Ray(transform.position, transform.forward);

        void Update()
        {
            if (!left.isValid) left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (!right.isValid) right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            prevSelect = select;
            prevGrab = grab;
            move = Vector2.zero;
            select = false;
            grab = false;

            if (left.isValid) left.TryGetFeatureValue(CommonUsages.primary2DAxis, out move);
            if (right.isValid)
            {
                right.TryGetFeatureValue(CommonUsages.triggerButton, out select);
                right.TryGetFeatureValue(CommonUsages.gripButton, out grab);
            }
        }
    }
}
