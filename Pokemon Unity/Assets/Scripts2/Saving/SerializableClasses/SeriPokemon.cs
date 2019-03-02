using System;

namespace PokemonUnity.Saving.SerializableClasses
{
    using PokemonUnity.Pokemon;
    using PokemonUnity.Attack;
    using PokemonUnity.Item;
    using PokemonUnity;

	/// <summary>
	/// Serializable version of Pokemon Unity's Pokemon class
	/// </summary>
	[System.Serializable]
	public class SeriPokemon
	{
		#region Variables
		public string NickName { get; private set; }
		public int Form { get; private set; }
		public int Species { get; private set; }

		public int Ability { get; private set; }
		public int Nature { get; private set; }

		public virtual bool IsShiny { get; private set; }
		public virtual bool? Gender { get; private set; }

		//public bool? PokerusStage { get; private set; }
		public int[] Pokerus { get; private set; }
		//public int PokerusStrain { get; private set; }

		public bool IsHyperMode { get; private set; }
		//public bool IsShadow { get; private set; }
		public int? ShadowLevel { get; private set; }

		public int CurrentHP { get; private set; }
		public int Item { get; private set; }

		public byte[] IV { get; private set; }
		public byte[] EV { get; private set; }

		public int ObtainedLevel { get; private set; }
		//public int CurrentLevel { get; private set; }
		public int CurrentExp { get; private set; }

		public int Happines { get; private set; }

		public int Status { get; private set; }
		public int StatusCount { get; private set; }

		public int EggSteps { get; private set; }

		public int BallUsed { get; private set; }
		//Creating a seperate Seri class for Mail
		public SeriMail Mail { get; private set; }

		public SeriMove[] Moves { get; private set; }

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

		#region Methods
		public SeriMove[] GetMoves()
		{
			return Moves;
		}
		#endregion

		public static implicit operator Pokemon(SeriPokemon pokemon)
		{
			if ((Pokemons)pokemon.Species == Pokemons.NONE || pokemon == null) return new Pokemon(Pokemons.NONE);
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

			Pokemon normalPokemon =
				new Pokemon
				(
					(Pokemons)pokemon.Species, new Trainer(new Player(pokemon.TrainerName,
					pokemon.TrainerIsMale), tID: pokemon.TrainerTrainerId, sID: pokemon.TrainerSecretId),
					pokemon.NickName, pokemon.Form, (Abilities)pokemon.Ability,
					(Natures)pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
					pokemon.Pokerus, pokemon.IsHyperMode, pokemon.ShadowLevel,
					pokemon.CurrentHP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
					pokemon.ObtainedLevel, /*pokemon.CurrentLevel,*/ pokemon.CurrentExp,
					pokemon.Happines, (Status)pokemon.Status, pokemon.StatusCount,
					pokemon.EggSteps, (Items)pokemon.BallUsed, pokemon.Mail.Message,
					moves, ribbons, pokemon.Markings, pokemon.PersonalId,
					(Pokemon.ObtainedMethod)pokemon.ObtainedMethod,
					pokemon.TimeReceived, pokemon.TimeEggHatched
				);
			return normalPokemon;
		}

		public static implicit operator SeriPokemon(Pokemon pokemon)
		{
			SeriPokemon seriPokemon = new SeriPokemon();

			if(pokemon.Species != Pokemons.NONE || pokemon != null)
			{
				seriPokemon.PersonalId			= pokemon.PersonalId;
				//PublicId in pokemon is null, so Pokemon returns null
				//seriPokemon.PublicId			= pokemon.PublicId;

				//ToDo: Uncomment and fix
				//if (pokemon.OT != null)
				//{
				//	seriPokemon.TrainerName			= pokemon.OT.Name;
				//	seriPokemon.TrainerIsMale		= pokemon.OT.Gender.Value;
				//	seriPokemon.TrainerTrainerId	= pokemon.OT.TrainerID;
				//	seriPokemon.TrainerSecretId		= pokemon.OT.SecretID;
				//}

				seriPokemon.Species				= (int)pokemon.Species;
				seriPokemon.Form				= pokemon.Form;
				//Creates an error System OutOfBounds inside Pokemon
				seriPokemon.NickName			= pokemon.Name;

				seriPokemon.Ability				= (int)pokemon.Ability;
				//Due to parts of Pokemon that's being reworked
				//These parts shouldn't be saved yet.

				//seriPokemon.Nature = pokemon.getNature();
				seriPokemon.Nature				= (int)pokemon.Nature;//new Nature(Natures.SASSY, 0,0,0,0,0); //TESTING ONLY
				//seriPokemon.IsShiny				= pokemon.IsShiny; //ToDo: Uncomment and fix
				seriPokemon.Gender				= pokemon.Gender;

				//seriPokemon.PokerusStage		= pokemon.PokerusStage;
				seriPokemon.Pokerus				= pokemon.Pokerus;
				//seriPokemon.PokerusStrain		= pokemon.PokerusStrain;

				seriPokemon.IsHyperMode			= pokemon.isHyperMode;
				//seriPokemon.IsShadow			= pokemon.isShadow;
				seriPokemon.ShadowLevel			= pokemon.ShadowLevel;

				seriPokemon.CurrentHP			= pokemon.HP;
				seriPokemon.Item				= (int)pokemon.Item;

				seriPokemon.IV					= pokemon.IV;
				seriPokemon.EV					= pokemon.EV;

				seriPokemon.ObtainedLevel		= pokemon.ObtainLevel;
				//seriPokemon.CurrentLevel		= pokemon.Level;
				seriPokemon.CurrentExp			= pokemon.Exp.Current;

				seriPokemon.Happines			= pokemon.Happiness;

				seriPokemon.Status				= (int)pokemon.Status;
				seriPokemon.StatusCount			= pokemon.StatusCount;

				seriPokemon.EggSteps			= pokemon.EggSteps;

				seriPokemon.BallUsed			= (int)pokemon.ballUsed;
				if (PokemonUnity.Item.Item.Mail.IsMail(pokemon.Item))
				{
					seriPokemon.Mail			= new SeriMail(pokemon.Item, pokemon.Mail);
				}

				seriPokemon.Moves = new SeriMove[4];
				for (int i = 0; i < 4; i++)
				{
					seriPokemon.Moves[i]		= pokemon.moves[i];
				}

				//Ribbons is also null, we add a null check
				if (pokemon.Ribbons != null)
				{
					seriPokemon.Ribbons			= new int[pokemon.Ribbons.Count];
					for (int i = 0; i < seriPokemon.Ribbons.Length; i++)
					{
						seriPokemon.Ribbons[i]	= (int)pokemon.Ribbons[i];
					}
				}
				else
				{
					seriPokemon.Ribbons			= new int[0];
				}
				seriPokemon.Markings			= pokemon.Markings;

				seriPokemon.ObtainedMethod		= (int)pokemon.ObtainedMode;
				seriPokemon.TimeReceived		= pokemon.TimeReceived;
				//try
				//{
					seriPokemon.TimeEggHatched	= pokemon.TimeEggHatched;
				//}
				//catch (Exception) { seriPokemon.TimeEggHatched = new DateTimeOffset(); }
			}

			return seriPokemon;
		}

        [System.Serializable]
        public class SeriMail
        {
            private int MailId { get; set; }
            public string Message { get; private set; }
            //Background will stay 0 (new int) until Mail's background feature is implemented
            //public int Background { get; private set; }

            public static implicit operator Item.Mail(SeriMail mail)
            {
                Item.Mail newMail = new Item.Mail((Items)mail.MailId);
                newMail.Message = mail.Message;
                return newMail;
            }

            public SeriMail(Items mailItem, string messsage)
            {
                MailId = (int)mailItem;
                Message = messsage;
            }
        }
    }

	public static class SeriArray
	{
		public static Pokemon[] Deserialize (this SeriPokemon[] pkmn)
		{
			return new Pokemon[]
			{
				pkmn[0],
				pkmn[1],
				pkmn[2],
				pkmn[3],
				pkmn[4],
				pkmn[5]
			};
		}
		public static SeriPokemon[] Serialize (this Pokemon[] pkmn)
		{
			return new SeriPokemon[]
			{
				pkmn[0],
				pkmn[1],
				pkmn[2],
				pkmn[3],
				pkmn[4],
				pkmn[5]
			};
		}
	}
}
