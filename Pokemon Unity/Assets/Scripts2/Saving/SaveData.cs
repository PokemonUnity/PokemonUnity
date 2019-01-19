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
        #region ImportantInfo
        public string BuildVersion = SaveManager.GetBuildVersion();
        public string SaveName = string.Empty;
        public int ActiveScene;

        public DateTime TimeCreated = System.DateTime.Now;
        #endregion

        #region User Settings
        public int Language;

        public byte WindowBorder;
        public byte DialogBorder;
        public byte TextSpeed;

        //public bool FullScreen;
        //public bool BattleScene;

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

        #region PlayerInfo
        public bool?[] Pokedex;
        public System.TimeSpan PlayerTime;

        //Player
        public SerializableVector3 PlayerPosition;
        public int PlayerDirection;
        //Follower
        public SerializableVector3 FollowerPosition;
        public int FollowerDirection;
        #endregion

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

        #region SeriClasses
        public SeriPokemon[] PlayerParty;
        public SeriPC PC;
        public List<Items> PlayerBag;

        public List<SaveEvent> EventList;
        #endregion

        public void AddPokemonCenter(int scene, SerializableVector3 position, int direction, SerializableVector3 fPosition, int fDirection)
        {
            pCenterScene = scene;

            pCenterPosition = position;
            pCenterDirection = direction;

            pCenterFposition = fPosition;
            pCenterFdirection = fDirection;
        }

        public SaveData
            (
            string saveName,

            Settings.Languages language,
            byte windowBorder,
            byte dialogBorder,
            byte textSpeed,
            float mvol,
            float svol,

            string playerName,
            int trainerID,
            int secretID,
            bool isMale,

            bool?[] pokedex,
            TimeSpan playerTime,

            SerializableVector3 playerPosition,
            int playerDirection,
            SerializableVector3 followerPosition,
            int followerDirection,

            int activeScene,

            SeriPokemon[] playerParty,
            SeriPC pc,
            List<Items> playerBag,

            List<SaveEvent> eventList
            )
        {
            SaveName = saveName;

            Language = (int)language;
            WindowBorder = windowBorder;
            DialogBorder = dialogBorder;
            TextSpeed = textSpeed;
            mVol = mvol;
            sVol = svol;

            PlayerName = playerName;
            TrainerID = trainerID;
            SecretID = secretID;
            IsMale = isMale;

            Pokedex = pokedex;
            PlayerTime = playerTime;

            PlayerPosition = playerPosition;
            PlayerDirection = playerDirection;
            FollowerPosition = followerPosition;
            FollowerDirection = followerDirection;

            ActiveScene = activeScene;

            PlayerParty = playerParty;
            PC = pc;
            PlayerBag = playerBag;

            EventList = eventList;
        }
    }
}
