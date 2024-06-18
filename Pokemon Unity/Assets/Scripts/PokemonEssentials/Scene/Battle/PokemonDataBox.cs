using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Represents the UI panel for the pokemons health and experience display
	/// </summary>
	/// <remarks>
	/// Different layouts for whether the pokemon is friend, foe, and if double or single battle
	/// </remarks>
	[RequireComponent(typeof(global::UnityEngine.UI.Image))]
	[ExecuteInEditMode]
	public class PokemonDataBox : SafariDataBox, IPokemonDataBox, IGameObject
	{
		public IBattlerIE battler { get; protected set; }
		IBattler IPokemonDataBox.battler { get { return this.battler; } }
		//public int selected { get; set; }
		//public bool appearing { get; }
		public bool animatingHP { get; protected set; }
		public bool animatingEXP { get; protected set; }
		public int Exp { get { return @animatingEXP ? @currentexp : @explevel; } }
		public int HP { get { return @animatingHP ? @currenthp : @battler.HP; } }

		#region Unity Inspector Variables
		[Header("Battle Display")]
		/// <summary>
		/// Reference to the UI's health bar.
		/// </summary>
		public global::UnityEngine.UI.Slider sliderHP;
		/// <summary>
		/// Reference to the UI's experience bar.
		/// </summary>
		public global::UnityEngine.UI.Slider sliderExp;
		public global::UnityEngine.UI.Image spriteItem, spriteStatus, spriteCaught, spriteFillHP, spriteFillExp, gender, shiny, primal, mega;
		//public global::UnityEngine.UI.Text currentHP, slash, maxHP, Name, level, gender;
		public TMPro.TextMeshProUGUI Name, HPValue, level;
		//protected UnityEngine.Color colorFillExp;
		//protected UnityEngine.UI.Image panelbg;
		//protected UnityEngine.Sprite databox; //AnimatedBitmap
		//protected UnityEngine.Sprite statuses;
		/// <summary>
		/// Only show exp for player's pokemon (not ally, or enemy)
		/// </summary>
		public bool showexp;
		/// <summary>
		/// Only show hp for player's pokemon (not ally, or enemy)
		/// </summary>
		public bool showhp;

		[Header("Debug View")]
		public int explevel;
		public int starthp;
		public int currenthp;
		public int currentexp;
		public int endhp;
		public int endexp;
		public int expflash;
		protected int frame;
		protected float spritebaseX;
		//protected float spriteX;
		//protected float spriteY;
		#endregion

		private void Awake()
		{
			panelbg = GetComponent<global::UnityEngine.UI.Image>();
			//databox = GetComponent<global::UnityEngine.Sprite>();

			//colorFillExp = spriteFillExp.color;
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
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			base.initialize(viewport);
			this.battler = battler as IBattlerIE;
			Core.Logger?.Log($"[PokemonDataBox] battler : #{battler.Index} - {battler.Name}");
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
			// Sprite should already be preset inside unity engine, but can be assigned here.
			if (doublebattle)
			{
				switch (@battler.Index)
				{
					case 0:
						//if playing or watching an npc?
						//@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battlePlayerBoxD");
						//@spriteX = PokeBattle_SceneConstants.PLAYERBOXD1_X;
						//@spriteY = PokeBattle_SceneConstants.PLAYERBOXD1_Y;
						break;
					case 1:
						//@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battleFoeBoxD");
						//@spriteX = PokeBattle_SceneConstants.FOEBOXD1_X;
						//@spriteY = PokeBattle_SceneConstants.FOEBOXD1_Y;
						break;
					case 2:
						//if player or ally?...
						//@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battlePlayerBoxD");
						//@spriteX = PokeBattle_SceneConstants.PLAYERBOXD2_X;
						//@spriteY = PokeBattle_SceneConstants.PLAYERBOXD2_Y;
						break;
					case 3:
						//@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battleFoeBoxD");
						//@spriteX = PokeBattle_SceneConstants.FOEBOXD2_X;
						//@spriteY = PokeBattle_SceneConstants.FOEBOXD2_Y;
						break;
				}
			}
			else //Single Battle
			{
				//if player...
				if (@battler.Index == 0) //switch(@battler.Index)
				{
					//case 0:
					//@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battlePlayerBoxS");
					//@spriteX = PokeBattle_SceneConstants.PLAYERBOX_X;
					//@spriteY = PokeBattle_SceneConstants.PLAYERBOX_Y;
					@showhp = true;
					@showexp = true;
					//break;
				}
				else
				{ //if (@battler.Index == 1) { //case 1:
				  //@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battleFoeBoxS");
				  //@spriteX = PokeBattle_SceneConstants.FOEBOX_X;
				  //@spriteY = PokeBattle_SceneConstants.FOEBOX_Y;
				  //break;
				}
			}
			//maxHP.gameObject.SetActive(@showhp);
			//currentHP.gameObject.SetActive(@showhp);
			HPValue.gameObject.SetActive(@showhp);
			/*panelbg.sprite = databox;
			//@statuses = Resources.Load<global::UnityEngine.Sprite>(Game._INTL("Graphics/Pictures/battleStatuses")); //if image is localized, grab the current region
			@spriteStatus.sprite = Resources.Load<global::UnityEngine.Sprite>(Game._INTL("Graphics/Pictures/battleStatuses"));
			//@contents = new BitmapWrapper(@databox.width, @databox.height);
			//this.bitmap = @contents;
			this.visible = false;
			this.z = 50;*/
			mega?.gameObject.SetActive(false); //it's one image, just toggle on/off
			//primal.sprite = Resources.Load<global::UnityEngine.Sprite>("null");
			primal?.gameObject.SetActive(false);
			shiny?.gameObject.SetActive(battler.IsShiny);
			spriteCaught?.gameObject.SetActive(false);
			spriteItem?.gameObject.SetActive(false);
			refreshExpLevel();
			refresh();
			return this;
		}

		public void refreshExpLevel()
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (!@battler.pokemon.IsNotNullOrNone())
			{
				@explevel = 0;
			}
			else
			{
				//Monster.LevelingRate growthrate = @battler.pokemon.GrowthRate;
				int startexp = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.ExperienceNeeded(battler.Level); //Monster.Data.Experience.GetStartExperience(growthrate, @battler.pokemon.Level);
				int endexp = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.NextLevel; //Monster.Data.Experience.GetStartExperience(growthrate, @battler.pokemon.Level + 1);
				if (startexp == endexp)
				{
					@explevel = 0;
				}
				else
				{
					//@explevel = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Current; //(int)(((@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Total - startexp) * PokeBattle_SceneConstants.EXPGAUGESIZE / (endexp - startexp));
					//sliderExp.maxValue = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Current + (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.PointsNeeded;
					@explevel = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.Total;
					sliderExp.minValue = startexp;
					sliderExp.maxValue = endexp;
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
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			@starthp = oldhp;
			@currenthp = newhp; //@endhp = newhp;
			@animatingHP = true;
			StopCoroutine("AnimateSliderHP"); //(AnimateSliderExp(@explevel));
			StartCoroutine(AnimateSliderHP(HP)); //sliderExp.value = @explevel;
		}

		/// <summary>
		/// Assigns variables to store new value changes that will take effect during next frame updates
		/// </summary>
		/// <param name="oldhp"></param>
		/// <param name="newhp"></param>
		public void animateEXP(int oldexp, int newexp)
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			@currentexp = newexp;
			@explevel = oldexp; //@endexp = newexp;
			@animatingEXP = true;
			StopCoroutine("AnimateSliderExp"); //(AnimateSliderExp(@explevel));
			StartCoroutine(AnimateSliderExp(Exp)); //sliderExp.value = @explevel;
		}

		/// <summary>
		/// Toggle active and animate sliding onto screen from side
		/// </summary>
		public override void appear()
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			refreshExpLevel();
			refresh();
			this.visible = true;
			this.opacity = 255;
			if ((@battler.Index & 1) == 0)		// If player's Pokémon
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
			//Core.Logger?.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//this.bitmap.clear();
			if (battler == null || @battler.pokemon == null)
			{
				visible = false; //If pokemon is none, clear and reset the HUD
				return;
			}
			else visible = true;
			//this.bitmap.blt(0,0,@databox.bitmap,new Rect(0,0,@databox.width,@databox.height));
			//IColor base_ = PokeBattle_SceneConstants.BOXTEXTBASECOLOR;
			//IColor shadow = PokeBattle_SceneConstants.BOXTEXTSHADOWCOLOR;
			string pokename = @battler.Name;
			//SetSystemFont(this.bitmap);
			//IList<ITextPosition> textpos = new List<ITextPosition>() {
			//	new TextPosition (pokename,@spritebaseX+8,6,false,base_,shadow)
			//};
			//Name.text = pokename;
			Name.SetText(pokename);
			//Set gender toggle on/off; change color based on gender
			//float genderX = this.bitmap.text_size(pokename).width;
			//genderX += @spritebaseX + 14;
			if (@battler.displayGender > -1) //switch (@battler.displayGender)
			{
				gender.gameObject.SetActive(true);
				if (@battler.displayGender == 1) //	case 0: // Male
				{
					//textpos.Add(new TextPosition(Game._INTL("♂"), genderX, 6, false, new Color(48, 96, 216), shadow));
					//gender.sprite = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/genderMale.png");
					gender.sprite = GameManager.current.FileTest.genderMale; // Male
					//break;
				}
				else //	case 1: // Female
				{
					//textpos.Add(new TextPosition(Game._INTL("♀"), genderX, 6, false, new Color(248, 88, 40), shadow));
					//gender.sprite = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/genderFemale.png");
					gender.sprite = GameManager.current.FileTest.genderFemale; // Female
					//break;
				}
			}
			else
			{
				gender.gameObject.SetActive(false);
				//gender.text = string.Empty;
			}
			//DrawTextPositions(this.bitmap,textpos);
			//SetSmallFont(this.bitmap);
			//textpos = new List<ITextPosition>() {
			//	new TextPosition (Game._INTL("Lv{1}",@battler.Level),@spritebaseX+202,8,true,base_,shadow)
			//};
			//level.text = Game._INTL("{0}", @battler.Level);
			level.SetText($"Lv. {@battler.Level}");
			Core.Logger?.Log(string.Format("Pokemon #{0} HP: `{1}/{2}`", battler.Index, this.HP, @battler.TotalHP));
			if (@showhp)
			{
				HPValue?.gameObject.SetActive(true); //Should already be preset in unity engine
				//string hpstring = string.Format("{1: 2d}/{2: 2d}", this.HP, @battler.TotalHP);
				//textpos.Add(new TextPosition(hpstring, @spritebaseX + 188, 48, true, base_, shadow));
				//maxHP.text = sliderHP.maxValue.ToString(); //Set text under hp to match slider maxHealth
				//currentHP.text = sliderHP.value.ToString(); //Set text under hp to match slider currentHealth
				HPValue.SetText($"{this.HP}/{@battler.TotalHP}"); //($"{sliderHP.value}/{sliderHP.maxValue}");
			}
			else HPValue?.gameObject.SetActive(false); //Just in case, if hp is not supposed to be seen, hide it
			//DrawTextPositions(this.bitmap,textpos);
			//IList<ITextPosition> imagepos = new List<ITextPosition>();
			//ToDo: Uncomment below if UI is setup for generation features/gimmick
			//if (@battler.IsShiny)
			//{
			//	//float shinyX = 206;
			//	//if ((@battler.Index & 1) == 0) shinyX = -6; // If player's Pokémon
			//	//imagepos.Add(new TextPosition("Graphics/Pictures/shiny.png", @spritebaseX + shinyX, 36, 0, 0, -1, -1));
			//}
			shiny?.gameObject.SetActive(battler.IsShiny);
			//if (@battler.isMega)
			//{
			//	imagepos.Add(new TextPosition("Graphics/Pictures/battleMegaEvoBox.png", @spritebaseX + 8, 34, 0, 0, -1, -1));
			//}
			mega?.gameObject.SetActive(battler.isMega);
			//else if (@battler.isPrimal)
			//{
			if (@battler.pokemon.Species == Pokemons.KYOGRE)
			{
				//imagepos.Add(new TextPosition("Graphics/Pictures/battlePrimalKyogreBox.png", @spritebaseX + 140, 4, 0, 0, -1, -1));
				//primal.sprite = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battlePrimalKyogreBox.png");
				primal.sprite = GameManager.current.FileTest.battlePrimalKyogre;
			}
			else if (@battler.pokemon.Species == Pokemons.GROUDON)
			{
				//imagepos.Add(new TextPosition("Graphics/Pictures/battlePrimalGroudonBox.png", @spritebaseX + 140, 4, 0, 0, -1, -1));
				//primal.sprite = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battlePrimalGroudonBox.png");
				primal.sprite = GameManager.current.FileTest.battlePrimalGroudon;
			}
			//}
			primal?.gameObject.SetActive(battler.isPrimal);
			if ((@battler.Index & 1) == 0) //if pokemon is on player side
			{
				spriteItem?.gameObject.SetActive(battler.Item != Inventory.Items.NONE);
			}
			else //if pokemon is on opponent side
			{
				if (@battler.owned)
				{
					//imagepos.Add(new TextPosition("Graphics/Pictures/battleBoxOwned.png", @spritebaseX + 8, 36, 0, 0, -1, -1));
					spriteCaught?.gameObject.SetActive(true); //gameObject.transform.Find("Caught").gameObject.SetActive(true);
				}
			}
			//DrawImagePositions(this.bitmap,imagepos);
			//switch (@battler.Status)
			//{
			//	case PokemonUnity.Status.SLEEP:
			//	case PokemonUnity.Status.POISON:
			//	case PokemonUnity.Status.PARALYSIS:
			//	case PokemonUnity.Status.BURN:
			//	case PokemonUnity.Status.FROZEN:
			//		StatusIcon.sprite = Resources.Load<Sprite>(Game._INTL("PCSprites/status{0}", @battler.Status.ToString()));
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
				spriteStatus.sprite = Resources.Load<global::UnityEngine.Sprite>(Game._INTL("PCSprites/status{0}", @battler.Status.ToString()));
			}
			else spriteStatus.sprite = Resources.Load<global::UnityEngine.Sprite>("null");
			//int hpGaugeSize = (int)PokeBattle_SceneConstants.HPGAUGESIZE;
			//int hpgauge = @battler.TotalHP == 0 ? 0 : (this.HP * hpGaugeSize / @battler.TotalHP);
			//if (hpgauge == 0 && this.HP > 0) hpgauge = 2;
			//int hpzone = 0;
			//if (this.HP <= Math.Floor(@battler.TotalHP / 2f)) hpzone = 1;
			//if (this.HP <= Math.Floor(@battler.TotalHP / 4f)) hpzone = 2;
			IColor[] hpcolors = new IColor[] {
				PokeBattle_SceneConstants.HPCOLORGREENDARK,
				PokeBattle_SceneConstants.HPCOLORGREEN,
				PokeBattle_SceneConstants.HPCOLORYELLOWDARK,
				PokeBattle_SceneConstants.HPCOLORYELLOW,
				PokeBattle_SceneConstants.HPCOLORREDDARK,
				PokeBattle_SceneConstants.HPCOLORRED
			};
			//  fill with black (shows what the HP used to be)
			//float hpGaugeX = PokeBattle_SceneConstants.HPGAUGE_X;
			//float hpGaugeY = PokeBattle_SceneConstants.HPGAUGE_Y;
			if (@animatingHP && this.HP > 0)
			{
				//value of hp bar is "@starthp * hpGaugeSize / @battler.TotalHP"; current hp divided by total multiply by length of sprite bg
				//this.bitmap.fill_rect(@spritebaseX + hpGaugeX, hpGaugeY,
				//   @starthp * hpGaugeSize / @battler.TotalHP, 6, new Color(0, 0, 0));
			}
			//  fill with HP color
			//this.bitmap.fill_rect(@spritebaseX + hpGaugeX, hpGaugeY, hpgauge, 2, hpcolors[hpzone * 2]);
			//this.bitmap.fill_rect(@spritebaseX + hpGaugeX, hpGaugeY + 2, hpgauge, 4, hpcolors[hpzone * 2 + 1]);
			// Text changes automatically when slider value is set...
			sliderHP.maxValue = @battler.TotalHP;
			sliderHP.value = this.HP;
			//StopCoroutine("AnimateSliderHP"); //(AnimateSliderHP(this.HP));
			//StartCoroutine(AnimateSliderHP(this.HP)); //sliderHP.value = this.HP;
			if (@showexp)
			{
				sliderExp?.gameObject.SetActive(true); //Should already be preset in unity engine
				//  fill with EXP color
				//float expGaugeX = PokeBattle_SceneConstants.EXPGAUGE_X;
				//float expGaugeY = PokeBattle_SceneConstants.EXPGAUGE_Y;
				//this.bitmap.fill_rect(@spritebaseX + expGaugeX, expGaugeY, this.Exp, 2,
				//   PokeBattle_SceneConstants.EXPCOLORSHADOW);
				//this.bitmap.fill_rect(@spritebaseX + expGaugeX, expGaugeY + 2, this.Exp, 2,
				//   PokeBattle_SceneConstants.EXPCOLORBASE); //Same X value, just 2 Y values lower
				sliderExp.minValue = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.ExperienceNeeded(battler.Level);
				sliderExp.maxValue = (@battler.pokemon as PokemonUnity.Monster.Pokemon).Experience.NextLevel;
				sliderExp.value = this.Exp;
				//StopCoroutine("AnimateSliderExp"); //(AnimateSliderHP(this.Exp));
				//StartCoroutine(AnimateSliderExp(this.Exp)); //sliderExp.value = this.Exp;
			} else sliderExp?.gameObject.SetActive(false); //Just in case, if exp is not supposed to be seen, hide it
		}

		public override void update()
		{
			//Core.Logger?.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			base.update();
			@frame += 1;
			if (@animatingHP)
			{
				// Everything inside this if statement is for animating the hp bar
				// Move to a coroutine to utilize Unity's built-in animation system or use a lerp...
				if (@currenthp < @endhp)
				{
					//@currenthp +=(int)Math.Max(1, Math.Floor(@battler.TotalHP / PokeBattle_SceneConstants.HPGAUGESIZE));
					@currenthp += (int)Math.Max(1, Math.Floor(@battler.TotalHP / sliderHP.maxValue));
					if (@currenthp > @endhp) @currenthp = @endhp;
				}
				else if (@currenthp > @endhp)
				{
					//@currenthp -=(int)Math.Max(1, Math.Floor(@battler.TotalHP / PokeBattle_SceneConstants.HPGAUGESIZE));
					@currenthp -= (int)Math.Max(1, Math.Floor(@battler.TotalHP / sliderHP.maxValue));
					if (@currenthp < @endhp) @currenthp = @endhp;
				}
				if (@currenthp == @endhp) @animatingHP = false;
				refresh();
			}
			//ToDo: New coroutine to flash white and blue when leveling-up, chime at 100%, and begin again from 0.
			if (@animatingEXP)
			{
				// Everything inside this if statement is for animating the exp bar
				// Move to a coroutine to utilize Unity's built-in animation system or use a lerp...
				if (!@showexp)
				{
					@currentexp = @endexp;
				}
				else if (@currentexp < @endexp)			// Gaining Exp
				{
					//if (@endexp >= PokeBattle_SceneConstants.EXPGAUGESIZE ||
					//   @endexp - @currentexp >= PokeBattle_SceneConstants.EXPGAUGESIZE / 4)
					if (@endexp >= sliderExp.maxValue ||
					   @endexp - @currentexp >= sliderExp.maxValue / 4)
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
						//@currentexp - @endexp >= PokeBattle_SceneConstants.EXPGAUGESIZE / 4)
						@currentexp - @endexp >= sliderExp.maxValue / 4)
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
					//if (@currentexp == PokeBattle_SceneConstants.EXPGAUGESIZE)
					if (@currentexp == sliderExp.maxValue)
					{
						if (@expflash == 0)
						{
							//(AudioHandler as IGameAudioPlay).SEPlay("expfull");
							this.flash(spriteFillExp, new SeriColor(64, 200, 248), 8); //this.flash(new SeriColor(64, 200, 248), 8);
							//spriteFillExp.color = new Color(64, 200, 248);
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
				// Everything inside this if statement is for animating the slide-in for the databox
				// Move to a coroutine to utilize Unity's built-in animation system or use a lerp...
				if ((@battler.Index & 1) == 0)		// If player's Pokémon
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
			else if (((int)Math.Floor(@frame / 10f) & 1) == 1 && @selected == 2)	// When targeted or damaged
			{
				this.y = @spriteY + 2;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					//@statuses.Dispose();
					//@databox.Dispose();
					//@contents.Dispose();
					base.Dispose(disposing);
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				//disposed = true;
			}
		}

		#region Unity Coroutine Animations
		/// <summary>
		/// Invoked when the value of the slider changes.
		/// </summary>
		protected virtual void ValueChangeCheck()
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			//if (sliderHP.value <= (sliderHP.maxValue / 4)) { Fill.color = hpzone2; }
			if (.3f > sliderHP.normalizedValue)
			{
				spriteFillHP.color = Color.red; //(SeriColor)PokeBattle_SceneConstants.HPCOLORRED;
			}
			//else if (sliderHP.value < (sliderHP.normalizedValue.CompareTo(0.5f))) //  / 2))
			else if (.75f > (sliderHP.normalizedValue)) //  / 2))
			{
				//Change color of hp bar
				spriteFillHP.color = Color.yellow; //(SeriColor)PokeBattle_SceneConstants.HPCOLORYELLOW;
				//Change background image for health slider
			}
			else
				spriteFillHP.color = Color.green; //(SeriColor)PokeBattle_SceneConstants.HPCOLORGREEN;
				//each time the slider's value is changed, write to text displaying the hp
				//maxHP.text = sliderHP.maxValue.ToString(); //Set text under hp to match slider maxHealth
				//currentHP.text = sliderHP.value.ToString(); //Set text under hp to match slider currentHealth
				//HPValue.SetText($"{sliderHP.value}/{sliderHP.maxValue}");
		}

		protected System.Collections.IEnumerator AnimateSliderHP(int amount) //Slider as input?
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Debug.Log(amount);
			//while (sliderHP.value != amount)
			while (Math.Abs(sliderHP.value - amount) > 0.001f)
			{
				sliderHP.value = Mathf.Lerp(sliderHP.value + amount, sliderHP.value, 1f * Time.deltaTime);
				yield return null;
			}
			animatingHP = false; //should disable after the values stop moving the slider...
			//new WaitForSeconds(1f);
			//fadeSlider.value = Mathf.Lerp(sliderHP.value, fadeSlider.value, .5f * Time.deltaTime);
			//yield return null;
		}
		/// <summary>
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		protected System.Collections.IEnumerator AnimateSliderExp(int amount) //Slider as input?
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Debug.Log(amount);
			while (sliderExp.value != amount) //ToDo: While != this.Exp, lerp(value + amount)?
			{
				sliderExp.value = Mathf.Lerp(sliderExp.value + amount, sliderExp.value, 1f * Time.deltaTime);
				yield return null; //each frame tick will/should call the update function, and perform flash animation if necessary.
			}
			animatingEXP = false; //should disable after the values stop moving the slider...
			//new WaitForSeconds(1f);
			//fadeSlider.value = Mathf.Lerp(sliderHP.value, fadeSlider.value, .5f * Time.deltaTime);
			//yield return null;
		}
		#endregion
	}

	/// <summary>
	/// </summary>
	public class SafariDataBox : SpriteWrapper, ISafariDataBox
	{
		public int selected { get; set; }
		public bool appearing { get; protected set; }
		public global::UnityEngine.UI.Image panelbg;
		public global::UnityEngine.Sprite databox; //AnimatedBitmap
		private IBattle battle;
		protected float spriteX;
		protected float spriteY;

		[Header("Safari Event Display")]
		[SerializeField] private TMPro.TextMeshProUGUI safariBallsText;
		[SerializeField] private TMPro.TextMeshProUGUI safariBallsCount;

		private void Awake()
		{
			panelbg = GetComponent<global::UnityEngine.UI.Image>();
			//databox = GetComponent<global::UnityEngine.Sprite>();
		}

		private void Start()
		{
		}

		public ISafariDataBox initialize(IBattle battle, IViewport viewport = null)
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			base.initialize(viewport);
			@selected = 0;
			this.battle = battle;
			// Let's assume below is already set inside unity, and dont need to be set with code
			//@databox = new AnimatedBitmap("Graphics/Pictures/battlePlayerSafari");
			//@databox = Resources.Load<global::UnityEngine.Sprite>("Graphics/Pictures/battlePlayerSafari");
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
			//Core.Logger?.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			refresh();
			this.visible = true;
			this.opacity = 255;
			this.x = @spriteX + 240;
			this.y = @spriteY;
			@appearing = true;
		}

		public virtual void refresh()
		{
			//Core.Logger?.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//this.bitmap.clear();
			//this.bitmap.blt(0, 0, @databox.bitmap, new Rect(0, 0, @databox.width, @databox.height));
			//SetSystemFont(this.bitmap);
			//IList<ITextPosition> textpos = new List<ITextPosition>();
			//IColor base_ = PokeBattle_SceneConstants.BOXTEXTBASECOLOR;
			//IColor shadow = PokeBattle_SceneConstants.BOXTEXTSHADOWCOLOR;
			//textpos.Add(new TextPosition(Game._INTL("Safari Balls"), 30, 8, false, base_, shadow));
			//textpos.Add(new TextPosition(Game._INTL("Left: {1}", (@battle as ISafariZone_Scene).ballCount), 30, 38, false, base_, shadow));
			safariBallsText.SetText("Safari Balls");
			safariBallsCount.SetText($"Left: {(@battle as ISafariZone_Scene).ballCount}");
			//DrawTextPositions(this.bitmap, textpos);
		}

		public override void update()
		{
			//Core.Logger?.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			base.update();
			if (@appearing)
			{
				//ToDo: Coroutine animation for sliding in from side to replace this?
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