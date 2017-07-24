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

After you have set up your tileset, you can now proceed to creating your tilemap. Make sure you do not use more than one tileset! Keep in mind that collision maps are always generated onto the first layer, although you can modify line 17 to use a different layer.
Once the tilemap is complete (and all of your collision is on the first layer), export the tilemap as a JSON file into this directory.

You can now proceed towards making the collision map.

## Launching the app
Right click the *Launch.bat*, click on 'Edit' and change *./tileMAP.json* to match the directory of your tilemap, and change *./tileSET.json* to match the directory of your tileset. You can also change the *collision.txt* although this isn't nessasary.

Once Node.js is confirmed to be installed, launch the batch file and wait until the finish prompt is outputted.
Afterwards, your collision map should be in the text file placed inside the arguments. Copy this collision map into the MapCollider on your Pokemon Unity map and test the game to make sure it works properly, if not, try setting some offsets.

## Thanks for using my tool! Tiled2PkUnity was developed by Kisu-Amare🐾