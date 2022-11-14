# ZoneRegion

Open World - Zone Regions

Required Packages

<https://docs.unity3d.com/Manual/NavMesh-BuildingComponents.html>
com.unity.ai.navigation

Create your terrain tiles

Add the Player Scene Zone Manager to your player object
Set the Max Distance to 1 ½ of the terrain distance
For example, if your terrain tiles are 1000 x 1000 then set it to 1500

Triple check that you don’t have any duplicate terrain names

Move your terrains to the root of the scene

Open Window/Runningbird Studios/ZoneRegion-Utilities
Click the Create Zone Regions button
Once it’s done then you should see all of your Zone Region Scene Managers listed in the Utilities panel and all of the scenes loaded.

This creates a sub folder under the Scenes directory that is the name of your current scene.

You can now Hit the Unload all scenes button to remove all the scenes from the current scene.
Make sure on the ZoneRegions GameObject to set the Scene Zone Manager Max Distance to the same as the Player Scene Zone Manager.

Add all of the Zone Region Scenes to the build settings
