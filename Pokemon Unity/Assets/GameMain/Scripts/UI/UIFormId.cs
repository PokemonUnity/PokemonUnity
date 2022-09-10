//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace PokemonUnity
{
	/// <summary>
	/// 界面编号。
	/// </summary>
	public enum UIFormId
	{
		Undefined = 0,

		/// <summary>
		/// 弹出框。
		/// </summary>
		DialogForm = 1,

		/// <summary>
		/// 主菜单。
		/// </summary>
		MenuForm = 100,

		/// <summary>
		/// 设置。
		/// </summary>
		SettingForm = 101,

		/// <summary>
		/// 关于。
		/// </summary>
		AboutForm = 102,
		
		//Pokemon UI Forms
		ProfOakIntro,	// Select Gender / Set Name
		HomeScreen,		// New Game, Continue
		Dialog,
		Typing,			// Set Player/Pokemon/PC-Box Name
		Evolution,
		Battle,
		Summary,		// Pokemon Profile, ???, Stats/Graph, Moves, Ribbon/Contest, Forget/Learn Move
		Pokedex,
		Trainer,
		Bag,
		Party,
		Pause,
		PC,
		Settings
	}
}
