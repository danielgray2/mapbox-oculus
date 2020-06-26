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
To install MRTK, click here, and then import the unity package into your project by going to Assets >
Import Package > Custom Package, and then browsing for the package.

## Step 3: Install Oculus SDK
To install the Oculus SDK, go to the asset store inside of the Unity Editor, and search for "Oculus". The
SDK should be the first one that pops up. Click "yes" anytime that it asks you if you want to do something.
The process is completed once Unity restarts. Once Unity restarts, a pop-up should appear for MRTK. Click the
button that lets MRTK do what it wants.

## Step 4: Install MRTK-Quest
Click here to install the package, and import it using the same process that was used for MRTK in Step 2

## Step 5: Install Standard Assets
Go to the Assets store in Unity and search for "Standard Assets". Download and import the one made by Unity
Technologies. Once it imports, you should get an error saying that something related to a GUI has been deprecated.
To fix this, double click on the error to bring up the file that is causing the issue, and comment out all the lines
that have an instance of the variable that has been deprecated. You should comment out 3 lines.

## Step 6: Configure XR-Plugin Settings
The project is configured to use Unity's plugin system for VR. The only thing that you have to do is go to Edit >
Project Settings > XR Plug-in Managment and check the box next to Oculus. If there is no checkbox, and you are asked
to install the XR Plugin, proceed with the installation, and then select the checkbox. If you run into issues, here is
a document on how to configure the Oculus SDK with Unity.

## Step 7: Configure MRTK Settings
In the project window, go to Scences > Sample Scene. Once it loads, there should be a gameobject called MixedReality
Toolkit. Click on it. A window should appear that says that no configuration has been assigned to MRTK. Assign the
DefaultMixedRealityToolkitConfigurationProfile.

## Step 8: Setup Mapbox
The installation for Mapbox is relatively straight-forward. All you should have to do is go to this link and follow
the steps to download the SDK and generate credentials. When you import the project into Unity (Assets > Import
Package > Custom Package), and it asks you which packages you which packages you would like to import, only import
the packages named MapBox and Third Party. Do not include any of the packages that relate to AR, as we will not be
using AR. If these packages are included, they will cause errors.

## Step 9: Run the application
That should be it. Now you should be able to run the application. See if you can teleport around the screen using the
hands in the visualization like can be seen in the README here.
