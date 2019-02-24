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
        public string BuildVersion { get; private set; } //SaveManager.GetBuildVersion();
        public string SaveName { get; private set; }
		public int ActiveScene { get; private set; }

		public DateTime TimeCreated { get; private set; }
		#endregion

		#region User Settings
		public int Language { get; private set; }

		public byte WindowBorder { get; private set; }
		public byte DialogBorder { get; private set; }
		public byte TextSpeed { get; private set; }

		//public bool FullScreen { get; private set; }
		//public bool BattleScene { get; private set; }

		public float mVol { get; private set; }
		public float sVol { get; private set; }
		#endregion

		#region Player
		public string PlayerName { get; private set; }
		
        //public int playerID { get { return (TrainerID + SecretID) * 65536; } }
        public int TrainerID { get; private set; }
        public int SecretID { get; private set; }
        public bool IsMale { get; private set; }
        //public Dictionary<GymBadges, System.DateTime?> GymBadges;
        #endregion

        #region PlayerInfo
        public bool?[] Pokedex { get; private set; }
		/// <summary>
		/// Duration of time accumulated on save file
		/// </summary>
		public System.TimeSpan PlayerTime { get; private set; }

		//Player
		public SerializableVector3 PlayerPosition { get; private set; }
		public int PlayerDirection { get; private set; }
		//Follower
		/// <summary>
		/// </summary>
		/// i would really just make this an offset of player position, i.e. 1-back, 1-left. 
		/// don't think it's necessary to store full on x,y,z position of something that's always xy-distance away from player
		public SerializableVector3 FollowerPosition { get; private set; }
		/// <summary>
		/// </summary>
		/// Don't need to save follower direction... should just be the same direction player is facing...
		public int FollowerDirection { get; private set; }
		/// <summary>
		/// Which pokemon from player party is out in overworld with player
		/// </summary>
		public int FollowerPokemon { get; private set; }
		#endregion

		#region Pokemon Center
		/// <summary>
		/// When the Player blacks out they need to return to the last visited Pokemon Center
		/// </summary>
		public int pCenterScene { get; private set; }
		//private SerializableVector3 pCenterPosition;
		//private int pCenterDirection;

		//private SerializableVector3 pCenterFposition;
		//private int pCenterFdirection;
		#endregion

		#region SeriClasses
		public SeriPokemon[] PlayerParty { get; private set; }
		public SeriPC PC { get; private set; }
		public List<Items> PlayerBag { get; private set; }

		public List<SaveEvent> EventList { get; private set; }
		#endregion

		//public void AddPokemonCenter(int scene, SerializableVector3 position, int direction, SerializableVector3 fPosition, int fDirection)
		//{
		//    pCenterScene = scene;
		//
		//    pCenterPosition = position;
		//    pCenterDirection = direction;
		//
		//    pCenterFposition = fPosition;
		//    pCenterFdirection = fDirection;
		//}

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

            Pokemon.Pokemon[] playerParty,
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

            PlayerParty = new SeriPokemon[playerParty.Length];
            for (int i = 0; i < playerParty.Length; i++)
            {
                PlayerParty[i] = playerParty[i];
            }
            PC = pc;
            PlayerBag = playerBag;

            EventList = eventList;
        }
    }
}
