# How to use Tiled2PkUnity
This tool is meant to replace the Pokemon Essentials part of creating a map in Pokemon Unity.
It creates a collision map out of a tilemap made with Tiled, using a tileset with specific variables.
To use this tool, you must first install the *Tiled Map Editor* (www.mapeditor.org) and the LTS version of *Node.js* (www.nodejs.org).

## Tiled
In order to use this application, you need a tileset with specific booleans to specify what tiles do what.
Simply create a tileset directing to your image source, and select all tiles.
Once you have selected every tile, click the plus icon in the bottom left corner and add three properties: Collision, Water, and Environment2. Make sure all properties are set to the 'bool' type and are all set to false.

Now that you have set all properties, you can now select tiles and set those booleans to true or false. Remember that a tile with collision cannot be water or env2, and a tile with env2 cannot be water or collidable, and vise versa.
Once you're completed with setting your properties, save the tileset into this directory as a 'JSON Tileset File (\*.json)', __NOT__ a 'Tiled Tileset File (\*.tsx)'.

After you have set up your tileset, you can now proceed to creating your tilemap. Make sure you do not use more than one tileset! Keep in mind that collision maps are always generated onto the first layer unless otherwise specified.
Once the tilemap is complete, export the tilemap as a JSON file into this directory.

You can now proceed towards making the collision map.

## Launching the app
Right click the *Launch.bat*, click on 'Edit' and change *./tileMAP.json* to match the directory of your tilemap, and change *./tileSET.json* to match the directory of your tileset. You can also change the *collision.txt* although this isn't nessasary. If you want to create a collision map for a layer other than #1, you can do so in the arguments following the filename.

Once Node.js is confirmed to be installed, launch the batch file and wait until the finish prompt is outputted.
Afterwards, your collision map should be in the text file placed inside the arguments. Copy this collision map into the MapCollider on your Pokemon Unity map and test the game to make sure it works properly, if not, try setting some offsets and make sure your width and height are set correctly.

Arguments:
>node tiled2pkunity.js [./Tilemap.JSON] [./Tileset.JSON] [Output.TXT] <Layer Number>

## Merging
In v1.5, it's possible to merge two collision maps as layers, this comes in handy when your transparent tiles have to have something under them.
To merge tiles, you need to have at least two collision maps. To do so, add an argument to the end of your output filename that has the layer number you would like to generate. Generate all layers one at a time in seperate files (such as collision1.txt, collision2.txt, etc). 

Once all layers are finished, edit the *Launch.bat* and remove all arguments, replace the first argument with *merge*, the second argument with your base layer (usually the bottommost layer, or if you're chain merging it should be the merge file), the third argument with the next layer, and the last argument with your output filename. The console output may not be accurate most of the time, so don't worry if there's something wrong.

Arguments:
>node tiled2pkunity.js merge [./Base.TXT] [./Mergewith.TXT] [Output.TXT]

## Thanks for using my tool!
Tiled2PkUnity was developed by Kisu-Amare 🐾