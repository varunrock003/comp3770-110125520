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



## Lab 04

- Engine and version: Unity 6.3 LTS (6000.3.25f1)

### 1. Text and binary files

Q1. What difference did you observe between the OBJ and FBX files?

The OBJ file was readable as normal text. I could see lines beginning with `v`, `vt`, `vn`, and `f`. The FBX file was binary, so most of its contents were not readable as normal text.

Q2. What information was visible in the OBJ file?

The OBJ contained vertex positions with `v`, texture coordinates with `vt`, normals with `vn`, and face definitions with `f`.

My source OBJ counts were:

- v: 1824
- vt: 2196
- vn: 1251
- f: 1399

The FBX showed the header `Kaydara FBX Binary`, but the rest of the file was stored in binary form.

Q3. How did the OBJ and FBX file sizes compare?

The OBJ file was 206,440 bytes and the equivalent FBX file was 90,284 bytes. The FBX file was smaller.

A vertex written as text in an OBJ needs several characters for the three coordinates, spaces, signs, decimal points, and a newline. The same three values can be stored more compactly as binary numbers.

Q4. Which file signatures did you observe?

The PNG file contained the `PNG` and `IHDR` signatures. The WAV file contained `RIFF`, `WAVE`, and `fmt`. The FBX file contained `Kaydara FBX Binary`. These headers help software identify the type and structure of a file.

### 2. Mesh data

| Model | Vertices | Triangles | UVs |
|---|---:|---:|---|
| Lab04_PropA.fbx | 2744 | 2738 | Yes |
| Lab04_PropA.obj | 2720 | 2738 | Yes |
| Lab04_PropB.fbx | 1317 | 824 | Yes |
| Lab04_PropC.fbx | 132 | 96 | Yes |

Q1. How did the source OBJ counts compare with Unity's imported mesh counts?

The source OBJ contained 1824 vertex-position lines and 1399 face lines. After importing it, Unity reported 2720 vertices and 2738 triangles.

The numbers are different because Unity triangulates polygon faces and may split vertices when different normals, UV coordinates, or other mesh attributes are required.

Q2. Which model had the highest triangle count?

PropA had the highest triangle count. Both the OBJ and FBX versions reported 2738 triangles in Unity.

Q3. How do OBJ labels relate to Unity mesh data?

`v` represents vertex positions, `vt` represents texture coordinates, `vn` represents normals, and `f` represents faces. Unity processes this source data into its internal mesh representation and triangulates the faces.

Q4. Why was Read/Write enabled?

Read/Write had to be enabled on the mesh used by the script so the mesh information could be accessed from C# at runtime. Keeping a CPU-readable copy of mesh data can use additional memory, so it should only be enabled when needed.

Q5. What does Lab04MeshReport do?

Lab04MeshReport loops through the target GameObjects and finds their MeshFilter components. It reads each shared mesh and logs its vertex count, triangle count, UV availability, normal count, and bounds size.

### 3. Imported assets

Q1. What is the difference between the source asset and Unity import settings?

The original FBX, PNG, and WAV files remain source assets. Unity's import settings control how those files are converted and used inside the project. Settings such as texture compression, maximum texture size, model options, and audio compression can change the imported runtime version without changing the original source file.

Q2. Why is the Library folder not stored in Git?

The Library folder contains generated and cached versions of imported Unity assets. Unity can rebuild this information from the Assets folder, project settings, and `.meta` files, so committing the Library folder would add a large amount of unnecessary generated data.

Q3. Why are `.meta` files important?

Unity `.meta` files contain GUIDs and importer settings. The GUIDs allow scenes, materials, scripts, and prefabs to keep stable references to assets. They should therefore be committed with the project.

### 4. Prefabs

Q1. What values did the two PropA instances have after changing the prefab?

After changing the prefab's Points value to 25, both PropA instances had Points = 25.

The first instance kept Spin Speed = 30, while the second instance kept Spin Speed = 120 because the second value was an instance override.

Q2. What type of file is a Unity prefab?

The prefab was stored as a text-based Unity asset. It contains serialized properties such as the GameObject name and component values, including the values used by Lab04Prop.

Q3. Which changes were prefab changes and which were instance changes?

Changing Points to 25 in Prefab Mode changed the prefab asset, so the change was applied to both instances.

Changing the second object's Spin Speed to 120 was an instance override, so it remained different from the prefab's default Spin Speed of 30.

Q4. What does Lab04Prop do?

Lab04Prop exposes the displayName, points, and spinSpeed fields in the Inspector. During Update it rotates the object around the Y axis using `spinSpeed * Time.deltaTime`, so the rotation speed is measured in degrees per second and is not directly dependent on frame rate.

### 5. Runtime instantiation

In the three-object test:

- 3 instantiations took 3.69 ms.

In the larger test:

- 500 instantiations took 138.92 ms.
- Average time per instantiation was approximately 0.278 ms.

Q1. Which lifecycle methods happened before Instantiate returned?

Awake and OnEnable happened before `Instantiate()` returned. The Console showed Awake and OnEnable before the `after Instantiate` message.

Start ran afterward during the first frame.

Q2. How long did 500 instantiations take?

The 500-object test took 138.92 ms total.

The average was approximately:

138.92 / 500 = 0.278 ms per object.

Q3. How could a game avoid creating hundreds of objects in one frame?

A game could spread object creation over several frames instead of creating everything at once. Another option is object pooling, where previously created objects are reused instead of repeatedly creating and destroying them.

Q4. What does Lab04Spawner do?

Lab04Spawner calculates positions in a grid and uses Instantiate to create copies of the selected prefab under a specified parent. It measures how long the operation takes with a Stopwatch and logs the result. For small counts it also logs a message immediately after each Instantiate call.

### 6. Data file and managers

#### Part 1 - DataManager

Q1. Which value changed after editing the JSON, and which build step was skipped?

I changed the first entity's points value from 10 to 99. After saving the JSON, Unity loaded the value 99 without changing or recompiling the C# scripts.

Q2. What happened when the JSON became invalid?

When I removed the final closing brace, the Console reported an error beginning with:

`[Lab04] DataManager cannot parse`

The DataManager handled the error by logging it and returning false instead of continuing with invalid data.

Q3. What advantage does a text data file give to a designer?

A designer can change values such as positions, points, prefab names, and spin speeds without editing the C# code.

Q4. How do Lab04EntityData fields relate to the JSON?

Each field corresponds to a JSON key. The fields `id`, `prefab`, `x`, `y`, `z`, `points`, and `spinSpeed` match the values stored for each entity in the JSON file.

Q5. What does Lab04DataManager do?

Lab04DataManager reads the JSON file from StreamingAssets, converts the JSON into Lab04EntityData objects, stores the objects in a list and dictionary, and allows other scripts to retrieve the data by id. It also reports file-reading and parsing errors.

#### Part 2 - EntityManager

Q1. Why does EntityManager use DataManager instead of reading JSON itself?

The DataManager is responsible for loading and storing data, while the EntityManager is responsible for creating and removing GameObjects. Separating these responsibilities makes the system easier to understand and maintain.

Q2. What happens when an entity is despawned?

The EntityManager looks up the object using its id, calls Destroy on the GameObject, removes it from the live dictionary, and logs that the entity was despawned.

If the id does not exist, the method returns without trying to destroy an invalid object.

Q3. Which Lab 03 callbacks occurred during spawning and despawning?

When entity_01 was spawned, Awake and OnEnable ran during Instantiate and Start ran afterward.

When entity_01 was despawned, OnDisable ran first and OnDestroy followed.

Q4. What does Lab04ManagerDemo do?

Lab04ManagerDemo gets the first entity id from the DataManager. At about 3 seconds it despawns that entity, and at about 5 seconds it spawns the entity again. The Console markers and lifecycle callbacks confirmed both operations.

Q5. What do the main EntityManager methods do?

`SpawnAll` loops through all entity data and spawns each entity.

`Spawn` finds the entity data, loads the matching prefab from Resources, creates it at its JSON position, applies its points and spin speed, and adds it to the live dictionary.

`Get` returns a currently live GameObject by id.

`Despawn` destroys and removes one live entity.

`Clear` destroys all currently live entities and clears the live dictionary.

### 7. Scenes

Q1. Which scenes were loaded at the same time?

During the additive-loading test, both Lab04_Main and Lab04_Level were loaded at the same time.

Lab04_Level finished loading at about 0.10 seconds.

Q2. What happened when Lab04_Level was unloaded?

At about 8.10 seconds, Lab04_Level was unloaded. Objects owned by that scene, such as its Plane and Directional Light, disappeared.

The managers and spawned entities remained because they belonged to Lab04_Main.

Q3. What design decision is needed if entities should disappear with their level?

The game must decide which scene owns the entities. If entities need to disappear when a level is unloaded, they should be created under an object belonging to that level scene instead of being kept under the main scene.

Q4. What does Lab04SceneLoader do?

Lab04SceneLoader asynchronously loads Lab04_Level in Additive mode. After Unity reports that loading is complete, it calls SpawnAll on the EntityManager. After the level has been loaded for eight seconds, it asynchronously unloads Lab04_Level and logs when the unload is complete.

### 8. Git LFS

I configured Git LFS for the required binary asset types.

The Lab 04 files tracked through Git LFS include:

- `Lab04_Sound.wav`
- `Lab04_PropA.fbx`
- `Lab04_PropB.fbx`
- `Lab04_PropC.fbx`
- `Lab04_Texture.png`

The OBJ and JSON files remain regular Git files because they are text-based formats.

I verified the Lab 04 LFS files with:

```text
0c65e3aa1c * unity/Lab02-Unity/Assets/Lab04/Audio/Lab04_Sound.wav
742758355e * unity/Lab02-Unity/Assets/Lab04/Models/Lab04_PropA.fbx
ab4fa392eb * unity/Lab02-Unity/Assets/Lab04/Models/Lab04_PropB.fbx
37a275d6ad * unity/Lab02-Unity/Assets/Lab04/Models/Lab04_PropC.fbx
bd32dac636 * unity/Lab02-Unity/Assets/Lab04/Textures/Lab04_Texture.png
```

### 9. Challenge

For the challenge, I created a second data file named `Lab04_Entities2.json` and a separate scene named `Lab04_Challenge`.

I used `Lab04ChallengeSwitcher` to alternate between `Lab04_Entities.json` and `Lab04_Entities2.json` every five seconds while the game remained running.

My test produced the following sequence:

- About 5 seconds: switched to `Lab04_Entities2.json`
- About 10 seconds: switched back to `Lab04_Entities.json`
- About 15 seconds: switched to `Lab04_Entities2.json`
- About 20 seconds: switched back to `Lab04_Entities.json`

Before loading the next file, the EntityManager clears the currently live objects. The DataManager then loads the new JSON data, and the EntityManager spawns the new entity set.

The second data file used different values, including points values of 50, 60, and 70. PropA also moved from the original position of approximately (-4, 0, 0) to approximately (-6, 0, 2) when the second file was loaded.

This showed that the data controlling the entities could be changed while the game continued running without restarting the application. 
