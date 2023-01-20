//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class FollowerMovement : MonoBehaviour, INeedDirection
{
	#region Variables

	public PokemonSO Pokemon;
	public bool DrawGizmos = false;

	public int pokemonID = 6;
	private int partyIndex = 0;
	private DialogBoxHandlerNew Dialog;

	[Header("Movement")]
	public DirectionSurrogate directionSurrogate;
	public int direction = 2;
	public float speed;
	public bool IsHidden;
	public bool moving = false;
	public bool CanMove = true;
	PlayerMovement playerMovement;
	private Vector3 destinationPosition;
	private Vector3 position;

	[Header("Transforms")]
	public Transform Pawn;
	public Transform Hitbox;

	[Header("Audio")]
	public AudioClip WithdrawClip;


	[Header("Light")]
	public bool HasLight;
	public Color LightColor;
	public float LightIntensity;
	private Light followerLight;

	[Header("Sprites")]
	private SpriteRenderer spriteRenderer;
	[SerializeField] SpriteRenderer spriteLightRenderer;
	[SerializeField] SpriteRenderer spriteReflectionRenderer;
	[SerializeField] SpriteRenderer spriteLightReflectionRenderer;
	private Sprite[] spriteSheet;
	private Sprite[] lightSheet;
	[SerializeField] 
	SpriteRenderer pawnShadow;
	public Sprite PokeBall;

	[SerializeField] SpriteAnimatorBehaviour animator;

	#endregion

	public DirectionSurrogate DirectionSurrogate { get => directionSurrogate; }
	
	public Vector3 FacingDirection { get => directionSurrogate.FacingDirection; }

	#region Unity Functions

	void OnDrawGizmos() {
		if (!DrawGizmos) return;
		// direction
		Gizmos.color = Color.white;
		Gizmos.DrawLine(Pawn.position, Pawn.position + (FacingDirection.normalized));
	}

	void OnValidate() {
		if (Pawn == null) Debug.LogError("No Pawn Transform provided", gameObject);
		if (pawnShadow == null) Debug.LogError("No Pawn Shadow Transform provided", gameObject);
		if (spriteReflectionRenderer == null) Debug.LogError("No Pawn Reflection Sprite Renderer provided", gameObject);
		if (spriteLightRenderer == null) Debug.LogError("No Pawn Light Sprite Renderer provided", gameObject);
		if (spriteLightReflectionRenderer == null) Debug.LogError("No Pawn Light Reflection Sprite Renderer provided", gameObject);
		if (Hitbox == null) Debug.LogError("No hitBox Tranform provided", gameObject); // Follower_Transparent
		if (animator == null) Debug.LogError("No Sprite Animator provided", gameObject);
        if (DirectionSurrogate == null) Debug.LogError("No Direction Surrogate provided", gameObject);

        animator.Animations = Pokemon.Animations;
		DirectionSurrogate.OnDirectionUpdated.AddListener(SwitchAnimation);
	}

	void Awake() {
		//Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
		spriteRenderer = Pawn.GetComponent<SpriteRenderer>();
		followerLight = GetComponentInChildren<Light>();
	}

	void Start() {
		playerMovement = PlayerMovement.Singleton;

        // FIXME: This is temporarily commented for debugging purposes
        //if (PokemonUnity.Game.GameData.Trainer.party[0] == null) {
        //    gameObject.SetActive(false);
        //    return;
        //}
		//if (!GlobalVariables.Singleton.isFollowerOut) {
		//	Hide();
		//	return;
		//}

		adjustLightColorAndIntensity();
		changeFollower(partyIndex);
		//StartCoroutine(animateSprite());
	}

	void LateUpdate() {
		if (IsHidden) return;
		Camera camera = PlayerMovement.Singleton.PlayerCamera;
		adjustScaleBasedOnCamera(camera);
		updateRotation(camera);
		//pawnSprite.transform.rotation = PlayerMovement.player.transform.Find("Pawn").transform.rotation; 
	}

    #endregion

	#region Movement

	public void move(Vector3 destination, float sentSpeed) {
		Vector3 startPosition = transform.position; // add follower's position offset
		if (CanMove) {
			IsHidden = false;
			followerLight.enabled = true;
			pawnShadow.enabled = true;
			speed = sentSpeed;
			directionSurrogate.UpdateDirection(destination - startPosition);
			direction = (int)FacingDirection.ToMovementDirection(Vector3.forward, Vector3.up);
			LeanTween.move(gameObject, destination, sentSpeed);
		}
	}

	void updateRotation(Camera camera) {
		Pawn.transform.LookAt(camera.transform);
		Pawn.transform.localRotation = Quaternion.Euler(camera.transform.rotation.x - 50, 180, 0);
		Pawn.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
		SwitchAnimation("Walk");
	}

	public void ActivateMove() => CanMove = true;

	public void updateFollower() => changeFollower(0);

	public IEnumerator jump() {
		float increment = 0f;
		float parabola = 0;
		float height = 2.1f;
		Vector3 startPosition = Pawn.position;

		playClip(PlayerMovement.Singleton.jumpClip);

		while (increment < 1) {
			increment += (1 / PlayerMovement.Singleton.WalkSpeed) * Time.deltaTime;
			if (increment > 1) {
				increment = 1;
			}
			parabola = -height * (increment * increment) + (height * increment);
			Pawn.position = new Vector3(Pawn.position.x, startPosition.y + parabola, Pawn.position.z);
			yield return null;
		}
		Pawn.position = new Vector3(Pawn.position.x, startPosition.y, Pawn.position.z);

		playClip(PlayerMovement.Singleton.landClip);
	}

	#endregion

	#region Animation

	public void SwitchAnimation(Vector3 facingDirection) {
		animator.SwitchAnimation(facingDirection);
	}

	public void SwitchAnimation(string newAnimationName) {
        animator.SwitchAnimation(FacingDirection, newAnimationName);
	}

	//private IEnumerator animateSprite() {
	//	int frame = 0;
	//	int light_frame = 0;
	//	while (true) {
	//		for (int i = 0; i < 6; i++) {
	//			if (!IsHidden) {
	//				int newDirection;
	//				switch (direction) {
	//					case 0:
	//						newDirection = 3;
	//						break;
	//					case 1:
	//						newDirection = 2;
	//						break;
	//					case 2:
	//						newDirection = 0;
	//						break;
	//					case 3:
	//						newDirection = 1;
	//						break;
	//					default:
	//						newDirection = 0;
	//						break;
	//				}
	//				spriteRenderer.sprite = spriteSheet[newDirection * 4 + frame];
	//				if (lightSheet.Length >= 16)
	//					spriteLightRenderer.sprite = lightSheet[newDirection * 4 + frame];
	//				pawnShadow.enabled = true;
	//			}
	//			else
	//			{
	//				spriteRenderer.sprite = null;
	//				spriteLightRenderer.sprite = null;
	//				pawnShadow.enabled = false;
	//			}
	//			spriteReflectionRenderer.sprite = spriteRenderer.sprite;
	//			spriteLightReflectionRenderer.sprite = spriteLightRenderer.sprite;
	//			if (i > 2)
	//			{
	//				//pawn.localPosition = new Vector3(-0.016f, 0.808f, -0.4f);
	//				//pawnLight.localPosition = new Vector3(0, 0.171f, -0.36f);
	//			}
	//			else
	//			{
	//				//pawn.localPosition = new Vector3(-0.016f, 0.808f, -0.4f);
	//				//pawnLight.localPosition = new Vector3(0, 0.201f, -0.305f);
	//			}

	//			float time = 0.055f;
				
	//			yield return new WaitForSeconds((PlayerMovement.Singleton.IsMoving && PlayerMovement.Singleton.IsRunning) ? time / 2 : time);
	//		}

	//		frame++;
	//		if (frame > 3) frame = 0;
	//		light_frame = (light_frame == 0) ? 1 : 0;
	//	}
	//}

	#endregion

	#region Audio 

	private float PlayCry(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
		//SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
		//return pokemon.GetCry().length / pokemon.GetCryPitch();
		return 0;
	}

	private void playClip(AudioClip clip) {
		AudioSource PlayerAudio = PlayerMovement.Singleton.GetAudio();

		PlayerAudio.clip = clip;
		PlayerAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
		PlayerAudio.Play();
	}

	private IEnumerator PlayCryAndWait(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
		yield return new WaitForSeconds(PlayCry(pokemon));
	}

	#endregion

	#region Player Interactions

	private enum Happiness {
		SAD,
		NORMAL,
		HAPPY
	}

	public IEnumerator interact() {
		if (IsHidden) yield break;
		if (!playerMovement.setCheckBusyWith(gameObject)) yield break;

		//calculate Player's position relative to target object's and set direction accordingly. (Face the player)
		float xDistance = this.transform.position.x - playerMovement.gameObject.transform.position.x;
		float zDistance = this.transform.position.z - playerMovement.gameObject.transform.position.z;
		if (xDistance >= Mathf.Abs(zDistance))
			//Mathf.Abs() converts zDistance to a positive always.
			direction = 3; //this allows for better accuracy when checking orientation.
		else if (xDistance <= Mathf.Abs(zDistance) * -1)
			direction = 1;
		else if (zDistance >= Mathf.Abs(xDistance))
			direction = 2;
		else
			direction = 0;

		// Interaction
		if (SaveData.currentSave.PC.boxes[0][partyIndex].getPercentHP() < 0.25f) // Low HP
			yield return StartCoroutine(interaction_tired());
		else { // Casual Interaction
			Happiness h = getFollowerWeatherHappiness();
			float val = UnityEngine.Random.value;
			if (h == Happiness.SAD)
				// Weather Interaction
				yield return StartCoroutine(interaction_weather_sad());
			else if (h == Happiness.HAPPY)
				if (val < 0.25f)
					yield return StartCoroutine(interaction_1());
				else if (val < 0.5f)
					yield return StartCoroutine(interaction_2());
				else // Weather Interaction
					yield return StartCoroutine(interaction_weather_happy());
			else
				if (val < 0.50f)
					yield return StartCoroutine(interaction_1());
				else
					yield return StartCoroutine(interaction_2());
		}

		yield return new WaitForSeconds(0.2f);
				
		// End
				
		playerMovement.unsetCheckBusyWith(this.gameObject);
	}

	private Happiness getFollowerWeatherHappiness() {
		if (WeatherHandler.currentWeather == null) return Happiness.NORMAL;
		/*if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
		{
			if (PokemonUnity.Game.GameData.Trainer.party[followerIndex].Type1 == PokemonUnity.Types.WATER ||
				PokemonUnity.Game.GameData.Trainer.party[followerIndex].Type2 == PokemonUnity.Types.WATER)
			{
				return Hapiness.HAPPY;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.FIRE ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.FIRE)
			{
				return Hapiness.SAD;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.ROCK ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.ROCK)
			{
				return Hapiness.SAD;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.GROUND ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.GROUND)
			{
				return Hapiness.SAD;
			}
		}
		else if (WeatherHandler.currentWeather.type == Weather.WeatherType.Sand)
		{
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.GROUND ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.GROUND)
			{
				return Hapiness.HAPPY;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.FIRE ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.FIRE)
			{
				return Hapiness.SAD;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.ROCK ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.ROCK)
			{
				return Hapiness.SAD;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.STEEL ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.STEEL)
			{
				return Hapiness.SAD;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.POISON ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.POISON)
			{
				return Hapiness.SAD;
			}
			
			if (PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType1() ==
				PokemonData.Type.WATER ||
				PokemonDatabase.getPokemon(PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species).getType2() ==
				PokemonData.Type.WATER)
			{
				return Hapiness.SAD;
			}
		}*/
		
		return Happiness.NORMAL;
	}
	
	public IEnumerator interaction_weather_happy() {
		yield return StartCoroutine(jump());
		yield return StartCoroutine(jump());
		yield return StartCoroutine(PlayCryAndWait(PokemonUnity.Game.GameData.Trainer.party[partyIndex]));
		yield return new WaitForSeconds(0.2f);
		Dialog.DrawBlackFrame();
		switch (Language.getLang())
		{
			default:
				if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" is enjoying the rain."));
				}
				else
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" is enjoying the weather."));
				}
				
				break;
			case (Language.Country.FRANCAIS):
				if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" apprécie être sous la pluie."));
				}
				else
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" apprécie la météo."));
				}
				
				break;
		}
		//is enjoying walking with you
		while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
		{
			yield return null;
		}
		Dialog.UndrawDialogBox();
	}
	
	public IEnumerator interaction_weather_sad() {
		yield return new WaitForSeconds(0.2f);
		Dialog.DrawBlackFrame();
		switch (Language.getLang())
		{
			default:
				if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" seems to hate being soaked."));
				}
				else if (WeatherHandler.currentWeather.name == "Sandstorm")
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" gets sand in the eyes."));
				}
				else
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" seems to complain about the weather."));
				}
				
				break;
			case (Language.Country.FRANCAIS):
				if (WeatherHandler.currentWeather.name == "Rain")
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" semble détester être trempé."));
				}
				else if (WeatherHandler.currentWeather.type == Weather.WeatherType.Sand)
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" a du sable dans les yeux."));
				}
				else
				{
					yield return
						Dialog.StartCoroutine(Dialog.DrawText(
							PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
							" semble se plaindre de la météo."));
				}
				
				break;
		}
		//is enjoying walking with you
		while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
		{
			yield return null;
		}
		Dialog.UndrawDialogBox();
	}

	public IEnumerator interaction_1() {
		yield return StartCoroutine(jump());
		yield return StartCoroutine(jump());
		yield return StartCoroutine(PlayCryAndWait(PokemonUnity.Game.GameData.Trainer.party[partyIndex]));
		yield return new WaitForSeconds(0.2f);
		Dialog.DrawBlackFrame();
		switch (Language.getLang())
		{
			default:
				yield return
					Dialog.StartCoroutine(Dialog.DrawText(
						PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
						" seems very happy."));
				break;
			case (Language.Country.FRANCAIS):
				yield return
					Dialog.StartCoroutine(Dialog.DrawText(
						PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
						" semble très heureux."));
				break;
		}
		 //is enjoying walking with you
		while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
		{
			yield return null;
		}
		Dialog.UndrawDialogBox();
	}
	
	public IEnumerator interaction_2() {
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.5f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.5f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.5f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.5f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.25f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.25f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.25f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.1f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.1f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.1f);
		direction = (direction + 1) % 4;
		yield return new WaitForSeconds(0.1f);
		direction = (direction + 1) % 4;
		yield return StartCoroutine(jump());
		yield return StartCoroutine(PlayCryAndWait(PokemonUnity.Game.GameData.Trainer.party[partyIndex]));
		yield return new WaitForSeconds(0.2f);
	}
	
	public IEnumerator interaction_tired() {
		yield return StartCoroutine(PlayCryAndWait(PokemonUnity.Game.GameData.Trainer.party[partyIndex]));
		yield return new WaitForSeconds(0.2f);
		Dialog.DrawBlackFrame();
		switch (Language.getLang())
		{
			default:
				yield return
					Dialog.StartCoroutine(Dialog.DrawText(
						PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
						" is tired."));
				break;
			case (Language.Country.FRANCAIS):
				yield return
					Dialog.StartCoroutine(Dialog.DrawText(
						PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
						" est fatigué."));
				break;
		}
		//is enjoying walking with you
		while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
		{
			yield return null;
		}
		Dialog.UndrawDialogBox();
	}

	#endregion

	#region Other

	void adjustLightColorAndIntensity() {
		followerLight.color = LightColor;
		if (HasLight)
			followerLight.intensity = LightIntensity;
		else
			followerLight.intensity = 0;
	}

	void adjustScaleBasedOnCamera(Camera camera) {
		float scale;

		Vector3 camPosition = camera.transform.position - PlayerMovement.Singleton.GetCamOrigin();

		if (transform.position.z > camPosition.z) {
			scale = 0.0334f * (Math.Abs(transform.position.z - camPosition.z)) + 0.9f;
			if (transform.position.z > camPosition.z + 3)
				scale = 1;
		} else
			scale = 0.9f;

		//scale = 0.0334f * (transform.position.z - PlayerMovement.player.transform.position.z)+0.9f;

		Pawn.transform.localScale = new Vector3(scale, scale, scale);
	}

	public IEnumerator withdrawToBall() {
		GameObject ball = spriteRenderer.transform.parent.Find("pokeball").gameObject;
		StopCoroutine("animateSprite");
		CanMove = false;
		followerLight.enabled = false;
		spriteRenderer.sprite = null;
		spriteLightRenderer.sprite = null;
		spriteReflectionRenderer.sprite = null;
		spriteLightReflectionRenderer.sprite = null;
		ball.SetActive(true);
		SfxHandler.Play(WithdrawClip);
		float increment = 0f;
		float time = 0.4f;
		Vector3 lockedPosition = transform.position;
		while (increment < 1) {
			increment += (1 / time) * Time.deltaTime;
			if (increment > 1) {
				increment = 1;
			}
			transform.position = lockedPosition;
			yield return null;
		}
		pawnShadow.enabled = false;
		spriteRenderer.sprite = null;
		spriteLightRenderer.sprite = null;
		spriteReflectionRenderer.sprite = null;
		spriteLightReflectionRenderer.sprite = null;
		IsHidden = true;
		ball.SetActive(false);
		transform.position = playerMovement.transform.position;

		GlobalVariables.Singleton.isFollowerOut = false;
		//StartCoroutine("animateSprite");
	}

	public void Hide() {
		StopCoroutine("animateSprite");
		CanMove = false;
		followerLight.enabled = false;
		spriteRenderer.sprite = null;
		spriteLightRenderer.sprite = null;
		spriteReflectionRenderer.sprite = null;
		spriteLightReflectionRenderer.sprite = null;
		pawnShadow.enabled = false;
		spriteRenderer.sprite = null;
		spriteLightRenderer.sprite = null;
		spriteReflectionRenderer.sprite = null;
		spriteLightReflectionRenderer.sprite = null;
		IsHidden = true;
		transform.position = PlayerMovement.Singleton.transform.position;
	}

	public void changeFollower(int partyIndex) {
		if (followerLight == null) {
			followerLight = GetComponentInChildren<Light>();
		}
		this.partyIndex = partyIndex;
        //pokemonID = (int)PokemonUnity.Game.GameData.Trainer.party[followerIndex].Species;
        PokemonSO pokemon = (PokemonSO)playerMovement.Player.party[this.partyIndex];
		pokemonID = (int)pokemon.Species;
		//spriteSheet = SaveData.currentSave.PC.boxes[0][followerIndex].GetNewSprite(false);
		animator.Animations = pokemon.Animations;
		return; // FIXME: need to merge where pokemon data is being grabbed between the PokemonDatabase class and Flaks current methods
        HasLight = PokemonDatabase.getPokemon(pokemonID).hasLight();
        LightIntensity = PokemonDatabase.getPokemon(pokemonID).getLuminance();
        LightColor = PokemonDatabase.getPokemon(pokemonID).getLightColor();
        lightSheet = SaveData.currentSave.PC.boxes[0][this.partyIndex].GetNewSprite(true);

        if (lightSheet[0] == null) {
			spriteLightRenderer.sprite = null;
			spriteLightReflectionRenderer.sprite = null;
		}

		followerLight.color = LightColor;
		followerLight.intensity = LightIntensity;
	}

	public void reflect(bool setState) {
		//Debug.Log ("F Reflect");
		spriteReflectionRenderer.enabled = setState;
		spriteLightReflectionRenderer.enabled = setState;
	}


	public void hideFollower() {
		IsHidden = true;
		transform.position = playerMovement.transform.position;
	}

	public IEnumerator releaseFromBall() {
		if (PokemonUnity.Game.GameData.Trainer.party[0] != null) {
			if (PlayerMovement.Singleton.NpcFollower == null) {
				GameObject ball = spriteRenderer.transform.parent.Find("pokeball").gameObject;
				playerMovement = PlayerMovement.Singleton;

				CanMove = false;
				followerLight.enabled = false;
				spriteRenderer.sprite = null;
				spriteLightRenderer.sprite = null;
				spriteReflectionRenderer.sprite = null;
				spriteLightReflectionRenderer.sprite = null;
				pawnShadow.enabled = false;
				spriteRenderer.sprite = null;
				spriteLightRenderer.sprite = null;
				spriteReflectionRenderer.sprite = null;
				spriteLightReflectionRenderer.sprite = null;
				IsHidden = true;
				ball.SetActive(false);

				followerLight.color = LightColor;
				if (HasLight) {
					followerLight.intensity = LightIntensity;
				} else {
					followerLight.intensity = 0;
				}

				switch (playerMovement.Direction) {
					case 0:
						transform.Translate(Vector3.back);
						transform.position = new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y, playerMovement.transform.position.z - 1);
						break;
					case 1:
						transform.Translate(Vector3.left);
						transform.position = new Vector3(playerMovement.transform.position.x - 1, playerMovement.transform.position.y, playerMovement.transform.position.z);
						break;
					case 2:
						transform.Translate(Vector3.forward);
						transform.position = new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y, playerMovement.transform.position.z + 1);
						break;
					case 3:
						transform.Translate(Vector3.right);
						transform.position = new Vector3(playerMovement.transform.position.x + 1, playerMovement.transform.position.y, playerMovement.transform.position.z);
						break;
				}

				direction = playerMovement.Direction;

				pawnShadow.enabled = true;
				ball.SetActive(true);
				yield return new WaitForSeconds(0.4f);
				SfxHandler.Play(WithdrawClip);

				IsHidden = false;
				ball.SetActive(false);
				followerLight.enabled = true;
				changeFollower(partyIndex);
				StartCoroutine("animateSprite");
				CanMove = true;

				GlobalVariables.Singleton.isFollowerOut = true;
			} else {
				if (playerMovement.setCheckBusyWith(this.gameObject)) {
					Dialog.DrawBlackFrame();

					if (Language.getLang() == Language.Country.FRANCAIS) {
						Dialog.StartCoroutine(Dialog.DrawText("Vous ne pouvez pas appeler " +
															  PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
															  " pour le moment."));
					} else {
						Dialog.StartCoroutine(Dialog.DrawText("You can't release " +
															  PokemonUnity.Game.GameData.Trainer.party[partyIndex].Name +
															  " for now."));
					}

					while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back")) {
						yield return null;
					}
					Dialog.UndrawDialogBox();

					playerMovement.unsetCheckBusyWith(this.gameObject);
				}
			}
		}

		yield return null;
	}

	#endregion

}