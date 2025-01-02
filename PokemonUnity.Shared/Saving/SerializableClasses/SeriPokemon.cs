using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonUnity;
using System;

namespace PokemonUnity.Saving.SerializableClasses
{
	/// <summary>
	/// Serializable version of Pokemon Unity's Pokemon class
	/// </summary>
	[System.Serializable]
	public struct SeriPokemon
	{
		#region Sample of Serialized Pokemon Profile
		//This is an *IDEA* of what it should look like in save file...
		/*TPSPECIES	,
		TPLEVEL		,
		TPITEM		,
		TPMOVE1		,
		TPMOVE2		,
		TPMOVE3		,
		TPMOVE4		,
		TPABILITY	,
		TPGENDER	,
		TPFORM		, 
		TPSHINY		,
		TPNATURE	,
		TPIV		,
		TPHAPPINESS ,
		TPNAME		,
		TPSHADOW	,
		TPBALL		,
		TPDEFAULTS = [0, 10, 0, 0, 0, 0, 0, nil, nil, 0, false, nil, 10, 70, nil, false, 0]*/
		#endregion

		#region Variables
		public string NickName { get; private set; }
		public int Form { get; private set; }
		public int Species { get; private set; }

		public int Ability { get; private set; }
		public int Nature { get; private set; }

		public bool IsShiny { get; private set; }
		public bool? Gender { get; private set; }

		//public bool? PokerusStage { get; private set; }
		public int[] Pokerus { get; private set; }
		//public int PokerusStrain { get; private set; }

		//public bool IsHyperMode { get; private set; }
		//public bool IsShadow { get; private set; }
		public int? ShadowLevel { get; private set; }
		public int HeartGuageSize { get; private set; }

		public int CurrentHP { get; private set; }
		public int Item { get; private set; }

		public int[] IV { get; private set; }
		public byte[] EV { get; private set; }

		public int ObtainedLevel { get; private set; }
		//public int CurrentLevel { get; private set; }
		public int CurrentExp { get; private set; }

		public int Happiness { get; private set; }

		public int Status { get; private set; }
		public int StatusCount { get; private set; }

		public int EggSteps { get; private set; }

		public int BallUsed { get; private set; }
		//Creating a seperate Seri class for Mail
		public SeriMail Mail { get; private set; }

		public SeriMove[] Moves { get; private set; }
		public int[] Archive { get; private set; }

		public int[] Ribbons { get; private set; }
		public bool[] Markings { get; private set; }

		/// <summary>
		/// Trading/Obtaining
		/// </summary>
		public int PersonalId { get; private set; }
		//public string PublicId { get; private set; }
		public string TrainerName { get; private set; }
		public bool TrainerIsMale { get; private set; }
		public int TrainerTrainerId { get; private set; }
		public int TrainerSecretId { get; private set; }

		public int ObtainedMethod { get; private set; }
		public DateTimeOffset TimeReceived { get; private set; }
		public DateTimeOffset? TimeEggHatched { get; private set; }
		#endregion

		//public static implicit operator SeriPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		//{
		//	SeriPokemon seriPokemon = new SeriPokemon();
		//	if (pokemon == null) return seriPokemon;
		//
		//	if(pokemon.IsNotNullOrNone())// != null && pokemon.Species != Pokemons.NONE)
		//	{
		//		seriPokemon.PersonalId			= pokemon.PersonalId;
		//		//PublicId in pokemon is null, so Pokemon returns null
		//		//seriPokemon.PublicId			= pokemon.PublicId;
		//
		//		if (!pokemon.OT.Equals((object)null))
		//		{
		//			seriPokemon.TrainerName			= pokemon.OT.Value.Name;
		//			seriPokemon.TrainerIsMale		= pokemon.OT.Value.Gender == true;
		//			seriPokemon.TrainerTrainerId	= pokemon.OT.Value.TrainerID;
		//			seriPokemon.TrainerSecretId		= pokemon.OT.Value.SecretID;
		//		}
		//
		//		seriPokemon.Species				= (int)pokemon.Species;
		//		seriPokemon.Form				= pokemon.FormId;
		//		//Creates an error System OutOfBounds inside Pokemon
		//		seriPokemon.NickName			= pokemon.Name;
		//
		//		seriPokemon.Ability				= (int)pokemon.Ability;
		//
		//		//seriPokemon.Nature = pokemon.getNature();
		//		seriPokemon.Nature				= (int)pokemon.Nature;
		//		seriPokemon.IsShiny				= pokemon.IsShiny; 
		//		seriPokemon.Gender				= pokemon.Gender;
		//
		//		//seriPokemon.PokerusStage		= pokemon.PokerusStage;
		//		seriPokemon.Pokerus				= pokemon.Pokerus;
		//		//seriPokemon.PokerusStrain		= pokemon.PokerusStrain;
		//
		//		//seriPokemon.IsHyperMode		= pokemon.isHyperMode;
		//		//seriPokemon.IsShadow			= pokemon.isShadow;
		//		seriPokemon.HeartGuageSize		= pokemon.HeartGuageSize;
		//		seriPokemon.ShadowLevel			= pokemon.ShadowLevel;
		//
		//		seriPokemon.IV					= pokemon.IV;
		//		seriPokemon.EV					= pokemon.EV;
		//
		//		seriPokemon.ObtainedLevel		= pokemon.ObtainLevel;
		//		//seriPokemon.CurrentLevel		= pokemon.Level;
		//		seriPokemon.CurrentExp			= pokemon.Experience.Total;
		//
		//		seriPokemon.CurrentHP			= pokemon.HP;
		//		seriPokemon.Item				= (int)pokemon.Item;
		//
		//		seriPokemon.Happiness			= pokemon.Happiness;
		//
		//		seriPokemon.Status				= (int)pokemon.Status;
		//		seriPokemon.StatusCount			= pokemon.StatusCount;
		//
		//		seriPokemon.EggSteps			= pokemon.EggSteps;
		//
		//		seriPokemon.BallUsed			= (int)pokemon.ballUsed;
		//		if (pokemon.Item != Items.NONE && Game.ItemData[pokemon.Item].IsLetter)//PokemonUnity.Inventory.Mail.IsMail(pokemon.Item))
		//		{
		//			seriPokemon.Mail			= new SeriMail(pokemon.Item, pokemon.Mail);
		//		}
		//
		//		seriPokemon.Moves = new SeriMove[4];
		//		for (int i = 0; i < 4; i++)
		//		{
		//			seriPokemon.Moves[i]		= pokemon.moves[i];
		//		}
		//
		//		if (pokemon.MoveArchive != null)
		//		{
		//			seriPokemon.Archive			= new int[pokemon.MoveArchive.Length];
		//			for (int i = 0; i < seriPokemon.Archive.Length; i++)
		//			{
		//				seriPokemon.Archive[i]	= (int)pokemon.MoveArchive[i];
		//			}
		//		}
		//
		//		//Ribbons is also null, we add a null check
		//		if (pokemon.Ribbons != null)
		//		{
		//			seriPokemon.Ribbons			= new int[pokemon.Ribbons.Length];
		//			for (int i = 0; i < seriPokemon.Ribbons.Length; i++)
		//			{
		//				seriPokemon.Ribbons[i]	= (int)pokemon.Ribbons[i];
		//			}
		//		}
		//		//else //Dont need else, should copy whatever value is given, even if null...
		//		//{
		//		//	seriPokemon.Ribbons			= new int[0];
		//		//}
		//		seriPokemon.Markings			= pokemon.Markings;
		//
		//		seriPokemon.ObtainedMethod		= (int)pokemon.ObtainedMode;
		//		seriPokemon.TimeReceived		= pokemon.TimeReceived;
		//		//try
		//		//{
		//			seriPokemon.TimeEggHatched	= pokemon.TimeEggHatched;
		//		//}
		//		//catch (Exception) { seriPokemon.TimeEggHatched = new DateTimeOffset(); }
		//	}
		//
		//	return seriPokemon;
		//}

		//public static implicit operator PokemonEssentials.Interface.PokeBattle.IPokemon(SeriPokemon pokemon)
		//{
		//	//if (pokemon == null) return null;
		//	if ((Pokemons)pokemon.Species == Pokemons.NONE) return new Pokemon(Pokemons.NONE);
		//	Ribbon[] ribbons = new Ribbon[pokemon.Ribbons.Length];
		//	for (int i = 0; i < ribbons.Length; i++)
		//	{
		//		ribbons[i] = (Ribbon)pokemon.Ribbons[i];
		//	}
		//
		//	Move[] moves = new Attack.Move[pokemon.Moves.Length];
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		moves[i] = pokemon.Moves[i];
		//	}
		//
		//	Moves[] history = new Moves[pokemon.Archive.Length];
		//	for (int i = 0; i < pokemon.Archive.Length; i++)
		//	{
		//		history[i] = (Moves)pokemon.Archive[i];
		//	}
		//
		//	PokemonEssentials.Interface.PokeBattle.IPokemon normalPokemon =
		//		new Pokemon
		//		(
		//			(Pokemons)pokemon.Species, (pokemon.TrainerName == null && 
		//			pokemon.TrainerTrainerId == 0 && pokemon.TrainerSecretId == 0) ? (TrainerData?)null :
		//			new TrainerData(pokemon.TrainerName, pokemon.TrainerIsMale, 
		//			tID: pokemon.TrainerTrainerId, sID: pokemon.TrainerSecretId),
		//			pokemon.NickName, pokemon.Form, (Abilities)pokemon.Ability,
		//			(Natures)pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
		//			pokemon.Pokerus, pokemon.HeartGuageSize, /*pokemon.IsHyperMode,*/ pokemon.ShadowLevel,
		//			pokemon.CurrentHP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
		//			pokemon.ObtainedLevel, /*pokemon.CurrentLevel,*/ pokemon.CurrentExp,
		//			pokemon.Happiness, (Status)pokemon.Status, pokemon.StatusCount,
		//			pokemon.EggSteps, (Items)pokemon.BallUsed, pokemon.Mail.Message,
		//			moves, history, ribbons, pokemon.Markings, pokemon.PersonalId,
		//			(Pokemon.ObtainedMethod)pokemon.ObtainedMethod,
		//			pokemon.TimeReceived, pokemon.TimeEggHatched
		//		);
		//	return normalPokemon;
		//}
	}
}