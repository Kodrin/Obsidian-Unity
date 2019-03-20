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
- Windows machine with 2 HDMI ports (Multi-Screen display with projectors)

### Branches

- Master branch ~ Main/Shaders
- Kinect PC branch ~ Kinect testing and point cloud coding
- Kinect Texture branch ~ Kinect testing/point cloud on textures
- Reaktion branch ~ Audio Reactive and testing stuff bro

---

### Software architecture

Core Scripts:
- Main.scene
- WorldManager.cs
- PlayerController.cs
- DisplayScript.cs (multi-display)

Tertiary Scripts:
- ...
- ...
- ...
- ...


---

### References

- Kinect API-Unity ~ https://docs.microsoft.com/en-us/previous-versions/windows/kinect/dn758543(v=ieb.10)
- Simple Depth Update/ Oliver Jones ~ https://stackoverflow.com/questions/37198974/microsoft-kinect-v2-unity-3d-depth-warping
- Keijiro Takahashi/ DataMosh Shader ~ https://github.com/keijiro/KinoDatamosh
- Keijiro Takahashi/ Kino Bokeh~ https://github.com/keijiro/KinoBokeh
- Ronja Shader/ Single step shader ~ https://www.ronja-tutorials.com/
- Minion Art/ Shader tutorial ~ https://www.patreon.com/minionsart
- Serializing and saving ~ https://www.raywenderlich.com/418-how-to-save-and-load-a-game-in-unity 
- Point Cloud rainbow ~ http://izmiz.hateblo.jp/entry/2017/12/30/003542
- Reading textures without ressource folder ~ https://answers.unity.com/questions/1225227/loading-textures-during-runtime-and-applying-to-ra.html
- Getting a list of files in directory ~ https://answers.unity.com/questions/16433/get-list-of-all-files-in-a-directory.html
- Creating folder/directory ~ https://answers.unity.com/questions/52401/creating-a-directory.html
- Overlaying Objects with video textures ~ https://www.youtube.com/watch?v=KG2aq_CY7pU
- Create Materials at Runtime ~ https://answers.unity.com/questions/1242929/how-to-create-new-materials-at-runtime.html
- Clipping shaders to ignore verteces ~ https://forum.unity.com/threads/stop-executing-the-pixel-shader-by-a-specified-vertex-distance-to-the-camera.532922/#post-3509802
- Vertex Displacement shaders ~ https://www.jordanstevenstechart.com/vertex-displacement
- Kinect joint ID ~ https://medium.com/@lisajamhoury/understanding-kinect-v2-joints-and-coordinate-system-4f4b90b9df16


### Notes
- Unity returns the file path differently in editor vs build mode ~ https://answers.unity.com/questions/272486/cannot-read-or-write-a-txt-file-after-build.html