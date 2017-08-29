//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	public int selectedButton = 0;
	public int selectedFile = 0;
	public Sprite johnSprite;
	public Sprite janeSprite;
	public Texture buttonSelected;
	public Texture buttonDimmed;

	private GameObject fileDataPanel;
	private GameObject continueButton;
	private GameObject intro;

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
	public AudioClip futabaTown; 
	public AudioClip introMusic; 
	private DialogBoxHandler Dialog;

	private bool running;
	private bool introFinished = false;
	private string playername;
	private bool gender;

	void Awake(){

		SaveLoad.Load();
		
		fileDataPanel = transform.Find("FileData").gameObject;
		continueButton = transform.Find("Continue").gameObject;
		intro = transform.Find("Intro").gameObject;
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
		introBackground = transform.Find("Intro").Find("Background").GetComponent<GUITexture>();
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

		if(SaveLoad.savedGames[selectedFile] != null){
			int badgeTotal = 0;
			for(int i = 0; i < 12; i++){
				if(SaveLoad.savedGames[selectedFile].gymsBeaten[i]){
					badgeTotal += 1;}
			}
			string playerTime = ""+SaveLoad.savedGames[selectedFile].playerMinutes;
			if(playerTime.Length == 1){
				playerTime = "0" + playerTime;}
			playerTime = SaveLoad.savedGames[selectedFile].playerHours +" : "+ playerTime;
			int pokeDex = SaveLoad.savedGames[selectedFile].pokeDex;
			mapNameText.text = SaveLoad.savedGames[selectedFile].mapName;
			mapNameTextShadow.text = mapNameText.text;
			dataText.text = SaveLoad.savedGames[selectedFile].playerName
					 +"\n"+ badgeTotal
					 +"\n"+ pokeDex //Pokedex not yet implemented
					 +"\n"+ playerTime;
			dataTextShadow.text = dataText.text;

			for(int i = 0; i < 6; i++){
				if(SaveLoad.savedGames[selectedFile].PC.boxes[0][i] != null){
					pokemon[i].texture = SaveLoad.savedGames[selectedFile].PC.boxes[0][i].GetIcons();}
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
		while(running){
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

	private IEnumerator openAnimNewGame(){
		BgmHandler.main.PlayMain(null, 0);
		BgmHandler.main.PlayMain(introMusic, 0);
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

				//intro script stuff here and then
				Dialog.drawDialogBox();
				yield return Dialog.StartCoroutine("drawText","So, are you a boy or a girl?");
				Dialog.drawChoiceBox(new string[]{"Boy","Girl"});
				yield return new WaitForSeconds(0.2f);
				yield return StartCoroutine(Dialog.choiceNavigate());
				int chosenIndex = Dialog.chosenIndex;
				if(chosenIndex == 1){ //boy
					playername = "Gold"; //john
					gender = true;
					Dialog.undrawDialogBox();
					Dialog.undrawChoiceBox();
					Dialog.drawDialogBox();
					yield return Dialog.StartCoroutine("drawText","What is your name?");
					while(!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){
						yield return null;}
					Dialog.undrawDialogBox();
					yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
						
					Scene.main.Typing.gameObject.SetActive(true);
					StartCoroutine(Scene.main.Typing.control(8,playername,Pokemon.Gender.MALE,new Sprite[]{johnSprite}));
					while(Scene.main.Typing.gameObject.activeSelf){
						yield return null;
					}
					if(Scene.main.Typing.typedString.Length > 0){
						playername = Scene.main.Typing.typedString;}

				} else if(chosenIndex == 0) { //girl
					playername = "Gold"; //jane
					gender = false;
					Dialog.undrawDialogBox();
					Dialog.undrawChoiceBox();
					Dialog.drawDialogBox();
					yield return Dialog.StartCoroutine("drawText","What is your name?");
					while(!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){
						yield return null;}
					Dialog.undrawDialogBox();
					yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
						
					Scene.main.Typing.gameObject.SetActive(true);
					StartCoroutine(Scene.main.Typing.control(8,playername,Pokemon.Gender.FEMALE,new Sprite[]{janeSprite}));
					while(Scene.main.Typing.gameObject.activeSelf){
						yield return null;
					}
					if(Scene.main.Typing.typedString.Length > 0){
						playername = Scene.main.Typing.typedString;}
				}
				yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
				Dialog.drawDialogBox();
				yield return Dialog.StartCoroutine("drawText","Hello " + playername + "!");
				while(!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){
					yield return null;}
				Dialog.undrawDialogBox();

				BgmHandler.main.PlayMain(null, 0);
				yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
				SaveData.currentSave = new SaveData(SaveLoad.getSavedGamesCount());

				GlobalVariables.global.CreateFileData(playername, gender); 

				GlobalVariables.global.playerPosition = new Vector3(82,0,33);
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

				yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
				SaveData.currentSave = SaveLoad.savedGames[selectedFile];

				Debug.Log(SaveLoad.savedGames[0]);
				Debug.Log(SaveLoad.savedGames[1]);
				Debug.Log(SaveLoad.savedGames[2]);
				GlobalVariables.global.playerPosition = SaveData.currentSave.playerPosition.v3;
				GlobalVariables.global.playerDirection = SaveData.currentSave.playerDirection;
					
				UnityEngine.SceneManagement.SceneManager.LoadScene(SaveData.currentSave.levelName);
				yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
			}
			yield return null;
		}
	}

	public IEnumerator control(){
		BgmHandler.main.PlayMain(futabaTown, 0);
		int fileCount = SaveLoad.getSavedGamesCount();

		if(fileCount == 0){

			updateButton(1);
			continueButton.SetActive(false);
			fileDataPanel.SetActive(false);
			for(int i = 1; i < 3; i++){
				button[i].pixelInset = new Rect(button[i].pixelInset.x, button[i].pixelInset.y + 64f, button[i].pixelInset.width, button[i].pixelInset.height);
				buttonHighlight[i].pixelInset = new Rect(buttonHighlight[i].pixelInset.x, buttonHighlight[i].pixelInset.y + 64f, buttonHighlight[i].pixelInset.width, buttonHighlight[i].pixelInset.height);
				buttonText[i].pixelOffset = new Vector2(buttonText[i].pixelOffset.x, buttonText[i].pixelOffset.y + 64f);
				buttonTextShadow[i].pixelOffset = new Vector2(buttonTextShadow[i].pixelOffset.x, buttonTextShadow[i].pixelOffset.y + 64f);
			}

		}
		else{
			updateButton(0);
			updateFile(0);

			StartCoroutine(animateIcons());

			if(fileCount == 1){
				fileNumbersText.text = "File     1";}
			else if(fileCount == 2){
				fileNumbersText.text = "File     1   2";}
			else if(fileCount == 3){
				fileNumbersText.text = "File     1   2   3";}
		}

		running = true;
		bool introup = true;
		StartCoroutine("animBG");
		if(introup == true) {
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
		}
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
					SaveLoad.resetSaveGame(selectedFile);
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
