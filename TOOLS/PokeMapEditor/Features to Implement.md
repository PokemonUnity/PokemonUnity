v1.0
☑ improve lighting
☑ set grid to perfect 1x1

v1.1
☑ add Z-Alignment

v1.2
☑ stabilise cursor position 
	(added x-ray to sensor, hitting only object with property: "this")
☑ prevent stretch rotation of objects on different Z to cursor
	(rotates around object's z position instead of cursor's)

v1.3
☑ allow holding add/remove to "paint"
	(changed Mouse1 ==1 to ==2, to check while held instead of when pressed)
☑ remember last direction
	(added a variable to own[] to remember the previous object's angle)

v1.4
☑ add export model feature
	(uses each placed object's vertices and polygons to recreate an .obj file)

v1.5
☑ add support for exporting rotated tiles
	(adjusts the vertex local position using the objects z orientation)
☑ decrease opacity of cursor

v1.6
☑ add camera controls
	(uses keypad, 5 to re-center)
☑ angle camera to assist perspective-editing
☑ increase maximum map size
	(75x75)
☑ add border to signify edge of grid

v1.7
☑ make new files export to a different name
	(generates a random number upon loading the program, using that as the name)

v1.8 (unfinished)
☐ auto-import models from a models-folder
☐ allow orthographic/perspective view switching
☐ create standalone executable
