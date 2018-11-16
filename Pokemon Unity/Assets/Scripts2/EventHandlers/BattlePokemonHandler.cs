using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
class BattlePokemonHandler : UnityEngine.MonoBehaviour
{

	#region Unity's MonoBehavior Variables	
	#region Pokemon Hit Points and Experience Meters
	public float HP {
		get { return hpSlider.value; }
		set
		{
			//hpSlider.value = value;
			StopCoroutine("AnimateSlider");
			StartCoroutine("AnimateSlider", value);
		} }
	public float TotalHP { get { return hpSlider.maxValue; } set { hpSlider.maxValue = value; maxHP.text = hpSlider.maxValue.ToString(); } }
	public Slider expSlider;                                    // Reference to the UI's experience bar.
	public Slider hpSlider;                                     // Reference to the UI's health bar.
	public Slider fadeSlider;									// Reference to the UI's 2nd health bar.
	public Image Fill;
	public Text currentHP, maxHP;

	/// <summary>
	/// Green
	/// </summary>
	public Color hpzone0 = new Color32(112, 248, 168, 255);			// Green
	/// <summary>
	/// Yellow
	/// </summary>
	public Color hpzone1 = new Color32(248, 224, 56, 255);			// Yellow
	/// <summary>
	/// Red
	/// </summary>
	public Color hpzone2 = new Color32(208, 40, 40, 255);			// Red
	#endregion
	#endregion


	void Awake()
	{
		hpSlider.minValue = 0;
		hpSlider.wholeNumbers = true;
		//Adds a listener to the main slider and invokes a method when the value changes.
		hpSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
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
		new WaitForSeconds(.5f);
		float hp = Mathf.Lerp(hpSlider.value + amount, hpSlider.value, 1f * Time.deltaTime);
		yield return null;
	}
}
