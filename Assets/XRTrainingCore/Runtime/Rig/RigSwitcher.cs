using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace XRTrainingCore
{
    /// <summary>
    /// Chooses between the XR rig and the desktop rig at startup.
    /// XR is used when an XR display subsystem is running; otherwise desktop.
    /// Force desktop with the inspector toggle or the "-desktop" command-line argument.
    /// WebGL builds always fall back to desktop because no XR display starts.
    /// </summary>
    public class RigSwitcher : MonoBehaviour
    {
        [SerializeField] GameObject xrRig;
        [SerializeField] GameObject desktopRig;
        [SerializeField] bool forceDesktop;
        [Tooltip("Seconds to wait for the XR runtime to start before falling back to desktop.")]
        [SerializeField] float xrDetectTimeout = 2f;

        public static bool IsXR { get; private set; }

        IEnumerator Start()
        {
            xrRig.SetActive(false);
            desktopRig.SetActive(false);

            bool wantDesktop = forceDesktop ||
                               Array.IndexOf(Environment.GetCommandLineArgs(), "-desktop") >= 0;
            bool xr = false;
            float t = 0f;
            while (!wantDesktop && t < xrDetectTimeout)
            {
                if (IsXRRunning()) { xr = true; break; }
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            IsXR = xr;
            xrRig.SetActive(xr);
            desktopRig.SetActive(!xr);
            Debug.Log($"[RigSwitcher] Mode: {(xr ? "XR" : "Desktop")}");
        }

        static bool IsXRRunning()
        {
            var displays = new List<XRDisplaySubsystem>();
            SubsystemManager.GetSubsystems(displays);
            foreach (var d in displays)
                if (d.running) return true;
            return false;
        }
    }
}
