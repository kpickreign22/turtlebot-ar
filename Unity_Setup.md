# Setup

To run the Unity project, open the VIO Frontend folder in Unity 2022.3.Later versions of Unity may work as well.
If you are building to an Android device, ensure your Unity installation includes Android Build Support. Building
to iOS should be possible, though it has not been tested and will likely require modifications. If you do choose
to build to iOS, ensure your Unity installation includes iOS Build Support.

After Unity opens the project, navigate to ExampleAssets/Scenes and open SampleScene.

## Notes
The 'Target', 'PlayerAgent', and 'JCC Floor 4' GameObjects are all invisible intentionally, and can be made visible
for testing purposes by enabling their Mesh Renderer components. Do not disable the GameObjects themselves to make
them invisible again, as this will break navigation functionality.

There is a disabled Camera object in the scene for testing in Game Mode. If you are trying to run tests in Game Mode,
enable this camera and disable both 'AR Session Origin' and 'AR Session'. When building to a device, make sure that
'Camera' is disabled and that both 'AR Session Origin' and 'AR Session' are enabled.
