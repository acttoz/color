The Sol System - Not to Scale
by Mark Nielsen
mark@secondmousestudios.com
Second Mouse Studios
=====================================================================

Description
--------------------
This package includes the star, Sol, nine planets (including Pluto), and a total of 23 well known 
moons.  The moons orbit their parent planet and the planets orbit the parent star.  The planets 
also include an orbit trail that marks their orbit around the star.  This model is not to scale.
See the sample at www.secondmousestudios.com/SolSystem/WebPlayer.html.  More planetary models are
on their way.

Included Assets
--------------------
1.	Included in this package are the star Sol and nine planets (including Pluto) and a total of 
	23 well known moons.
2.	Each body is textured with NASA satellite images and have been colorized and the
	blanks filled in as necessary to complete the texture map.  In some cases (like Pluto and
	its moon Charon) there are no satellite images so I made them up. I tried to be as accurate
	as possible when texturing the planets. 
3.	Each Planet orbits the sun, Sol and each moon orbits its respective parent.
4.	Each planet has a tracer trail.  It follows the planet, marking its orbital path.  The length
 	of this can be adjusted on the OrbitTrail object.  It’s a child of the planet body.	If you
 	don’t want to have the trails, simply remove or disable the object child from the planet body.
5.	Venus has two associated maps and spheres.  The visible one is the cloud layer and the 
	smaller on within it is the ground layer.  If you want to use the ground map, simply 
	uncheck the ‘renderable’ option on the cloud layer 
6.	Similarly, Earth has two maps: One with clouds (default) and one without.  To change
	them, simply drag the “3Earth” or “3EarthwithClouds” PNG file from the Planet Maps
	folder in the project window, and drop it on Earth’s sphere.
7.	The speed of the orbits of the planets around the sun are approximated to reflect the
	relative orbits of each planet's actual orbit and the numbers rounded for simplicity.
8.	The orbits of the moons around their parent planet are not accurate nor are they in any	
	particular order. 
9.	The distance of each body from its parent is not to scale.  
10.	Also included is a basic starfield skybox, a pointlight to illuminate the scene, and a
	spotlight to illuminate the sun.  None of the objects (namely the Sun) are self 
	illuminating or animated so if you want to have the sun cast light or produce solar flares, 
	you’ll have to build that.

Using this Asset
--------------------
The orbit of each body has a “rotate” script attached to it.  The variables are modifiable along
each X,Y,Z axis via this script.  Select the Orbit object to adjust.  For example, to adjust the
Earth’s moon’s orbit (Luna), drill down from 0Sol, to 3EarthOrbit, 3Earth, and select 3aOrbit.  
Modify the axis you want to change.  The default setting for Luna is: x=0, y=-200, and z=0.

If you need to adjust the orbital distance of any planet or moon, make sure you remove the planet
or moon from its parent orbit first.  Then change the Orbit objects scale evenly across the XYZ
axis (i.e. from 3,3,3 to 4,4,4).  Then move the planet or moon to the proper position and drag it
back to its parent.  If you don’t do it this way, the planet or moon will scale with the orbit and 
become larger or smaller than intended. 

The other option is to simply move the planet or moon independently from the orbit object to the 
desired distance from the parent.  The problem with that, however, is that it can get unclear which
orbit object belongs to which planet or moon object. 

Finally, keep an eye on the Asset Store.  I will be placing Planet Packs that will include stars/suns,
planets, moons and asteroids.  Some of those will have effects like comets, atmospheres, cloud layers,
and artificial satellites.

See a sample here: www.secondmousestudios.com/SolSystem/WebPlayer.html
Visit our site at www.secondmousestudios.com