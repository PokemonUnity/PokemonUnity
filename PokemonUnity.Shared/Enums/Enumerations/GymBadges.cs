namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Whatever gym badges you want to incorporate throughout your game.
	/// This is where the ID of the badges are stored, and the game can
	/// validate certain actions based on said ID or # of badges...
	/// </summary>
	public class GymBadges : Enumeration
	{
		protected GymBadges(int id, string name) : base(id, name) { }
	}
}