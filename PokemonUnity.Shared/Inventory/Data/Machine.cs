using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Inventory
{
	/// <summary>
	/// </summary
	public struct MachineData
	{
		/// <summary>
		/// Id of this Machine
		/// </summary>
		public int Id { get; private set; } 
		//public string Name { get; private set; }
		/// <summary>
		/// If this machine is a TM or HM (or possibly other)
		/// </summary>
		public MachineType Type { get; private set; }
		public Moves Move { get; private set; }
		/// <summary>
		/// The Region this TM or HM belongs to
		/// </summary>
		/// <remarks>
		/// In case you decide to region lock your TM#Id, 
		/// to give different Moves based on Region acquired from.
		/// </remarks>
		public Regions Region { get; private set; } //= "Johto";

		public MachineData(int id, Moves move, Regions region, MachineType type = MachineType.TechnicalMachine)
		{
			Id = id;
			Move = move;
			Region = region;
			Type = type;
		}

		//ToDo: Create an Interface and Move Function to Application Library
		//public string ToString(TextScripts text)
		//{
		//	if (text == TextScripts.Name)
		//	{
		//		if (Type == MachineType.TechnicalMachine)
		//			return string.Format("TM{0}", Id.ToString("00"));
		//		if (Type == MachineType.HiddenMachine)
		//			return string.Format("HM{0}", Id.ToString("00"));
		//		if (Type == MachineType.TechnicalRecord)
		//			return string.Format("TR{0}", Id.ToString("00"));
		//	}
		//	if (text == TextScripts.Description)
		//		return string.Format("Teaches {0} to a compatible Pokémon", Move.ToString(TextScripts.Name));
		//	else return null;
		//}

		public enum MachineType
		{
			TechnicalMachine,//TechniqueMove,
			HiddenMachine,//HiddenMove
			TechnicalRecord
		}
	}
}