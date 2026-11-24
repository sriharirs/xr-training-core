# xr-training-core

A small Unity foundation for training and simulation apps that run **in VR (Meta Quest 3, HTC Vive via OpenXR) and in normal desktop mode** from the same scene.

## Idea

Gameplay code never reads controllers, mouse or keyboard directly. It talks to one interface, `IInputProvider`, and a `RigSwitcher` picks the matching rig at startup.

| Piece | Purpose |
|---|---|
| `IInputProvider` | One input contract: move, look, select, grab, aim ray |
| `XRInputProvider` | Headset controllers via `UnityEngine.XR` (OpenXR) |
| `DesktopInputProvider` | Mouse, keyboard, gamepad |
| `RigSwitcher` | Starts the XR rig if an XR display is running, else the desktop rig (`-desktop` forces desktop) |
| `SimpleLocomotion` | Continuous movement for both modes |
| `SimpleInteractor` | Ray select and grab for both modes |
| `Grabbable`, `Interactable` | Components that mark objects as grabbable or selectable |

## Requirements

- Unity 2022 LTS or Unity 6, with the **Universal Render Pipeline**
- Packages: **Input System**, **XR Plug-in Management**, **OpenXR** (with the Meta Quest feature group for Quest builds), **XR Interaction Toolkit** (for the XR Origin rig)

## Setup

1. Create a URP project and install the packages above. Enable OpenXR in XR Plug-in Management (PC and Android tabs).
2. Copy `Assets/XRTrainingCore` into your project.
3. **XR rig prefab:** add an XR Origin (from XR Interaction Toolkit), then on its root add `XRInputProvider` (set `aimTransform` to the right controller), `SimpleLocomotion` (rigRoot = root, head = main camera, mouse look off) and `SimpleInteractor` (holdPoint = an empty child of the right controller).
4. **Desktop rig prefab:** an empty root with a child Camera, then add `DesktopInputProvider` (aimCamera = that camera), `SimpleLocomotion` (rigRoot = root, head = camera, mouse look on) and `SimpleInteractor` (holdPoint = an empty child of the camera, about 1.5 m forward).
5. In the scene, add an empty object with `RigSwitcher` and assign both rigs.
6. Add a floor, a few cubes with `Grabbable`, and a cube with `Interactable`.

## Quest 3 build notes

Android platform, IL2CPP, ARM64, Vulkan or OpenGLES3, URP with a mobile-friendly quality level, OpenXR with the Meta Quest Support feature. Profile with the Unity Profiler and OVR Metrics Tool.

## Roadmap

- [x] Dual-mode input and rig layer
- [ ] xAPI / REST session analytics module
- [ ] Profiling notes with before/after numbers
- [ ] Hand-tracking input provider
- [ ] WebGL desktop-mode build
