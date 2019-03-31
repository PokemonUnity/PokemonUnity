namespace PokemonUnity.Overworld.Entity
{
	public abstract class BaseEntity
	{
		public enum EntityTypes
		{
			XmlEntity,
			Entity
		}

		private EntityTypes _entityType;

		/// <summary>
		/// Creates a new instance of the BaseEntity class.
		/// </summary>
		/// <param name="EntityType">The type of entity the super of this class is.</param>
		public BaseEntity(EntityTypes EntityType)
		{
			this._entityType = EntityType;
		}

	}
}