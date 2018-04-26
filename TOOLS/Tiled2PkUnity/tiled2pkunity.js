//Do not edit or save this JS file using Notepad or any other non-code text editor.
//Please use a formatted editor such as Visual Studio Code (NOT Visual Studio) or Notepad++ in order to modify this file.

//Tiled2PkUnity was developed in Node.js by TeamPopplio. 
//Please ask me for help/support with this app rather than contacting the Pokemon Unity team.
console.log(process.argv.join(" "))
if(!process.argv.slice(2)[0] || !process.argv.slice(2)[1]) 
    console.log("Missing arguments!")
    //node tiled2pkunity.js ./map.json [./collision.txt]
    //node tiled2pkunity.js (Map JSON) [(Collision File)]
    //Only the map's JSON is required
    
var fs = require('fs'), //Filesystem Module
    json = require(process.argv.slice(2)[0]), //JSON file
    filename = process.argv.slice(2)[2], //Filename
    tiles = json.layers[0].data, //JSON Tilemap
    tileset = {file:json.tilesets[0],c:[],nc:[],w:[],e2:[]}, //Tileset data
    compiled = []; //Compiled map data
if(!filename)
    filename = process.argv.slice(2)[0].split(".json").push("_COMP.txt").join("")

//Tileset Terrain Tags\\
/*console.log("\nCreating tileset terrain tags...\n")
for(var z = 0; z< tileset.file.tilecount; z++)
{
    var tile = tileset.file.tileproperties[z.toString()]
    if(tile.Collision)
    {
        tileset.c.push(z+1)
        console.log("Tileset #" + z.toString() + " = Collidable")
    }
    else if(tile.Water)
    {
        tileset.w.push(z+1)
        console.log("Tileset #" + z.toString() + " = Water")
    }
    else if(tile.Environment2)
    {
        tileset.nc.push(z+1)
        console.log("Tileset #" + z.toString() + " = Env2")
    }
    else
    {
        nocollision.push(z+1)
        console.log("Tileset #" + z.toString() + " = Non-Collidable")
    }
}*/
    
//Collision\\
console.log("\nLayer 1:\n")
for(var l = 0; a< json.layers.length; a++) {
    var layer = compiled.push([])
    for(var a = 0; a< json.layers[l].data.length; a++) {
        if((json.layers[l].data[a]-1) < 0)
        {
            layer.push(0) //Blank tile, no collision
            console.log("Tile #"+json.layers[l].data[a])
        }
        else
            layer.push(tileset.file.tileproperties[(json.layers[l].data[a]-1).toString()]["Type"])
    }
}
//Writing the file\\
fs.writeFileSync(filename,collision.join(" "), (err) => {
    if (err) throw err;
    console.log("\nFinished conversion process for " + process.argv.slice(2)[0] + ", printed collision map to " + filename + "\n")
})
//Version 1.5