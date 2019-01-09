//Tiled2PkUnity Version 2.1: "There was a typo" Edition
//Do not edit or save this JS file using Notepad or any other non-code text editor.
//Please use a formatted editor such as Visual Studio Code (NOT Visual Studio) or Notepad++ in order to modify this file.
//This app was developed in Node.js by TeamPopplio.
//Please ask me for help/support with this app rather than contacting the Pokemon Unity team.
if(!process.argv.slice(2)[0]) {
    console.log("You are missing the tilemap's JSON file in the arguments.\n")
    process.exit()
}
var fs, //Filesystem module, if installed
    json = require(process.argv.slice(2)[0]), //JSON file
    filename = process.argv.slice(2)[2], //Filename
    compiled = [], //Compiled map data
    p1 = true; //For the phases in the compilation
if(!filename) filename = process.argv.slice(2)[0].replace(".json","_cmap.txt") //If you did not provide a filename
try{fs = require('fs')}
catch(e) {
    if (e.code == 'MODULE_NOT_FOUND') {
        console.log("You do not have the FS module installed.\nCollision data will instead be outputted via the console.\n")
        fs = false;
    } else throw e;
}
//Compiling\\
console.log("Compiling collsion map for "+process.argv.slice(2)[0]+":")
for(var l = 0; l< json.layers.length; l++) {
    console.log("\nLayer "+(l+1)+":\n")
    for(var a = 0; a< json.layers[l].data.length; a++) {
        if((json.layers[l].data[a]-1) < 0) {
            if(p1) {
                compiled.push(0) //Blank tile, no collision
                console.log("Tile #"+(a+1)+" = Floor/Blank")
            }
        } else {
            try{var t = json.tilesets[0].tileproperties[(json.layers[l].data[a]-1).toString()]["Type"]}
            catch(e) {console.log("Cannot find the type of tile #"+(a+1)+"!");t=NaN}
            if(!isNaN(t)) {
                if(p1) compiled.push(t)
                else compiled[a] = t //Replace whatever was in this same position
                if(t == 2) console.log("Tile #"+(a+1)+" = Water")
                else if(t == 3) console.log("Tile #"+(a+1)+" = Environment2")
                else if(!t) console.log("Tile #"+(a+1)+" = Floor")
                else console.log("Tile #"+(a+1)+" = Wall")
            }
        }
    }
    p1 = false //Next layers should instead merge.
}
if(fs) {
    fs.writeFileSync(filename,compiled.join(" "), (err) => {if (err) throw err;})
    console.log("\nFinished compilation process for " + process.argv.slice(2)[0] + ", printed collision map to: \n" + filename + "\n")
}
else console.log("\nFinished compilation process for " + process.argv.slice(2)[0] + ":\n\n"+compiled.join(" ") + "\n")
