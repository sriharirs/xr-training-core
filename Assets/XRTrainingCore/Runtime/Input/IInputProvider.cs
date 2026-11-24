using UnityEngine;

namespace XRTrainingCore
{
    /// <summary>
    /// Single input contract used by all gameplay code.
    /// Gameplay never reads controllers, mouse or keyboard directly,
    /// so the same scene runs in VR and in desktop (non-VR) mode.
    /// </summary>
    public interface IInputProvider
    {
        Vector2 Move { get; }      // locomotion axis
        Vector2 Look { get; }      // desktop look delta (zero in VR: the head does the looking)
        bool SelectDown { get; }   // "click / trigger" pressed this frame
        bool GrabDown { get; }     // grab pressed this frame
        bool GrabHeld { get; }     // grab held
        bool GrabUp { get; }       // grab released this frame
        Ray AimRay { get; }        // controller ray (VR) or screen-centre ray (desktop)
    }
}
