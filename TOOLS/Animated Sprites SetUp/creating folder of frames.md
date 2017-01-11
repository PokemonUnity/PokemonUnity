# Creating Folder of Frames

I apologise that this process isn't completely automated.
Hopefully in future all Pokémon sprites will be processed and available.
Check the subreddit and other documentation for processed files. There may be something there.

## How to use

### open in GIMP

* increase canvas size to 128x128, centre, anchored to the bottom by increasing Y offset to maximum
* "Layer to Image Size" on the first frame/bottom later
* export to Resized (Ctrl+e) as animation, loop forever, unspecified frame disposal set to "replace", 100 milliseconds


### open in gifsplitter

* tick Autofill directory
* tick single background color
* set background color to pure green (#00ff00 / R:0, G:255, B:0) 
* Split Now
* copy the folder "_output" into the gif's split-up folder (or create an empty folder called "_output")
* open the split-up folder
* right click "_output" and open propeties
* copy the Location string

### open cmd (WITH IMAGEMAGICK INSTALLED)

* type "cd "
* right click "Paste" (has to be a right click)
* press enter
* copy, paste, and run the following line: `mogrify -alpha Set -format png *.bmp`
* copy, paste, and run the following line: `FOR %G IN ("*.png") DO convert "%G" -transparent #00ff00 "_output\%G"`
* rename "_output" to the Pokédex number


Folder of Frames is now finished. Drag this folder into your
Unity Assets Resources folder with the others to add it to the game
