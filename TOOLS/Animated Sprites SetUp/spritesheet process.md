# Spritesheet Process

## open in GIMP

* increase canvas size to 128x128, centre, anchored to the bottom by increasing Y offset to maximum

* layer to image size on the first frame/bottom later (Alt+X)

* export (Ctrl+e) as animation, loop forever, frame disposal to replace

## open in gifsplitter

* tick Autofill directory
* tick single background color
* set background color to pure green
* Split Now

## open in SpriteSheetPacker

* click and drag every image made by gifsplitter onto the list
* set image padding to 0
* tick Require Square Output
* name the Image File the same as the gif (place in spritesheets folder)
* untick generate map
* Build Sprite Sheet (you can delete the auto generated folder from gifsplitter now)

## open spritesheet in GIMP

* Select By Color, threshold = 0.0	
* click the background and delete
* overwrite (Shift+Ctrl+Alt+S)
