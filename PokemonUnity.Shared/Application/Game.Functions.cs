using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	/*List of Commands in Alphabet Order
FreezeMessageBox
Geonet
GiveCoins
GiveEgg
GiveMoney
GivePokemon
GivePoketch
GiveRunningShoes
GreatMarshBynocule
HallFameData
HealPokemon
HealPokemonAnimation
HiddenMachineEffect
HideBattlePointsBox
HideCoins
HideMoney
HidePicture
HideSaveBox
HideSavingClock
HoneyTreeBattle
If
If2
Interview
Jump
LeagueCastleView
Lock
LockAll
LockCam
LostGoPokecenter
Mailbox
Menu
Message
Message2
Message3
MoveInfo
Multi
Multi2
Multi3
MultiRow
NoMapMessageBox
Nop
Nop1
OpenBerryPouch
OpenDoor
OpenPcAnimation
PhraseBox1W
PhraseBox2W
PlayCry
PlayFanfare
PlayFanfare2
PlayMusic
PlaySound
Pokecasino
Pokemart
Pokemart1
Pokemart2
Pokemart3
PokemonContest
PokemonInfo
PokemonPartyPicture
PokemonPicture
PortalEffect
PrepareDoorAnimation
PreparePcAnimation
RandomBattle
RecordList
RecordMixingUnion
Release
ReleaseAll
ReleaseOverworld
RememberMove
RemovePeople
ResetScreen
RestartMusic
RetSprtSave
Return
Return2
RideBike
RockClimbAnimation
SetDoorLocked
SetDoorPassable
SetFlag
SetOverworldMovement
SetOverworldPosition
SetPokemonPartyStored
SetPositionAfterShip
SetValue
SetVar
SetVarAlterStored
SetVarHeroStored
SetVariableAlter
SetVariableAttack
SetVariableAttackItem
SetVariableHero
SetVariableItem
SetVariableNickname
SetVariableNumber
SetVariableObject
SetVariablePokemon
SetVariablePokemonHeight
SetVariableRival
SetVariableTrainer
SetVarPokemonStored
SetVarRivalStored
ShipAnimation
ShowBattlePointsBox
ShowCoins
ShowLinkCountRecord
ShowMoney
ShowNationalSheet
ShowSaveBox
ShowSavingClock
ShowSinnohSheet
SinnohMaps
SpinTradeUnion
SprtSave
StarterBattle
StopBerryHiroAnimation
StopFollowHero
StopMusic
StopTrade
StoreMove
StorePokemonMenu2
StorePokemonNumber
StorePokemonParty
StorePoketchApp
StoreStarter
SurfAnimation
SwitchMusic
SwitchMusic2
TakeCoins
TakeItem
TakeMoney
TeachMove
TextMessageScriptMulti
TextScriptMulti
ThankNameInsert
TradeChosenPokemon
TradeUnion
TrainerBattle
TrainerCaseUnion
Tuxedo
TypeMessageBox
UnionRoom
UnownMessageBox
UpdateCoins
UpdateMoney
WaitAction
WaitButton
WaitClose
WaitCry
WaitFanfare
WaitFor
WaitMovement
Warp
WarpLastElevator
WarpMapElevator
WaterfallAnimation
WFC
WFC1
WildBattle
WildBattle2
WirelessBattleWait
WriteAutograph
YesNoBox
*/
	public partial class Game
	{
		// ToDo: Based on enum value, run method below
		public bool RunCommand(ScriptCommands command, params object[] input)
		{
			switch (command)
			{
				case ScriptCommands.Faceplayer:
					break;
				case ScriptCommands.Message:
					break;
				case ScriptCommands.CloseMsgOnKeyPress:
					break;
				case ScriptCommands.Nop:
					break;
				case ScriptCommands.Nop1:
					break;
				case ScriptCommands.End:
					break;
				case ScriptCommands.Return2:
					break;
				case ScriptCommands.SetVar:
					break;
				case ScriptCommands.CopyVar:
					break;
				case ScriptCommands.Message2:
					break;
				case ScriptCommands.WaitButton:
					break;
				case ScriptCommands.ColorMessageBox:
					break;
				case ScriptCommands.TypeMessageBox:
					break;
				case ScriptCommands.NoMapMessageBox:
					break;
				case ScriptCommands.CallMessageBoxText:
					break;
				case ScriptCommands.Menu:
					break;
				case ScriptCommands.YesNoBox:
					break;
				case ScriptCommands.WaitFor:
					break;
				case ScriptCommands.TextScriptMulti:
					break;
				case ScriptCommands.TextMessageScriptMulti:
					break;
				case ScriptCommands.PlayFanfare:
					break;
				case ScriptCommands.PlayFanfare2:
					break;
				case ScriptCommands.WaitCry:
					break;
				case ScriptCommands.PlaySound:
					break;
				case ScriptCommands.PlayMusic:
					break;
				case ScriptCommands.RestartMusic:
					break;
				case ScriptCommands.ApplyMovement:
					break;
				case ScriptCommands.WaitMovement:
					break;
				case ScriptCommands.LockAll:
					break;
				case ScriptCommands.ReleaseAll:
					break;
				case ScriptCommands.Lock:
					break;
				case ScriptCommands.Release:
					break;
				case ScriptCommands.AddPeople:
					break;
				case ScriptCommands.LockCam:
					break;
				case ScriptCommands.CheckSpritePosition:
					break;
				case ScriptCommands.CheckPersonPosition:
					break;
				case ScriptCommands.ContinueFollow:
					break;
				case ScriptCommands.FollowHero:
					break;
				case ScriptCommands.StopFollowHero:
					break;
				case ScriptCommands.GiveMoney:
					break;
				case ScriptCommands.TakeMoney:
					break;
				case ScriptCommands.ShowMoney:
					break;
				case ScriptCommands.UpdateMoney:
					break;
				case ScriptCommands.ShowCoins:
					break;
				case ScriptCommands.HideCoins:
					break;
				case ScriptCommands.UpdateCoins:
					break;
				case ScriptCommands.CheckCoins:
					break;
				case ScriptCommands.GiveCoins:
					break;
				case ScriptCommands.TakeCoins:
					break;
				case ScriptCommands.TakeItem:
					break;
				case ScriptCommands.CheckStoreItem:
					break;
				case ScriptCommands.CheckItem:
					break;
				case ScriptCommands.CheckUndergroundPcStatus:
					break;
				case ScriptCommands.StorePokemonParty:
					break;
				case ScriptCommands.CallEnd:
					break;
				case ScriptCommands.DisplayDressedPokemon:
					break;
				case ScriptCommands.DisplayContestPokemon:
					break;
				case ScriptCommands.CapsuleEditor:
					break;
				case ScriptCommands.SinnohMaps:
					break;
				case ScriptCommands.BoxPokemon:
					break;
				case ScriptCommands.DrawUnion:
					break;
				case ScriptCommands.TrainerCaseUnion:
					break;
				case ScriptCommands.TradeUnion:
					break;
				case ScriptCommands.RecordMixingUnion:
					break;
				case ScriptCommands.HallFameData:
					break;
				case ScriptCommands.WFC1:
					break;
				case ScriptCommands.ChoosePlayerName:
					break;
				case ScriptCommands.ChoosePokemonName:
					break;
				case ScriptCommands.FadeScreen:
					break;
				case ScriptCommands.ResetScreen:
					break;
				case ScriptCommands.Warp:
					break;
				case ScriptCommands.RockClimbAnimation:
					break;
				case ScriptCommands.SurfAnimation:
					break;
				case ScriptCommands.WaterfallAnimation:
					break;
				case ScriptCommands.FlyAnimation:
					break;
				case ScriptCommands.Tuxedo:
					break;
				case ScriptCommands.CheckBike:
					break;
				case ScriptCommands.RideBike:
					break;
				case ScriptCommands.BerryHiroAnimation:
					break;
				case ScriptCommands.StopBerryHiroAnimation:
					break;
				case ScriptCommands.SetVariableHero:
					break;
				case ScriptCommands.SetVariableRival:
					break;
				case ScriptCommands.SetVariableAlter:
					break;
				case ScriptCommands.SetVariablePokemon:
					break;
				case ScriptCommands.SetVariableItem:
					break;
				case ScriptCommands.SetVarHeroStored:
					break;
				case ScriptCommands.ShowLinkCountRecord:
					break;
				case ScriptCommands.WarpMapElevator:
					break;
				case ScriptCommands.CheckFloor:
					break;
				case ScriptCommands.SetPositionAfterShip:
					break;
				case ScriptCommands.WildBattle:
					break;
				case ScriptCommands.ExplanationBattle:
					break;
				case ScriptCommands.HoneyTreeBattle:
					break;
				case ScriptCommands.ExpectDecisionOther:
					break;
				case ScriptCommands.Pokemart1:
					break;
				case ScriptCommands.Pokemart3:
					break;
				case ScriptCommands.DefeatGoPokecenter:
					break;
				case ScriptCommands.CheckGender:
					break;
				case ScriptCommands.HealPokemon:
					break;
				case ScriptCommands.UnionRoom:
					break;
				case ScriptCommands.ActivatePokedex:
					break;
				case ScriptCommands.GiveRunningShoes:
					break;
				case ScriptCommands.CheckBadge:
					break;
				case ScriptCommands.EnableBadge:
					break;
				case ScriptCommands.DisableBadge:
					break;
				case ScriptCommands.PrepareDoorAnimation:
					break;
				case ScriptCommands.WaitAction:
					break;
				case ScriptCommands.WaitClose:
					break;
				case ScriptCommands.CloseDoor:
					break;
				case ScriptCommands.CheckPartyNumber:
					break;
				case ScriptCommands.SetDoorLocked:
					break;
				case ScriptCommands.ShowSavingClock:
					break;
				case ScriptCommands.HideSavingClock:
					break;
				case ScriptCommands.ChoosePokemonMenu:
					break;
				case ScriptCommands.ChoosePokemonMenu2:
					break;
				case ScriptCommands.StorePokemonMenu2:
					break;
				case ScriptCommands.PokemonInfo:
					break;
				case ScriptCommands.StorePokemonNumber:
					break;
				case ScriptCommands.CheckPartyNumber2:
					break;
				case ScriptCommands.CheckHappiness:
					break;
				case ScriptCommands.CheckPosition:
					break;
				case ScriptCommands.CheckPokemonParty2:
					break;
				case ScriptCommands.ComparePokemonHeight:
					break;
				case ScriptCommands.CheckPokemonHeight:
					break;
				case ScriptCommands.MoveInfo:
					break;
				case ScriptCommands.StoreMove:
					break;
				case ScriptCommands.DeleteMove:
					break;
				case ScriptCommands.BerryPoffin:
					break;
				case ScriptCommands.BattleRoomResult:
					break;
				case ScriptCommands.CheckNationalPokedex:
					break;
				case ScriptCommands.ShowSinnohSheet:
					break;
				case ScriptCommands.ShowNationalSheet:
					break;
				case ScriptCommands.CheckFossil:
					break;
				case ScriptCommands.CheckPokemonLevel:
					break;
				case ScriptCommands.WarpLastElevator:
					break;
				case ScriptCommands.GreatMarshBynocule:
					break;
				case ScriptCommands.PokemonPicture:
					break;
				case ScriptCommands.HidePicture:
					break;
				case ScriptCommands.RememberMove:
					break;
				case ScriptCommands.TeachMove:
					break;
				case ScriptCommands.ShipAnimation:
					break;
				case ScriptCommands.PhraseBox1W:
					break;
				case ScriptCommands.OpenPcAnimation:
					break;
				case ScriptCommands.ClosePcAnimation:
					break;
				case ScriptCommands.CheckLottoNumber:
					break;
				case ScriptCommands.CheckAccessories:
					break;
				case ScriptCommands.Pokecasino:
					break;
				case ScriptCommands.PokemonPartyPicture:
					break;
				default:
					break;
			}
			return false;
		}
		#region List of Pokemon Framework Game Functions
/// <summary>
/// Makes the overworld npc using the script face towards the player.
/// </summary>
public void Faceplayer () { }
/// <summary>
/// Displays a message with index {#}. The index matches how it appears in the Text tab, i.e.Message 0 refers to text_0.
/// </summary>
public void Message () { }
/// <summary>
/// This closes the message box when the player presses a button. In this case the same button will take the message off and close the box.
/// </summary>
public void CloseMsgOnKeyPress () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void Nop () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void Nop1 () { }
///<summary>
///Number of Parameter Inputs: 0
/// Signifies the end of a script.Doesn't strictly do anything but essential for the game to parse the scripts correctly.
///</summary>
public void End () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void Return2 () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void SetVar () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CopyVar () { }
///<summary
///>Number of Parameter Inputs: 1
/// Displays a message with index {#}. The index matches how it appears in the Text tab, i.e.Message 0 refers to text_0.
/// </summary>
public void Message2 () { }
///<summary>
/// Number of Parameter Inputs: 0
/// This holds the message in place on the screen after it finishes until the player presses a button.
/// </summary>
public void WaitButton () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void ColorMessageBox () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void TypeMessageBox () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void NoMapMessageBox () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void CallMessageBoxText () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void Menu () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void YesNoBox () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void WaitFor () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void TextScriptMulti () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void TextMessageScriptMulti () { }
///<summary>
/// Number of Parameter Inputs: 1
/// The little sound that plays when you speak to an NPC.
/// </summary>
public void PlayFanfare () { }
///<summary>
/// Number of Parameter Inputs: 1
/// The little sound that plays when you speak to an NPC.
/// </summary>
public void PlayFanfare2 () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void WaitCry () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void PlaySound () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void PlayMusic () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void RestartMusic () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void ApplyMovement () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void WaitMovement () { }
///<summary>
/// Number of Parameter Inputs: 0
/// Locks all overworld npcs in place in the room. A standard when a script is active.
/// </summary>
public void LockAll () { }
///<summary>
///Number of Parameter Inputs: 0
/// Allows all overworld npcs to walk again.
/// </summary>
public void ReleaseAll () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void Lock () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void Release () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void AddPeople () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void LockCam () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckSpritePosition () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void CheckPersonPosition () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void ContinueFollow () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void FollowHero () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void StopFollowHero () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void GiveMoney () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void TakeMoney () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void ShowMoney () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void UpdateMoney () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void ShowCoins () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void HideCoins () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void UpdateCoins () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CheckCoins () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void GiveCoins () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void TakeCoins () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void TakeItem () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void CheckStoreItem () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void CheckItem () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckUndergroundPcStatus () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void StorePokemonParty () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void CallEnd () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void DisplayDressedPokemon () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void DisplayContestPokemon () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CapsuleEditor () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void SinnohMaps () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void BoxPokemon () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void DrawUnion () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void TrainerCaseUnion () { }
///<summary>Number of Parameter Inputs: 4</summary>
public void TradeUnion () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void RecordMixingUnion () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void HallFameData () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void WFC1 () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void ChoosePlayerName () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void ChoosePokemonName () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void FadeScreen () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void ResetScreen () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void Warp () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void RockClimbAnimation () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void SurfAnimation () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void WaterfallAnimation () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void FlyAnimation () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void Tuxedo () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckBike () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void RideBike () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void BerryHiroAnimation () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void StopBerryHiroAnimation () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void SetVariableHero () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void SetVariableRival () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void SetVariableAlter () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void SetVariablePokemon () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void SetVariableItem () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void SetVarHeroStored () { }
///<summary>Number of Parameter Inputs: 4</summary>
public void ShowLinkCountRecord () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void WarpMapElevator () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void CheckFloor () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void SetPositionAfterShip () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void WildBattle () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void ExplanationBattle () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void HoneyTreeBattle () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void ExpectDecisionOther () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void Pokemart1 () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void Pokemart3 () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void DefeatGoPokecenter () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CheckGender () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void HealPokemon () { }
///<summary>Number of Parameter Inputs: 5</summary>
public void UnionRoom () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void ActivatePokedex () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void GiveRunningShoes () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckBadge () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void EnableBadge () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void DisableBadge () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void PrepareDoorAnimation () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void WaitAction () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void WaitClose () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CloseDoor () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CheckPartyNumber () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void SetDoorLocked () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void ShowSavingClock () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void HideSavingClock () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void ChoosePokemonMenu () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void ChoosePokemonMenu2 () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void StorePokemonMenu2 () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void PokemonInfo () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void StorePokemonNumber () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckPartyNumber2 () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckHappiness () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CheckPosition () { }
///<summary>Number of Parameter Inputs: 5</summary>
public void CheckPokemonParty2 () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void ComparePokemonHeight () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckPokemonHeight () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void MoveInfo () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void StoreMove () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void DeleteMove () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void BerryPoffin () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void BattleRoomResult () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CheckNationalPokedex () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void ShowSinnohSheet () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void ShowNationalSheet () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void CheckFossil () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void CheckPokemonLevel () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void WarpLastElevator () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void GreatMarshBynocule () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void PokemonPicture () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void HidePicture () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void RememberMove () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void TeachMove () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void ShipAnimation () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void PhraseBox1W () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void OpenPcAnimation () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void ClosePcAnimation () { }
///<summary>Number of Parameter Inputs: 1</summary>
public void CheckLottoNumber () { }
///<summary>Number of Parameter Inputs: 0</summary>
public void CheckAccessories () { }
///<summary>Number of Parameter Inputs: 2</summary>
public void Pokecasino () { }
///<summary>Number of Parameter Inputs: 3</summary>
public void PokemonPartyPicture () { }
	#endregion
	}
}