using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BattlePokemonHandler : UnityEngine.MonoBehaviour
{
	//private Battle ActiveBattle { get { return StartupSceneHandler.PersistantPlayerData.; } }
	#region Unity's MonoBehavior Variables	
	#region Pokemon HUD
	public float Exp {
		//get { return expSlider.value; }
		set
		{
			//hpSlider.value = value;
			StopCoroutine("AnimateExpSlider");
			StartCoroutine("AnimateExpSlider", value);
		} }
	public float HP {
		//get { return hpSlider.value; }
		set
		{
			//hpSlider.value = value;
			StopCoroutine("AnimateSlider");
			StartCoroutine("AnimateSlider", value);
		} }
	public float TotalHP { get { return hpSlider.maxValue; } set { hpSlider.maxValue = value; maxHP.text = hpSlider.maxValue.ToString(); } }
	public bool? Gender
	{
		set
		{
			if (value.HasValue)
			{
				if (value.Value)
				{
					gender.text = "♀";
					gender.color = new Color32(255, 34, 34, 255);	// Red
				}
				else
				{
					gender.text = "♀";
					gender.color = new Color32(255, 34, 34, 255);	// Red
				}
			}
			else
				gender.text = string.Empty;
		}
	}
	public PokemonUnity.Move.Status Status
	{
		set
		{
			switch (value)
			{
				case PokemonUnity.Move.Status.Sleep:
				case PokemonUnity.Move.Status.Poison:
				case PokemonUnity.Move.Status.Paralysis:
				case PokemonUnity.Move.Status.Burn:
				case PokemonUnity.Move.Status.Frozen:
					StatusIcon.sprite = Resources.Load<Sprite>(string.Format("PCSprites/status{0}" + value.ToString()));
					break;
				case PokemonUnity.Move.Status.None:
				default:
					StatusIcon.sprite = Resources.Load<Sprite>("null");
					break;
			}
		}
	}
	public bool Item { set { gameObject.transform.Find("HeldItem").gameObject.SetActive(value); } }
	public bool Caught { set { gameObject.transform.Find("Caught").gameObject.SetActive(value); } }
	public Slider expSlider;                                    // Reference to the UI's experience bar.
	public Slider hpSlider;                                     // Reference to the UI's health bar.
	public Slider fadeSlider;									// Reference to the UI's 2nd health bar.
	public Image Fill, StatusIcon;
	public Text currentHP, maxHP, Name, Level, gender;
	public int BattleIndex = -1;

	/// <summary>
	/// Green
	/// </summary>
	//public Color hpzone0 = new Color32(112, 248, 168, 255);		// Green
	public Color hpzone0 = new Color32(32, 160, 0, 255);			// Darker Green
	/// <summary>
	/// Yellow
	/// </summary>
	public Color hpzone1 = new Color32(248, 224, 56, 255);			// Yellow
	/// <summary>
	/// Red
	/// </summary>
	public Color hpzone2 = new Color32(208, 40, 40, 255);           // Red
																	// <summary>
																	// Orange
																	// </summary>
																	//public Color fade = new Color32(255, 113, 0, 255);				// Orange
	#endregion
	#region Pokemon Battler
	//public PokemonUnity.Item.ItemCategory
	public Sprite Pokemon, Shadow, Pokeball;
	public AnimationClip BattlerAnim, PokeBallAnim;
	#endregion
	#endregion

	#region Unity Engine Runtime Only
	void Awake()
	{
		hpSlider.minValue = expSlider.minValue = 0;
		hpSlider.wholeNumbers = expSlider.wholeNumbers = true;
		//Adds a listener to the main slider and invokes a method when the value changes.
		hpSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });


		if (BattleIndex > -1 && BattleIndex < GameVariables.battle.battlers.Length) GameVariables.battle.battlers[BattleIndex].BattlerUI(this);
	}

    void OnEnable()
    {
		//Refresh/Resync Variable Data, Either here, or OnUpdate
    }

    void Start()
    {
	}

    void Update()
    {
	}
	#endregion

	/// <summary>
	/// Invoked when the value of the slider changes.
	/// </summary>
	public void ValueChangeCheck()
	{
		//Debug.Log(hpSlider.value);

		//if (hpSlider.value <= (hpSlider.maxValue / 4)) { Fill.color = hpzone2; }
		if (.3f > (hpSlider.normalizedValue)) { Fill.color = hpzone2; }
		//else if (hpSlider.value < (hpSlider.normalizedValue.CompareTo(0.5f))) //  / 2)) 
		else if (.75f > (hpSlider.normalizedValue)) //  / 2)) 
		{ 
			//Change color of hp bar
			Fill.color = hpzone1;
			//Change background image for health slider
		}
		else
			Fill.color = hpzone0;

		//each time the silder's value is changed, write to text displaying the hp
		currentHP.text = hpSlider.value.ToString(); //Set text under hp to match slider currentHealth
	}
	
	System.Collections.IEnumerator AnimateSlider(int amount) //Slider as input?
	{
		Debug.Log(amount);
		while (hpSlider.value != amount)
		{
			hpSlider.value = Mathf.Lerp(hpSlider.value + amount, hpSlider.value, 1f * Time.deltaTime);
			yield return null;
		}
		//yield return 
		new WaitForSeconds(1f);
		fadeSlider.value = Mathf.Lerp(hpSlider.value, fadeSlider.value, .5f * Time.deltaTime);
		//yield return null;
	}
	/// <summary>
	/// Needs to flash white and blue when animating, and if makes it to 100% needs to chime, and begin again from 0.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns></returns>
	System.Collections.IEnumerator AnimateExpSlider(int amount) //Slider as input?
	{
		Debug.Log(amount);
		while (hpSlider.value != amount)
		{
			hpSlider.value = Mathf.Lerp(hpSlider.value + amount, hpSlider.value, 1f * Time.deltaTime);
			yield return null;
		}
		//yield return 
		new WaitForSeconds(1f);
		fadeSlider.value = Mathf.Lerp(hpSlider.value, fadeSlider.value, .5f * Time.deltaTime);
		//yield return null;
	}
}

/*[ExecuteInEditMode]
public class BattlePartyHandler : UnityEngine.MonoBehaviour
{
	public Selectable slot;

	

	#region Unity Engine Runtime Only
	void Awake()
	{
		//slot.spriteState = new SpriteState().
	}

    void OnEnable()
    {
    }

    void Start()
    {
	}

    void Update()
    {
	}
	#endregion
}*/

[ExecuteInEditMode]
public class BattleMoveHandler : UnityEngine.MonoBehaviour
{
	#region MOVE BUTTON DETAILS
	public bool IsEnabled { get; set; }
	public GameObject Move { get; private set; }
	//private Image buttonMove { get; set; }
	public Text MoveName, MovePP, MoveMaxPP;
	public Image MoveType, MoveCover;
	#endregion
	

	#region Unity Engine Runtime Only
	void Awake()
	{
	}

    void OnEnable()
    {
    }

    void Start()
    {
	}

    void Update()
    {
	}
	#endregion
}