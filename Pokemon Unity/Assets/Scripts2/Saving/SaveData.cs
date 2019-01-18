using System;
using System.Collections.Generic;
using PokemonUnity.Item;
using PokemonUnity.Saving.Location;
using PokemonUnity.Saving.SerializableClasses;

namespace PokemonUnity.Saving
{

    [System.Serializable]
    public class SaveData
    {
        public string BuildVersion = SaveManager.GetBuildVersion();
        public string SaveName = string.Empty;

        public DateTime TimeCreated;

        #region User Settings
        public int Language;

        public byte WindowBorder;
        public byte DialogBorder;
        public byte TextSpeed;

        public float mVol;
        public float sVol;
        #endregion


        #region Player
        public string PlayerName;

        //PlayerID doesn't need to be public: TrainerID + SecretID * 65536 = PlayerID
        private int playerID;
        public int TrainerID;
        public int SecretID;
        public bool IsMale;
        //public Dictionary<GymBadges, System.DateTime?> GymBadges;
        #endregion

        public bool?[] Pokedex;
        public System.TimeSpan PlayerTime;

        //Player
        public SerializableVector3 PlayerPosition;
        public int PlayerDirection;
        //Follower
        public SerializableVector3 FollowerPosition;
        public int FollowerDirection;

        public int ActiveScene;

        #region Pokemon Center
        /// <summary>
        /// When the Player blacks out they need to return to the last visited Pokemon Center
        /// </summary>
        private int pCenterScene;
        private SerializableVector3 pCenterPosition;
        private int pCenterDirection;

        private SerializableVector3 pCenterFposition;
        private int pCenterFdirection;
        #endregion

        //These can now use the new Seri- classes
        public SeriPokemon[] PlayerParty;
        public SeriPC PC;
        public List<Items> PlayerBag;

        public List<SaveEvent> EventList;

        public void AddPokemonCenter(int scene, SerializableVector3 position, int direction, SerializableVector3 fPosition, int fDirection)
        {
            pCenterScene = scene;

            pCenterPosition = position;
            pCenterDirection = direction;

            pCenterFposition = fPosition;
            pCenterFdirection = fDirection;
        }

        public SaveData(
            string saveName,
            int activeScene,
            string playerName, int trainerID, int secretID, bool isMale,
            //List<GymBadges> gymBadges,
            bool?[] pokedex,
            System.TimeSpan playerTime,
            SerializableVector3 playerPosition, int playerDirection,
            SerializableVector3 followerPosition, int followerDirection,
            Pokemon.Pokemon[] playerParty, Pokemon.Pokemon[,] playerPC, List<Items> playerBag,
            List<SaveEvent> eventList
            )
        {
            //Important
            SaveName = saveName;
            ActiveScene = activeScene;

            //Player Information
            PlayerName = playerName;
            playerID = (trainerID + secretID) * 65536;
            TrainerID = trainerID;
            SecretID = secretID;
            IsMale = isMale;

            Pokedex = pokedex;
            PlayerTime = playerTime;

            //Player Vector3
            PlayerPosition = playerPosition;
            PlayerDirection = playerDirection;
            //Follower Vector3
            FollowerPosition = followerPosition;
            FollowerDirection = followerDirection;

            //Player Items and Pokemons
            PlayerParty = new SeriPokemon[playerParty.Length];
            for (int i = 0; i < PlayerParty.Length; i++)
            {
                PlayerParty[i] = playerParty[i];
            }
            PlayerBag = playerBag;

            //List of Registered Events
            EventList = eventList;
        }
    }
}
