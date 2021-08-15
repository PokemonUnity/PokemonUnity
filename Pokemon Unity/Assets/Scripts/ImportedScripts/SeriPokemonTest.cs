using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using System;

namespace PokemonUnity.Saving.Test
{
	[System.Serializable]
	public struct SeriPokemon
	{
		#region Variables
		public string NickName { get; private set; }
		public int Form { get; private set; }
		public int Species { get; private set; }

		public int Ability { get; private set; }
		public int Nature { get; private set; }

		public bool IsShiny { get; private set; }
		public bool? Gender { get; private set; }
		public int[] Pokerus { get; private set; }
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
		public SerializableClasses.SeriMail Mail { get; private set; }

		public SerializableClasses.SeriMove[] Moves { get; private set; }
		public int[] Archive { get; private set; }

		public int[] Ribbons { get; private set; }
		public bool[] Markings { get; private set; }
		public int PersonalId { get; private set; }
		public string TrainerName { get; private set; }
		public bool TrainerIsMale { get; private set; }
		public int TrainerTrainerId { get; private set; }
		public int TrainerSecretId { get; private set; }

		public int ObtainedMethod { get; private set; }
		public DateTimeOffset TimeReceived { get; private set; }
		public DateTimeOffset? TimeEggHatched { get; private set; }
		#endregion

		public static implicit operator PokemonUnity.Monster.Pokemon(SeriPokemon pokemon)
		{
			if ((Pokemons)pokemon.Species == Pokemons.NONE) return new PokemonUnity.Monster.Pokemon(Pokemons.NONE);
			Ribbon[] ribbons = new Ribbon[pokemon.Ribbons.Length];
			for (int i = 0; i < ribbons.Length; i++)
			{
				ribbons[i] = (Ribbon)pokemon.Ribbons[i];
			}

			Move[] moves = new Attack.Move[pokemon.Moves.Length];
			for (int i = 0; i < moves.Length; i++)
			{
				moves[i] = pokemon.Moves[i];
			}
			
			Moves[] history = new Moves[pokemon.Archive.Length];
			for (int i = 0; i < pokemon.Archive.Length; i++)
			{
			
				history[i] = (Moves)pokemon.Archive[i];
			}

			PokemonUnity.Monster.Pokemon normalPokemon =
				new PokemonUnity.Monster.Pokemon
				(
					(Pokemons)pokemon.Species, new TrainerData(pokemon.TrainerName, pokemon.TrainerIsMale,
					tID: pokemon.TrainerTrainerId, sID: pokemon.TrainerSecretId),
					pokemon.NickName, pokemon.Form, (Abilities)pokemon.Ability,
					(Natures)pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
					pokemon.Pokerus, pokemon.HeartGuageSize, /*pokemon.IsHyperMode,*/ pokemon.ShadowLevel,
					pokemon.CurrentHP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
					pokemon.ObtainedLevel, /*pokemon.CurrentLevel,*/ pokemon.CurrentExp,
					pokemon.Happiness, (Status)pokemon.Status, pokemon.StatusCount,
					pokemon.EggSteps, (Items)pokemon.BallUsed, pokemon.Mail.Message,
					moves, history, ribbons, pokemon.Markings, pokemon.PersonalId,
					(PokemonUnity.Monster.Pokemon.ObtainedMethod)pokemon.ObtainedMethod,
					pokemon.TimeReceived, pokemon.TimeEggHatched
				);
			return normalPokemon;
		}

		public static implicit operator SeriPokemon(PokemonUnity.Monster.Pokemon pokemon)
		{
			SeriPokemon seriPokemon = new SeriPokemon();
			if (pokemon == null) return seriPokemon;

			if (pokemon.IsNotNullOrNone())
			{
				seriPokemon.PersonalId = pokemon.PersonalId;

				if (!pokemon.OT.Equals((object)null))
				{
					seriPokemon.TrainerName = pokemon.OT.Value.Name;
					seriPokemon.TrainerIsMale = pokemon.OT.Value.Gender == true;
					seriPokemon.TrainerTrainerId = pokemon.OT.Value.TrainerID;
					seriPokemon.TrainerSecretId = pokemon.OT.Value.SecretID;
				}

				seriPokemon.Species = (int)pokemon.Species;
				seriPokemon.Form = pokemon.FormId;
				
				seriPokemon.NickName = pokemon.Name;

				seriPokemon.Ability = (int)pokemon.Ability;

				seriPokemon.Nature = (int)pokemon.Nature;
				seriPokemon.IsShiny = pokemon.IsShiny;
				seriPokemon.Gender = pokemon.Gender;

				seriPokemon.Pokerus = pokemon.Pokerus;
				seriPokemon.HeartGuageSize = pokemon.HeartGuageSize;
				seriPokemon.ShadowLevel = pokemon.ShadowLevel;

				seriPokemon.CurrentHP = pokemon.HP;
				seriPokemon.Item = (int)pokemon.Item;

				seriPokemon.IV = pokemon.IV;
				seriPokemon.EV = pokemon.EV;

				seriPokemon.ObtainedLevel = pokemon.ObtainLevel;
				seriPokemon.CurrentExp = pokemon.Experience.Total;

				seriPokemon.Happiness = pokemon.Happiness;

				seriPokemon.Status = (int)pokemon.Status;
				seriPokemon.StatusCount = pokemon.StatusCount;

				seriPokemon.EggSteps = pokemon.EggSteps;

				seriPokemon.BallUsed = (int)pokemon.ballUsed;
				if (pokemon.Item != Items.NONE && Game.ItemData[pokemon.Item].IsLetter)
				{
					seriPokemon.Mail = new SerializableClasses.SeriMail(pokemon.Item, pokemon.Mail);
				}

				seriPokemon.Moves = new SerializableClasses.SeriMove[4];
				for (int i = 0; i < 4; i++)
				{
					seriPokemon.Moves[i] = pokemon.moves[i];
				}

				if (pokemon.MoveArchive != null)
				{
					seriPokemon.Archive = new int[pokemon.MoveArchive.Length];
					for (int i = 0; i < seriPokemon.Archive.Length; i++)
					{
						seriPokemon.Archive[i] = (int)pokemon.MoveArchive[i];
					}
				}
				if (pokemon.Ribbons != null)
				{
					seriPokemon.Ribbons = new int[pokemon.Ribbons.Length];
					for (int i = 0; i < seriPokemon.Ribbons.Length; i++)
					{
						seriPokemon.Ribbons[i] = (int)pokemon.Ribbons[i];
					}
				}
				seriPokemon.Markings = pokemon.Markings;

				seriPokemon.ObtainedMethod = (int)pokemon.ObtainedMode;
				seriPokemon.TimeReceived = pokemon.TimeReceived;
				seriPokemon.TimeEggHatched = pokemon.TimeEggHatched;
			}

			return seriPokemon;
		}
	}
}
