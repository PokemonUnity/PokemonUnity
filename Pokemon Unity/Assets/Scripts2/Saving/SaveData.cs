using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Saving
{
    using PokemonUnity.Pokemon;
    using PokemonUnity.Item;
    using PokemonUnity.Saving.Location;

    [System.Serializable]
    public class SaveData
    {
        public string BuildVersion = GlobalSaveManager.GetBuildVersion();
        public string SaveName = string.Empty;

        public DateTime TimeCreated;

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

        public Pokemon[] PlayerParty;
        public Pokemon[,] PC;
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
            SerializableVector3 playerPosition, int playerDirection,
            SerializableVector3 followerPosition, int followerDirection,
            Pokemon[] playerParty, Pokemon[,] playerPC, List<Items> playerBag,
            List<SaveEvent> eventList
            )
        {
            SaveName = saveName;
            ActiveScene = activeScene;

            PlayerPosition = playerPosition;
            PlayerDirection = playerDirection;

            FollowerPosition = followerPosition;
            FollowerDirection = followerDirection;

            PlayerParty = playerParty;
            PC = playerPC;
            PlayerBag = playerBag;

            EventList = eventList;
        }
    }
}
