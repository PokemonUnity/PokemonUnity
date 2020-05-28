using PokemonUnity.Inventory.Plants;

namespace PokemonUnity.Monster
{
	public struct Nature
	{
		public Natures Natures { get; private set; }
		public Stats Increases { get; private set; }
		public Stats Decreases { get; private set; }
		public Flavours Likes { get; private set; }
		public Flavours Dislikes { get; private set; }
		float[] Stat_mod { get; set; }//= new float[5];
		public float ATK { get { return Stat_mod[(int)Stats.ATTACK]; } }
		public float DEF { get { return Stat_mod[(int)Stats.DEFENSE]; } }
		public float SPA { get { return Stat_mod[(int)Stats.SPATK]; } }
		public float SPD { get { return Stat_mod[(int)Stats.SPDEF]; } }
		public float SPE { get { return Stat_mod[(int)Stats.SPEED]; } }

		/*public Nature()
		 {
			 this.Natures = getRandomNature();
		 }
		 public Nature(Natures nature)
		 {
			 this.Natures = natures[(int)nature].Natures;
		 }
		 public Nature(Natures name, float ATK_mod, float DEF_mod, float SPA_mod, float SPD_mod, float SPE_mod)
		 {
			 this.Natures = name;
			 this.Stat_mod = new float[6];
			 this.Stat_mod[(int)Stats.ATTACK]	= ATK_mod;
			 this.Stat_mod[(int)Stats.DEFENSE]	= DEF_mod;
			 this.Stat_mod[(int)Stats.SPATK]	= SPA_mod;
			 this.Stat_mod[(int)Stats.SPDEF]	= SPD_mod;
			 this.Stat_mod[(int)Stats.SPEED]	= SPE_mod;
		 }
		 private static Nature[] natures = new Nature[]
		 {
			 new Nature(Natures.HARDY, 1, 1, 1, 1, 1),
			 new Nature(Natures.LONELY, 1.1f, 0.9f, 1, 1, 1),
			 new Nature(Natures.BRAVE, 1.1f, 1, 1, 1, 0.9f),
			 new Nature(Natures.ADAMANT, 1.1f, 1, 0.9f, 1, 1),
			 new Nature(Natures.NAUGHTY, 1.1f, 1, 1, 0.9f, 1),
			 new Nature(Natures.BOLD, 0.9f, 1.1f, 1, 1, 1),
			 new Nature(Natures.DOCILE, 1, 1, 1, 1, 1),
			 new Nature(Natures.RELAXED, 1, 1.1f, 1, 1, 0.9f),
			 new Nature(Natures.IMPISH, 1, 1.1f, 0.9f, 1, 1),
			 new Nature(Natures.LAX, 1, 1.1f, 1, 0.9f, 1),
			 new Nature(Natures.TIMID, 0.9f, 1, 1, 1, 1.1f),
			 new Nature(Natures.HASTY, 1, 0.9f, 1, 1, 1.1f),
			 new Nature(Natures.SERIOUS, 1, 1, 1, 1, 1),
			 new Nature(Natures.JOLLY, 1, 1, 0.9f, 1, 1.1f),
			 new Nature(Natures.NAIVE, 1, 1, 1, 0.9f, 1.1f),
			 new Nature(Natures.MODEST, 0.9f, 1, 1.1f, 1, 1),
			 new Nature(Natures.MILD, 1, 0.9f, 1.1f, 1, 1),
			 new Nature(Natures.QUIET, 1, 1, 1.1f, 1, 0.9f),
			 new Nature(Natures.BASHFUL, 1, 1, 1, 1, 1),
			 new Nature(Natures.RASH, 1, 1, 1.1f, 0.9f, 1),
			 new Nature(Natures.CALM, 0.9f, 1, 1, 1.1f, 1),
			 new Nature(Natures.GENTLE, 1, 0.9f, 1, 1.1f, 1),
			 new Nature(Natures.SASSY, 1, 1, 1, 1.1f, 0.9f),
			 new Nature(Natures.CAREFUL, 1, 1, 0.9f, 1.1f, 1),
			 new Nature(Natures.QUIRKY, 1, 1, 1, 1, 1)
		 };*/
		public Nature(Natures nature, Stats increase, Stats decrease, Flavours like, Flavours dislike)
		{
			Natures = nature;
			Increases = increase;
			Decreases = decrease;
			Likes = like;
			Dislikes = dislike;
			
			this.Stat_mod = new float[6];
			this.Stat_mod[(int)Stats.ATTACK]	 = 1;
			this.Stat_mod[(int)Stats.DEFENSE]	 = 1;
			this.Stat_mod[(int)Stats.SPATK]		 = 1;
			this.Stat_mod[(int)Stats.SPDEF]		 = 1;
			this.Stat_mod[(int)Stats.SPEED]		 = 1;
			this.Stat_mod[(int)increase]		+= .1f;
			this.Stat_mod[(int)decrease]		-= .1f;
		}
	}
}