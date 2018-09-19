//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {

	public int selectedButton = 0;
	public int selectedFile = 0;
	public bool newGame = false;
	public Sprite playerSprite;
	public PokemonOld.Gender playerGender;
	public Texture buttonSelected;
	public Texture buttonDimmed;

	private GameObject fileDataPanel;
	private GameObject continueButton;
	private GameObject intro;
	private GameObject importantThings;
	private GUITexture[] button = new GUITexture[3];
	private GUITexture[] buttonHighlight = new GUITexture[3];
	private GUIText[] buttonText = new GUIText[3];
	private GUIText[] buttonTextShadow = new GUIText[3];

	private GUIText fileNumbersText;
	private GUIText fileNumbersTextShadow;
	private GUIText fileSelected;

	private GUIText mapNameText;
	private GUIText mapNameTextShadow;
	private GUIText dataText;
	private GUIText dataTextShadow;
	private GUITexture[] pokemon = new GUITexture[6];
	private GUITexture background;
	private GUITexture introBackground;
	public AudioClip selectClip; 
	public AudioClip menuBGM; 
	public AudioClip professorMusic; 
	private DialogBoxHandler Dialog;

	private bool running;
	//private bool introFinished = false; //I wonder what I did differently, this is used in Code: Celestial Binary but unsued after I ported it
	private string playerName;
	private bool gender;

	void Awake(){

		SaveLoadOld.Load();
		
		fileDataPanel = transform.Find("FileData").gameObject;
		continueButton = transform.Find("Continue").gameObject;
		intro = transform.Find("Intro(unfinished)").gameObject;
		importantThings = transform.Find("ImportantThings").gameObject;
		Transform newGameButton = transform.Find("NewGame");
		Transform settingsButton = transform.Find("Settings");

		Transform[] buttonTransforms = new Transform[]{
			continueButton.transform,
			newGameButton,
			settingsButton};
		for(int i = 0; i < 3; i++){
			button[i] = buttonTransforms[i].Find("ButtonTexture").GetComponent<GUITexture>();
			buttonHighlight[i] = buttonTransforms[i].Find("ButtonHighlight").GetComponent<GUITexture>();
			buttonText[i] = buttonTransforms[i].Find("Text").GetComponent<GUIText>();
			buttonTextShadow[i] = buttonText[i].transform.Find("TextShadow").GetComponent<GUIText>();
		}

		fileNumbersText = continueButton.transform.Find("FileNumbers").GetComponent<GUIText>();
		fileNumbersTextShadow = fileNumbersText.transform.Find("FileNumbersShadow").GetComponent<GUIText>();
		fileSelected = fileNumbersText.transform.Find("FileSelected").GetComponent<GUIText>();

		mapNameText = fileDataPanel.transform.Find("MapName").GetComponent<GUIText>();
		mapNameTextShadow = mapNameText.transform.Find("MapNameShadow").GetComponent<GUIText>();
		dataText = fileDataPanel.transform.Find("DataText").GetComponent<GUIText>();
		dataTextShadow = dataText.transform.Find("DataTextShadow").GetComponent<GUIText>();
		background = transform.Find("Background").GetComponent<GUITexture>();
		introBackground = transform.Find("Intro(unfinished)").Find("Background").GetComponent<GUITexture>();
		Dialog = gameObject.GetComponent<DialogBoxHandler>();
		
		for(int i = 0; i < 6; i++){
			pokemon[i] = fileDataPanel.transform.Find("Pokemon"+i).GetComponent<GUITexture>();
		}
	}

	void Start(){
		StartCoroutine(control());
	}

	
	private void updateButton(int newButtonIndex){
		if(newButtonIndex != selectedButton){
			button[selectedButton].texture = buttonDimmed;
			buttonHighlight[selectedButton].enabled = false;
		}
		selectedButton = newButtonIndex;

		button[selectedButton].texture = buttonSelected;
		buttonHighlight[selectedButton].enabled = true;
	}

	private void updateFile(int newFileIndex){
		selectedFile = newFileIndex;

		Vector2[] highlightPositions = new Vector2[]{
			new Vector2(132,143),
			new Vector2(147,143),
			new Vector2(162,143)};
		fileSelected.pixelOffset = highlightPositions[selectedFile];
		fileSelected.text = ""+(selectedFile+1);

        if (SaveLoadOld.savedGames[selectedFile] != null)
        {
            int badgeTotal = 0;
            for (int i = 0; i < 12; i++)
            {
                if (SaveLoadOld.savedGames[selectedFile].gymsBeatTime[i] != null)//SaveLoad.savedGames[selectedFile].gymsBeaten[i]
                {
                    badgeTotal += 1;
                }
            }
            /*string playerTime = "" + SaveLoad.savedGames[selectedFile].playerMinutes;
            if (playerTime.Length == 1)
            {
                playerTime = "0" + playerTime;
            }
            playerTime = SaveLoad.savedGames[selectedFile].playerHours + " : " + playerTime;*/

            mapNameText.text = SaveLoadOld.savedGames[selectedFile].mapName;
            mapNameTextShadow.text = mapNameText.text;
            Debug.Log(PokemonDatabaseOld.LoadPokedex().Length);
            Debug.Log(SaveLoadOld.savedGames[selectedFile].pokedexCaught + "/" + SaveLoadOld.savedGames[selectedFile].pokedexSeen);
            dataText.text = SaveLoadOld.savedGames[selectedFile].playerName
                            + "\n" + badgeTotal
                            + "\n" + "0" //Pokedex not yet implemented
                            + "\n" + System.String.Format("{0} : {1:00}",SaveLoadOld.savedGames[selectedFile].playerTime.Hours, SaveLoadOld.savedGames[selectedFile].playerTime.Minutes);
            dataTextShadow.text = dataText.text;

			for(int i = 0; i < 6; i++){
				if(SaveLoadOld.savedGames[selectedFile].PC.boxes[0][i] != null){
					pokemon[i].texture = SaveLoadOld.savedGames[selectedFile].PC.boxes[0][i].GetIcons();}
				else{
					pokemon[i].texture = null;}
			}
		}

	}

	private IEnumerator animateIcons(){
		while(true){
			for(int i = 0; i < 6; i++){
				pokemon[i].border = new RectOffset(32,0,0,0);}
			yield return new WaitForSeconds(0.15f);
			for(int i = 0; i < 6; i++){
				pokemon[i].border = new RectOffset(0,32,0,0);}
			yield return new WaitForSeconds(0.15f);
		}
	}

	private IEnumerator animBG(){
		float scrollSpeed = 1.2f;
		while(running || newGame){
			float increment = 0;
			while (increment < 1){
				increment += (1/scrollSpeed)*Time.deltaTime;
				if (increment > 1){
					increment = 1;}
				background.pixelInset = new Rect(Mathf.RoundToInt(-32f*increment),Mathf.RoundToInt(32f*increment),background.pixelInset.width,background.pixelInset.height);
				introBackground.pixelInset = new Rect(Mathf.RoundToInt(-32f*increment),Mathf.RoundToInt(32f*increment),introBackground.pixelInset.width,introBackground.pixelInset.height);
				yield return null;
			}
		}
	}

	private IEnumerator gotoGender(){
		Dialog.drawDialogBox();
		yield return Dialog.StartCoroutine("drawText","Are you a boy?\nOr are you a girl?");
		while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
		yield return Dialog.StartCoroutine("scrollText",0.2f);
		yield return Dialog.StartCoroutine("drawTextSilent","Won't you please tell me?");
		Dialog.drawChoiceBox(new string[]{"Boy","Girl"});
		yield return new WaitForSeconds(0.2f);
		yield return StartCoroutine(Dialog.choiceNavigate());
		int chosenIndexa = Dialog.chosenIndex;
		if(chosenIndexa == 1){ //boy
			playerName = "Ethan"; //john
			gender = true;
			Dialog.undrawChoiceBox();	
		} else if(chosenIndexa == 0) { //girl
			playerName = "Lyra"; //jane
			gender = false;
			Dialog.undrawChoiceBox();
		}
		if(gender){
			Dialog.drawDialogBox();
			yield return Dialog.StartCoroutine("drawText","So, you're a boy then?");
			Dialog.drawChoiceBox(new string[]{"Yes","No"});
			yield return new WaitForSeconds(0.2f);
			yield return StartCoroutine(Dialog.choiceNavigate());
			int chosenIndexb = Dialog.chosenIndex;
			if(chosenIndexb == 1){ //yes
				Dialog.undrawChoiceBox();
			} else if(chosenIndexb == 0) { //no
				Dialog.undrawChoiceBox();
				yield return StartCoroutine("gotoGender");
			}
		}
		else {
			Dialog.drawDialogBox();
			yield return Dialog.StartCoroutine("drawText","So, you're a girl then?");
			Dialog.drawChoiceBox(new string[]{"Yes","No"});
			yield return new WaitForSeconds(0.2f);
			yield return StartCoroutine(Dialog.choiceNavigate());
			int chosenIndexc = Dialog.chosenIndex;
			if(chosenIndexc == 1){ //yes
				Dialog.undrawChoiceBox();
			} else if(chosenIndexc == 0) { //no
				Dialog.undrawChoiceBox();
				yield return StartCoroutine("gotoGender");
			}
		}

		Dialog.drawDialogBox();
		yield return Dialog.StartCoroutine("drawText","Please tell me your name.");
		while(!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
		Dialog.undrawDialogBox();
		yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
		Scene.main.Typing.gameObject.SetActive(true);
		if(gender){
			playerGender = PokemonOld.Gender.MALE;
			playerSprite = null;
		}
		else {
			playerGender = PokemonOld.Gender.FEMALE;
			playerSprite = null;
		}
		StartCoroutine(Scene.main.Typing.control(7,playerName,playerGender,new Sprite[]{playerSprite}));
		while(Scene.main.Typing.gameObject.activeSelf){yield return null;}
		if(Scene.main.Typing.typedString.Length > 0){playerName = Scene.main.Typing.typedString;}
		yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
		Dialog.drawDialogBox();
		yield return Dialog.StartCoroutine("drawText","Your name is " + playerName + "?");
		Dialog.drawChoiceBox(new string[]{"Yes","No"});
		yield return new WaitForSeconds(0.2f);
		yield return StartCoroutine(Dialog.choiceNavigate());
		int chosenIndexd = Dialog.chosenIndex;
		if(chosenIndexd == 1){ //yes
			Dialog.undrawChoiceBox();
		} else if(chosenIndexd == 0) { //no
			Dialog.undrawChoiceBox();
			yield return StartCoroutine("gotoGender");
		}
	}
	private IEnumerator openAnimNewGame(){
		float scrollSpeed = 0.5f;
		float increment = 0;
		while (increment < 1){
			if(!newGame) {
				increment += (1/scrollSpeed)*Time.deltaTime;
				if (increment > 1){
					increment = 1;}
				transform.Find("FileData").position = new Vector3(0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				transform.Find("Continue").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				transform.Find("NewGame").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				transform.Find("Settings").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				
			}
			if(transform.Find("FileData").position == new Vector3(0.5f, transform.Find("").position.y, transform.Find("FileData").position.z) || newGame == true) {
				if(!newGame) {
					yield return StartCoroutine(ScreenFade.main.Fade(false, 0.2f));
					transform.Find("OpeningLecture").gameObject.SetActive(true);
					yield return StartCoroutine(ScreenFade.main.Fade(true, 0f));
				}
				Dialog.drawDialogBox();
				yield return Dialog.StartCoroutine("drawText","...\n...");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Hmm... Intresting...\n...");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Huh? Oh! Excuse me, sorry!\nI was just reading this book here.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.undrawDialogBox();
				
				yield return StartCoroutine(ScreenFade.main.Fade(false, 0f));
				transform.Find("OpeningLecture").Find("Background").GetComponent<GUITexture>().color = new UnityEngine.Color(0.5f,0.5f,0.5f);
				transform.Find("OpeningLecture").Find("Professor").gameObject.SetActive(true);
				BgmHandler.main.PlayMain(null, 0);
				BgmHandler.main.PlayMain(professorMusic, 0);
				yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Sorry to keep you waiting!");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Welcome to the world of Pokémon!");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","My name is Professor Oak.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","But everyone calls me the Pokémon\nProfessor.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Before we go any further, I'd like to \ntell you a few things you should");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				yield return Dialog.StartCoroutine("scrollText",0.2f);
				yield return Dialog.StartCoroutine("drawTextSilent","know about this world!");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","This world is widely inhabited by\ncreatures known as Pokémon.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","We humans live alongside Pokemon\nas friends.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","At times we play together, and at\nother times we work together.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Some people use their Pokemon to\nbattle and develop closer bonds");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				yield return Dialog.StartCoroutine("scrollText",0.2f);
				yield return Dialog.StartCoroutine("drawTextSilent","with them.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();
				
				yield return Dialog.StartCoroutine("drawText","Now, why don't you tell me a little bit\nabout yourself?");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				yield return gotoGender();

				Dialog.drawDialogBox();
				yield return Dialog.StartCoroutine("drawText",playerName + "!\nAre you ready?");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Your very own tale of grand adventure\nis about to unfold.");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Fun experiences, difficult experiences,\nthere's so much waiting for you!");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","Dreams! Adventure!\nLet's go to the world of Pokemon!");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();

				yield return Dialog.StartCoroutine("drawText","I'll see you later!");
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
				Dialog.drawDialogBox();
				yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
				
				BgmHandler.main.PlayMain(null, 0);
				SaveDataOld.currentSave = new SaveDataOld(SaveLoadOld.getSavedGamesCount());

				GlobalVariables.global.CreateFileData(playerName, gender); 

				GlobalVariables.global.playerPosition = new Vector3(79,0,31);
				GlobalVariables.global.playerDirection = 2;
				GlobalVariables.global.fadeIn = true;
				UnityEngine.SceneManagement.SceneManager.LoadScene("indoorsNW");
			}
			yield return null;
		}
	}

	private IEnumerator openAnim(){
		BgmHandler.main.PlayMain(null, 0);
		float scrollSpeed = 0.5f;
		float increment = 0;
		while (increment < 1){
			increment += (1/scrollSpeed)*Time.deltaTime;
			if (increment > 1){
				increment = 1;}
			transform.Find("FileData").position = new Vector3(0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			transform.Find("Continue").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			transform.Find("NewGame").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			transform.Find("Settings").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			if(transform.Find("FileData").position == new Vector3(0.5f, transform.Find("FileData").position.y, transform.Find("FileData").position.z)) {

                //CONTINUE
                #region CONTINUE
                yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
				SaveDataOld.currentSave = SaveLoadOld.savedGames[selectedFile];

				Debug.Log(SaveLoadOld.savedGames[0]);
				Debug.Log(SaveLoadOld.savedGames[1]);
				Debug.Log(SaveLoadOld.savedGames[2]);
                SaveDataOld.currentSave.startTime = System.DateTime.UtcNow;
                GlobalVariables.global.playerPosition = SaveDataOld.currentSave.playerPosition.v3;
				GlobalVariables.global.playerDirection = SaveDataOld.currentSave.playerDirection;
					
				UnityEngine.SceneManagement.SceneManager.LoadScene(SaveDataOld.currentSave.levelName);
                //yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                #endregion
            }
            yield return null;
		}
	}

	public IEnumerator control(){
		int fileCount = SaveLoadOld.getSavedGamesCount();

        //NEW GAME
        #region NEW GAME
        if (fileCount == 0){
			BgmHandler.main.PlayMain(menuBGM, 0);
			newGame = true;
			importantThings.SetActive(true);
			updateButton(1);
			continueButton.SetActive(false);
			fileDataPanel.SetActive(false);
			for(int i = 1; i < 3; i++){
				button[i].pixelInset = new Rect(button[i].pixelInset.x, button[i].pixelInset.y + 64f, button[i].pixelInset.width, button[i].pixelInset.height);
				buttonHighlight[i].pixelInset = new Rect(buttonHighlight[i].pixelInset.x, buttonHighlight[i].pixelInset.y + 64f, buttonHighlight[i].pixelInset.width, buttonHighlight[i].pixelInset.height);
				buttonText[i].pixelOffset = new Vector2(buttonText[i].pixelOffset.x, buttonText[i].pixelOffset.y + 64f);
				buttonTextShadow[i].pixelOffset = new Vector2(buttonTextShadow[i].pixelOffset.x, buttonTextShadow[i].pixelOffset.y + 64f);
			}
			transform.Find("NewGame").gameObject.SetActive(false);
			transform.Find("Settings").gameObject.SetActive(false);
			StartCoroutine("animBG");
			yield return new WaitForSeconds(2f);
            SaveDataOld.currentSave.startTime = System.DateTime.UtcNow;
            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
			importantThings.SetActive(false);
			transform.Find("OpeningLecture").gameObject.SetActive(true);
			yield return StartCoroutine(ScreenFade.main.Fade(true, 0f));
			yield return StartCoroutine("openAnimNewGame");
		}
        #endregion
        else
        {
			BgmHandler.main.PlayMain(menuBGM, 0);
			updateButton(0);
			updateFile(0);

			StartCoroutine(animateIcons());

			if(fileCount == 1){
				fileNumbersText.text = "File     1";
				fileNumbersTextShadow.text = "File     1";}
			else if(fileCount == 2){
				fileNumbersText.text = "File     1   2";
				fileNumbersTextShadow.text = "File     1   2";}
			else if(fileCount == 3){
				fileNumbersText.text = "File     1   2   3";
				fileNumbersTextShadow.text = "File     1   2";}
		}

		running = true;
		//bool introup = true;
		StartCoroutine("animBG");
		/*if(introup == true) {
			yield return new WaitForSeconds(3.2f);
			yield return StartCoroutine(ScreenFade.main.Fade(false, 0.2f));
			yield return new WaitForSeconds(0.2f);
			yield return StartCoroutine(ScreenFade.main.Fade(true, 0.2f));
			yield return new WaitForSeconds(3.2f);
			yield return StartCoroutine(ScreenFade.main.Fade(false, 0.2f));
			yield return new WaitForSeconds(0.2f);
			introBackground.color = new UnityEngine.Color(0.5f,0.5f,0.5f);
			yield return StartCoroutine(ScreenFade.main.Fade(true, 0.2f));
			yield return new WaitForSeconds(15.5f);
			yield return StartCoroutine(ScreenFade.main.Fade(false, 0.2f));
			yield return new WaitForSeconds(0.2f);
			introup = false;
			intro.SetActive(false);
			yield return StartCoroutine(ScreenFade.main.Fade(true, 0.2f));
			//yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
		}*/
		while(running){
			if(Input.GetButtonDown("Select")){
				if(selectedButton == 0){		//CONTINUE
					SfxHandler.Play(selectClip);
					yield return StartCoroutine("openAnim");
					
				}
				else if(selectedButton == 1){	//NEW GAME
					SfxHandler.Play(selectClip);
					yield return StartCoroutine("openAnimNewGame");
				}
				else if(selectedButton == 2){	//SETTINGS
					SfxHandler.Play(selectClip);
					//yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
					yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

					Scene.main.Settings.gameObject.SetActive(true);
					StartCoroutine(Scene.main.Settings.control());
					while(Scene.main.Settings.gameObject.activeSelf){
						yield return null;}

					//yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
					yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
				}
			}
			/*else if(Input.GetKeyDown(KeyCode.Delete)){ //delete save file
				SfxHandler.Play(selectClip2);
				float time = Time.time;
				bool released = false;
				Debug.Log("Save "+(selectedFile+1)+" will be deleted! Release 'Delete' key to prevent this!");
				while(Time.time < time+4 && !released){
					if(Input.GetKeyUp(KeyCode.Delete)){
						released = true;
						SfxHandler.Play(selectClip3);
					}
					yield return null;}
				
				if(Input.GetKey(KeyCode.Delete) && !released){
					SfxHandler.Play(selectClip4);
					SaveLoad.resetSaveGame(selectedFile);
					Debug.Log("Save "+(selectedFile+1)+" was deleted!");
					
					yield return new WaitForSeconds(1f);
					
					Application.LoadLevel(Application.loadedLevel);
				}
				else{
					Debug.Log("'Delete' key was released!");
				}
			yield return null;
		}*/
			else if(Input.GetKeyDown(KeyCode.Delete)){
				Dialog.drawDialogBox();
				yield return Dialog.StartCoroutine("drawText","Are you sure you want to delete Save #"+(selectedFile+1)+"?");
				Dialog.drawChoiceBoxNo();
				yield return new WaitForSeconds(0.2f);
				yield return StartCoroutine(Dialog.choiceNavigateNo());
				int chosenIndex = Dialog.chosenIndex;
				if(chosenIndex == 1){
					SaveLoadOld.resetSaveGame(selectedFile);
					Debug.Log("Save "+(selectedFile+1)+" was deleted!");
					Dialog.undrawDialogBox();
					Dialog.undrawChoiceBox();
					Dialog.drawDialogBox();
					yield return Dialog.StartCoroutine("drawText","Save #"+(selectedFile+1)+" was deleted!");
					yield return new WaitForSeconds(2f);
					yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					
					UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
				} else {
					Dialog.undrawDialogBox();
					Dialog.undrawChoiceBox();
				}
			}
			else{
				if(Input.GetAxisRaw("Vertical") > 0){
					float minimumButton = (continueButton.activeSelf)? 0 : 1;
					if(selectedButton > minimumButton){
						updateButton(selectedButton-1);
						SfxHandler.Play(selectClip);
						yield return new WaitForSeconds(0.2f);
					}
				}
				else if(Input.GetAxisRaw("Vertical") < 0){
					if(selectedButton < 2){
						updateButton(selectedButton+1);
						SfxHandler.Play(selectClip);
						yield return new WaitForSeconds(0.2f);
					}
				}
				if(Input.GetAxisRaw("Horizontal") > 0){
					if(selectedButton == 0){
						if(selectedFile < fileCount-1){
							updateFile(selectedFile+1);
							SfxHandler.Play(selectClip);
							yield return new WaitForSeconds(0.2f);
						}
					}
				}
				else if(Input.GetAxisRaw("Horizontal") < 0){
					if(selectedButton == 0){
						if(selectedFile > 0){
							updateFile(selectedFile-1);
							SfxHandler.Play(selectClip);
							yield return new WaitForSeconds(0.2f);
						}
					}
				}
			}


			yield return null;
		}
	}


}
