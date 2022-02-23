//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace PokemonUnity
{
	public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase, IScreen
	{
		/// <summary>
		/// Does this scene have it's own dedicated "Dialog Window" for prompts?
		/// </summary>
		/// Not really sure if i need this, as i will manage this using interfaces...
		public abstract bool UseNativeDialog
		{
			get;
		}
	}
}
