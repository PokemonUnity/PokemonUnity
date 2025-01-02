namespace PokemonEssentials.Interface
{
	public interface ISceneMap 
	{
		IGameMap spriteset { get; }

		void disposeSpritesets();

		void createSpritesets();

		void updateMaps();

		void updateSpritesets();

		void main();

		void miniupdate();

		void update();

		void call_name();

		void call_menu();

		void call_debug();

		void autofade(int mapid);

		void transfer_player(bool cancelVehicles = true);
	}
}