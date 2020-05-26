//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Debugger;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    public class DebuggerWindow : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;
        private bool m_NeedRestart = false;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_NeedRestart)
            {
                m_NeedRestart = false;
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
            }
        }

        public void OnDraw()
        {
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionChangeLanguage();
            }
            GUILayout.EndScrollView();
        }

        private void DrawSectionChangeLanguage()
        {
            GUILayout.Label("<b>Change Language</b>");
            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Chinese Simplified", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.ChineseSimplified;
                    SaveLanguage();
                }
                if (GUILayout.Button("Chinese Traditional", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.ChineseTraditional;
                    SaveLanguage();
                }
                if (GUILayout.Button("English", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.English;
                    SaveLanguage();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void SaveLanguage()
        {
            GameEntry.Setting.SetString(Constant.Setting.Language, GameEntry.Localization.Language.ToString());
            GameEntry.Setting.Save();
            m_NeedRestart = true;
        }
		/*def pbDebugMenu
  viewport=Viewport.new(0,0,Graphics.width,Graphics.height)
  viewport.z=99999
  sprites={}
  commands=CommandList.new
  commands.add("switches",_INTL("Switches"))
  commands.add("variables",_INTL("Variables"))
  commands.add("refreshmap",_INTL("Refresh Map"))
  commands.add("warp",_INTL("Warp to Map"))
  commands.add("healparty",_INTL("Heal Party"))
  commands.add("additem",_INTL("Add Item"))
  commands.add("fillbag",_INTL("Fill Bag"))
  commands.add("clearbag",_INTL("Empty Bag"))
  commands.add("addpokemon",_INTL("Add Pokémon"))
  commands.add("fillboxes",_INTL("Fill Storage Boxes"))
  commands.add("clearboxes",_INTL("Clear Storage Boxes"))
  commands.add("usepc",_INTL("Use PC"))
  commands.add("setplayer",_INTL("Set Player Character"))
  commands.add("renameplayer",_INTL("Rename Player"))
  commands.add("randomid",_INTL("Randomise Player's ID"))
  commands.add("changeoutfit",_INTL("Change Player Outfit"))
  commands.add("setmoney",_INTL("Set Money"))
  commands.add("setcoins",_INTL("Set Coins"))
  commands.add("setbadges",_INTL("Set Badges"))
  commands.add("demoparty",_INTL("Give Demo Party"))
  commands.add("toggleshoes",_INTL("Toggle Running Shoes Ownership"))
  commands.add("togglepokegear",_INTL("Toggle Pokégear Ownership"))
  commands.add("togglepokedex",_INTL("Toggle Pokédex Ownership"))
  commands.add("dexlists",_INTL("Dex List Accessibility"))
  commands.add("readyrematches",_INTL("Ready Phone Rematches"))
  commands.add("mysterygift",_INTL("Manage Mystery Gifts"))
  commands.add("daycare",_INTL("Day Care Options..."))
  commands.add("quickhatch",_INTL("Quick Hatch"))
  commands.add("roamerstatus",_INTL("Roaming Pokémon Status"))
  commands.add("roam",_INTL("Advance Roaming"))
  commands.add("setencounters",_INTL("Set Encounters")) 
  commands.add("setmetadata",_INTL("Set Metadata")) 
  commands.add("terraintags",_INTL("Set Terrain Tags"))
  commands.add("trainertypes",_INTL("Edit Trainer Types"))
  commands.add("resettrainers",_INTL("Reset Trainers"))
  commands.add("testwildbattle",_INTL("Test Wild Battle"))
  commands.add("testdoublewildbattle",_INTL("Test Double Wild Battle"))
  commands.add("testtrainerbattle",_INTL("Test Trainer Battle"))
  commands.add("testdoubletrainerbattle",_INTL("Test Double Trainer Battle"))
  commands.add("relicstone",_INTL("Relic Stone"))
  commands.add("purifychamber",_INTL("Purify Chamber"))
  commands.add("extracttext",_INTL("Extract Text"))
  commands.add("compiletext",_INTL("Compile Text"))
  commands.add("compiledata",_INTL("Compile Data"))
  commands.add("mapconnections",_INTL("Map Connections"))
  commands.add("animeditor",_INTL("Animation Editor"))
  commands.add("debugconsole",_INTL("Debug Console"))
  commands.add("togglelogging",_INTL("Toggle Battle Logging"))
  sprites["cmdwindow"]=Window_CommandPokemonEx.new(commands.list)
  cmdwindow=sprites["cmdwindow"]
  cmdwindow.viewport=viewport
  cmdwindow.resizeToFit(cmdwindow.commands)
  cmdwindow.height=Graphics.height if cmdwindow.height>Graphics.height
  cmdwindow.x=0
  cmdwindow.y=0
  cmdwindow.visible=true
  pbFadeInAndShow(sprites)
  ret=-1
  loop do
    loop do
      cmdwindow.update
      Graphics.update
      Input.update
      if Input.trigger?(Input::B)
        ret=-1
        break
      end
      if Input.trigger?(Input::C)
        ret=cmdwindow.index
        break
      end
    end
    break if ret==-1
    cmd=commands.getCommand(ret)
    if cmd=="switches"
      pbFadeOutIn(99999) { pbDebugScreen(0) }
    elsif cmd=="variables"
      pbFadeOutIn(99999) { pbDebugScreen(1) }
    elsif cmd=="refreshmap"
      $game_map.need_refresh = true
      Kernel.pbMessage(_INTL("The map will refresh."))
    elsif cmd=="warp"
      map=pbWarpToMap()
      if map
        pbFadeOutAndHide(sprites)
        pbDisposeSpriteHash(sprites)
        viewport.dispose
        if $scene.is_a?(Scene_Map)
          $game_temp.player_new_map_id=map[0]
          $game_temp.player_new_x=map[1]
          $game_temp.player_new_y=map[2]
          $game_temp.player_new_direction=2
          $scene.transfer_player
          $game_map.refresh
        else
          Kernel.pbCancelVehicles
          $MapFactory.setup(map[0])
          $game_player.moveto(map[1],map[2])
          $game_player.turn_down
          $game_map.update
          $game_map.autoplay
          $game_map.refresh
        end
        return
      end
    elsif cmd=="healparty"
      for i in $Trainer.party
        i.heal
      end
      Kernel.pbMessage(_INTL("Your Pokémon were healed."))
    elsif cmd=="additem"
      item=pbListScreen(_INTL("ADD ITEM"),ItemLister.new(0))
      if item && item>0
        params=ChooseNumberParams.new
        params.setRange(1,BAGMAXPERSLOT)
        params.setInitialValue(1)
        params.setCancelValue(0)
        qty=Kernel.pbMessageChooseNumber(
           _INTL("Choose the number of items."),params
        )
        if qty>0
          if qty==1
            Kernel.pbReceiveItem(item)
          else
            Kernel.pbMessage(_INTL("The item was added."))
            $PokemonBag.pbStoreItem(item,qty)
          end
        end
      end
    elsif cmd=="fillbag"
      params=ChooseNumberParams.new
      params.setRange(1,BAGMAXPERSLOT)
      params.setInitialValue(1)
      params.setCancelValue(0)
      qty=Kernel.pbMessageChooseNumber(
         _INTL("Choose the number of items."),params
      )
      if qty>0
        itemconsts=[]
        for i in PBItems.constants
          itemconsts.push(PBItems.const_get(i))
        end
        itemconsts.sort!{|a,b| a<=>b}
        for i in itemconsts
          $PokemonBag.pbStoreItem(i,qty)
        end
        Kernel.pbMessage(_INTL("The Bag was filled with {1} of each item.",qty))
      end
    elsif cmd=="clearbag"
      $PokemonBag.clear
      Kernel.pbMessage(_INTL("The Bag was cleared."))
    elsif cmd=="addpokemon"
      species=pbChooseSpeciesOrdered(1)
      if species!=0
        params=ChooseNumberParams.new
        params.setRange(1,PBExperience::MAXLEVEL)
        params.setInitialValue(5)
        params.setCancelValue(0)
        level=Kernel.pbMessageChooseNumber(
           _INTL("Set the Pokémon's level."),params)
        if level>0
          pbAddPokemon(species,level)
        end
      end
    elsif cmd=="fillboxes"
      $Trainer.formseen=[] if !$Trainer.formseen
      $Trainer.formlastseen=[] if !$Trainer.formlastseen
      added=0; completed=true
      for i in 1..PBSpecies.maxValue
        if added>=STORAGEBOXES*30
          completed=false; break
        end
        cname=getConstantName(PBSpecies,i) rescue nil
        next if !cname
        pkmn=PokeBattle_Pokemon.new(i,50,$Trainer)
        $PokemonStorage[(i-1)/$PokemonStorage.maxPokemon(0),
                        (i-1)%$PokemonStorage.maxPokemon(0)]=pkmn
        $Trainer.seen[i]=true
        $Trainer.owned[i]=true
        $Trainer.formlastseen[i]=[] if !$Trainer.formlastseen[i]
        $Trainer.formlastseen[i]=[0,0] if $Trainer.formlastseen[i]==[]
        $Trainer.formseen[i]=[[],[]] if !$Trainer.formseen[i]
        for j in 0..27
          $Trainer.formseen[i][0][j]=true
          $Trainer.formseen[i][1][j]=true
        end
        added+=1
      end
      Kernel.pbMessage(_INTL("Boxes were filled with one Pokémon of each species."))
      if !completed
        Kernel.pbMessage(_INTL("Note: The number of storage spaces ({1} boxes of 30) is less than the number of species.",STORAGEBOXES))
      end
    elsif cmd=="clearboxes"
      for i in 0...$PokemonStorage.maxBoxes
        for j in 0...$PokemonStorage.maxPokemon(i)
          $PokemonStorage[i,j]=nil
        end
      end
      Kernel.pbMessage(_INTL("The Boxes were cleared."))
    elsif cmd=="usepc"
      pbPokeCenterPC
    elsif cmd=="setplayer"
      limit=0
      for i in 0...8
        meta=pbGetMetadata(0,MetadataPlayerA+i)
        if !meta
          limit=i
          break
        end
      end
      if limit<=1
        Kernel.pbMessage(_INTL("There is only one player defined."))
      else
        params=ChooseNumberParams.new
        params.setRange(0,limit-1)
        params.setDefaultValue($PokemonGlobal.playerID)
        newid=Kernel.pbMessageChooseNumber(
           _INTL("Choose the new player character."),params)
        if newid!=$PokemonGlobal.playerID
          pbChangePlayer(newid)
          Kernel.pbMessage(_INTL("The player character was changed."))
        end
      end
    elsif cmd=="renameplayer"
      trname=pbEnterPlayerName("Your name?",0,7,$Trainer.name)
      if trname==""
        trainertype=pbGetPlayerTrainerType
        gender=pbGetTrainerTypeGender(trainertype) 
        trname=pbSuggestTrainerName(gender)
      end
      $Trainer.name=trname
      Kernel.pbMessage(_INTL("The player's name was changed to {1}.",$Trainer.name))
    elsif cmd=="randomid"
      $Trainer.id=rand(256)
      $Trainer.id|=rand(256)<<8
      $Trainer.id|=rand(256)<<16
      $Trainer.id|=rand(256)<<24
      Kernel.pbMessage(_INTL("The player's ID was changed to {1} (2).",$Trainer.publicID,$Trainer.id))
    elsif cmd=="changeoutfit"
      oldoutfit=$Trainer.outfit
      params=ChooseNumberParams.new
      params.setRange(0,99)
      params.setDefaultValue(oldoutfit)
      $Trainer.outfit=Kernel.pbMessageChooseNumber(_INTL("Set the player's outfit."),params)
      Kernel.pbMessage(_INTL("Player's outfit was changed.")) if $Trainer.outfit!=oldoutfit
    elsif cmd=="setmoney"
      params=ChooseNumberParams.new
      params.setMaxDigits(6)
      params.setDefaultValue($Trainer.money)
      $Trainer.money=Kernel.pbMessageChooseNumber(
         _INTL("Set the player's money."),params)
      Kernel.pbMessage(_INTL("You now have ${1}.",$Trainer.money))
    elsif cmd=="setcoins"
      params=ChooseNumberParams.new
      params.setRange(0,MAXCOINS)
      params.setDefaultValue($PokemonGlobal.coins)
      $PokemonGlobal.coins=Kernel.pbMessageChooseNumber(
         _INTL("Set the player's Coin amount."),params)
      Kernel.pbMessage(_INTL("You now have {1} Coins.",$PokemonGlobal.coins))
    elsif cmd=="setbadges"
      badgecmd=0
      loop do
        badgecmds=[]
        for i in 0...32
          badgecmds.push(_INTL("{1} Badge {2}",$Trainer.badges[i] ? "[Y]" : "[  ]",i+1))
        end
        badgecmd=Kernel.pbShowCommands(nil,badgecmds,-1,badgecmd)
        break if badgecmd<0
        $Trainer.badges[badgecmd]=!$Trainer.badges[badgecmd]
      end
    elsif cmd=="demoparty"
      pbCreatePokemon
      Kernel.pbMessage(_INTL("Filled party with demo Pokémon."))
    elsif cmd=="toggleshoes"
      $PokemonGlobal.runningShoes=!$PokemonGlobal.runningShoes
      Kernel.pbMessage(_INTL("Gave Running Shoes.")) if $PokemonGlobal.runningShoes
      Kernel.pbMessage(_INTL("Lost Running Shoes.")) if !$PokemonGlobal.runningShoes
    elsif cmd=="togglepokegear"
      $Trainer.pokegear=!$Trainer.pokegear
      Kernel.pbMessage(_INTL("Gave Pokégear.")) if $Trainer.pokegear
      Kernel.pbMessage(_INTL("Lost Pokégear.")) if !$Trainer.pokegear
    elsif cmd=="togglepokedex"
      $Trainer.pokedex=!$Trainer.pokedex
      Kernel.pbMessage(_INTL("Gave Pokédex.")) if $Trainer.pokedex
      Kernel.pbMessage(_INTL("Lost Pokédex.")) if !$Trainer.pokedex
    elsif cmd=="dexlists"
      dexescmd=0
      loop do
        dexescmds=[]
        d=pbDexNames
        for i in 0...d.length
          name=d[i]
          name=name[0] if name.is_a?(Array)
          dexindex=i
          unlocked=$PokemonGlobal.pokedexUnlocked[dexindex]
          dexescmds.push(_INTL("{1} {2}",unlocked ? "[Y]" : "[  ]",name))
        end
        dexescmd=Kernel.pbShowCommands(nil,dexescmds,-1,dexescmd)
        break if dexescmd<0
        dexindex=dexescmd
        if $PokemonGlobal.pokedexUnlocked[dexindex]
          pbLockDex(dexindex)
        else
          pbUnlockDex(dexindex)
        end
      end
    elsif cmd=="readyrematches"
      if !$PokemonGlobal.phoneNumbers || $PokemonGlobal.phoneNumbers.length==0
        Kernel.pbMessage(_INTL("There are no trainers in the Phone."))
      else
        for i in $PokemonGlobal.phoneNumbers
          if i.length==8 # A trainer with an event
            i[4]=2
            pbSetReadyToBattle(i)
          end
        end
        Kernel.pbMessage(_INTL("All trainers in the Phone are now ready to rebattle."))
      end
    elsif cmd=="mysterygift"
      pbManageMysteryGifts
    elsif cmd=="daycare"
      daycarecmd=0
      loop do
        daycarecmds=[
           _INTL("Summary"),
           _INTL("Deposit Pokémon"),
           _INTL("Withdraw Pokémon"),
           _INTL("Generate egg"),
           _INTL("Collect egg"),
           _INTL("Dispose egg")
        ]
        daycarecmd=Kernel.pbShowCommands(nil,daycarecmds,-1,daycarecmd)
        break if daycarecmd<0
        case daycarecmd
        when 0 # Summary
          if $PokemonGlobal.daycare
            num=pbDayCareDeposited
            Kernel.pbMessage(_INTL("{1} Pokémon are in the Day Care.",num))
            if num>0
              txt=""
              for i in 0...num
                next if !$PokemonGlobal.daycare[i][0]
                pkmn=$PokemonGlobal.daycare[i][0]
                initlevel=$PokemonGlobal.daycare[i][1]
                gender=[_INTL("♂"),_INTL("♀"),_INTL("genderless")][pkmn.gender]
                txt+=_INTL("{1}) {2} ({3}), Lv.{4} (deposited at Lv.{5})",
                   i,pkmn.name,gender,pkmn.level,initlevel)
                txt+="\n" if i<num-1
              end
              Kernel.pbMessage(txt)
            end
            if $PokemonGlobal.daycareEgg==1
              Kernel.pbMessage(_INTL("An egg is waiting to be picked up."))
            elsif pbDayCareDeposited==2
              if pbDayCareGetCompat==0
                Kernel.pbMessage(_INTL("The deposited Pokémon can't breed."))
              else
                Kernel.pbMessage(_INTL("The deposited Pokémon can breed."))
              end
            end
          end
        when 1 # Deposit Pokémon
          if pbEggGenerated?
            Kernel.pbMessage(_INTL("Egg is available, can't deposit Pokémon."))
          elsif pbDayCareDeposited==2
            Kernel.pbMessage(_INTL("Two Pokémon are deposited already."))
          elsif $Trainer.party.length==0
            Kernel.pbMessage(_INTL("Party is empty, can't desposit Pokémon."))
          else
            pbChooseNonEggPokemon(1,3)
            if pbGet(1)>=0
              pbDayCareDeposit(pbGet(1))
              Kernel.pbMessage(_INTL("Deposited {1}.",pbGet(3)))
            end
          end
        when 2 # Withdraw Pokémon
          if pbEggGenerated?
            Kernel.pbMessage(_INTL("Egg is available, can't withdraw Pokémon."))
          elsif pbDayCareDeposited==0
            Kernel.pbMessage(_INTL("No Pokémon are in the Day Care."))
          elsif $Trainer.party.length>=6
            Kernel.pbMessage(_INTL("Party is full, can't withdraw Pokémon."))
          else
            pbDayCareChoose(_INTL("Which one do you want back?"),1)
            if pbGet(1)>=0
              pbDayCareGetDeposited(pbGet(1),3,4)
              pbDayCareWithdraw(pbGet(1))
              Kernel.pbMessage(_INTL("Withdrew {1}.",pbGet(3)))
            end
          end
        when 3 # Generate egg
          if $PokemonGlobal.daycareEgg==1
            Kernel.pbMessage(_INTL("An egg is already waiting."))
          elsif pbDayCareDeposited!=2
            Kernel.pbMessage(_INTL("There aren't 2 Pokémon in the Day Care."))
          elsif pbDayCareGetCompat==0
            Kernel.pbMessage(_INTL("The Pokémon in the Day Care can't breed."))
          else
            $PokemonGlobal.daycareEgg=1
            Kernel.pbMessage(_INTL("An egg is now waiting in the Day Care."))
          end
        when 4 # Collect egg
          if $PokemonGlobal.daycareEgg!=1
            Kernel.pbMessage(_INTL("There is no egg available."))
          elsif $Trainer.party.length>=6
            Kernel.pbMessage(_INTL("Party is full, can't collect the egg."))
          else
            pbDayCareGenerateEgg
            $PokemonGlobal.daycareEgg=0
            $PokemonGlobal.daycareEggSteps=0
            Kernel.pbMessage(_INTL("Collected the {1} egg.",
               PBSpecies.getName($Trainer.party[$Trainer.party.length-1].species)))
          end
        when 5 # Dispose egg
          if $PokemonGlobal.daycareEgg!=1
            Kernel.pbMessage(_INTL("There is no egg available."))
          else
            $PokemonGlobal.daycareEgg=0
            $PokemonGlobal.daycareEggSteps=0
            Kernel.pbMessage(_INTL("Disposed of the egg."))
          end
        end
      end
    elsif cmd=="quickhatch"
      for pokemon in $Trainer.party
        pokemon.eggsteps=1 if pokemon.isEgg?
      end
      Kernel.pbMessage(_INTL("All eggs on your party now require one step to hatch."))
    elsif cmd=="roamerstatus"
      if RoamingSpecies.length==0
        Kernel.pbMessage(_INTL("No roaming Pokémon defined."))
      else
        text="\\l[8]"
        for i in 0...RoamingSpecies.length
          poke=RoamingSpecies[i]
          if $game_switches[poke[2]]
            status=$PokemonGlobal.roamPokemon[i]
            if status==true
              if $PokemonGlobal.roamPokemonCaught[i]
                text+=_INTL("{1} (Lv.{2}) caught.",
                   PBSpecies.getName(getID(PBSpecies,poke[0])),poke[1])
              else
                text+=_INTL("{1} (Lv.{2}) defeated.",
                   PBSpecies.getName(getID(PBSpecies,poke[0])),poke[1])
              end
            else
              curmap=$PokemonGlobal.roamPosition[i]
              if curmap
                mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata")
                text+=_INTL("{1} (Lv.{2}) roaming on map {3} ({4}){5}",
                   PBSpecies.getName(getID(PBSpecies,poke[0])),poke[1],curmap,
                   mapinfos[curmap].name,(curmap==$game_map.map_id) ? _INTL("(this map)") : "")
              else
                text+=_INTL("{1} (Lv.{2}) roaming (map not set).",
                   PBSpecies.getName(getID(PBSpecies,poke[0])),poke[1])
              end
            end
          else
            text+=_INTL("{1} (Lv.{2}) not roaming (switch {3} is off).",
               PBSpecies.getName(getID(PBSpecies,poke[0])),poke[1],poke[2])
          end
          text+="\n" if i<RoamingSpecies.length-1
        end
        Kernel.pbMessage(text)
      end
    elsif cmd=="roam"
      if RoamingSpecies.length==0
        Kernel.pbMessage(_INTL("No roaming Pokémon defined."))
      else
        pbRoamPokemon(true)
        $PokemonGlobal.roamedAlready=false
        Kernel.pbMessage(_INTL("Pokémon have roamed."))
      end
    elsif cmd=="setencounters"
      encdata=load_data("Data/encounters.dat")
      oldencdata=Marshal.dump(encdata)
      mapedited=false
      map=pbDefaultMap()
      loop do
        map=pbListScreen(_INTL("SET ENCOUNTERS"),MapLister.new(map))
        break if map<=0
        mapedited=true if map==pbDefaultMap()
        pbEncounterEditorMap(encdata,map)
      end
      save_data(encdata,"Data/encounters.dat")
      pbSaveEncounterData()
      pbClearData()
    elsif cmd=="setmetadata"
      pbMetadataScreen(pbDefaultMap())
      pbClearData()
    elsif cmd=="terraintags"
      pbFadeOutIn(99999) { pbTilesetScreen }
    elsif cmd=="trainertypes"
      pbFadeOutIn(99999) { pbTrainerTypeEditor }
    elsif cmd=="resettrainers"
      if $game_map
        for event in $game_map.events.values
          if event.name[/Trainer\(\d+\)/]
            $game_self_switches[[$game_map.map_id,event.id,"A"]]=false
            $game_self_switches[[$game_map.map_id,event.id,"B"]]=false
          end
        end
        $game_map.need_refresh=true
        Kernel.pbMessage(_INTL("All Trainers on this map were reset."))
      else
        Kernel.pbMessage(_INTL("This command can't be used here."))
      end
    elsif cmd=="testwildbattle"
      species=pbChooseSpeciesOrdered(1)
      if species!=0
        params=ChooseNumberParams.new
        params.setRange(1,PBExperience::MAXLEVEL)
        params.setInitialValue(5)
        params.setCancelValue(0)
        level=Kernel.pbMessageChooseNumber(
           _INTL("Set the Pokémon's level."),params)
        if level>0
          pbWildBattle(species,level)
        end
      end
    elsif cmd=="testdoublewildbattle"
      Kernel.pbMessage(_INTL("Choose the first Pokémon."))
      species1=pbChooseSpeciesOrdered(1)
      if species1!=0
        params=ChooseNumberParams.new
        params.setRange(1,PBExperience::MAXLEVEL)
        params.setInitialValue(5)
        params.setCancelValue(0)
        level1=Kernel.pbMessageChooseNumber(
           _INTL("Set the first Pokémon's level."),params)
        if level1>0
          Kernel.pbMessage(_INTL("Choose the second Pokémon."))
          species2=pbChooseSpeciesOrdered(1)
          if species2!=0
            params=ChooseNumberParams.new
            params.setRange(1,PBExperience::MAXLEVEL)
            params.setInitialValue(5)
            params.setCancelValue(0)
            level2=Kernel.pbMessageChooseNumber(
               _INTL("Set the second Pokémon's level."),params)
            if level2>0
              pbDoubleWildBattle(species1,level1,species2,level2)
            end
          end
        end
      end
    elsif cmd=="testtrainerbattle"
      battle=pbListScreen(_INTL("SINGLE TRAINER"),TrainerBattleLister.new(0,false))
      if battle
        trainerdata=battle[1]
        pbTrainerBattle(trainerdata[0],trainerdata[1],"...",false,trainerdata[4],true)
      end
    elsif cmd=="testdoubletrainerbattle"
      battle1=pbListScreen(_INTL("DOUBLE TRAINER 1"),TrainerBattleLister.new(0,false))
      if battle1
        battle2=pbListScreen(_INTL("DOUBLE TRAINER 2"),TrainerBattleLister.new(0,false))
        if battle2
          trainerdata1=battle1[1]
          trainerdata2=battle2[1]
          pbDoubleTrainerBattle(trainerdata1[0],trainerdata1[1],trainerdata1[4],"...",
                                trainerdata2[0],trainerdata2[1],trainerdata2[4],"...",
                                true)
        end
      end
    elsif cmd=="relicstone"
      pbRelicStone()
    elsif cmd=="purifychamber"
      pbPurifyChamber()
    elsif cmd=="extracttext"
      pbExtractText
    elsif cmd=="compiletext"
      pbCompileTextUI
    elsif cmd=="compiledata"
      msgwindow=Kernel.pbCreateMessageWindow
      pbCompileAllData(true) {|msg| Kernel.pbMessageDisplay(msgwindow,msg,false) }
      Kernel.pbMessageDisplay(msgwindow,_INTL("All game data was compiled."))
      Kernel.pbDisposeMessageWindow(msgwindow)
    elsif cmd=="mapconnections"
      pbFadeOutIn(99999) { pbEditorScreen }
    elsif cmd=="animeditor"
      pbFadeOutIn(99999) { pbAnimationEditor }
    elsif cmd=="debugconsole"
      Console::setup_console
    elsif cmd=="togglelogging"
      $INTERNAL=!$INTERNAL
      Kernel.pbMessage(_INTL("Debug logs for battles will be made in the Data folder.")) if $INTERNAL
      Kernel.pbMessage(_INTL("Debug logs for battles will not be made.")) if !$INTERNAL
    end
  end
  pbFadeOutAndHide(sprites)
  pbDisposeSpriteHash(sprites)
  viewport.dispose
end*/
	}
}
