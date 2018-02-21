﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class PokedexHandler : MonoBehaviour {

	// Usage Variables
	public Vector2 cursorPosition;
	public float boxNum;
	private float pokemonNum;
	private Texture[] pokeSprites;
	private bool screen2;

	// GameObject like vars
	private GUITexture cursor;
	private GUITexture preview;
	private GUIText name;
	private GUIText nameShadow;
	private GUIText id;
	private GUIText idShadow;
	private GUIText boxLabel;
	private GUIText boxLabelShadow;
	public GUITexture[] pokePreview;
	public GUITexture background;
	private GUITexture type1;
	private GUITexture type2;
	private GUIText ability1;
	private GUIText ability2;
	private GUIText ability1shadow;
	private GUIText ability2shadow;

	[SerializeField]
	private float moveSpeed = 0.16f;

	// Ressource vars
	public Texture[] types;
	public AudioClip selectClip;


	void Awake () {
		cursor = GameObject.Find ("Cursor").GetComponent<GUITexture>();
		preview = GameObject.Find ("SelectedSprite").GetComponent<GUITexture> ();
		name = GameObject.Find ("SelectedName").GetComponent<GUIText>();
		nameShadow = GameObject.Find ("SelectedNameShadow").GetComponent<GUIText>();
		id = GameObject.Find ("SelectedID").GetComponent<GUIText>();
		idShadow = GameObject.Find ("SelectedIDShadow").GetComponent<GUIText>();
		boxLabel = GameObject.Find ("BoxHeader").GetComponent<GUIText>();
		boxLabelShadow = GameObject.Find ("BoxHeaderShadow").GetComponent<GUIText>();
		pokeSprites = new Texture[45];
		for (int i = 0; i < 45; i++) {
			pokePreview[i] = GameObject.Find ("Pokemon" + i).GetComponent<GUITexture>();
		}
		//background = GameObject.Find ("PokedexBackground").GetComponent<GUITexture> ();

		type1 = GameObject.Find ("SelectedType1").GetComponent<GUITexture>();
		type2 = GameObject.Find ("SelectedType2").GetComponent<GUITexture>();
		ability1 = GameObject.Find ("SelectedAbility1").GetComponent<GUIText> ();
		ability2 = GameObject.Find ("SelectedAbility2").GetComponent<GUIText> ();
		ability1shadow = GameObject.Find ("SelectedAbility1Shadow").GetComponent<GUIText> ();
		ability2shadow = GameObject.Find ("SelectedAbility2Shadow").GetComponent<GUIText> ();

	}



	public IEnumerator control(){
		cursorPosition = Vector2.zero;
		boxNum = 0;
		updateBox ((int) boxNum);
		StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
		bool running = true;
		while (running) {
			// Input


			if (cursorPosition.y == -1 && screen2 != true) {
				if (Input.GetAxis ("Horizontal") > 0 && boxNum != 18) {
					
					boxNum += 1;
					updateBox ((int)boxNum);
					SfxHandler.Play (selectClip);
					yield return new WaitForSeconds (0.2f);
				} else if (Input.GetAxis ("Horizontal") < 0 && boxNum != 0) {
					
					boxNum -= 1;
					updateBox ((int)boxNum);
					SfxHandler.Play (selectClip);
					yield return new WaitForSeconds (0.2f);
				}
			} else {
				if (Input.GetAxis ("Horizontal") > 0 && cursorPosition.x != 8 && cursorPosition.y != -1 && cursorPosition.y != 6  && screen2 != true) {
					cursorPosition += new Vector2 (1, 0);
					SfxHandler.Play (selectClip);
					yield return new WaitForSeconds (0.2f);
				} else if (Input.GetAxis ("Horizontal") < 0 && cursorPosition.x != 0 && cursorPosition.y != -1 && cursorPosition.y != 6 && screen2 != true) {
					cursorPosition -= new Vector2 (1, 0);
					SfxHandler.Play (selectClip);
					yield return new WaitForSeconds (0.2f);
				} 

				if (Input.GetButton ("Select")) {
					yield return StartCoroutine (informationScreen ());
				}
			}

			if (Input.GetAxis ("Vertical") > 0 && cursorPosition.y != -1 && screen2 != true) {
				cursorPosition -= new Vector2 (0, 1);
				SfxHandler.Play (selectClip);
				yield return new WaitForSeconds (0.2f);
			} else if (Input.GetAxis ("Vertical") < 0 && cursorPosition.y != 5 && screen2 != true) {
				cursorPosition += new Vector2 (0, 1);
				SfxHandler.Play (selectClip);
				yield return new WaitForSeconds (0.2f);
			}

			// Cursor Position
			if (screen2 != true){
				if (cursorPosition.y == -1) {
					//cursor.transform.position = new Vector3(200, 307, 13);
				} else if (cursorPosition.y == 5) {
				} else {
				
					float y = 5f - cursorPosition.y;
					float x = cursorPosition.x;
					//cursor.transform.position = new Vector3(64 + x * 42f, 128 - 38 + y * 38, 13);
					yield return StartCoroutine(moveCursor(new Vector2(25 + x * 24, 25 + y * 24)));
					y = cursorPosition.y;
					pokemonNum = boxNum * 45f + y * 9f + x + 1;
					updatePreview ((int) pokemonNum,(int)( y * 9 + x));
						
				}
			}

		




			yield return null;
		}

		StopCoroutine("animateIcons");
		//yield return new WaitForSeconds(sceneTransition.FadeOut());
		yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
		GlobalVariables.global.resetFollower();
		this.gameObject.SetActive(false);

	}


	public IEnumerator informationScreen(){
		float increment = 0;
		float startX = background.pixelInset.x;
		float startY = background.pixelInset.y;
		float distanceX = -200;

		while (increment < 1)
		{
			increment += (1 / moveSpeed) * Time.deltaTime;
			if (increment > 1)
			{
				increment = 1;
			}

			GameObject[] pokedexUI = GameObject.FindGameObjectsWithTag ("Pokedex");

			for (int x = 0; x < pokedexUI.Length; x++) {
				
				pokedexUI [x].GetComponent<GUITexture> ().pixelInset = new Rect (startX + (distanceX * increment), startY, pokedexUI [x].GetComponent<GUITexture> ().pixelInset.width, pokedexUI [x].GetComponent<GUITexture> ().pixelInset.height);
				
				
			}
		

			yield return null;
		}
	}

	// Usability Functions

	private void updatePreview(int id, int num){
		//setTyping (id);
		preview.texture = pokeSprites[num];
		setText (id);
		setTyping (id);
		updateAbilities (id);
	}

	private Texture getSprite(int id){
		Texture[] animation;

		animation = Resources.LoadAll<Texture>("PokemonSprites" + "/" + toNum(id) + "/");
		if (animation.Length == 0){
			Debug.LogWarning ("Attempt Loading Charizard");
			animation = Resources.LoadAll<Texture>("PokemonSprites" + "/" + toNum(6).ToString() + "/");
		}

		return animation [0];
	}

	private string toNum(int n){

		string result = n.ToString();
		while (result.Length < 3)
		{
			result = "0" + result;
		}
		return result;

	}

	private void updateAbilities(int i){
	
		PokemonData pokemon = PokemonDatabase.getPokemon (i);
		if (pokemon == null) {
			ability1.text = "Unknown";
			ability2.text = "Unknown";
			ability1shadow.text = "Unknown";
			ability2shadow.text = "Unknown";
		} else {
			ability1.text = pokemon.getAbility (0);
			ability2.text = pokemon.getAbility (1);
			ability1shadow.text = pokemon.getAbility (0);
			ability2shadow.text = pokemon.getAbility (1);
			if (ability1.text == ability2.text) {
				ability2.text = "";
				ability2shadow.text = "";
			}
		}
	
	}

	private IEnumerator moveCursor(Vector2 destination)
	{
		float increment = 0;
		float startX = cursor.pixelInset.x;
		float startY = cursor.pixelInset.y;
		float distanceX = destination.x - startX;
		float distanceY = destination.y - startY;
		while (increment < 1)
		{
			increment += (1 / moveSpeed) * Time.deltaTime;
			if (increment > 1)
			{
				increment = 1;
			}
			cursor.pixelInset = new Rect(startX + (distanceX * increment), startY + (distanceY * increment), cursor.pixelInset.width, cursor.pixelInset.height);
			
			yield return null;
		}
	}

	private void setText(int i){
		id.text = "#" + toNum (i);
		idShadow.text = "#" + toNum (i);
		PokemonData pokemon = PokemonDatabase.getPokemon (i);
		if (pokemon == null) {
			name.text = "None";
			nameShadow.text = "None";
		} else {
			name.text = pokemon.getName ();
			nameShadow.text = pokemon.getName ();
		}

	}

	private void setTyping(int id){
		if (PokemonDatabase.getPokemon (id) != null) {
			type1.texture = typeToImage (PokemonDatabase.getPokemon (id).getType1 ());

			if (PokemonDatabase.getPokemon (id).getType2 () == PokemonData.Type.NONE) {
				type2.gameObject.SetActive (false);
			} else {
				type2.gameObject.SetActive (true);
				type2.texture = typeToImage (PokemonDatabase.getPokemon (id).getType2 ());
			}
		} else {
			type1.texture = typeToImage (PokemonData.Type.NONE);
			type2.texture = typeToImage (PokemonData.Type.NONE);
		}
	}

	private Texture getIcon(int id){

		Texture[] icons = Resources.LoadAll<Texture>("PokemonIcons/icon" + toNum(id));
		if (icons.Length == 0)
		{
			
			icons = Resources.LoadAll<Texture>("PokemonIcons/icon006");
		}

		return icons[0];

	}

	private void updateBox(int boxNum){
		string label = "Pokedex " + toNum(boxNum * 45 + 1) +  " - " + toNum(boxNum * 45 + 45);
		boxLabel.text = label;
		boxLabelShadow.text = label;

		for (int i = 0; i < 45; i++) {
			pokePreview [i].texture = getIcon (boxNum * 45 + i + 1);

		}

		for (int i = 0; i < 45; i++) {
			
			pokeSprites [i] = getSprite (boxNum * 45 + i + 1);
		}
	}

	private Texture typeToImage(PokemonData.Type type){




		if (type == PokemonData.Type.NORMAL) {
			return types [0];
		} else if (type == PokemonData.Type.FIGHTING) {
			return types [1];
		} else if (type == PokemonData.Type.FLYING) {
			return types [2];
		} else if (type == PokemonData.Type.POISON) {
			return types [3];
		} else if (type == PokemonData.Type.GROUND) {
			return types [4];
		} else if (type == PokemonData.Type.ROCK) {
			return types [5];
		} else if (type == PokemonData.Type.BUG) {
			return types [6];
		} else if (type == PokemonData.Type.GHOST) {
			return types [7];
		} else if (type == PokemonData.Type.STEEL) {
			return types [8];
		} else if (type == PokemonData.Type.NONE) {
			return types [9];
		} else if (type == PokemonData.Type.FIRE) {
			return types [10];
		} else if (type == PokemonData.Type.WATER) {
			return types [11];
		} else if (type == PokemonData.Type.GRASS) {
			return types [12];
		} else if (type == PokemonData.Type.ELECTRIC) {
			return types [13];
		} else if (type == PokemonData.Type.PSYCHIC) {
			return types [14];
		} else if (type == PokemonData.Type.ICE) {
			return types [15];
		} else if (type == PokemonData.Type.DRAGON) {
			return types [16];
		} else if (type == PokemonData.Type.DARK) {
			return types [17];
		} else if (type == PokemonData.Type.FAIRY) {
			return types [18];
		} else {
			return types [9];
		}
	
	}

}
