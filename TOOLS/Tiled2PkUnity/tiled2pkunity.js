//Do not edit or save this JS file using Notepad or any other non-code text editor.
//Please use a formatted editor such as Visual Studio Code (NOT Visual Studio) or Notepad++ in order to modify this file.

//Tiled2PkUnity was developed in Node.js by Kisu-Amare/TeamPopplio. 
//Please ask me for help/support with this app rather than contacting the Pokemon Unity team.

const fs = require('fs') //Filesystem Module
if(process.argv.slice(2)[0] != "merge") { //If you're not executing the merger

if(process.argv.slice(2)[0] != undefined && process.argv.slice(2)[0] != null && process.argv.slice(2)[1] != undefined && process.argv.slice(2)[1] != null && process.argv.slice(2)[2] != undefined && process.argv.slice(2)[2] != null) {

    //Consistents\\
    const json = require(process.argv.slice(2)[0]) //JSON file
    const tilesetagain = require(process.argv.slice(2)[1]) //Tileset file
    const filename = process.argv.slice(2)[2] //Filename

    //Variables\\
    var fd = fs.openSync(filename, 'w') //Create file
    var tiles = json.layers[0].data //JSON Tilemap
    var collision = [] //Blank array
    var nocollision = [] //Blank array
    var water = [] //Blank array
    var environment2 = [] //Blank array
    var layer = parseInt(process.argv.slice(2)[3]) - 1 //Layers
    if(isNaN(layer)) {
        layer = 0 //No layer? I'll do it at #1 instead
    }
    var realnum = layer + 1
    
    //Tileset Terrain Tags\\
    console.log(" ")
    console.log("Creating tileset terrain tags...")
    console.log(" ")
    for(var z = 0; z< tilesetagain.tilecount; z++) {
        var tile = tilesetagain.tileproperties[z.toString()]
        var num = z + 1
        if(tile.Collision == true && tile.Water == false && tile.Environment2 == false) {
            collision.push(num)
            console.log("Tileset #" + z.toString() + " = Collidable")
        } else if(tile.Collision == false && tile.Water == false && tile.Environment2 == false) {
            nocollision.push(num)
            console.log("Tileset #" + z.toString() + " = Non-collidable")
        } else if(tile.Collision == false && tile.Water == true && tile.Environment2 == false) {
            water.push(num)
            console.log("Tileset #" + z.toString() + " = Water")
        } else if(tile.Collision == false && tile.Water == false && tile.Environment2 == true) {
            nocollision.push(num)
            console.log("Tileset #" + z.toString() + " = Env2")
        } else {
            console.log("Tileset #" + z.toString() + " = ??? Please make sure that water tiles have no collision and env2 tiles are not water/collidable, and make sure that all properties are present and are booleans.")
            process.exit()
        }
    }
    
    //Collision Map\\
    var tileset = [] //Blank array
    console.log(" ")
    console.log("Creating collision map...")
    console.log(" ")
    for(var a = 0; a< json.layers[layer].data.length; a++) { //Excuse my horrible workaround lmao
        var ti = json.layers[layer].data[a]
        if(ti == 0) {
            tileset.push(0)
            console.log("Tile #" + a.toString() + " = Blank")
        }
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
        console.log("Finished conversion process for layer #" + realnum + ", printed collision map to " + filename)
        console.log(" ")
        fs.closeSync(fd)
    })
} else {
    console.log("Missing arguments!") //Error output
}} else {

    //Merging\\
    if(process.argv.slice(2)[0] != undefined && process.argv.slice(2)[0] != null && process.argv.slice(2)[1] != undefined && process.argv.slice(2)[1] != null && process.argv.slice(2)[2] != undefined && process.argv.slice(2)[2] != null) {
        const collision1 = process.argv.slice(2)[1] //First collision map layer
        const collision2 = process.argv.slice(2)[2] //Second collision map layer
        const filename = process.argv.slice(2)[3] //Filename
        var fd = fs.openSync(filename, 'w') //Create file
        console.log(" ")
        console.log("Merging " + process.argv.slice(2)[1] + " with " + process.argv.slice(2)[2] + "...")
        console.log(" ")
        var array1 = [] //Blank array
        var array2 = [] //Blank array
        var text1 = fs.readFileSync(collision1, "utf-8")
        var text2 = fs.readFileSync(collision2, "utf-8")
        array1 = text1.split(" ")
        array2 = text2.split(" ")
        for(var a = 0; a< array2.length; a++) {
            if(array2[a] != 0) {
                console.log("#" + array1[a].toString() + " -> #" + array2[a])
                array1.splice(a, 1, array2[a]) //Replace the value in the first array with the new one
            }
        }

        //Writing the file\\
        fs.writeFile(filename,array1.join(" "), (err) => {
            if (err) throw err;
            console.log(" ")
            console.log("Finished merging " + process.argv.slice(2)[2] + " with " + process.argv.slice(2)[1] + ", printed collision map to " + filename)
            console.log(" ")
            fs.closeSync(fd)
        })
    } else {
        console.log("Missing arguments!") //Error output
    }
}
//Version 1.5