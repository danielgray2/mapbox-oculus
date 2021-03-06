# oculus-mapbox
Testing to see if Mapbox, MRTK, and Oculus will work together

## Setting Up
In order to keep this repository as small as possible, I removed a lot of the dependencies. Because of this,
when you first open this project, you will see alot of errors. This is okay though because they will be fixed
as soon as the dependencies are installed

## Step 1: Get Unity Version 2019.4.1f1
This version was used for this project because the latest version of MRTK-Quest supported it. It was used as
a simplification to get us up and running quickly.

## Step 2: Install MRTK
To install MRTK, click [here](https://github.com/microsoft/MixedRealityToolkit-Unity/releases/download/v2.4.0/Microsoft.MixedReality.Toolkit.Unity.Foundation.2.4.0.unitypackage),
and then import the unity package into your project by going to Assets > Import Package > Custom Package, and
then browsing for the package.

## Step 3: Install Oculus Integration
To install the Oculus Integration, go to the asset store inside of the Unity Editor, and search for "Oculus". The
Integration should be the first one that pops up. Click "yes" anytime that it asks you if you want to do something.
The process is completed once Unity restarts. Once Unity restarts, a pop-up should appear for MRTK. Click the
button that lets MRTK do what it wants.

## Step 4: Install MRTK-Quest
Click [here](https://github.com/provencher/MRTK-Quest/releases/download/v1.0.1/MRTK-Quest_v101_Core.unitypackage)
to install the package, and import it using the same process that was used for MRTK in Step 2.

## Step 5: Install MRTK-Quest Examples
Click [here](https://github.com/provencher/MRTK-Quest/releases/download/v1.0.1/MRTK-Quest_v101_Examples.unitypackage)
to install the package, and import it using the same process that was used for MRTK in Step 2.

## Step 6: Install Standard Assets
Go to the Assets store in Unity and search for "Standard Assets". Download and import the one made by Unity
Technologies. Once it imports, you should get an error saying that GUIText is obsolete. To fix this, double
click on the error to bring up the file that is causing the issue, and comment out all the lines that have an
instance of the variable that has been deprecated (the variable name is camSwitchButton). You should comment out 3
lines.

## Step 7: Configure XR-Plugin Settings
The project is configured to use Unity's plugin system for VR. The only thing that you have to do is go to Edit >
Project Settings > XR Plug-in Managment and check the box next to Oculus. If you run into issues,
[here](https://developer.oculus.com/documentation/unity/unity-conf-settings/) is a document on how to configure
the Oculus SDK with Unity.

## Step 8: Setup Mapbox
The installation for Mapbox is relatively straight-forward. All you should have to do is go to [this]
(https://www.mapbox.com/install/unity/) link and follow the steps to download the SDK and generate credentials. 
When you import the project into Unity (Assets > Import Package > Custom Package), and it asks you which packages
you which packages you would like to import, only import the packages named MapBox and Third Party Assets. Do not
include any of the packages that relate to AR, as we will not be using AR. If these packages are included, they will
cause errors.

## Step 9: Configure MRTK Settings
In the project window, go to Scences > MapboxTestScene. Once it loads, there should be a gameobject called MixedReality
Toolkit. Click on it. A window should appear that says that no configuration has been assigned to MRTK. Assign the
MRTK-Quest Profile.

## Step 10: Add in the data file
Insert the data file that I sent you (in our email convertation) into Assets/Resources.

## Step 11: Add in the mesh data file
Insert the new data file that I sent you (in our email conversation) into Assets/Resources.

## Step 12: Upgrade to the URP
Unity has a couple of different render pipelines that users can use. In order to make the mesh coloring work, I needed
to change to the Universal Render Pipeline (URP). Follow the instructions 
[here](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.1/manual/InstallURPIntoAProject.html)
to install and configure the URP.

## Step 13: Update your shaders
Since we are now using the URP, we need to update our shaders to reflect that.
See [here](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.1/manual/upgrading-your-shaders.html)
for a tutorial on how to do that.

## Step 14: Update MRTK shaders
For some reason, the shaders that MRTK uses do not update when all of the shaders are updated. In order to do update these
shaders, select "Mixed Reality Toolkit" from the menu then go to "Utilities" => "Upgrade MRTK Shaders for Lightweight Render Pipeline".
After this, the shaders should be ready to go.

## Step 16: All Sky SkyBox
Go to the Unity Asset Store inside of the editor and search for "All Sky". There should be an asset called "All Sky" that is free. Install
that one.

## Step 15: Run the application
That should be it. Now you should be able to run the application. If everything is working, you should have all of the functionality
that I demonstrated in the videos.
