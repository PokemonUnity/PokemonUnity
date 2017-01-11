# DeKay's Pokémon Unity Collision Map String Compiler Extension for Pokémon Essentials.

This takes two coordinates on an essentials map and outputs the Terrain Tags that are within the given rect to a notepad file.

To use this extension, run the game in Essentials, and open `Debug` in the Start Menu.

Open Pokémon Essentials (v13 confirmed to work) and add the following into the PokemonDebug script.

```
  [underneath]
def pbDebugMenu
    [underneath]
  commands=CommandList.new
    [add this line]
  commands.add("dumptags",_INTL("Dump Terraintags"))


      [add the following code at (if cmd=="switches")]

    if cmd=="dumptags"
      @map_id = Kernel.pbMessageFreeText("Enter Map ID","#{$game_map.map_id}",false,5,Graphics.width).to_i
      return if @map_id < 0
      @map=load_data(sprintf("Data/Map%03d.%s", @map_id,$RPGVX ? "rvdata" : "rxdata"))
      tileset = $data_tilesets[@map.tileset_id]
      @terrain_tags = tileset.terrain_tags
      maptags = []
      pStart = Kernel.pbMessageFreeText("Enter Start (X,Y)","0,0",false,9,Graphics.width).split(',')
      pEnd = Kernel.pbMessageFreeText("Enter End (X,Y)","#{@map.width-1},#{@map.height-1}",false,9,Graphics.width).split(',')
      startX,startY = pStart[0].to_i,pStart[1].to_i
      endX,endY = pEnd[0].to_i,pEnd[1].to_i
      for y in startY..endY
        for x in startX..endX
          tag = 0
          for layer in [0,1,2]
            tile = @map.data[x,y,layer]
            tag = @terrain_tags[tile] if @terrain_tags[tile] != 0
          end
          maptags << tag
        end
        maptags << "\n\r"
      end
      chosenName=Kernel.pbMessageFreeText(_INTL("Enter a Name"),_INTL("map"),false,256,Graphics.width)
      Dir.mkdir("mapdumps") unless File.exists?("mapdumps")
      filename = "mapdumps/#{@map_id}_#{chosenName}_tags.txt"
      File.open(filename, 'w'){ |file|
      file.write("Mapinfo:\nWidth: #{endX-startX+1}    Height: #{endY-startY+1}\n")
        for t in maptags
          file.write(t)
          file.write(" ") if t!="\n\r"
        end
      }
      filename = "mapdumps/#{chosenName}_#{endX-startX+1}x#{endY-startY+1}_collision.txt"
      File.open(filename, 'w'){ |file|
        for i in 0..maptags.length-3
              file.write("#{maptags[i]} ") if maptags[i] != "\n\r"
        end
        file.write(maptags[-2])
      }
    els

    [The els should merge with the (if cmd=="switches") to become (elsif cmd=="switches")]
```
