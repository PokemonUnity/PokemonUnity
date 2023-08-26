using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.EventArg;

namespace PokemonUnity
{
	public partial class Game : IGameMetadataMisc
	{		
		#region Manipulation methods for metadata, phone data and Pokémon species data
		public IDictionary<int,IPokemonMetadata> pbLoadMetadata() {
			if (PokemonTemp == null) PokemonTemp=new PokemonTemp();
			if (PokemonTemp.pokemonMetadata == null) {
				//if (pbRgssExists("Data/metadata.dat") == null) {
				//	PokemonTemp.pokemonMetadata=new Dictionary<int,IPokemonMetadata>(); //IPokemonMetadata[0]
				//} else {
				//	PokemonTemp.pokemonMetadata=load_data("Data/metadata.dat");
				//}
			}
			return PokemonTemp.pokemonMetadata;
		}

		//public object pbGetMetadata(int mapid,int metadataType) {
		public IPokemonMetadata pbGetMetadata(int mapid) { //,int metadataType
			IDictionary<int,IPokemonMetadata> meta=pbLoadMetadata();
			//if (meta.ContainsKey(mapid) && meta[mapid] != null) return meta[mapid][metadataType];
			if (meta.ContainsKey(mapid) && meta[mapid] != null) return meta[mapid];
			return null;
		}

		public PokemonEssentials.Interface.Field.GlobalMetadata? pbGetMetadata(int mapid,GlobalMetadatas metadataType) {
			IDictionary<int,IPokemonMetadata> meta=pbLoadMetadata();
			if (meta[mapid] != null) return meta[mapid].Global; //[metadataType]
			return null;
		}

		public MapMetadata? pbGetMetadata(int mapid,MapMetadatas metadataType) {
			IDictionary<int,IPokemonMetadata> meta=pbLoadMetadata();
			if (meta[mapid] != null) return meta[mapid].Map; //[metadataType]
			return null;
		}

		public IList<int> pbLoadPhoneData() {
			if (PokemonTemp == null) PokemonTemp=new PokemonTemp();
			if (PokemonTemp.pokemonPhoneData == null) {
				//pbRgssOpen("Data/phone.dat","rb"){|f|
				//   PokemonTemp.pokemonPhoneData=Marshal.load(f);
				//}
			}
			return PokemonTemp.pokemonPhoneData;
		}

		public IList<string> pbOpenDexData(Func<IList<string>,IList<string>> block = null) {
			if (PokemonTemp == null) PokemonTemp=new PokemonTemp();
			if (PokemonTemp.pokemonDexData == null) {
				//pbRgssOpen("Data/dexdata.dat","rb"){|f|
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

		//public void pbDexDataOffset(IList<string> dexdata,Pokemons species,int offset) {
		//  dexdata.pos=76*(species-1)+offset;
		//}

		public void pbClearData() {
			if (PokemonTemp != null) {
				//PokemonTemp.pokemonDexData=null;
				//PokemonTemp.pokemonMetadata=null;
				//PokemonTemp.pokemonPhoneData=null;
				PokemonTemp.pokemonDexData.Clear();
				PokemonTemp.pokemonMetadata.Clear();
				PokemonTemp.pokemonPhoneData.Clear();
			}
			//MapFactoryHelper.clear(); //ToDo: Create static class and uncomment
			if (GameMap != null && PokemonEncounters != null) {
				PokemonEncounters.setup(GameMap.map_id);
			}
			//if (pbRgssExists("Data/Tilesets.rxdata")) {
			//  DataTilesets=load_data("Data/Tilesets.rxdata");
			//}
			//if (pbRgssExists("Data/Tilesets.rvdata")) {
			//  DataTilesets=load_data("Data/Tilesets.rvdata");
			//}
		}
		#endregion
	}
}