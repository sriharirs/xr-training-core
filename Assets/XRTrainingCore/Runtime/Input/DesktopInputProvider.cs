using UnityEngine;
using UnityEngine.InputSystem;

namespace XRTrainingCore
{
    /// <summary>Mouse, keyboard and gamepad implementation of IInputProvider.</summary>
    [DefaultExecutionOrder(-100)]
    public class DesktopInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] Camera aimCamera;
        [SerializeField] float mouseSensitivity = 0.1f;
        [SerializeField] float stickLookSpeed = 120f;

        public Vector2 Move
        {
            get
            {
                Vector2 v = Vector2.zero;
                var k = Keyboard.current;
                if (k != null)
                {
                    if (k.wKey.isPressed) v.y += 1;
                    if (k.sKey.isPressed) v.y -= 1;
                    if (k.dKey.isPressed) v.x += 1;
                    if (k.aKey.isPressed) v.x -= 1;
                }
                if (Gamepad.current != null) v += Gamepad.current.leftStick.ReadValue();
                return Vector2.ClampMagnitude(v, 1f);
            }
        }

        public Vector2 Look
        {
            get
            {
                Vector2 v = Vector2.zero;
                if (Mouse.current != null && Cursor.lockState == CursorLockMode.Locked)
                    v += Mouse.current.delta.ReadValue() * mouseSensitivity;
                if (Gamepad.current != null)
                    v += Gamepad.current.rightStick.ReadValue() * stickLookSpeed * Time.deltaTime;
                return v;
            }
        }

        public bool SelectDown =>
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

        public bool GrabDown =>
            (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame);

        public bool GrabHeld =>
            (Mouse.current != null && Mouse.current.rightButton.isPressed) ||
            (Keyboard.current != null && Keyboard.current.eKey.isPressed) ||
            (Gamepad.current != null && Gamepad.current.rightShoulder.isPressed);

        public bool GrabUp =>
            (Mouse.current != null && Mouse.current.rightButton.wasReleasedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.eKey.wasReleasedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.rightShoulder.wasReleasedThisFrame);

        public Ray AimRay => aimCamera != null
            ? aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))
            : new Ray(transform.position, transform.forward);

        void OnEnable() => SetCursorLocked(true);
        void OnDisable() => SetCursorLocked(false);

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) SetCursorLocked(false);
            else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame &&
                     Cursor.lockState != CursorLockMode.Locked) SetCursorLocked(true);
        }

        static void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
