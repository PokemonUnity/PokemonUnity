using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;
using PokemonUnity.Utility;

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// Same class can be used for both regular battles and safari battles, 
	/// but just need to call the initialize for the right one to load the correct attributes.
	/// Otherwise, you might just need to make an abstract factory for the constructor
	//ToDo: SafariDataBox has different Update/Refresh values when overriding...
	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class PokemonDataBox : SafariDataBox, IPokemonDataBox
	{
		public IBattler battler { get; private set; }
		//public int selected { get; set; }
		//public bool appearing { get; }
		public bool animatingHP { get; private set; }
		public bool animatingEXP { get; private set; }
		public int Exp { get { return @animatingEXP ? @currentexp : @explevel; } }
		public int HP { get { return @animatingHP ? @currenthp : @battler.HP; } }
		/// <summary>
		/// Reference to the UI's health bar.
		/// </summary>
		public UnityEngine.UI.Slider sliderHP;
		/// <summary>
		/// Reference to the UI's experience bar.
		/// </summary>
		public UnityEngine.UI.Slider sliderExp;
		public UnityEngine.UI.Image spriteStatus, spriteFillHP;
		public UnityEngine.UI.Text currentHP, maxHP, Name, level, gender;
		//private UnityEngine.UI.Image panelbg;
		//private UnityEngine.Sprite databox; //AnimatedBitmap
		//private UnityEngine.Sprite statuses;
		private int frame;
		private int explevel;
		private int starthp;
		private int currenthp;
		private int currentexp;
		private int endhp;
		private int endexp;
		private int expflash;
		private bool showexp;
		private bool showhp;
		private float spritebaseX;
		//private float spriteX;
		//private float spriteY;

		private void Awake()
		{
			panelbg = GetComponent<UnityEngine.UI.Image>();

			sliderHP.minValue = sliderExp.minValue = 0;
			sliderHP.wholeNumbers = sliderExp.wholeNumbers = true;
			//Adds a listener to the main slider and invokes a method when the value changes.
			sliderHP.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
		}

		private void Start()
		{

		}

		public IPokemonDataBox initialize(IBattler battler, bool doublebattle, IViewport viewport = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			base.initialize(viewport);
			this.battler = battler;
			@explevel = 0;
			@selected = 0;
			@frame = 0;
			@showhp = false;
			@showexp = false;
			@appearing = false;
			@animatingHP = false;
			@starthp = 0;
			@currenthp = 0;
			@endhp = 0;
			@expflash = 0;
			//if ((@battler.Index & 1) == 0)       // If player's Pokémon
			//{
			//	@spritebaseX = 34;
			//}
			//else
			//{
			//	@spritebaseX = 16;
			//}
			/*if (doublebattle)
			{
				switch (@battler.Index)
				{
					case 0:
						@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battlePlayerBoxD");
						@spriteX = PokeBattle_SceneConstants.PLAYERBOXD1_X;
						@spriteY = PokeBattle_SceneConstants.PLAYERBOXD1_Y;
						break;
					case 1:
						@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battleFoeBoxD");
						@spriteX = PokeBattle_SceneConstants.FOEBOXD1_X;
						@spriteY = PokeBattle_SceneConstants.FOEBOXD1_Y;
						break;
					case 2:
						@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battlePlayerBoxD");
						@spriteX = PokeBattle_SceneConstants.PLAYERBOXD2_X;
						@spriteY = PokeBattle_SceneConstants.PLAYERBOXD2_Y;
						break;
					case 3:
						@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battleFoeBoxD");
						@spriteX = PokeBattle_SceneConstants.FOEBOXD2_X;
						@spriteY = PokeBattle_SceneConstants.FOEBOXD2_Y;
						break;
				}
			}
			else
			{
				switch (@battler.Index)
				{
					case 0:
						@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battlePlayerBoxS");
						@spriteX = PokeBattle_SceneConstants.PLAYERBOX_X;
						@spriteY = PokeBattle_SceneConstants.PLAYERBOX_Y;
						@showhp = true;
						@showexp = true;
						currentHP.gameObject.SetActive(true);
						maxHP.gameObject.SetActive(true);
						break;
					case 1:
						@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battleFoeBoxS");
						@spriteX = PokeBattle_SceneConstants.FOEBOX_X;
						@spriteY = PokeBattle_SceneConstants.FOEBOX_Y;
						break;
				}
			}
			panelbg.sprite = databox;
			//@statuses = Resources.Load<UnityEngine.Sprite>(Game._INTL("Graphics/Pictures/battleStatuses")); //if image is localized, grab the current region
			@spriteStatus.sprite = Resources.Load<UnityEngine.Sprite>(Game._INTL("Graphics/Pictures/battleStatuses"));
			//@contents = new BitmapWrapper(@databox.width, @databox.height);
			//this.bitmap = @contents;*/
			this.visible = false;
			this.z = 50;
			refreshExpLevel();
			refresh();
			return this;
		}

		//public void Dispose()
		//{
		//	GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//	@statuses.dispose();
		//	@databox.dispose();
		//	@contents.dispose();
		//	base.Dispose();
		//}
				
		public void refreshExpLevel()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (!@battler.pokemon.IsNotNullOrNone())
			{
				@explevel = 0;
			}
			else
			{
				Monster.LevelingRate growthrate = @battler.pokemon.GrowthRate;
				int startexp = Monster.Data.Experience.GetStartExperience(growthrate, @battler.pokemon.Level);
				int endexp = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.NextLevel; //Monster.Data.Experience.GetStartExperience(growthrate, @battler.pokemon.Level + 1);
				if (startexp == endexp)
				{
					@explevel = 0;
				}
				else
				{
					@explevel = (int)(((@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Total - startexp) * PokeBattle_SceneConstants.EXPGAUGESIZE / (endexp - startexp));
					sliderExp.maxValue = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Current + (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.PointsNeeded;
					StopCoroutine("AnimateSliderExp"); //(AnimateSliderHP(currenthp));
					StartCoroutine(AnimateSliderExp((@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Current)); //sliderExp.value = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Current;
				}
			}
		}
				
		/// <summary>
		/// Assigns variables to store new value changes that will take effect during next frame updates
		/// </summary>
		/// <param name="oldhp"></param>
		/// <param name="newhp"></param>
		public void animateHP(int oldhp, int newhp)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			@starthp = oldhp;
			@currenthp = oldhp;
			@endhp = newhp;
			@animatingHP = true;
		}
				
		/// <summary>
		/// Assigns variables to store new value changes that will take effect during next frame updates
		/// </summary>
		/// <param name="oldhp"></param>
		/// <param name="newhp"></param>
		public void animateEXP(int oldexp, int newexp)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			@currentexp = oldexp;
			@endexp = newexp;
			@animatingEXP = true;
		}
		 
		/// <summary>
		/// Toggle active and animate sliding onto screen from side
		/// </summary>
		public override void appear()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			refreshExpLevel();
			refresh();
			this.visible = true;
			this.opacity = 255;
			if ((@battler.Index & 1) == 0)       // If player's Pokémon
			{
				this.x = @spriteX + 320;
			}
			else
			{
				this.x = @spriteX - 320;
			}
			this.y = @spriteY;
			@appearing = true;
		}
				
		public override void refresh()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			//this.bitmap.clear();
			if (!@battler.pokemon.IsNotNullOrNone()) return;
			//this.bitmap.blt(0,0,@databox.bitmap,new Rect(0,0,@databox.width,@databox.height));
			//IColor base_ = PokeBattle_SceneConstants.BOXTEXTBASECOLOR;
			//IColor shadow = PokeBattle_SceneConstants.BOXTEXTSHADOWCOLOR;
			string pokename = @battler.Name;
			//pbSetSystemFont(this.bitmap);
			//IList<ITextPosition> textpos = new List<ITextPosition>() {
			//	new TextPosition (pokename,@spritebaseX+8,6,false,base_,shadow)
			//};
			Name.text = pokename;
			//Set gender toggle on/off; chage color based on gender
			//float genderX = this.bitmap.text_size(pokename).width;
			//genderX += @spritebaseX + 14;
			//switch (@battler.displayGender)
			//{
			//	case 0: // Male
			//		textpos.Add(new TextPosition(Game._INTL("♂"), genderX, 6, false, new Color(48, 96, 216), shadow));
			//		break;
			//	case 1: // Female
			//		textpos.Add(new TextPosition(Game._INTL("♀"), genderX, 6, false, new Color(248, 88, 40), shadow));
			//		break;
			//}
			if (@battler.displayGender > 0)
			{
				if (@battler.displayGender == 1)
				{
					gender.text = "♂"; // Male
					gender.color = new Color32(255, 34, 34, 255);   // Red
				}
				else
				{
					gender.text = "♀"; // Female
					gender.color = new Color32(255, 34, 34, 255);   // Red
				}
			}
			else
				gender.text = string.Empty;
			//pbDrawTextPositions(this.bitmap,textpos);
			//pbSetSmallFont(this.bitmap);
			//textpos = new List<ITextPosition>() {
			//	new TextPosition (Game._INTL("Lv{1}",@battler.Level),@spritebaseX+202,8,true,base_,shadow)
			//};
			level.text = Game._INTL("{0}", @battler.Level);
			if (@showhp)
			{
				//string hpstring = string.Format("{1: 2d}/{2: 2d}", this.HP, @battler.TotalHP);
				//textpos.Add(new TextPosition(hpstring, @spritebaseX + 188, 48, true, base_, shadow));
				// Text changes automatically when slider value is set...
				StopCoroutine("AnimateSliderHP"); //(AnimateSliderHP(currenthp));
				StartCoroutine(AnimateSliderHP(currenthp)); //sliderHP.value = currenthp;
			}
			//pbDrawTextPositions(this.bitmap,textpos);
			//IList<ITextPosition> imagepos = new List<ITextPosition>();
			//ToDo: Uncomment below if UI is setup for generation features/gimmick
			//if (@battler.IsShiny)
			//{
			//	float shinyX = 206;
			//	if ((@battler.Index & 1) == 0) shinyX = -6; // If player's Pokémon
			//	imagepos.Add(new TextPosition("Graphics/Pictures/shiny.png", @spritebaseX + shinyX, 36, 0, 0, -1, -1));
			//}
			//if (@battler.isMega)
			//{
			//	imagepos.Add(new TextPosition("Graphics/Pictures/battleMegaEvoBox.png", @spritebaseX + 8, 34, 0, 0, -1, -1));
			//}
			//else if (@battler.isPrimal)
			//{
			//	if (@battler.pokemon.Species == Pokemons.KYOGRE)
			//	{
			//		imagepos.Add(new TextPosition("Graphics/Pictures/battlePrimalKyogreBox.png", @spritebaseX + 140, 4, 0, 0, -1, -1));
			//	}
			//	else if (@battler.pokemon.Species == Pokemons.GROUDON)
			//	{
			//		imagepos.Add(new TextPosition("Graphics/Pictures/battlePrimalGroudonBox.png", @spritebaseX + 140, 4, 0, 0, -1, -1));
			//	}
			//}
			if (@battler.owned && (@battler.Index & 1) == 1)
			{
				//imagepos.Add(new TextPosition("Graphics/Pictures/battleBoxOwned.png", @spritebaseX + 8, 36, 0, 0, -1, -1));
				gameObject.transform.Find("Caught").gameObject.SetActive(true);
			}
			//pbDrawImagePositions(this.bitmap,imagepos);
			//switch (@battler.Status)
			//{
			//	case PokemonUnity.Status.SLEEP:
			//	case PokemonUnity.Status.POISON:
			//	case PokemonUnity.Status.PARALYSIS:
			//	case PokemonUnity.Status.BURN:
			//	case PokemonUnity.Status.FROZEN:
			//		StatusIcon.sprite = Resources.Load<Sprite>(string.Format("PCSprites/status{0}" + @battler.Status.ToString()));
			//		break;
			//	case PokemonUnity.Status.NONE:
			//	default:
			//		StatusIcon.sprite = Resources.Load<Sprite>("null");
			//		break;
			//}
			if (@battler.Status > 0)
			{
				//Enable the sprite that represents this status on the pokemon's HUD panel
				//this.bitmap.blt(@spritebaseX + 24, 36, @statuses.bitmap,
				//   new Rect(0, (@battler.Status - 1) * 16, 44, 16));
				spriteStatus.sprite = Resources.Load<UnityEngine.Sprite>(string.Format("PCSprites/status{0}" + @battler.Status.ToString()));
			}
			else spriteStatus.sprite = Resources.Load<UnityEngine.Sprite>("null");
			int hpGaugeSize = (int)PokeBattle_SceneConstants.HPGAUGESIZE;
			int hpgauge = @battler.TotalHP == 0 ? 0 : (this.HP * hpGaugeSize / @battler.TotalHP);
			if (hpgauge == 0 && this.HP > 0) hpgauge = 2;
			int hpzone = 0;
			if (this.HP <= Math.Floor(@battler.TotalHP / 2f)) hpzone = 1;
			if (this.HP <= Math.Floor(@battler.TotalHP / 4f)) hpzone = 2;
			IColor[] hpcolors = new IColor[] {
				PokeBattle_SceneConstants.HPCOLORGREENDARK,
				PokeBattle_SceneConstants.HPCOLORGREEN,
				PokeBattle_SceneConstants.HPCOLORYELLOWDARK,
				PokeBattle_SceneConstants.HPCOLORYELLOW,
				PokeBattle_SceneConstants.HPCOLORREDDARK,
				PokeBattle_SceneConstants.HPCOLORRED
			};
			//  fill with black (shows what the HP used to be)
			float hpGaugeX = PokeBattle_SceneConstants.HPGAUGE_X;
			float hpGaugeY = PokeBattle_SceneConstants.HPGAUGE_Y;
			if (@animatingHP && this.HP > 0)
			{
				//value of hp bar is "@starthp * hpGaugeSize / @battler.TotalHP"; current hp divided by total multiply by length of sprite bg
				//this.bitmap.fill_rect(@spritebaseX + hpGaugeX, hpGaugeY,
				//   @starthp * hpGaugeSize / @battler.TotalHP, 6, new Color(0, 0, 0));
			}
			//  fill with HP color
			//this.bitmap.fill_rect(@spritebaseX + hpGaugeX, hpGaugeY, hpgauge, 2, hpcolors[hpzone * 2]);
			//this.bitmap.fill_rect(@spritebaseX + hpGaugeX, hpGaugeY + 2, hpgauge, 4, hpcolors[hpzone * 2 + 1]);
			StopCoroutine("AnimateSliderHP"); //(AnimateSliderHP(currenthp));
			StartCoroutine(AnimateSliderHP(this.HP)); //sliderHP.value = this.HP;
			if (@showexp)
			{
				//  fill with EXP color
				//float expGaugeX = PokeBattle_SceneConstants.EXPGAUGE_X;
				//float expGaugeY = PokeBattle_SceneConstants.EXPGAUGE_Y;
				//this.bitmap.fill_rect(@spritebaseX + expGaugeX, expGaugeY, this.Exp, 2,
				//   PokeBattle_SceneConstants.EXPCOLORSHADOW);
				//this.bitmap.fill_rect(@spritebaseX + expGaugeX, expGaugeY + 2, this.Exp, 2,
				//   PokeBattle_SceneConstants.EXPCOLORBASE); //Same X value, just 2 Y values lower
				StopCoroutine("AnimateSliderExp"); //(AnimateSliderHP(currenthp));
				StartCoroutine(AnimateSliderExp(this.Exp)); //sliderExp.value = this.Exp;
			}
		}
				
		public override void update()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			base.update();
			@frame += 1;
			if (@animatingHP)
			{
				if (@currenthp < @endhp)
				{
					@currenthp +=(int)Math.Max(1, Math.Floor(@battler.TotalHP / PokeBattle_SceneConstants.HPGAUGESIZE));
					if (@currenthp > @endhp) @currenthp = @endhp;
				}
				else if (@currenthp > @endhp)
				{
					@currenthp -=(int)Math.Max(1, Math.Floor(@battler.TotalHP / PokeBattle_SceneConstants.HPGAUGESIZE));
					if (@currenthp < @endhp) @currenthp = @endhp;
				}
				if (@currenthp == @endhp) @animatingHP = false;
				refresh();
			}
			//ToDo: New coroutine to flash white and blue when leveling-up, chime at 100%, and begin again from 0.
			if (@animatingEXP)
			{
				if (!@showexp)
				{
					@currentexp = @endexp;
				}
				else if (@currentexp < @endexp)			// Gaining Exp
				{
					if (@endexp >= PokeBattle_SceneConstants.EXPGAUGESIZE ||
					   @endexp - @currentexp >= PokeBattle_SceneConstants.EXPGAUGESIZE / 4)
					{
						@currentexp += 4;
					}
					else
					{
						@currentexp += 2;
					}
					if (@currentexp > @endexp) @currentexp = @endexp;
				}
				else if (@currentexp > @endexp)			// Losing Exp
				{
					if (@endexp == 0 ||
					   @currentexp - @endexp >= PokeBattle_SceneConstants.EXPGAUGESIZE / 4)
					{
						@currentexp -= 4;
					}
					else if (@currentexp > @endexp)
					{
						@currentexp -= 2;
					}
					if (@currentexp < @endexp) @currentexp = @endexp;
				}
				refresh();
				if (@currentexp == @endexp)
				{
					if (@currentexp == PokeBattle_SceneConstants.EXPGAUGESIZE)
					{
						if (@expflash == 0)
						{
							//(AudioHandler as IGameAudioPlay).pbSEPlay("expfull");
							//this.flash(new Color(64, 200, 248), 8);
							@expflash = 8;
						}
						else
						{
							@expflash -= 1;
							if (@expflash == 0)
							{
								@animatingEXP = false;
								refreshExpLevel();
							}
						}
					}
					else
					{
						@animatingEXP = false;
					}
				}
			}
			if (@appearing)
			{
				if ((@battler.Index & 1) == 0)       // If player's Pokémon
				{
					this.x -= 12;
					if (this.x < @spriteX) this.x = @spriteX;
					if (this.x <= @spriteX) @appearing = false;
				}
				else
				{
					this.x += 12;
					if (this.x > @spriteX) this.x = @spriteX;
					if (this.x >= @spriteX) @appearing = false;
				}
				this.y = @spriteY;
				return;
			}
			this.x = @spriteX;
			this.y = @spriteY;
			//  Data box bobbing while Pokémon is selected
			if (((int)Math.Floor(@frame / 10f) & 1) == 1 && @selected == 1)			// Choosing commands for this Pokémon
			{
				this.y = @spriteY + 2;
			}
			else if (((int)Math.Floor(@frame / 10f) & 1) == 1 && @selected == 2)    // When targeted or damaged
			{
				this.y = @spriteY + 2;
			}
		}

		/// <summary>
		/// Invoked when the value of the slider changes.
		/// </summary>
		public void ValueChangeCheck()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			//if (sliderHP.value <= (sliderHP.maxValue / 4)) { Fill.color = hpzone2; }
			if (.3f > sliderHP.normalizedValue) { spriteFillHP.color = (SeriColor)PokeBattle_SceneConstants.HPCOLORRED; }
			//else if (sliderHP.value < (sliderHP.normalizedValue.CompareTo(0.5f))) //  / 2)) 
			else if (.75f > (sliderHP.normalizedValue)) //  / 2)) 
			{
				//Change color of hp bar
				spriteFillHP.color = (SeriColor)PokeBattle_SceneConstants.HPCOLORYELLOW;
				//Change background image for health slider
			}
			else
				spriteFillHP.color = (SeriColor)PokeBattle_SceneConstants.HPCOLORGREEN;
			//each time the silder's value is changed, write to text displaying the hp
			currentHP.text = sliderHP.value.ToString(); //Set text under hp to match slider currentHealth
		}

		private System.Collections.IEnumerator AnimateSliderHP(int amount) //Slider as input?
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Debug.Log(amount);
			while (sliderHP.value != amount)
			{
				sliderHP.value = Mathf.Lerp(sliderHP.value + amount, sliderHP.value, 1f * Time.deltaTime);
				yield return null;
			}
			//new WaitForSeconds(1f);
			//fadeSlider.value = Mathf.Lerp(sliderHP.value, fadeSlider.value, .5f * Time.deltaTime);
			//yield return null;
		}
		/// <summary>
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		private System.Collections.IEnumerator AnimateSliderExp(int amount) //Slider as input?
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Debug.Log(amount);
			while (sliderExp.value != amount)
			{
				sliderExp.value = Mathf.Lerp(sliderExp.value + amount, sliderExp.value, 1f * Time.deltaTime);
				yield return null;
			}
			//new WaitForSeconds(1f);
			//fadeSlider.value = Mathf.Lerp(sliderHP.value, fadeSlider.value, .5f * Time.deltaTime);
			//yield return null;
		}
	}

	/// <summary>
	/// </summary>
	public class SafariDataBox : SpriteWrapper, ISafariDataBox
	{
		public int selected { get; set; }
		public bool appearing { get; protected set; }
		public UnityEngine.UI.Image panelbg;
		public UnityEngine.Sprite databox; //AnimatedBitmap
		private IBattle battle;
		protected float spriteX;
		protected float spriteY;

		private void Awake()
		{
			panelbg = GetComponent<UnityEngine.UI.Image>();
		}

		private void Start()
		{

		}

		public ISafariDataBox initialize(IBattle battle, IViewport viewport = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			base.initialize(viewport);
			@selected = 0;
			this.battle = battle;
			//@databox = new AnimatedBitmap("Graphics/Pictures/battlePlayerSafari");
			//@databox = Resources.Load<UnityEngine.Sprite>("Graphics/Pictures/battlePlayerSafari");
			//panelbg.sprite = databox;
			@spriteX = PokeBattle_SceneConstants.SAFARIBOX_X;
			@spriteY = PokeBattle_SceneConstants.SAFARIBOX_Y;
			@appearing = false;
			//@contents = new BitmapWrapper(@databox.width, @databox.height);
			//this.bitmap = @contents;
			this.visible = false;
			this.z = 50;
			refresh();
			return this;
		}
		 
		/// <summary>
		/// Toggle active and animate sliding onto screen from side
		/// </summary>
		public override void appear()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			refresh();
			this.visible = true;
			this.opacity = 255;
			this.x = @spriteX + 240;
			this.y = @spriteY;
			@appearing = true;
		}
				
		public virtual void refresh()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			//this.bitmap.clear();
			//this.bitmap.blt(0, 0, @databox.bitmap, new Rect(0, 0, @databox.width, @databox.height));
			//pbSetSystemFont(this.bitmap);
			IList<ITextPosition> textpos = new List<ITextPosition>();
			IColor base_ = PokeBattle_SceneConstants.BOXTEXTBASECOLOR;
			IColor shadow = PokeBattle_SceneConstants.BOXTEXTSHADOWCOLOR;
			textpos.Add(new TextPosition(Game._INTL("Safari Balls"), 30, 8, false, base_, shadow));
			textpos.Add(new TextPosition(Game._INTL("Left: {1}", (@battle as ISafariZone).ballCount), 30, 38, false, base_, shadow));
			//pbDrawTextPositions(this.bitmap, textpos);
		}
				
		public override void update()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			base.update();
			if (@appearing)
			{
				this.x -= 12;
				if (this.x < @spriteX) this.x = @spriteX;
				if (this.x <= @spriteX) @appearing = false;
				this.y = @spriteY;
				return;
			}
			this.x = @spriteX;
			this.y = @spriteY;
		}
	}
}