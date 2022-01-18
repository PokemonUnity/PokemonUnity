namespace PokemonUnity.Shared
{
	public class Languages : PokemonUnity.Shared.Enums.Languages
	{
		public override string Name { get { return "Language_Name_{0}"; } }
		public string ISO639 { get; private set; }
		public string ISO3166 { get; private set; }
		protected Languages(int id, string iso639, string iso3166, string name) : base(id, name) { ISO639 = iso639; ISO3166 = iso3166; }
		/// <summary>
		/// Japanese without Kanji
		/// </summary>
		public static readonly Languages Hiragana		= new Languages(1,		"ja",		"jp",	"ja-Hrkt"	);
		/// <summary>
		/// Official Roomaji
		/// </summary>
		public static readonly Languages Roomaji		= new Languages(2,		"ja",		"jp",	"roomaji"	);
		public static readonly Languages Korean			= new Languages(3,		"ko",		"kr",	"ko"		);
		/// <summary>
		/// Mandarin Chinese as written in Traditional script
		/// </summary>
		public static readonly Languages Chinese		= new Languages(4,		"zh",		"cn",	"zh-Hant"	);
		/// <summary>
		/// Mandarin Chinese, Simplified script
		/// </summary>
		public static readonly Languages Chinese2		= new Languages(12,		"zh",		"cn",	"zh-Hans"	);
		/// <summary>
		/// French (Standard)
		/// </summary>
		public static readonly Languages French			= new Languages(5,		"fr",		"fr",	"fr"		);
		/// <summary>
		/// German (Standard)
		/// </summary>
		public static readonly Languages German			= new Languages(6,		"de",		"de",	"de"		);
		/// <summary>
		/// Spanish (Spain)
		/// </summary>
		public static readonly Languages Spanish		= new Languages(7,		"es",		"es",	"es"		);
		/// <summary>
		/// Italian (Standard)
		/// </summary>
		public static readonly Languages Italian		= new Languages(8,		"it",		"it",	"it"		);
		/// <summary>
		/// US English
		/// </summary>
		public static readonly Languages English		= new Languages(9,		"en",		"us",	"en-us"		);
		//public static readonly Languages English		= new Languages(9,		"en",		"us",	"English"	);
		public static readonly Languages Czech			= new Languages(10,		"cs",		"cz",	"cs"		);
		/// <summary>
		/// Japanese Kanji with Hiragana Characters
		/// </summary>
		public static readonly Languages Kanji			= new Languages(11,		"ja",		"jp",	"ja"		);
		/// <summary>
		/// Portuguese (Brazil)
		/// </summary>
		public static readonly Languages Portuguese 	= new Languages(13,		"pt-BR",	"br",	"pt-BR"		);
	}
}