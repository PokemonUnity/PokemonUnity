using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Battle;

namespace PokemonUnity
{
	public partial class Game : IGameMetadataMisc
	{
		#region Manipulation methods for metadata, phone data and Pokémon species data
		public IDictionary<int,IPokemonMetadata> LoadMetadata() {
			if (PokemonTemp == null) PokemonTemp=new PokemonTemp();
			if (PokemonTemp.pokemonMetadata == null) {
				//if (RgssExists("Data/metadata.dat") == null) {
				//	PokemonTemp.pokemonMetadata=new Dictionary<int,IPokemonMetadata>(); //IPokemonMetadata[0]
				//} else {
				//	PokemonTemp.pokemonMetadata=load_data("Data/metadata.dat");
				//}
				PokemonTemp.pokemonMetadata=new Dictionary<int,IPokemonMetadata>(); //IPokemonMetadata[0]
				Kernal.load_data(PokemonTemp.pokemonMetadata,"Data/metadata.dat");
			}
			return PokemonTemp.pokemonMetadata;
		}

		//public object GetMetadata(int mapid,int metadataType) {
		public IPokemonMetadata GetMetadata(int mapid) { //,int metadataType
			IDictionary<int,IPokemonMetadata> meta=LoadMetadata();
			//if (meta.ContainsKey(mapid) && meta[mapid] != null) return meta[mapid][metadataType];
			if (meta.ContainsKey(mapid) && meta[mapid] != null) return meta[mapid];
			return null;
		}

		//public PokemonEssentials.Interface.Field.GlobalMetadata? GetMetadata(int mapid,GlobalMetadatas metadataType) {
		//	IDictionary<int,IPokemonMetadata> meta=LoadMetadata();
		//	if (meta[mapid] != null) return meta[mapid].Global; //[metadataType]
		//	return null;
		//}

		//public MapMetadata? GetMetadata(int mapid,MapMetadatas metadataType) {
		//	IDictionary<int,IPokemonMetadata> meta=LoadMetadata();
		//	if (meta[mapid] != null) return meta[mapid].Map; //[metadataType]
		//	return null;
		//}

		public IList<IPhoneMessageData> LoadPhoneData() {
			if (PokemonTemp == null) PokemonTemp=new PokemonTemp();
			if (PokemonTemp.pokemonPhoneData == null) {
				//RgssOpen("Data/phone.dat","rb"){|f|
				//   PokemonTemp.pokemonPhoneData=Marshal.load(f);
				//}
			}
			return PokemonTemp.pokemonPhoneData;
		}

		public IList<string> OpenDexData(Func<IList<string>,IList<string>> block = null) {
			if (PokemonTemp == null) PokemonTemp=new PokemonTemp();
			if (PokemonTemp.pokemonDexData == null) {
				//RgssOpen("Data/dexdata.dat","rb"){|f|
				//   PokemonTemp.pokemonDexData=f.read();
				//}
			}
			if (block != null) { //PokemonTemp.pokemonDexData != null
				//StringInput.open(PokemonTemp.pokemonDexData) {|f| yield f }
				return block.Invoke(PokemonTemp.pokemonDexData);
			} else {
				//return StringInput.open(PokemonTemp.pokemonDexData);
				return PokemonTemp.pokemonDexData;
			}
		}

		//public void DexDataOffset(IList<string> dexdata,Pokemons species,int offset) {
		//  dexdata.pos=76*(species-1)+offset;
		//}

		public void ClearData() {
			if (PokemonTemp != null) {
				//PokemonTemp.pokemonDexData=null;
				//PokemonTemp.pokemonMetadata=null;
				//PokemonTemp.pokemonPhoneData=null;
				PokemonTemp.pokemonDexData.Clear();
				PokemonTemp.pokemonMetadata.Clear();
				PokemonTemp.pokemonPhoneData.Clear();
			}
			//MapFactoryHelper.clear(); //ToDo: Create static class and uncomment
			if (GameMap != null && GameMap is IGameMapOrgBattle gmo && PokemonEncounters != null) {
				PokemonEncounters.setup(gmo.map_id);
			}
			//if (RgssExists("Data/Tilesets.rxdata")) {
			//  DataTilesets=load_data("Data/Tilesets.rxdata");
			//}
			//if (RgssExists("Data/Tilesets.rvdata")) {
			//  DataTilesets=load_data("Data/Tilesets.rvdata");
			//}
		}
		#endregion
	}
}