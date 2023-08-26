using System.Collections;

namespace PokemonUnity
{
	public static class GameExtension
	{
		public static PokemonUnity.Game SetScenes(this PokemonUnity.Game game, params PokemonEssentials.Interface.Screen.IScene[] scenes)
		{
			foreach (PokemonEssentials.Interface.Screen.IScene scene in scenes)
			{
				//ToDo: Using Game Singleton, Register and Assign Scenes to their appropriate variable entity (game object)
				//if (scene is PokemonEssentials.Interface.Screen.IPokemonEvolutionScene s)
				//	//ToDo: Maybe instead of using a static var, make it game specific? (dependent on the game var used)
				//	//Note: I don't believe application will be in a state of managing multiple game instance UIs from one source 
				//	//Maybe if doing game cast to sync with handheld game device for head-to-head as use case; but will device have separate install?
				//	// (i.e. one exe handles different simultaneous gameplays with UI in different states)
				//	Game.PokemonEvolutionScene = s;
			}
			return game;
		}
		//public static PokemonUnity.Game SetScenes(this PokemonUnity.Game game, params PokemonUnity.IScene[] scenes)
		//{
		//	foreach (PokemonUnity.IScene scene in scenes)
		//	{
		//		//ToDo: Using Game Singleton, Register and Assign Scenes to their appropriate variable entity (game object)
		//		if (scene is PokemonUnity.IPokemonEvolutionScene s)
		//			Game.PokemonEvolutionScene = s;
		//	}
		//	return game;
		//}
	}
}