# comp3770-110125520

## Git LFS verification

```
155412fe85 * test-asset.png
```

## Lab 02

- Engine and version: Unity 6.3 LTS (6000.3.25f1)
- Template: 3D (URP)

### Interface panels
- Scene: The editable 3D view of the current level, where GameObjects are placed and manipulated.
- Game: Shows what the active camera renders when the project is played.
- Hierarchy: Lists all GameObjects in the current scene.
- Inspector: Shows the properties and components of the currently selected GameObject, where components such as HelloGame can be added.
- Project: The file browser for all assets in the Unity project, including the Scripts folder.

### Engine execution
- Start() is called once before the first frame update for an enabled MonoBehaviour script.
- Update() is called once per rendered frame while the script is enabled; in HelloGame it increments frameCount and logs a message on the first frame.

### Engine architecture
- Engine/editor responsibility: Unity renders the scene and Game view, manages the GameObject/Component inspector interface, and calls the MonoBehaviour lifecycle methods each frame.
- Game-code responsibility: The HelloGame C# script implements the custom behaviour by counting frames and writing the Start and first-frame Update messages to the Console.
