Contact me at:
name: Nechifor Andrei Marian
email: neomarian@gmail.com
email: neom_2005@yahoo.com
facebook: https://www.facebook.com/andreimarian.nechifor

Plugin Name: LoadingAnimation
Version: 1.2
Release Date: 28 January 2013

Changelog
v1.2
-	+2 loading sprites and +3 backgrounds
-	added loading bar (can display percent on top, hover or bottom of loading bar or can hide percent)
-	fixed a bug on loading sprites that consist of a single row
-	fixed a bug on text points

1.	Description
This package is designed to show a loading animation while something is loading behind. You can use it to display a loading on top of hole view or on a rect. This loading is fully optimized for Android, Web Player, PC, Max, Linux, Flash Player, Google Native Client and iOS platforms.

2.	Package Content
-	One background image (used for loading background)
-	Two png sprites (used for loading animation)
-	One prefab (loading prefab)
-	Two C# clases (Loading and Example)

3.	How to Use
First you need to import package in your project. Then you can open `Example` class to see some examples about how to use this package. If you want to test it you cand create an empty scene and add `Example` script on Camera then add `Loading` script on camera and uncheck `isLoadingNow` property from Loading class.
If you want to show loading animation on hole view window you can instantiate `Loading` prefab from `Prefabs` directory.
If you want to disable application functionality while loading is running you can use `isLoading` static variable (use this only if you have only one instance of `Loading` class) or `isLoadingNow` variable.
You can change background by changing `background` value from Unity Editor. Also you can change loading margin by changing `loadingMargin` value from Unity Editor (this variable is used only when loading is displayed on hole view). You show/hide background using `showBackground` variable.
Customize text:
-`showText` used to show/hide text
-`loadingText` text witch will be displayed
-`showLoadingTextPoints` used to show dots at the end of text
-`loadingTextPointsNumber` maximum number of dots from the end of text
-`loadingTextStyle` used to set text style
You can set another sprite by changing `loadingSprite` value from Unity Editor but you need to set one image size in `loadingImageSize` variable and number of images from last row of sprite image in `lastRowImagesNr` variable.
If you want to use multiple instances of Loading class you can add file on multiple objects from scene using Unity Editor.
To set a Rect where loading to be displayed use `SetLoadingRect` function.
To disable auto-start you need to set `isLoadingNow` on `false`.
To start loading call `StartLoading` function and to stop loading call `StopLoading` function.