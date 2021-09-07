using UnityEngine;

public class Startup : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        //ToDo: resolve issues of init
        //InitSqlite(Application.dataPath + "/Data/database.sqlite");
        
    }

    void InitSqlite(string datapath)
    {
        PokemonUnity.Game.DatabasePath = $"Data Source=" + datapath;

        PokemonUnity.Game.ResetSqlConnection();

        PokemonUnity.Game.con.Open();

        DefaultInitSqlite();
    }

    void DefaultInitSqlite()
    {
        PokemonUnity.Game.InitTypes();
        PokemonUnity.Game.InitNatures();
        PokemonUnity.Game.InitPokemons();
        PokemonUnity.Game.InitPokemonForms();
        PokemonUnity.Game.InitPokemonMoves();
        PokemonUnity.Game.InitPokemonEvolutions();
        PokemonUnity.Game.InitPokemonItems();
        PokemonUnity.Game.InitMoves();
        //PokemonUnity.Game.InitMachines();
        PokemonUnity.Game.InitItems();
        PokemonUnity.Game.InitBerries();
        PokemonUnity.Game.InitTrainers();
        PokemonUnity.Game.InitRegions();
        PokemonUnity.Game.InitLocations();
    }
}
