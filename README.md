### Obsidian ~ The human archive

---

### Authors

- Ali Egseem - https://www.aliegseem.com/
- Codrin Tablan Negrei - http://www.miha-ko.ca/
- Valerie Bourdon - https://www.valeriebourdon.com/

---

### Compatibility

Unity 2018.3.4f1

### Setup

- Kinect V2
- Kinect SDK package
- Windows machine with 2 HDMI ports (Multi-Screen display with 2 projectors)

### Branches

- Master branch ~ Main/Shaders
- Kinect PC branch ~ Kinect testing and point cloud coding
- Kinect Texture branch ~ Kinect testing/point cloud on textures
- Reaktion branch ~ Audio Reactive and testing stuff bro

---

### Software architecture

Kinect SDK:
- BodySourceManager.cs
- BodySourceView.cs (Allows to track bones/body)

Core Scripts:
- WorldManager.cs (Manages scene transition/conditions)
- PlayerController.cs (Manages the kinect controls/interaction)
- LoadData.cs (Loads the data/shifts it with new participants)
- PointCloudDataManager.cs (Handles the depth texture)

Post-processing scripts:
- DataMoshController.cs (The grain effect)

Projections Scripts:
- VideoOnTerminal.cs (Main wall projection/Reference from Charles Doucet)
- FloorProjection.cs (Floor projection)

Camera Scripts:
- CameraForward.cs
- OrbitControls.cs

---

### Development log

Interaction:
As is, the installation features interactive components in only the 2 first phases, when the participants have to raise their hands and when the participants interact with their projected point clouds. For the point cloud interaction, I fecthed the hand distances and also calculated the angle of the arms. The hand distance was linked to the camera orbit controls and allowed the participant to zoom in/out as they were extending/contracting their arms. On the other hand, the angle of the arms was mapped to the amount of entropy in the shader which made the point cloud projection more or less distorted. 

Shader work:
For the shader that went into the point cloud, I used a shader template that I found from Atsushi Izumihara which I modified heavily to create my desired shader. Izumihara created an efficient way to display depth data in 3D world space as opposed to camera space. This simplified a lot of things as I could just code a fragment shader in shaderlab. For the fragment shader, I added color gradients, entropy and sin() modifiers. The entropy modifier distorts the verteces so it gets rid of that sharp edge look while the sin() modifier cuts the depth data on a z-axis threshold and distorts the data in a wavy pattern after the participant steps beyond that threshold.

Managing data:
Another main component of the installation is the capturing, sorting and saving of data. The participant's data is captured as soon as they put their hands together at the beginning. The data is then shifted through an array of similar point clouds with a unique hash (name tag generated dynamically) which is outputted by a function I wrote. The output is "o_" which stands for obsidian_ followed by a set of 10 random characters (lower case/ upper case/ numbers). Every time a participant is saved, the data is randomized for the participant as an effort to create a unique experience each time obsidian is initialized. The purpose of the randomization is 2-fold as the randomizing the data also prompts the user to look for their scan. After the experience, the data is then saved locally on disk and uploaded directly onto the web version of our installation for people to see. 


---

### References

- Kinect API-Unity ~ https://docs.microsoft.com/en-us/previous-versions/windows/kinect/dn758543(v=ieb.10)
- Simple Depth Update/ Oliver Jones ~ https://stackoverflow.com/questions/37198974/microsoft-kinect-v2-unity-3d-depth-warping
- Keijiro Takahashi/ DataMosh Shader ~ https://github.com/keijiro/KinoDatamosh
- Keijiro Takahashi/ Kino Bokeh~ https://github.com/keijiro/KinoBokeh
- Ronja Shader/ Single step shader ~ https://www.ronja-tutorials.com/
- Minion Art/ Shader tutorial ~ https://www.patreon.com/minionsart
- Serializing and saving ~ https://www.raywenderlich.com/418-how-to-save-and-load-a-game-in-unity 
- Point Cloud rainbow/ Izumihara Atsushi ~ http://izmiz.hateblo.jp/entry/2017/12/30/003542
- Reading textures without ressource folder ~ https://answers.unity.com/questions/1225227/loading-textures-during-runtime-and-applying-to-ra.html
- Getting a list of files in directory ~ https://answers.unity.com/questions/16433/get-list-of-all-files-in-a-directory.html
- Creating folder/directory ~ https://answers.unity.com/questions/52401/creating-a-directory.html
- Overlaying Objects with video textures ~ https://www.youtube.com/watch?v=KG2aq_CY7pU
- Create Materials at Runtime ~ https://answers.unity.com/questions/1242929/how-to-create-new-materials-at-runtime.html
- Clipping shaders to ignore verteces ~ https://forum.unity.com/threads/stop-executing-the-pixel-shader-by-a-specified-vertex-distance-to-the-camera.532922/#post-3509802
- Vertex Displacement shaders ~ https://www.jordanstevenstechart.com/vertex-displacement
- Kinect joint ID ~ https://medium.com/@lisajamhoury/understanding-kinect-v2-joints-and-coordinate-system-4f4b90b9df16
- VHS shader ~ http://www.shaderslab.com/demo-38---vhs-tape-effect.html
- Casting unity.object[] to texture2d ~ https://answers.unity.com/questions/15446/converting-unityengineobject-to-unityenginetexture.html

### Notes
- Unity returns the file path differently in editor vs build mode ~ https://answers.unity.com/questions/272486/cannot-read-or-write-a-txt-file-after-build.html

---