//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Network;
//using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public class SCHeartBeatHandler : PacketHandlerBase
	{
		public override int Id
		{
			get
			{
				return 2;
			}
		}

		public override void Handle(object sender, Packet packet)
		{
			SCHeartBeat packetImpl = (SCHeartBeat)packet;
			//Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
		}
	}
}
