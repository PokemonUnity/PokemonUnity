//Do not edit or save this JS file using Notepad or any other non-code text editor.
//Please use a formatted editor such as Visual Studio Code (NOT Visual Studio) or Notepad++ in order to modify this file.

//Tiled2PkUnity was developed in Node.js by Kisu-Amare/TeamPopplio. 
//Please ask me for help/support with this app rather than contacting the Pokemon Unity team.

if(process.argv.slice(2)[0] != undefined && process.argv.slice(2)[0] != null && process.argv.slice(2)[1] != undefined && process.argv.slice(2)[1] != null && process.argv.slice(2)[2] != undefined && process.argv.slice(2)[2] != null) {

    //Consistents\\
    const fs = require('fs') //Filesystem Module
    const json = require(process.argv.slice(2)[0]) //JSON file
    const tilesetagain = require(process.argv.slice(2)[1]) //Tileset file
    const filename = process.argv.slice(2)[2] //Filename

    //Variables\\
    var fd = fs.openSync(filename, 'w') //Create file
    var tiles = json.layers[0].data //JSON Tilemap
    var errors = 0 //Error count
    var collision = [] //Blank array
    var nocollision = [] //Blank array
    var water = [] //Blank array
    var environment2 = [] //Blank array
    
    //Tileset Terrain Tags\\
    console.log(" ")
    console.log("Creating tileset terrain tags...")
    console.log(" ")
    for(var z = 0; z< tilesetagain.tilecount; z++) {
        var tile = tilesetagain.tileproperties[z.toString()]
        if(tile.Collision == true && tile.Water == false && tile.Environment2 == false) {
            collision.push(z + 1)
            console.log("Tileset #" + z.toString() + " = Collidable")
        } else if(tile.Collision == false && tile.Water == false && tile.Environment2 == false) {
            nocollision.push(z + 1)
            console.log("Tileset #" + z.toString() + " = Non-collidable")
        } else if(tile.Collision == false && tile.Water == true && tile.Environment2 == false) {
            water.push(z + 1)
            console.log("Tileset #" + z.toString() + " = Water")
        } else if(tile.Collision == false && tile.Water == false && tile.Environment2 == true) {
            nocollision.push(z + 1)
            console.log("Tileset #" + z.toString() + " = Env2")
        } else {
            errors = errors+1
            console.log("Error! Tileset #" + z.toString() + " = ??? Please make sure that water tiles have no collision and env2 tiles are not water/collidable, and make sure that all properties are present and are booleans.")
        }
    }
    
    //Collision Map\\
    var tileset = []
    console.log(" ")
    console.log("Creating collision map...")
    console.log(" ")
    for(var a = 0; a< tiles.length; a++) {
        var ti = tiles[a]
        for(var b = 0; b< collision.length; b++) {
            if(ti == collision[b]) {
                tileset.push(1)
                console.log("Tile #" + a.toString() + " = Collidable")
            }
        }
        for(var c = 0; c< nocollision.length; c++) {
            if(ti == nocollision[c]) {
                tileset.push(0)
                console.log("Tile #" + a.toString() + " = Non-collidable")
            }
        }
        for(var d = 0; d< water.length; d++) {
            if(ti == water[d]) {
                tileset.push(2)
                console.log("Tile #" + a.toString() + " = Water")
            }
        }
        for(var e = 0; d< environment2.length; e++) {
            if(ti == environment2[e]) {
                tileset.push(3)
                console.log("Tile #" + a.toString() + " = Env2")
            }
        }
    }

    //Writing the file\\
    fs.writeFile(filename,tileset.join(" "), (err) => {
        if (err) throw err;
        console.log(" ")
        console.log("Finished conversion process with " + errors.toString() + " tileset collision error(s), printed collision map to " + filename)
        console.log(" ")
        fs.closeSync(fd)
    })
} else {
    console.log("Missing arguments!") //Error output
}
//Version 1.0