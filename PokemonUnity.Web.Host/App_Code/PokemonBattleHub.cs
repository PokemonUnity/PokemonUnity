using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using Microsoft.AspNet.SignalR;

namespace PokemonUnity.Web
{
	public class PokemonUnityHub : Hub
	{
		private static readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);
		private static readonly ConcurrentDictionary<string, HashSet<string>> _trainerRooms = new ConcurrentDictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
		private static readonly ConcurrentDictionary<string, BattleRoom> _rooms = new ConcurrentDictionary<string, BattleRoom>(StringComparer.OrdinalIgnoreCase);
		//private static readonly ConcurrentDictionary<string, Trainer> _trainers = new ConcurrentDictionary<string, Trainer>(StringComparer.OrdinalIgnoreCase);
		//private static readonly ConcurrentDictionary<string, ChatUser> _users = new ConcurrentDictionary<string, ChatUser>(StringComparer.OrdinalIgnoreCase);
		private static readonly ConcurrentDictionary<string, HashSet<string>> _userRooms = new ConcurrentDictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);


		#region Methods
		/// <summary>
		/// Provides the handler for SignalR OnConnected event
		/// supports async threading
		/// </summary>
		/// <returns></returns>
		public override Task OnConnected()
		{
			string profileId = Context.QueryString["id"]; //"111";
			string connectionId = Context.ConnectionId;
			User user = _users.GetOrAdd(profileId, _ => new User
			{
				ProfileId = !string.IsNullOrEmpty(profileId) ? profileId : new Guid().ToString(),
				ConnectionIds = new HashSet<string>()
			});
			lock (user.ConnectionIds)
			{
				user.ConnectionIds.Add(connectionId);
				Groups.Add(connectionId, user.ProfileId);
			}
			return base.OnConnected();
		}

		/// <summary>
		/// Provides the handler for SignalR OnDisconnected event
		/// supports async threading
		/// </summary>
		/// <returns></returns>
		public override Task OnDisconnected(bool stopCalled)
		{
			string profileId = Context.QueryString["id"];
			string connectionId = Context.ConnectionId;
			User user = 
				_users.Values.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
				//_users.TryGetValue(profileId, out user);
				//_users.GetOrAdd(profileId, _ => new User
				//{
				//	ProfileId = profileId, //!string.IsNullOrEmpty(profileId) ? profileId : new Guid().ToString(),
				//	ConnectionIds = new HashSet<string>()
				//});
			/*if(stopCalled)
			{
				// We know that Stop() was called on the client,
				// and the connection shut down gracefully.
			} else
			{
				// This server hasn't heard from the client in the last ~35 seconds.
				// If SignalR is behind a load balancer with scale-out configured, 
				// the client may still be connected to another SignalR server.
			}*/
			if (user != null)
			{
				User ignoredUser;
				//_users.TryRemove(user.Name, out ignoredUser);
				_users.TryRemove(user.Trainer.secretID().ToString(), out ignoredUser);

				// Leave all rooms
				HashSet<string> rooms;
				//if (_userRooms.TryGetValue(user.Name, out rooms))
				if (_userRooms.TryGetValue(user.Trainer.secretID().ToString(), out rooms))
				{
					foreach (var room in rooms)
					{
						Clients.Group(room).leave(user);
						BattleRoom chatRoom = _rooms[room];
						//chatRoom.Users.Remove(user.Name);
						chatRoom.Users.Remove(user.Trainer.secretID().ToString());
					}
				}

				HashSet<string> ignoredRoom;
				//_userRooms.TryRemove(user.Name, out ignoredRoom);
				_userRooms.TryRemove(user.Trainer.secretID().ToString(), out ignoredRoom);
				//lock (user.ConnectionIds)
				//{
				//	user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
				//	Groups.Remove(connectionId, user.ProfileId);
				//	//if (!user.ConnectionIds.Any())
				//	//{
				//	//	User removedUser;
				//	//	Users.TryRemove(profileId, out removedUser);
				//	//}
				//}
			}
			return base.OnDisconnected(stopCalled);
		}

		/// <summary>
		/// Provides the handler for SignalR OnReconnected event
		/// supports async threading
		/// </summary>
		/// <returns></returns>
		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}

		public bool Join()
		{
			// Check the user id cookie
			Microsoft.AspNet.SignalR.Cookie userIdCookie;

			if (!Context.RequestCookies.TryGetValue("userid", out userIdCookie))
			{
				return false;
			}

			User user = _users.Values.FirstOrDefault(u => u.Id == userIdCookie.Value);

			if (user != null)
			{
				// Update the users's client id mapping
				user.ConnectionId = Context.ConnectionId;

				// Set some client state
				Clients.Caller.id = user.Id;
				Clients.Caller.name = user.Name;
				Clients.Caller.hash = user.Hash;

				// Leave all rooms
				HashSet<string> rooms;
				if (_userRooms.TryGetValue(user.Name, out rooms))
				{
					foreach (var room in rooms)
					{
						Clients.Group(room).leave(user);
						ChatRoom chatRoom = _rooms[room];
						chatRoom.Users.Remove(user.Name);
					}
				}

				_userRooms[user.Name] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

				// Add this user to the list of users
				Clients.Caller.addUser(user);
				return true;
			}

			return false;
		}

		public bool Join(string newRoom)
		{
			// Check the user id cookie
			Microsoft.AspNet.SignalR.Cookie userIdCookie;

			if (!Context.RequestCookies.TryGetValue("userid", out userIdCookie))
			{
				return false;
			}

			User user = _users.Values.FirstOrDefault(u => u.Id == userIdCookie.Value);

			if (user != null)
			{
				string room = Clients.Caller.room;
				string name = Clients.Caller.name;

				BattleRoom chatRoom;
				// Create the room if it doesn't exist
				if (!_rooms.TryGetValue(newRoom, out chatRoom))
				{
					chatRoom = new BattleRoom();
					_rooms.TryAdd(newRoom, chatRoom);
				}

				// Remove the old room
				if (!string.IsNullOrEmpty(room))
				{
					_userRooms[name].Remove(room);
					_rooms[room].Users.Remove(name);

					Clients.Group(room).leave(_users[name]);
					Groups.Remove(Context.ConnectionId, room);
				}

				_userRooms[name].Add(newRoom);
				if (!chatRoom.Users.Add(name))
				{
					throw new InvalidOperationException("You're already in that room!");
				}

				Clients.Group(newRoom).addUser(_users[name]);

				// Set the room on the caller
				Clients.Caller.room = newRoom;

				Groups.Add(Context.ConnectionId, newRoom);

				Clients.Caller.refreshRoom(newRoom);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Provides the facility to send individual user notification message
		/// </summary>
		/// <param name="profileId">
		/// Set to the ProfileId of user who will receive the notification
		/// </param>
		/// <param name="message">
		/// set to the notification message
		/// </param>
		public void Send(string profileId, string message)
		{
			//Clients.User(profileId).send(message);
		}

		//public void Send(string name, string message)
		//{
		//	// Call the broadcastMessage method to update clients.
		//	Clients.All.broadcastMessage(name, message);
		//}

		/// <summary>
		/// Provides the facility to send group notification message
		/// </summary>
		/// <param name="username">
		/// set to the user grouped name who will receive the message
		/// </param>
		/// <param name="message">
		/// set to the notification message
		/// </param>
		//public void SendUserMessage(string username, string message)
		public void SendServerMessage(string groupname, string message)
		{
			Clients.Group(groupname).serverMessage(message);
		}

		public IEnumerable<User> GetUsers()
		{
			string room = Clients.Caller.room;

			if (string.IsNullOrEmpty(room))
			{
				return Enumerable.Empty<User>();
			}

			return from name in _rooms[room].Users
				   select _users[name];
		}

		/// <summary>
		/// Provides the ability to get User from the dictionary for passed in profileId
		/// </summary>
		/// <param name="profileId">
		/// set to the profileId of user that need to be fetched from the dictionary
		/// </param>
		/// <returns>
		/// return User object if found otherwise returns null
		/// </returns>
		private User GetUser(string profileId)
		{
			User user;
			_users.TryGetValue(profileId, out user);
			return user;
		}

		private string GetMD5Hash(string name)
		{
			return string.Join("", System.Security.Cryptography.MD5.Create()
						 .ComputeHash(Encoding.Default.GetBytes(name))
						 .Select(b => b.ToString("x2")));
		}

		/// <summary>
		/// Provide the ability to get currently connected user
		/// </summary>
		/// <returns>
		/// profileId of user based on current connectionId
		/// </returns>
		//public IEnumerable<string> GetConnectedUser()
		//{
		//	return Users.Where(x =>
		//	{
		//		lock (x.Value.ConnectionIds)
		//		{
		//			return !x.Value.ConnectionIds.Contains(Context.ConnectionId, StringComparer.InvariantCultureIgnoreCase);
		//		}
		//	}).Select(x => x.Key);
		//}
		#endregion
	}

	[Serializable]
	public class User
	{
		#region Constructor
		public User()
		{
			// TODO: Add constructor logic here
			Id = Guid.NewGuid().ToString("d");
		}
		#endregion

		#region Properties
		public string Id { get; set; }
		/// <summary>
		/// Property to get/set ProfileId
		/// </summary>
		public string ProfileId { get; set; }

		/// <summary>
		/// </summary>
		public ITrainer Trainer { get; set; }

		/// <summary>
		/// Property to get/set multiple ConnectionId
		/// </summary>
		/// Multiple ConnectionId is only good if user can log in from multiple devices/terminals
		//public HashSet<string> ConnectionIds { get; set; }
		public string ConnectionId { get; set; }
		#endregion
	}

	public class BattleRoom
	{
		private IBattle _battle;
		private IPokeBattle_DebugSceneNoGraphics _scene;
		public string RoomName { get; set; }
		//public IList<string> Messages { get { return Message.Text.Split(' '); } }
		//public List<ChatMessage> Messages { get; set; }
		public HashSet<string> Users { get; set; } //Active Players and Spectators
		public KeyValuePair<int, string>[] Trainers { get; private set; }

		public BattleRoom()
		{
			//Messages = new List<ChatMessage>();
			//Message = new ChatMessage("0", string.Join(" ",Messages.Select(x => x.Text)));
			Users = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}
	}

	/// <summary>
	/// Battle scene for All clients viewing current battle, and receiving broadcast data
	/// </summary>
	public class OnlinePokeBattleScene : IPokeBattle_DebugSceneNoGraphics, IPokeBattle_SceneNonInteractive //IPokeBattle_Scene,
	{
		private readonly IHubContext context = GlobalHost.ConnectionManager.GetHubContext<PokemonUnityHub>();
		private PokemonEssentials.Interface.PokeBattle.IBattle battle;
		private bool aborted;
		private bool abortable;
		private MenuCommands[] lastcmd;
		private int[] lastmove;
		private int messageCount = 0;

		/// <summary>
		/// Scene Id
		/// </summary>
		public int Id { get { return 0; } }

		public OnlinePokeBattleScene()
		{
			initialize();
		}

		public IPokeBattle_DebugSceneNoGraphics initialize()
		{
			battle = null;
			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			lastmove = new int[] { 0, 0, 0, 0 };
			//@pkmnwindows = new GameObject[] { null, null, null, null };
			//@sprites = new Dictionary<string, GameObject>();
			//@battlestart = true;
			//@messagemode = false;
			abortable = true;
			aborted = false;

			return this;
		}

		public void pbDisplay(string v)
		//void IHasDisplayMessage.pbDisplay(string v)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//GameDebug.Log(v);
			System.Console.WriteLine(v);
			//return context.Clients.All.RecieveNotification();
			return context.Clients.Group(message.Group).RecieveNotification();
		}

		void IPokeBattle_DebugSceneNoGraphics.pbDisplayMessage(string msg, bool brief)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbDisplay(msg);
			@messageCount += 1;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbDisplayPausedMessage(string msg)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbDisplay(msg);
			@messageCount += 1;
		}

		bool IPokeBattle_DebugSceneNoGraphics.pbDisplayConfirmMessage(string msg)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbDisplay(msg);
			@messageCount += 1;

			System.Console.WriteLine("Y/N?");
			bool appearing = true;
			bool result = false;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.Y)
				{
					appearing = false;
					result = true;
				}
				else if (fs.Key == ConsoleKey.N)
				{
					appearing = false;
					result = false;
				}
			} while (appearing);
			return result;
		}

		bool IHasDisplayMessage.pbDisplayConfirm(string v)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			return (this as IPokeBattle_DebugSceneNoGraphics).pbDisplayConfirmMessage(v);
		}

		bool IPokeBattle_DebugSceneNoGraphics.pbShowCommands(string msg, string[] commands, bool defaultValue)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			GameDebug.Log(msg);
			@messageCount += 1;
			return false;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbShowCommands(string msg, string[] commands, int defaultValue)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			GameDebug.Log(msg);
			@messageCount += 1;
			return 0;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBeginCommandPhase()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (@messageCount > 0)
			{
				GameDebug.Log($"[message count: #{@messageCount}]");
			}
			@messageCount = 0;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbStartBattle(PokemonEssentials.Interface.PokeBattle.IBattle battle)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			this.battle = battle;
			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			lastmove = new int[] { 0, 0, 0, 0 };
			@messageCount = 0;

			if (battle.player?.Length == 1)
			{
				GameDebug.Log("One player battle!");
			}

			if (battle.opponent != null)
			{
				GameDebug.Log("Opponent found!");
				if (battle.opponent.Length == 1)
				{
					GameDebug.Log("One opponent battle!");
				}
				if (battle.opponent.Length > 1)
				{
					GameDebug.Log("Multiple opponents battle!");
				}
				else
					GameDebug.Log("Wild Pokemon battle!");
			}

			if (battle.player?.Length > 0 && battle.opponent?.Length > 0 && !battle.doublebattle)
			{
				GameDebug.Log("Single Battle");
				System.Console.WriteLine("Player: {0} has {1} in their party", battle.player[0].name, battle.party1.Length);
				System.Console.WriteLine("Opponent: {0} has {1} in their party", battle.opponent?[0].name, battle.party2.Length);
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.pbEndBattle(BattleResults result)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbTrainerSendOut(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbSendOut(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbTrainerWithdraw(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbWithdraw(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		int IPokeBattle_DebugSceneNoGraphics.pbForgetMove(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, Moves moveToLearn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IMove[] moves = pokemon.moves;
			string[] commands = new string[4] {
			   pbMoveString(moves[0], 1),
			   pbMoveString(moves[1], 2),
			   pbMoveString(moves[2], 3),
			   pbMoveString(moves[3], 4) };
			for (int i = 0; i < commands.Length; i++)
			{
				System.Console.WriteLine(commands[i]);
			}
			System.Console.WriteLine("Press 0 to Cancel");
			bool appearing = true;
			do 
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D0)
				{
					appearing = false;
					return -1;
				}
				else if (fs.Key == ConsoleKey.D1)
				{
					appearing = false;
					return 0;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					appearing = false;
					return 1;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					appearing = false;
					return 2;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					appearing = false;
					return 3;
				}
			} while (appearing);

			return -1;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBeginAttackPhase()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		}

		int IPokeBattle_DebugSceneNoGraphics.pbCommandMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool shadowTrainer = //(hasConst(PBTypes,:SHADOW) && //Game has shadow pokemons
				//@battle.opponent != null;
				battle.battlers[index] is IPokemonShadowPokemon p && p.hypermode;

			System.Console.WriteLine("Enemy: {0} HP: {1}/{2}", battle.battlers[index].pbOpposing1.Name, battle.battlers[index].pbOpposing1.HP, battle.battlers[index].pbOpposing1.TotalHP);
			if (battle.battlers[index].pbOpposing2.IsNotNullOrNone()) 
				System.Console.WriteLine("Enemy: {0} HP: {1}/{2}", battle.battlers[index].pbOpposing2.Name, battle.battlers[index].pbOpposing2.HP, battle.battlers[index].pbOpposing2.TotalHP);

			System.Console.WriteLine("What will {0} do?", battle.battlers[index].Name);
			System.Console.WriteLine("Fight - 0");
			System.Console.WriteLine("Bag - 1");
			System.Console.WriteLine("Pokémon - 2");
			System.Console.WriteLine(shadowTrainer ? "Call - 3" : "Run - 3");

			bool appearing = true;
			int result = -1;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);
				if (fs.Key == ConsoleKey.D0)
				{
					result = 0;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D1)
				{
					result = 1;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					result = 2;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					if (shadowTrainer)
						result = 4;
					else
						result = 3;
					appearing = false;
				}
			}
			while (appearing);

			//GameDebug.LogError("Invalid Input!");

			return result;
			//if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
			//return ret;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbFightMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IBattleMove[] moves = @battle.battlers[index].moves;
			string[] commands = new string[4] {
			   pbMoveString(moves[0].thismove, 1),
			   pbMoveString(moves[1].thismove, 2),
			   pbMoveString(moves[2].thismove, 3),
			   pbMoveString(moves[3].thismove, 4) };
			int index_ = @lastmove[index];
			for (int i = 0; i < commands.Length; i++)
			{
				System.Console.WriteLine(commands[i]);
			}
			System.Console.WriteLine("Press Q to return back to Command Menu");
			bool appearing = true;
			int result = -2;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D1)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 0;
					GameDebug.Log($"int=#{result}, pp=#{moves[result].PP}");
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 1;
					GameDebug.Log($"int=#{result}, pp=#{moves[result].PP}");
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 2;
					GameDebug.Log($"int=#{result}, pp=#{moves[result].PP}");
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 3;
					GameDebug.Log($"int=#{result}, pp=#{moves[result].PP}");
				}
				else if (fs.Key == ConsoleKey.Q)
				{
					appearing = false;
					result = -1; //CANCEL FIGHT MENU
				}
			} while (appearing && (result == -2 || battle.battlers[index].moves[result].id == Moves.NONE));

			return result;
		}

		Items IPokeBattle_DebugSceneNoGraphics.pbItemMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//System.Console.WriteLine("Need to implement item system in textbased-line");
			return Items.NONE;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//Doesnt include multiple targets at once...
			List<int> targets = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				//if (@battle.battlers[index].pbIsOpposing(i) &&
				//   !@battle.battlers[i].isFainted()) targets.Add(i);
				if (!@battle.battlers[i].isFainted())
					if ((targettype == Targets.RANDOM_OPPONENT
						//|| targettype == Targets.ALL_OPPONENTS
						//|| targettype == Targets.OPPONENTS_FIELD
						|| targettype == Targets.SELECTED_POKEMON
						|| targettype == Targets.SELECTED_POKEMON_ME_FIRST) &&
						@battle.battlers[index].pbIsOpposing(i))
						targets.Add(i);
					else if ((targettype == Targets.ALLY
						//|| targettype == Targets.USERS_FIELD
						//|| targettype == Targets.USER_AND_ALLIES
						|| targettype == Targets.USER_OR_ALLY) &&
						!@battle.battlers[index].pbIsOpposing(i))
						targets.Add(i);
			}
			if (targets.Count == 0) return -1;
			//return targets[Core.Rand.Next(targets.Count)];

			for (int i = 0; i < targets.Count; i++)
			{
				System.Console.WriteLine("Target {0}: {1} HP: {2}/{3} => {4}", targets[i] % 2 == 1 ? "Enemy" : "Ally", battle.battlers[targets[i]].Name, battle.battlers[targets[i]].HP, battle.battlers[targets[i]].TotalHP, i);
			}
			bool appearing = true;
			int result = 0;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D1)
				{
					appearing = false;
					result = 0;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					appearing = false;
					result = 1;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					appearing = false;
					result = 2;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					appearing = false;
					result = 3;
				}
			} while (appearing && targets.Contains(result));

			return result;
		}

		public void pbRefresh()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//int IPokeBattle_DebugSceneNoGraphics.pbSwitch(int index, bool lax, bool cancancel)
		int IPokeBattle_SceneNonInteractive.pbSwitch(int index, bool lax, bool cancancel)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IPokemon[] party = @battle.pbParty(index);
			IList<string> commands = new List<string>();
			bool[] inactives = new bool[] { true, true, true, true, true, true };
			IList<int> partypos = new List<int>();
			//int activecmd = 0; //if cursor is on first or second pokemon when viewing ui
			int numactive = battle.doublebattle ? 2 : 1;
			IBattler battler = @battle.battlers[0];
			//commands[commands.Count] = pbPokemonString(party[battler.pokemonIndex]);
			commands.Add(pbPokemonString(party[battler.pokemonIndex]));
			//if (battler.Index == index) activecmd = 0;
			inactives[battler.pokemonIndex] = false;
			//partypos[partypos.Count] = battler.pokemonIndex;
			partypos.Add(battler.pokemonIndex);
			if (@battle.doublebattle)
			{
				battler = @battle.battlers[2];
				//commands[commands.Count] = pbPokemonString(party[battler.pokemonIndex]);
				commands.Add(pbPokemonString(party[battler.pokemonIndex]));
				//if (battler.Index == index) activecmd = 1;
				inactives[battler.pokemonIndex] = false;
				//partypos[partypos.Count] = battler.pokemonIndex;
				partypos.Add(battler.pokemonIndex);
			}
			for (int i = 0; i < party.Length; i++)
			{
				if (inactives[i])
				{
					//commands[commands.Count] = pbPokemonString(party[i]);
					commands.Add(pbPokemonString(party[i]));
					//System.Console.WriteLine(pbPokemonString(party[i]));
					//partypos[partypos.Count] = i;
					partypos.Add(i);
				}
			}
			for (int i = 0; i < commands.Count; i++)
			{
				System.Console.WriteLine("Press {0} => {1}",i+1,commands[i]);
			}
			System.Console.WriteLine("Press Q to return back to Command Menu");
			bool appearing = true;
			int ret = -2;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);
				bool canswitch = false; int pkmnindex = -1;
				if (fs.Key == ConsoleKey.D1)
				{
					pkmnindex = partypos[0];
					canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					pkmnindex = partypos[1];
					canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					pkmnindex = partypos[2];
					canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					pkmnindex = partypos[3];
					canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D5)
				{
					pkmnindex = partypos[4];
					canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D6)
				{
					pkmnindex = partypos[5];
					canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.Q && cancancel)
				{
					appearing = false;
					ret = -1; //CANCEL POKEMON MENU
				}
			} while (appearing && (ret == -2 || ret == -2 || inactives[ret]));//!battle.pbParty(index)[ret].IsNotNullOrNone()

			return ret;
		}

		//public IEnumerator pbHPChanged(PokemonEssentials.Interface.PokeBattle.IBattler pkmn, int oldhp, bool animate)
		void IPokeBattle_DebugSceneNoGraphics.pbHPChanged(IBattler pkmn, int oldhp, bool anim)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			int hpchange = pkmn.HP - oldhp;
			if (hpchange < 0)
			{
				hpchange = -hpchange;
				GameDebug.Log($"[HP change] #{pkmn.ToString()} lost #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
			}
			else
			{
				GameDebug.Log($"[HP change] #{pkmn.ToString()} gained #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
			}
			pbRefresh();

			//System.Console.WriteLine("[HP Changed] {0}: oldhp: {1} and animate: {2}", pkmn.Name, oldhp, animate.ToString());
			//System.Console.WriteLine("[HP Changed] {0}: CurrentHP: {1}", pkmn.Name, pkmn.HP);

			//yield return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbFainted(IBattler pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.pbChooseEnemyCommand(int index)
		void IPokeBattle_SceneNonInteractive.pbChooseEnemyCommand(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) b.pbDefaultChooseEnemyCommand(index);
		}

		//void IPokeBattle_DebugSceneNoGraphics.pbChooseNewEnemy(int index, IPokemon[] party)
		int IPokeBattle_SceneNonInteractive.pbChooseNewEnemy(int index, IPokemon[] party)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) return b.pbDefaultChooseNewEnemy(index, party);
			return -1;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbWildBattleSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbTrainerBattleSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbEXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbLevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		int IPokeBattle_DebugSceneNoGraphics.pbBlitz(int keys)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			return battle.pbRandom(30);
		}

		void ISceneHasChatter.pbChatter(PokemonEssentials.Interface.PokeBattle.IBattler attacker, PokemonEssentials.Interface.PokeBattle.IBattler opponent)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.pbChatter(IBattler attacker, IBattler opponent)
		//{
		//	GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	(this as ISceneHasChatter).pbChatter(attacker, opponent);
		//}

		void IPokeBattle_DebugSceneNoGraphics.pbShowOpponent(int opp)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbHideOpponent()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbRecall(int battlerindex)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbDamageAnimation(IBattler pkmn, TypeEffective effectiveness)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//GameDebug.Log($"[Judgment] #{b1.ToString()}:#{r1.Inspect()}, #{b2.ToString()}:#{r2.Inspect()}");
			GameDebug.Log($"[Judgment] #{b1.ToString()}:#[{r1.JoinAsString(", ")}], #{b2.ToString()}:#[{r2.JoinAsString(", ")}]");
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBattleArenaBattlers(IBattle b1, IBattle b2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			GameDebug.Log($"[#{b1.ToString()} VS #{b2.ToString()}]");
		}

		void IPokeBattle_DebugSceneNoGraphics.pbCommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (attacker.IsNotNullOrNone())
			{
				if (opponent.IsNotNullOrNone())
				{
					GameDebug.Log($"[pbCommonAnimation] #{moveid}, #{attacker.ToString()}, #{opponent.ToString()}");
				}
				else
				{
					GameDebug.Log($"[pbCommonAnimation] #{moveid}, #{attacker.ToString()}");
				}
			}
			else
			{
				GameDebug.Log($"[pbCommonAnimation] #{moveid}");
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.pbAnimation(Moves moveid, PokemonEssentials.Interface.PokeBattle.IBattler user, PokemonEssentials.Interface.PokeBattle.IBattler target, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("{0} attack {1} With {2} for {3} hit times", user.Name, target.Name, moveid.ToString(), hitnum);

			if (user.IsNotNullOrNone())
			{
				if (target.IsNotNullOrNone())
				{
					GameDebug.Log($"[pbAnimation] #{user.ToString()}, #{target.ToString()}");
				}
				else
				{
					GameDebug.Log($"[pbAnimation] #{user.ToString()}");
				}
			}
			else
			{
				GameDebug.Log($"[pbAnimation]");
			}
		}

		#region Non Interactive Battle Scene
		int IPokeBattle_SceneNonInteractive.pbCommandMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (battle.pbRandom(15) == 0) return 1;
			//return 0;
			return (this as IPokeBattle_DebugSceneNoGraphics).pbCommandMenu(index);
		}

		int IPokeBattle_SceneNonInteractive.pbFightMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//IBattler battler = @battle.battlers[index];
			//int i = 0;
			//do {
			//	i = Core.Rand.Next(4);
			//} while (battler.moves[i].id==0);
			//GameDebug.Log($"i=#{i}, pp=#{battler.moves[i].PP}");
			////PBDebug.flush;
			//return i;
			return (this as IPokeBattle_DebugSceneNoGraphics).pbFightMenu(index);
		}

		Items IPokeBattle_SceneNonInteractive.pbItemMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//return -1;
			return (this as IPokeBattle_DebugSceneNoGraphics).pbItemMenu(index);
		}

		int IPokeBattle_SceneNonInteractive.pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//List<int> targets = new List<int>();
			//for (int i = 0; i < 4; i++)
			//{
			//	if (@battle.battlers[index].pbIsOpposing(i) &&
			//	   !@battle.battlers[i].isFainted())
			//	{
			//		targets.Add(i);
			//	}
			//}
			//if (targets.Count == 0) return -1;
			//return targets[Core.Rand.Next(targets.Count)];
			return (this as IPokeBattle_DebugSceneNoGraphics).pbChooseTarget(index, targettype);
		}

		/*int IPokeBattle_SceneNonInteractive.pbSwitch(int index, bool lax, bool cancancel)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			for (int i = 0; i < @battle.pbParty(index).Length - 1; i++)
			{
				if (lax)
				{
					if (@battle.pbCanSwitchLax(index, i, false)) return i;
				}
				else
				{
					if (@battle.pbCanSwitch(index, i, false)) return i;
				}
			}
			return -1;
		}

		void IPokeBattle_SceneNonInteractive.pbChooseEnemyCommand(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_SceneNonInteractive.pbChooseNewEnemy(int index, IPokemon[] party)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}*/
		#endregion

		private string pbPokemonString(IPokemon pkmn)
		{
			string status = string.Empty;
			if (pkmn.HP <= 0)
			{
				status = " [FNT]";
			}
			else
			{
				switch (pkmn.Status)
				{
					case Status.SLEEP:
						status = " [SLP]";
						break;
					case Status.FROZEN:
						status = " [FRZ]";
						break;
					case Status.BURN:
						status = " [BRN]";
						break;
					case Status.PARALYSIS:
						status = " [PAR]";
						break;
					case Status.POISON:
						status = " [PSN]";
						break;
				}
			}
			return $"#{pkmn.Name} (Lv. #{pkmn.Level})#{status} HP: #{pkmn.HP}/#{pkmn.TotalHP}";
		}
		
		private string pbPokemonString(IBattler pkmn)
		{
			if (!pkmn.pokemon.IsNotNullOrNone())
			{
				return "";
			}
			string status = string.Empty;
			if (pkmn.HP <= 0)
			{
				status = " [FNT]";
			}
			else
			{
				switch (pkmn.Status)
				{
					case Status.SLEEP:
						status = " [SLP]";
						break;
					case Status.FROZEN:
						status = " [FRZ]";
						break;
					case Status.BURN:
						status = " [BRN]";
						break;
					case Status.PARALYSIS:
						status = " [PAR]";
						break;
					case Status.POISON:
						status = " [PSN]";
						break;
				}
			}
			return $"#{pkmn.Name} (Lv. #{pkmn.Level})#{status} HP: #{pkmn.HP}/#{pkmn.TotalHP}";
		}

		private string pbMoveString(IMove move, int index)
		{
			string ret = string.Format("{0} - Press {1}", Game._INTL(move.id.ToString(TextScripts.Name)), index);
			string typename = Game._INTL(move.Type.ToString(TextScripts.Name));
			if (move.id > 0)
			{
				ret += string.Format(" ({0}) PP: {1}/{2}", typename, move.PP, move.TotalPP);
			}
			return ret;
		}
	}
}