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



## Lab 03

- Engine and version: Unity 6.3 LTS (6000.3.25f1)

### 1. Callback order

Q1. What callback order did you observe when the scene started?

In my test, Unity called Awake and OnEnable first for each object. The order I observed was Capsule, Sphere, and then Cube. After that, Start was called for Capsule, Sphere, and Cube in the same observed object order.

Q2. What order did the repeating callbacks run in?

I observed FixedUpdate before Update, followed by LateUpdate. For each callback, the objects appeared in the order Capsule, Sphere, and Cube.

Q3. Did the object order stay the same between your runs?

Yes. In my two test runs, the observed object order stayed Capsule, Sphere, and Cube. The frame numbers changed, but the order I observed stayed the same.

### 2. Components and exposed attributes

Q1. What components are attached to the Cube?

The Cube has a Transform, Mesh Filter, Mesh Renderer, Box Collider, and the custom Lab03LifecycleLogger script.

Q2. Which fields appear in the Inspector and why?

The Inspector shows id, logInterval, and logCount because they are public or marked with SerializeField. Private variables that are not serialized, such as the next log timing variables, do not appear.

Q3. Can different objects use different values with the same script?

Yes. The same Lab03LifecycleLogger script can be attached to multiple objects, and each object can have its own logInterval value.

Q4. What happened when you changed a value during Play mode?

I changed the Cube's log interval during Play mode, but after stopping Play mode the value returned to its previous setting. This showed that Inspector changes made during Play mode are temporary unless saved another way.

### 3. Lifecycle timeline

Q1. What happened when the Capsule was activated at about 3 seconds?

The Capsule called Awake, then OnEnable, then Start before continuing with its normal repeating callbacks.

Q2. What happened when the Cube script was disabled and enabled?

At about 2 seconds, the Cube script was disabled and Cube.OnDisable was called. Its FixedUpdate, Update, and LateUpdate callbacks stopped. At about 4 seconds, the script was enabled again and Cube.OnEnable was called, after which its repeating callbacks resumed.

Q3. What happened when the Sphere was disabled and enabled?

At about 6 seconds, the Sphere was deactivated and Sphere.OnDisable was called. At about 8 seconds, it was activated again and Sphere.OnEnable was called.

Q4. What happened when the Capsule was destroyed?

At about 10 seconds, the Capsule was destroyed. Capsule.OnDisable was called first, followed by Capsule.OnDestroy.

### 4. Frame rate and fixed step

| Row | Traveled at 30 FPS | Traveled at 120 FPS |
|---|---:|---:|
| Move_Update_dt | 19.9 u | 20.6 u |
| Move_Update_perCall | 12.0 u | 48.5 u |
| Move_Fixed_dt | 19.8 u | 20.6 u |
| Move_Fixed_perCall | 19.8 u | 20.6 u |

Q1. Which rows traveled approximately the same distance at both frame rates?

Move_Update_dt, Move_Fixed_dt, and Move_Fixed_perCall traveled approximately the same distance at both frame rates. Small differences were caused by the exact time when I paused the test.

Q2. Which row changed the most between 30 FPS and 120 FPS?

Move_Update_perCall changed the most. It traveled about 12.0 units at 30 FPS and about 48.5 units at 120 FPS.

Q3. Why did this happen?

Move_Update_perCall moves by a fixed amount every Update without using delta time. At a higher frame rate, Update is called more times every second, so the object moves farther.

### 5. Update order

| Setting | Fresh | Stale |
|---|---:|---:|
| Update | 0 | 1195 |
| LateUpdate | 1170 | 0 |

Q1. What did the fresh and stale counts show?

When the follower used Update, I recorded 0 fresh and 1195 stale frames because the follower was set to execute before the leader. When the follower used LateUpdate, I recorded 1170 fresh and 0 stale frames.

Q2. Which setting gives the follower the leader's position from the same frame?

LateUpdate gives the follower the leader's position from the same frame because the leader moves during Update first and the follower reads the updated position during LateUpdate.

Q3. In the Child update logs, which position changed over time, world or local, and why?

The world position changed over time while the local position stayed at approximately (0, 0, 1). This happened because Child_Local was attached to Move_Update_dt. Its local position stayed fixed relative to its parent, while its world position changed as the parent moved and rotated.

### 6. Interpolation

Q1. With Fixed Timestep set to 0.1, how did Move_Fixed_dt compare with Move_FixedInterpolated?

Move_Fixed_dt moved in more noticeable steps because its position changed only during fixed updates. Move_FixedInterpolated appeared smoother because it blended between the previous and current fixed-step positions.

Q2. How did Interp_Linear compare with Interp_Smooth?

Interp_Linear moved at a mostly constant speed during each trip. Interp_Smooth moved faster when it was farther from the target and gradually slowed down as it approached the target.

Q3. What happened when Use Delta Time was unchecked at 30 FPS and 120 FPS?

With Use Delta Time unchecked, Interp_Smooth moved faster at 120 FPS than at 30 FPS because the same interpolation fraction was applied more times each second. This made the movement frame-rate dependent.

### 7. Game loop

```c
int game_loop(float dt)
{
    while (accumulator >= fixedDeltaTime)
    {
        FixedUpdate();
        accumulator -= fixedDeltaTime;
    }

    Input(dt);

    Update();

    Action();

    LateUpdate();

    Output();

    return score();
}

int main()
{
    Awake();
    OnEnable();
    Start();

    previous = Now();
    accumulator = 0;

    while (experience)
    {
        current = Now();
        dt = current - previous;
        previous = current;
        accumulator += dt;

        experience_score += game_loop(dt);

        t += dt;
        experience =
            t < experience_duration &&
            !player_quit_level &&
            !game_over;
    }

    OnDisable();
    OnDestroy();
}