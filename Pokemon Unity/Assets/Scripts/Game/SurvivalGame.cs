//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace PokemonUnity
{
    public class SurvivalGame : GameBase
    {
        private float m_ElapseSeconds = 0f;

        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            if (m_ElapseSeconds >= 1f)
            {
                m_ElapseSeconds = 0f;
                //IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
                //float randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)GameFramework.Utility.Random.GetRandomDouble();
                //float randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)GameFramework.Utility.Random.GetRandomDouble();
                //GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + GameFramework.Utility.Random.GetRandom(dtAsteroid.Count))
                //{
                //    Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                //});
            }
        }
    }
}
