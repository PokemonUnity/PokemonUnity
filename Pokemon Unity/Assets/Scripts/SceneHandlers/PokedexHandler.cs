using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System;

public class PokedexHandler : MonoBehaviour {

	// Usage Variables
	public Vector2 cursorPosition;
	public float boxNum;
	private float pokemonNum;
	private Texture[] pokeSprites;
	private int[] pokemon;
	private bool screen2;
	private bool screenMoving;
	private int cursor2pos;
	public bool sort;
	public bool returnSelected;
	public Rect cursorPixel;
	public Rect cursor2Pixel;
	public PokemonData[] alphabeticalOrder;


	// GameObject like vars
	private GUITexture cursor;
	private GUITexture preview;
	private GUIText guiname;
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
	public GameObject selectedInfo;
	private GUIText selectedDex;
	private GUIText selectedDexShadow;
	private GUITexture selectedIcon;
	private GUIText selectedStats;
	private GUIText selectedStatsShadow;
	public GUITexture cursor2;
	public GUITexture filterArrow;
	public GUITexture filterType;
	public Rect filterArrowRect;
	public Rect filterTypeRect;

	[SerializeField]
	private float moveSpeed = 0.16f;

	// Ressource vars
	public Texture[] types;
	public AudioClip selectClip;


	private void generateAlphabetical (){
		
		alphabeticalOrder = PokemonDatabase.pokedex;

		/*PokemonDatabase.pokedex.Sort(alphabeticalOrder, delegate(PokemonData pokemon1, PokemonData pokemon2) {
			string name1 = pokemon1.getName();
			string name2 = pokemon2.getName();
			return -1;
		});*/
		Array.Sort (alphabeticalOrder, delegate(PokemonData pokemon1, PokemonData pokemon2) {
			try {
				return pokemon1.getName().CompareTo(pokemon2.getName());
			} catch {
				return -1;
			}
		});

	}

	void Awake () {
		
		filterArrowRect = filterArrow.pixelInset;
		filterTypeRect = filterType.pixelInset;
		returnSelected = true;
		cursor = GameObject.Find ("Cursor").GetComponent<GUITexture>();
		preview = GameObject.Find ("SelectedSprite").GetComponent<GUITexture> ();
		guiname = GameObject.Find ("SelectedName").GetComponent<GUIText>();
		nameShadow = GameObject.Find ("SelectedNameShadow").GetComponent<GUIText>();
		id = GameObject.Find ("SelectedID").GetComponent<GUIText>();
		idShadow = GameObject.Find ("SelectedIDShadow").GetComponent<GUIText>();
		boxLabel = GameObject.Find ("BoxHeader").GetComponent<GUIText>();
		boxLabelShadow = GameObject.Find ("BoxHeaderShadow").GetComponent<GUIText>();
		pokeSprites = new Texture[45];
		pokemon = new int[45];
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
		selectedDex = GameObject.Find ("SelectedEntry").GetComponent<GUIText> ();
		selectedDexShadow = GameObject.Find ("SelectedEntryShadow").GetComponent<GUIText> ();
		selectedIcon = GameObject.Find ("SelectedIcon").GetComponent<GUITexture> ();
		selectedStats = GameObject.Find ("Stats").GetComponent<GUIText> ();
		selectedStatsShadow = GameObject.Find ("StatsShadow").GetComponent<GUIText> ();
		cursor2pos = 0;
		generateAlphabetical ();
	}
	void Start()
    {
        this.gameObject.SetActive(false);
    }


	public IEnumerator control(){
		cursorPosition = Vector2.zero;
		boxNum = 0;
		updateBox ((int) boxNum);
		StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
		bool running = true;
		while (running) {
			// Input

			if (!screen2) {
				if (cursorPosition.y == -1) {
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
				} else if (cursorPosition.y == 5) {
					if (Input.GetAxis ("Horizontal") < 0 && !returnSelected)
						returnSelected = true;
					if (Input.GetAxis ("Horizontal") > 0 && returnSelected)
						returnSelected = false;
					if (Input.GetButton ("Select")) {
						if (returnSelected) {
							running = false;
						} else {
							sort = !sort;
							boxNum = 0;
							updateBox ((int) boxNum);
							SfxHandler.Play (selectClip);
							yield return new WaitForSeconds (0.2f);
						}
					}

				} else {
					if (Input.GetAxis ("Horizontal") > 0 && cursorPosition.x != 8 && cursorPosition.y != -1 && cursorPosition.y != 6 && screen2 != true) {
						cursorPosition += new Vector2 (1, 0);
						SfxHandler.Play (selectClip);
						yield return new WaitForSeconds (0.2f);
					} else if (Input.GetAxis ("Horizontal") < 0 && cursorPosition.x != 0 && cursorPosition.y != -1 && cursorPosition.y != 6 && screen2 != true) {
						cursorPosition -= new Vector2 (1, 0);
						SfxHandler.Play (selectClip);
						yield return new WaitForSeconds (0.2f);
					} 

					if (Input.GetButton ("Select")) {
						if (!screen2 && !screenMoving) {
							yield return StartCoroutine (startInfoScreen ());
						} 
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
			} else {
			
				if (Input.GetAxis ("Horizontal") > 0 && cursor2pos != 1) {

					cursor2pos += 1;
					SfxHandler.Play (selectClip);
					yield return new WaitForSeconds (0.2f);
				} else if (Input.GetAxis ("Horizontal") < 0 && cursor2pos != -1) {

					cursor2pos -= 1;
					SfxHandler.Play (selectClip);
					yield return new WaitForSeconds (0.2f);
				}

				if (Input.GetButton ("Select") && !screenMoving) {

					if (cursor2pos == 1 && !audioPlaying){
						
						yield return StartCoroutine (playCry ());

					} else if (cursor2pos == -1) {
						yield return StartCoroutine (stopInfoScreen ());
					}
					
				}
			
			}



			// Cursor Position
			if (screen2 != true) {
				cursor2.gameObject.SetActive (false);
				if (cursorPosition.y == -1) {
					//cursor.transform.position = new Vector3(200, 307, 13);
				} else if (cursorPosition.y == 5) {
					
					if (returnSelected) {
						yield return StartCoroutine (moveCursor (new Vector2 (30, 25)));
					} else {
						yield return StartCoroutine (moveCursor(new Vector2(200, 25)));
					}

				} else {
				
					float y = 5f - cursorPosition.y;
					float x = cursorPosition.x;
					//cursor.transform.position = new Vector3(64 + x * 42f, 128 - 38 + y * 38, 13);
					yield return StartCoroutine (moveCursor (new Vector2 (25 + x * 24, 25 + y * 24)));
					y = cursorPosition.y;
					pokemonNum = boxNum * 45f + y * 9f + x + 1;
					updatePreview ((int)pokemonNum, (int)(y * 9 + x));
						
				}
			} else {
				cursor2.gameObject.SetActive(true);

				if (cursor2pos == 0) {
					
					yield return StartCoroutine (moveCursor2 (new Vector2 (200, 125)));
				} else if (cursor2pos == -1) {
					yield return StartCoroutine (moveCursor2 (new Vector2 (120, 50)));
				} else if (cursor2pos == 1) {
					yield return StartCoroutine (moveCursor2 (new Vector2 (290, 50)));
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
		
	public void resetFilterPosition(){
		filterType.pixelInset = filterTypeRect;
		filterArrow.pixelInset = filterArrowRect;
	}


	private bool audioPlaying;
	public IEnumerator playCry(){
		audioPlaying = true;
		try {
			AudioClip cry =  Resources.Load<AudioClip>("Audio/cry/" + toNum((int) pokemonNum));
			Debug.Log (cry);
			SfxHandler.Play(cry);
		} catch {

		}
		yield return new WaitForSeconds (1);
		audioPlaying = false;
	}

	public IEnumerator startInfoScreen(){
		cursorPixel = cursor.pixelInset;


		screen2 = true;
		screenMoving = true;
		float increment = 0;
		float startX = background.pixelInset.x;
		float startY = background.pixelInset.y;
		float distanceX = -250;
		float disX = -0.73f;
		float starX = selectedInfo.transform.position.x;
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

			selectedInfo.transform.position = new Vector3(starX + (disX * increment), selectedInfo.transform.position.y, selectedInfo.transform.position.z);
			//cursor2.pixelInset = cursor2Pixel;

		

			yield return null;
		}

		screenMoving = false;
	}

	public IEnumerator stopInfoScreen(){
		cursor2Pixel = cursor2.pixelInset;
		screenMoving = true;
		float increment = 0;
		float startX = background.pixelInset.x;
		float startY = background.pixelInset.y;
		float distanceX = 250;
		float disX = 0.73f;
		float starX = selectedInfo.transform.position.x;
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

			selectedInfo.transform.position = new Vector3(starX + (disX * increment), selectedInfo.transform.position.y, selectedInfo.transform.position.z);
			cursor.pixelInset = cursorPixel;





			yield return null;
		}
		screenMoving = false;
		screen2 = false;
		resetFilterPosition ();
	}

	// Usability Functions

	private void updatePreview(int id, int num){
		//setTyping (id);
		id = pokemon[num];
		preview.texture = pokeSprites[num];
		setText (id);
		setTyping (id);
		updateAbilities (id);
		updateEntry (id);
		updateIcon (id);
		updateStats (id);
		resetFilterPosition ();
	}

	private void updateIcon(int id){

		selectedIcon.texture = getIcon (id);

	}


	private void updateStats(int id){
		try {
			string stats = PokemonDatabase.getPokemon (id).getHeight() + "m\n" + PokemonDatabase.getPokemon (id).getWeight() + "kg";
			selectedStats.text = stats;
			selectedStatsShadow.text = stats;
		} catch {
			string stats = "0m\n0kg";
			selectedStats.text = stats;
			selectedStatsShadow.text = stats;
		}
	}



	private void updateEntry (int id){
		PokemonData pokemon = PokemonDatabase.getPokemon (id);

		if (pokemon != null) {
			string s = wrapString (pokemon.getPokedexEntry (), 35);
			selectedDex.text = s;
			selectedDexShadow.text = s;
		} else {

			selectedDex.text = "None";
			selectedDexShadow.text = "None";

		}
	}

	string wrapString(string msg, int width) {
			string[] words = msg.Split (" " [0]);
			string retVal = ""; //returning string 
			string NLstr = "";  //leftover string on new line
			for (int index = 0 ; index < words.Length ; index++ ) {
				string word = words[index].Trim();
				//if word exceeds width
				if (words[index].Length >= width+2) {
					string[] temp = new string[5];
					int i = 0;
					while (words[index].Length > width) { //word exceeds width, cut it at widrh
						temp[i] = words[index].Substring(0,width) +"\n"; //cut the word at width
						words[index] = words[index].Substring(width);     //keep remaining word
						i++;
						if (words[index].Length <= width) { //the balance is smaller than width
							temp[i] = words[index];
							NLstr = temp[i];
						}
					}
					retVal += "\n";
					for (int x = 0 ; x < i+1 ; x++) { //loops through temp array
						retVal = retVal+temp[x];
					}
				}
				else if (index == 0) {
					retVal = words[0];
					NLstr = retVal;
				}
				else if (index > 0) {
					if (NLstr.Length + words[index].Length <= width ) {
						retVal = retVal+" "+words[index];
						NLstr = NLstr+" "+words[index]; //add the current line length
					}
					else if (NLstr.Length + words[index].Length > width) {
						retVal = retVal+ "\n" + words[index];
						NLstr = words[index]; //reset the line length
						//print ("newline! at word "+ words[index]);
					}
				}
			}


		if (retVal.Length > 150) {
			try {
				retVal = retVal.Split ("." [0]) [0] + ". " + retVal.Split ("." [0]) [1] + ".";
			} catch {
				retVal += ".";
			}
		}
		if (retVal.Length > 150) {
			try {
				retVal = retVal.Split ("." [0]) [0] + ".";
			} catch {
				retVal += ".";
			}
		}
			
			return retVal;
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

	private IEnumerator moveCursor2(Vector2 destination)
	{


		float increment = 0;
		float startX = cursor2.pixelInset.x;
		float startY = cursor2.pixelInset.y;
		float distanceX = destination.x - startX;
		float distanceY = destination.y - startY;
		while (increment < 1)
		{
			increment += (1 / moveSpeed) * Time.deltaTime;
			if (increment > 1)
			{
				increment = 1;
			}
			cursor2.pixelInset = new Rect(startX + (distanceX * increment), startY + (distanceY * increment), cursor2.pixelInset.width, cursor2.pixelInset.height);

			yield return null;
		}
	}

	private void setText(int i){
		id.text = "#" + toNum (i);
		idShadow.text = "#" + toNum (i);
		PokemonData pokemon = PokemonDatabase.getPokemon (i);
		if (pokemon == null) {
			guiname.text = "None";
			nameShadow.text = "None";
		} else {
			guiname.text = pokemon.getName ();
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
		if (!sort) {
			string label = "Pokedex " + toNum (boxNum * 45 + 1) + " - " + toNum (boxNum * 45 + 45);
			boxLabel.text = label;
			boxLabelShadow.text = label;

			for (int i = 0; i < 45; i++) {
				pokePreview [i].texture = getIcon (boxNum * 45 + i + 1);
				pokeSprites [i] = getSprite (boxNum * 45 + i + 1);
				pokemon [i] = boxNum * 45 + i + 1;

			}


				
		} else {
			string label;

			try {
				label = "Pokedex "  + alphabeticalOrder[boxNum * 45 + 1].getName() + " - " + alphabeticalOrder[boxNum * 45 + 45].getName();
			} catch {
				if (boxNum * 45 + 1 > alphabeticalOrder.Length) {
					boxNum -= 1;
					updateBox (boxNum);
					return;
				} else {
					label = "Pokedex "  + alphabeticalOrder[boxNum * 45 + 1].getName() + " - " + "End";
				}
			}
			boxLabel.text = label;
			boxLabelShadow.text = label;

			for (int i = 0; i < 45; i++) {
				//Debug.Log (alphabeticalOrder [boxNum * 45 + i + 1].getID ());
				try {
					pokePreview [i].texture = getIcon (alphabeticalOrder[boxNum * 45 + i + 1].getID());
					pokeSprites [i] = getSprite (alphabeticalOrder[boxNum * 45 + i + 1].getID());
					pokemon [i] = alphabeticalOrder[boxNum * 45 + i + 1].getID();
				} catch {
					
					pokePreview [i].texture = getIcon (0);
					pokeSprites [i] = getSprite (0);
					pokemon [i] = 0;

				}

			}



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
