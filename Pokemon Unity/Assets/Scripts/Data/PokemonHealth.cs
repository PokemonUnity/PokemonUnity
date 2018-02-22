#region Dll Import
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using System.Data;
//using System.Data.SqlClient;
#endregion

public class PokemonHealth : MonoBehaviour
{
	//public static PokemonHealth instance;
	
	public int maxHealth = 100;                                     // The max amount of health the pokemon can gain.
	private int currentHealth;                                      // The current health the pokemon has.
	public Text displayCurrentHealth;                               // The visual health for pokemon HUD.
	public Text displayMaxHealth;                                   // The visual health for pokemon HUD.
	public Slider healthSlider;                                     // Reference to the UI's health bar.
	//public Slider fadeHealthSlider;                               // Reference to the UI's 2nd health bar.
	//public Image damageImage;                                     // Reference to an image to flash on the screen on being hurt.
	//public AudioClip deathClip;                                   // The audio clip to play when the pokemon dies.
	//public float flashSpeed = 5f;                                 // The speed the damageImage will fade at.
	//public Color flashColour = new Color(1f, 0f, 0f, 0.1f);       // The colour the damageImage is set to, to flash.
	
	public Image Fill;
	//public int StartValue; //animate current hp between startvalue and endvalue
	//public int EndValue;
	public Color hpzone0 = new Color32(112, 248, 168, 1);		// Green
	public Color hpzone1 = new Color32(248, 224, 56, 1);		// Yellow
	public Color hpzone2 = new Color32(208, 40, 40, 1);			// Red
	
	public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChange;
	
	
	//Animator anim;                                              // Reference to the Animator component.
	//AudioSource pokemonAudio;                                    // Reference to the AudioSource component.
	//PlayerMovement pokemonMovement;                              // Reference to the pokemon's movement.
	//PlayerShooting pokemonShooting;                              // Reference to the PlayerShooting script.
	bool isDead;                                                // Whether the pokemon is dead.
	bool damaged;                                               // True when the pokemon gets damaged.
	
	
	void Awake ()
	{
		//instance = this;
		
		// Setting up the references.
//		anim = GetComponent <Animator> ();
//		pokemonAudio = GetComponent <AudioSource> ();
//		pokemonMovement = GetComponent <PlayerMovement> ();
//		pokemonShooting = GetComponentInChildren <PlayerShooting> ();
//
		//Adds a listener to the main slider and invokes a method when the value changes.
		healthSlider.onValueChanged.AddListener (delegate {ValueChangeCheck();});
        OnVariableChange += ValueChangeCheck;//VariableChangeHandler;

		// Set the initial health of the pokemon.
		currentHealth = maxHealth;
		
		//Fill = gameObject.GetComponentInChildren<Image>();
		Fill = healthSlider.GetComponentInChildren<Image>();
	}
	
	
	void Update ()
	{
		/*// If the pokemon has just been damaged...
		if(damaged)
		{
			// ... set the colour of the damageImage to the flash colour.
			damageImage.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		
		// Reset the damaged flag.
		damaged = false;*/		
        if (displayCurrentHealth.text != currentHealth.ToString() && OnVariableChange != null)
     {
         displayCurrentHealth.text = currentHealth.ToString();
         OnVariableChange();
      }
        if (displayMaxHealth.text != maxHealth.ToString() && OnVariableChange != null)
     {
         displayMaxHealth.text = maxHealth.ToString();
         OnVariableChange();
      }
		
	}
	
	
	public void TakeDamage (int amount)
	{
		
		/*tothp=@pokemon.totalhp
          textpos.push([_ISPRINTF("{1: 3d}/{2: 3d}",@pokemon.hp,tothp),
             @hpX,@hpY,1,base,shadow])
          barbg=(@pokemon.hp<=0) ? @hpbarfnt : @hpbar
          barbg=(self.preselected || (self.selected && @switching)) ? @hpbarswap : barbg
          self.bitmap.blt(@hpbarX,@hpbarY,barbg.bitmap,Rect.new(0,0,@hpbar.width,@hpbar.height))
          hpgauge=@pokemon.totalhp==0 ? 0 : (self.hp*96/@pokemon.totalhp)
          //hpgauge=1 if hpgauge==0 && self.hp>0
          hpzone=0
          hpzone=1 if self.hp<=(@pokemon.totalhp/2).floor
          hpzone=2 if self.hp<=(@pokemon.totalhp/4).floor*/
          
		
		
		// Set the damaged flag so the screen will flash.
		//damaged = true;
		
		// Reduce the current health by the damage amount.
		currentHealth -= amount;
		
		// Set the health bar's value to the current health.
		healthSlider.value = currentHealth; //int.Lerp (currentHealth + amount, currentHealth, .5f * Time.deltaTime);
		//fadeHealthSlider.value = currentHealth; //2nd hp bar, with a 5 sec delay

		
		// Play the hurt sound effect.
		//pokemonAudio.Play ();
		
		// If the pokemon has lost all it's health and the death flag hasn't been set yet...
		if(currentHealth <= 0 && !isDead)
		{
			// ... it should die.
			//Death();
		}
	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()//int newValue
	{
		Debug.Log ("value change:"+healthSlider.value);
		//GoodSlider(this.GetComponent<Slider>().value);
		//images =     gameObject.GetComponentsInChildren<Image>();

		//each time the silder's value is changed, write to text displaying the hp
		
		Fill.color = hpzone0;
		if (healthSlider.value <= (healthSlider.normalizedValue.CompareTo(0.5f))){ //  / 2)) {
			//Change color of hp bar
				Fill.color = hpzone1;
			//Change background image for health slider
		}
		if (healthSlider.value<=(maxHealth/4)){Fill.color = hpzone2;}

		//Text = healthSlider.value; //Set text under hp to match slider currentHealth
	}
	
	public void HPChangeValue(Slider slider){ //this
		Debug.Log("slider change:"+slider.value);
		//GoodSlider(this.GetComponent<Slider>().value);
		//images =     gameObject.GetComponentsInChildren<Image>();
		Fill.color = hpzone0;
		if (slider.value <= (slider.normalizedValue.CompareTo(0.5f))){ // (maxHealth/2)) {
			//Change color of hp bar
            Fill.color = hpzone1;
			//Change background image for health slider
		}
		if (slider.value <= (slider.normalizedValue.CompareTo(0.25f)))// (maxHealth/4))
        { Fill.color = hpzone2;}
	}
	
public void changeColor(){}

}