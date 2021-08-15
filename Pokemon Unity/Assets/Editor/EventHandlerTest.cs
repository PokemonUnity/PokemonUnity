using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.IO;

/// <summary>
/// </summary>
/// Use the Assert class to test conditions.
/// A UnityTest behaves like a coroutine in PlayMode
/// and allows you to yield null to skip a frame in EditMode
public class EventHandlerTest {

	[Test]
	public void NewEditModeTestSimplePasses() {
		
	}

	[UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// yield to skip a frame
		yield return null;
	}

	#region CanvasUI Event Handler
	[Test]
	public void DisableLayersWhenSwitchingUI() {
		
	}
	[Test]
	public void AllCanvasUIDisablesPlayerActionWhenActive() {
		
	}
	#endregion

	#region StartUp Event Handler
	[Test]
	public void If_No_SaveData_IsFound_Remove_ContinueAndPlayerProfile_From_StartUp() {
		
	}
	[Test]
	public void DisabledConitueSlotsToMatchNumberOfSavedProfiles() {
		
	}
	[Test]
	public void ContinueDataMatchesSavedPlayerProfiles() {
		
	}
	[Test]
	public void DisablePokemonPartyFromSaveDataToMatchPlayerProfileCount() {
		
	}
	#endregion

	#region Dialog Event Handler
	[Test]
	public void AllDialogDisablesEveryUIandPlayerActionsWhenActive() {
		
	}
	#endregion

	#region Item Event Handler
	#endregion

	#region Battle Scene Test
	[UnityTest]
	public IEnumerator TestBattlePokemonHUD() {
		//Not sure how to write a Unity Unit Test to display real-time feedback...
		yield return null;
	}
	#endregion

	#region Asset (3d Model) Test
	[UnityTest]
	public IEnumerator TestAssetModelExist() {
		int pokemonNumber = 1;
		System.Collections.Generic.Dictionary<int, string> dir = new System.Collections.Generic.Dictionary<int, string>();
		int dirID;
		dirID = pokemonNumber - 1;

		//[Header("Spawn")]
		Transform SpawnPoint;

		AnimatorOverrideController animatorOverrideController;

		var dirinfo = new DirectoryInfo(dir[dirID]);
		string dirname = "Assets/Pokemons/" + dirinfo.Name + "/";
		FileInfo[] fileInfo = dirinfo.GetFiles();

		GameObject Pokemon;
		GameObject PokeModel = GameObject.FindWithTag("Pokemon");

		int animID = 0;

		//if (Input.GetButtonDown("Start"))
		//{
			for (int i = 0; i < fileInfo.Length; i++) //foreach (FileInfo f in fileInfo)
			{
				FileInfo f = fileInfo[i];
				if (f.Name.Contains(".fbx") && !f.Name.Contains(".meta"))
				{
					//string dirname = "Assets/Pokemons/" + dirinfo.Name + "/";
					Object pok = (Object)AssetDatabase.LoadAssetAtPath(dirname + (f.Name), typeof(Object));
					//Pokemon = (GameObject)Instantiate(pok, SpawnPoint.position, SpawnPoint.rotation);
					//Pokemon.tag = "Pokemon";
				}
			}
		//}

		//if (Input.GetButtonDown("Next"))
		//{
			Animator anim = PokeModel.GetComponent<Animator>();
			AnimationClip[] animations = AnimationUtility.GetAnimationClips(PokeModel); //The problem is here

			animID += 1;

			anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/pkm.controller", typeof(RuntimeAnimatorController));
			animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
			anim.runtimeAnimatorController = animatorOverrideController;

			animatorOverrideController["Anim"] = animations[animID];


			if (animID >= animations.Length)
			{

				animID = 0;
				//DestroyImmediate(PokeModel);
				pokemonNumber += 1;
				dirID = pokemonNumber - 1;

				//foreach (FileInfo f in fileInfo)
				//{
					//if (f.Name.Contains(".fbx") && !f.Name.Contains(".meta"))
					//{
						//ing dirname = "Assets/Pokemons/" + dirinfo.Name + "/";
					//	Pokemon = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(dirname + f.Name, typeof(GameObject)), SpawnPoint.position, SpawnPoint.rotation);
					//	Pokemon.AddComponent<Animator>();
					//	Pokemon.tag = "Pokemon";
					//}
				//}
			}
		//}
		yield return null;
	}
	#endregion
}
