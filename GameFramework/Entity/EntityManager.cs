//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Entity
{
	/// <summary>
	/// Entity Manager.
	/// </summary>
	internal sealed partial class EntityManager : GameFrameworkModule, IEntityManager
	{
		private readonly Dictionary<int, EntityInfo> m_EntityInfos;
		private readonly Dictionary<string, EntityGroup> m_EntityGroups;
		private readonly Dictionary<int, int> m_EntitiesBeingLoaded;
		private readonly HashSet<int> m_EntitiesToReleaseOnLoad;
		private readonly Queue<EntityInfo> m_RecycleQueue;
		private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
		private IObjectPoolManager m_ObjectPoolManager;
		private IResourceManager m_ResourceManager;
		private IEntityHelper m_EntityHelper;
		private int m_Serial;
		private bool m_IsShutdown;
		private EventHandler<ShowEntitySuccessEventArgs> m_ShowEntitySuccessEventHandler;
		private EventHandler<ShowEntityFailureEventArgs> m_ShowEntityFailureEventHandler;
		private EventHandler<ShowEntityUpdateEventArgs> m_ShowEntityUpdateEventHandler;
		private EventHandler<ShowEntityDependencyAssetEventArgs> m_ShowEntityDependencyAssetEventHandler;
		private EventHandler<HideEntityCompleteEventArgs> m_HideEntityCompleteEventHandler;

		/// <summary>
		/// Initialize a new instance of the entity manager.
		/// </summary>
		public EntityManager()
		{
			m_EntityInfos = new Dictionary<int, EntityInfo>();
			m_EntityGroups = new Dictionary<string, EntityGroup>();
			m_EntitiesBeingLoaded = new Dictionary<int, int>();
			m_EntitiesToReleaseOnLoad = new HashSet<int>();
			m_RecycleQueue = new Queue<EntityInfo>();
			m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
			m_ObjectPoolManager = null;
			m_ResourceManager = null;
			m_EntityHelper = null;
			m_Serial = 0;
			m_IsShutdown = false;
			m_ShowEntitySuccessEventHandler = null;
			m_ShowEntityFailureEventHandler = null;
			m_ShowEntityUpdateEventHandler = null;
			m_ShowEntityDependencyAssetEventHandler = null;
			m_HideEntityCompleteEventHandler = null;
		}

		/// <summary>
		/// Get the number of entities.
		/// </summary>
		public int EntityCount
		{
			get
			{
				return m_EntityInfos.Count;
			}
		}

		/// <summary>
		/// Get the number of entity groups.
		/// </summary>
		public int EntityGroupCount
		{
			get
			{
				return m_EntityGroups.Count;
			}
		}

		/// <summary>
		/// Show entity success events.
		/// </summary>
		public event EventHandler<ShowEntitySuccessEventArgs> ShowEntitySuccess
		{
			add
			{
				m_ShowEntitySuccessEventHandler += value;
			}
			remove
			{
				m_ShowEntitySuccessEventHandler -= value;
			}
		}

		/// <summary>
		/// Show entity failure events.
		/// </summary>
		public event EventHandler<ShowEntityFailureEventArgs> ShowEntityFailure
		{
			add
			{
				m_ShowEntityFailureEventHandler += value;
			}
			remove
			{
				m_ShowEntityFailureEventHandler -= value;
			}
		}

		/// <summary>
		/// Display entity update events.
		/// </summary>
		public event EventHandler<ShowEntityUpdateEventArgs> ShowEntityUpdate
		{
			add
			{
				m_ShowEntityUpdateEventHandler += value;
			}
			remove
			{
				m_ShowEntityUpdateEventHandler -= value;
			}
		}

		/// <summary>
		/// Load dependent resource events when displaying entities.
		/// </summary>
		public event EventHandler<ShowEntityDependencyAssetEventArgs> ShowEntityDependencyAsset
		{
			add
			{
				m_ShowEntityDependencyAssetEventHandler += value;
			}
			remove
			{
				m_ShowEntityDependencyAssetEventHandler -= value;
			}
		}

		/// <summary>
		/// Hide entity completion events.
		/// </summary>
		public event EventHandler<HideEntityCompleteEventArgs> HideEntityComplete
		{
			add
			{
				m_HideEntityCompleteEventHandler += value;
			}
			remove
			{
				m_HideEntityCompleteEventHandler -= value;
			}
		}

		/// <summary>
		/// Entity manager polling.
		/// </summary>
		/// <param name="elapseSeconds"> The logical elapsed time, in seconds. </param>
		/// <param name="realElapseSeconds"> Real elapsed time in seconds. </param>
		internal override void Update(float elapseSeconds, float realElapseSeconds)
		{
			while (m_RecycleQueue.Count > 0)
			{
				EntityInfo entityInfo = m_RecycleQueue.Dequeue();
				IEntity entity = entityInfo.Entity;
				EntityGroup entityGroup = (EntityGroup)entity.EntityGroup;
				if (entityGroup == null)
				{
					throw new GameFrameworkException("Entity group is invalid.");
				}

				entityInfo.Status = EntityStatus.WillRecycle;
				entity.OnRecycle();
				entityInfo.Status = EntityStatus.Recycled;
				entityGroup.UnspawnEntity(entity);
				ReferencePool.Release(entityInfo);
			}

			foreach (KeyValuePair<string, EntityGroup> entityGroup in m_EntityGroups)
			{
				entityGroup.Value.Update(elapseSeconds, realElapseSeconds);
			}
		}

		/// <summary>
		/// Close and clean up the entity manager.
		/// </summary>
		internal override void Shutdown()
		{
			m_IsShutdown = true;
			HideAllLoadedEntities();
			m_EntityGroups.Clear();
			m_EntitiesBeingLoaded.Clear();
			m_EntitiesToReleaseOnLoad.Clear();
			m_RecycleQueue.Clear();
		}

		/// <summary>
		/// Set the object pool manager.
		/// </summary>
		/// <param name="objectPoolManager"> Object Pool Manager. </param>
		public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
		{
			if (objectPoolManager == null)
			{
				throw new GameFrameworkException("Object pool manager is invalid.");
			}

			m_ObjectPoolManager = objectPoolManager;
		}

		/// <summary>
		/// Set the resource manager.
		/// </summary>
		/// <param name="resourceManager"> Explorer. </param>
		public void SetResourceManager(IResourceManager resourceManager)
		{
			if (resourceManager == null)
			{
				throw new GameFrameworkException("Resource manager is invalid.");
			}

			m_ResourceManager = resourceManager;
		}

		/// <summary>
		/// Set the physical helper.
		/// </summary>
		/// <param name="entityHelper"> entity helper. </param>
		public void SetEntityHelper(IEntityHelper entityHelper)
		{
			if (entityHelper == null)
			{
				throw new GameFrameworkException("Entity helper is invalid.");
			}

			m_EntityHelper = entityHelper;
		}

		/// <summary>
		///Is there an entity group?
		/// </summary>
		/// <param name="entityGroupName"> entity group name. </param>
		/// <returns> Whether there is an entity group. </returns>
		public bool HasEntityGroup(string entityGroupName)
		{
			if (string.IsNullOrEmpty(entityGroupName))
			{
				throw new GameFrameworkException("Entity group name is invalid.");
			}

			return m_EntityGroups.ContainsKey(entityGroupName);
		}

		/// <summary>
		/// Get the entity group.
		/// </summary>
		/// <param name="entityGroupName"> entity group name. </param>
		/// <returns> the entity group to get. </returns>
		public IEntityGroup GetEntityGroup(string entityGroupName)
		{
			if (string.IsNullOrEmpty(entityGroupName))
			{
				throw new GameFrameworkException("Entity group name is invalid.");
			}

			EntityGroup entityGroup = null;
			if (m_EntityGroups.TryGetValue(entityGroupName, out entityGroup))
			{
				return entityGroup;
			}

			return null;
		}

		/// <summary>
		/// Get all entity groups.
		/// </summary>
		/// <returns> all entity groups. </returns>
		public IEntityGroup[] GetAllEntityGroups()
		{
			int index = 0;
			IEntityGroup[] results = new IEntityGroup[m_EntityGroups.Count];
			foreach (KeyValuePair<string, EntityGroup> entityGroup in m_EntityGroups)
			{
				results[index++] = entityGroup.Value;
			}

			return results;
		}

		/// <summary>
		/// Get all entity groups.
		/// </summary>
		/// <param name="results"> All entity groups. </param>
		public void GetAllEntityGroups(List<IEntityGroup> results)
		{
			if (results == null)
			{
				throw new GameFrameworkException("Results is invalid.");
			}

			results.Clear();
			foreach (KeyValuePair<string, EntityGroup> entityGroup in m_EntityGroups)
			{
				results.Add(entityGroup.Value);
			}
		}

		/// <summary>
		/// Add an entity group.
		/// </summary>
		/// <param name="entityGroupName"> entity group name. </param>
		/// <param name="instanceAutoReleaseInterval"> The number of seconds between the entity instance object pool and the free release of the freeable object. </param>
		/// <param name="instanceCapacity"> Entity instance object pool capacity. </param>
		/// <param name="instanceExpireTime"> Entity instance object pool object expiration seconds. </param>
		/// <param name="instancePriority"> The priority of the entity instance object pool. </param>
		/// <param name="entityGroupHelper"> Entity group helper. </param>
		/// <returns> Whether to increase the entity group successfully. </returns>
		public bool AddEntityGroup(string entityGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IEntityGroupHelper entityGroupHelper)
		{
			if (string.IsNullOrEmpty(entityGroupName))
			{
				throw new GameFrameworkException("Entity group name is invalid.");
			}

			if (entityGroupHelper == null)
			{
				throw new GameFrameworkException("Entity group helper is invalid.");
			}

			if (m_ObjectPoolManager == null)
			{
				throw new GameFrameworkException("You must set object pool manager first.");
			}

			if (HasEntityGroup(entityGroupName))
			{
				return false;
			}

			m_EntityGroups.Add(entityGroupName, new EntityGroup(entityGroupName, instanceAutoReleaseInterval, instanceCapacity, instanceExpireTime, instancePriority, entityGroupHelper, m_ObjectPoolManager));

			return true;
		}

		/// <summary>
		/// Whether there is an entity.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <returns> Whether there is an entity. </returns>
		public bool HasEntity(int entityId)
		{
			return m_EntityInfos.ContainsKey(entityId);
		}

		/// <summary>
		/// Whether there is an entity.
		/// </summary>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <returns> Whether there is an entity. </returns>
		public bool HasEntity(string entityAssetName)
		{
			if (string.IsNullOrEmpty(entityAssetName))
			{
				throw new GameFrameworkException("Entity asset name is invalid.");
			}

			foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
			{
				if (entityInfo.Value.Entity.EntityAssetName == entityAssetName)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Get the entity.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <returns> the entity to get. </returns>
		public IEntity GetEntity(int entityId)
		{
			EntityInfo entityInfo = GetEntityInfo(entityId);
			if (entityInfo == null)
			{
				return null;
			}

			return entityInfo.Entity;
		}

		/// <summary>
		/// Get the entity.
		/// </summary>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <returns> the entity to get. </returns>
		public IEntity GetEntity(string entityAssetName)
		{
			if (string.IsNullOrEmpty(entityAssetName))
			{
				throw new GameFrameworkException("Entity asset name is invalid.");
			}

			foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
			{
				if (entityInfo.Value.Entity.EntityAssetName == entityAssetName)
				{
					return entityInfo.Value.Entity;
				}
			}

			return null;
		}

		/// <summary>
		/// Get the entity.
		/// </summary>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <returns> the entity to get. </returns>
		public IEntity[] GetEntities(string entityAssetName)
		{
			if (string.IsNullOrEmpty(entityAssetName))
			{
				throw new GameFrameworkException("Entity asset name is invalid.");
			}

			List<IEntity> results = new List<IEntity>();
			foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
			{
				if (entityInfo.Value.Entity.EntityAssetName == entityAssetName)
				{
					results.Add(entityInfo.Value.Entity);
				}
			}

			return results.ToArray();
		}

		/// <summary>
		/// Get the entity.
		/// </summary>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <param name="results"> The entity to get. </param>
		public void GetEntities(string entityAssetName, List<IEntity> results)
		{
			if (string.IsNullOrEmpty(entityAssetName))
			{
				throw new GameFrameworkException("Entity asset name is invalid.");
			}

			if (results == null)
			{
				throw new GameFrameworkException("Results is invalid.");
			}

			results.Clear();
			foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
			{
				if (entityInfo.Value.Entity.EntityAssetName == entityAssetName)
				{
					results.Add(entityInfo.Value.Entity);
				}
			}
		}

		/// <summary>
		/// Get all loaded entities.
		/// </summary>
		/// <returns> all loaded entities. </returns>
		public IEntity[] GetAllLoadedEntities()
		{
			int index = 0;
			IEntity[] results = new IEntity[m_EntityInfos.Count];
			foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
			{
				results[index++] = entityInfo.Value.Entity;
			}

			return results;
		}

		/// <summary>
		/// Get all loaded entities.
		/// </summary>
		/// <param name="results"> All loaded entities. </param>
		public void GetAllLoadedEntities(List<IEntity> results)
		{
			if (results == null)
			{
				throw new GameFrameworkException("Results is invalid.");
			}

			results.Clear();
			foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
			{
				results.Add(entityInfo.Value.Entity);
			}
		}

		/// <summary>
		/// Get the number of all the entities being loaded.
		/// </summary>
		/// <returns> the number of all the entities being loaded. </returns>
		public int[] GetAllLoadingEntityIds()
		{
			int index = 0;
			int[] results = new int[m_EntitiesBeingLoaded.Count];
			foreach (KeyValuePair<int, int> entityBeingLoaded in m_EntitiesBeingLoaded)
			{
				results[index++] = entityBeingLoaded.Key;
			}

			return results;
		}

		/// <summary>
		/// Get the number of all the entities being loaded.
		/// </summary>
		/// <param name="results"> The number of all the entities being loaded. </param>
		public void GetAllLoadingEntityIds(List<int> results)
		{
			if (results == null)
			{
				throw new GameFrameworkException("Results is invalid.");
			}

			results.Clear();
			foreach (KeyValuePair<int, int> entityBeingLoaded in m_EntitiesBeingLoaded)
			{
				results.Add(entityBeingLoaded.Key);
			}
		}

		/// <summary>
		/// Is the entity being loaded?
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <returns> Whether the entity is being loaded. </returns>
		public bool IsLoadingEntity(int entityId)
		{
			return m_EntitiesBeingLoaded.ContainsKey(entityId);
		}

		/// <summary>
		/// Is it a legal entity?
		/// </summary>
		/// <param name="entity">实体。</param>
		/// <returns> Whether the entity is legal. </returns>
		public bool IsValidEntity(IEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			return HasEntity(entity.Id);
		}

		/// <summary>
		/// Show entities.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <param name="entityGroupName"> entity group name. </param>
		public void ShowEntity(int entityId, string entityAssetName, string entityGroupName)
		{
			ShowEntity(entityId, entityAssetName, entityGroupName, Constant.DefaultPriority, null);
		}

		/// <summary>
		/// Show entities.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <param name="entityGroupName"> entity group name. </param>
		/// <param name="priority"> The priority of the loaded entity resource. </param>
		public void ShowEntity(int entityId, string entityAssetName, string entityGroupName, int priority)
		{
			ShowEntity(entityId, entityAssetName, entityGroupName, priority, null);
		}

		/// <summary>
		/// Show entities.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <param name="entityGroupName"> entity group name. </param>
		/// <param name="userData"> User-defined data. </param>
		public void ShowEntity(int entityId, string entityAssetName, string entityGroupName, object userData)
		{
			ShowEntity(entityId, entityAssetName, entityGroupName, Constant.DefaultPriority, userData);
		}

		/// <summary>
		/// Show entities.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <param name="entityAssetName"> entity resource name. </param>
		/// <param name="entityGroupName"> entity group name. </param>
		/// <param name="priority"> The priority of the loaded entity resource. </param>
		/// <param name="userData"> User-defined data. </param>
		public void ShowEntity(int entityId, string entityAssetName, string entityGroupName, int priority, object userData)
		{
			if (m_ResourceManager == null)
			{
				throw new GameFrameworkException("You must set resource manager first.");
			}

			if (m_EntityHelper == null)
			{
				throw new GameFrameworkException("You must set entity helper first.");
			}

			if (string.IsNullOrEmpty(entityAssetName))
			{
				throw new GameFrameworkException("Entity asset name is invalid.");
			}

			if (string.IsNullOrEmpty(entityGroupName))
			{
				throw new GameFrameworkException("Entity group name is invalid.");
			}

			if (HasEntity(entityId))
			{
				throw new GameFrameworkException(Utility.Text.Format("Entity id '{0}' is already exist.", entityId.ToString()));
			}

			if (IsLoadingEntity(entityId))
			{
				throw new GameFrameworkException(Utility.Text.Format("Entity '{0}' is already being loaded.", entityId.ToString()));
			}

			EntityGroup entityGroup = (EntityGroup)GetEntityGroup(entityGroupName);
			if (entityGroup == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Entity group '{0}' is not exist.", entityGroupName));
			}

            EntityInstanceObject entityInstanceObject = entityGroup.SpawnEntityInstanceObject(entityAssetName);
            if (entityInstanceObject == null)
            {
                int serialId = ++m_Serial;
                m_EntitiesBeingLoaded.Add(entityId, serialId);
                m_ResourceManager.LoadAsset(entityAssetName, priority, m_LoadAssetCallbacks, ShowEntityInfo.Create(serialId, entityId, entityGroup, userData));
                return;
            }

			InternalShowEntity(entityId, entityAssetName, entityGroup, entityInstanceObject.Target, false, 0f, userData);
		}

		/// <summary>
		/// Hide the entity.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		public void HideEntity(int entityId)
		{
			HideEntity(entityId, null);
		}

		/// <summary>
		/// Hide the entity.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <param name="userData"> User-defined data. </param>
		public void HideEntity(int entityId, object userData)
		{
			if (IsLoadingEntity(entityId))
			{
				m_EntitiesToReleaseOnLoad.Add(m_EntitiesBeingLoaded[entityId]);
				m_EntitiesBeingLoaded.Remove(entityId);
				return;
			}

			EntityInfo entityInfo = GetEntityInfo(entityId);
			if (entityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find entity '{0}'.", entityId.ToString()));
			}

			InternalHideEntity(entityInfo, userData);
		}

		/// <summary>
		/// Hide the entity.
		/// </summary>
		/// <param name="entity">实体。</param>
		public void HideEntity(IEntity entity)
		{
			HideEntity(entity, null);
		}

		/// <summary>
		/// Hide the entity.
		/// </summary>
		/// <param name="entity">实体。</param>
		/// <param name="userData"> User-defined data. </param>
		public void HideEntity(IEntity entity, object userData)
		{
			if (entity == null)
			{
				throw new GameFrameworkException("Entity is invalid.");
			}

			HideEntity(entity.Id, userData);
		}

		/// <summary>
		/// Hide all loaded entities.
		/// </summary>
		public void HideAllLoadedEntities()
		{
			HideAllLoadedEntities(null);
		}

		/// <summary>
		/// Hide all loaded entities.
		/// </summary>
		/// <param name="userData"> User-defined data. </param>
		public void HideAllLoadedEntities(object userData)
		{
			while (m_EntityInfos.Count > 0)
			{
				foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
				{
					InternalHideEntity(entityInfo.Value, userData);
					break;
				}
			}
		}

		/// <summary>
		/// Hide all the entities that are loading.
		/// </summary>
		public void HideAllLoadingEntities()
		{
			foreach (KeyValuePair<int, int> entityBeingLoaded in m_EntitiesBeingLoaded)
			{
				m_EntitiesToReleaseOnLoad.Add(entityBeingLoaded.Value);
			}

			m_EntitiesBeingLoaded.Clear();
		}

		/// <summary>
		/// Get the parent entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the child entity to get the parent entity. </param>
		/// <returns> the parent entity of the child entity. </returns>
		public IEntity GetParentEntity(int childEntityId)
		{
			EntityInfo childEntityInfo = GetEntityInfo(childEntityId);
			if (childEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find child entity '{0}'.", childEntityId.ToString()));
			}

			return childEntityInfo.ParentEntity;
		}

		/// <summary>
		/// Get the parent entity.
		/// </summary>
		/// <param name="childEntity"> Get the child entity of the parent entity. </param>
		/// <returns> the parent entity of the child entity. </returns>
		public IEntity GetParentEntity(IEntity childEntity)
		{
			if (childEntity == null)
			{
				throw new GameFrameworkException("Child entity is invalid.");
			}

			return GetParentEntity(childEntity.Id);
		}

		/// <summary>
		/// Get the child entity.
		/// </summary>
		/// <param name="parentEntityId"> The entity number of the parent entity to get the child entity. </param>
		/// <returns> an array of child entities. </returns>
		public IEntity[] GetChildEntities(int parentEntityId)
		{
			EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
			if (parentEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find parent entity '{0}'.", parentEntityId.ToString()));
			}

			return parentEntityInfo.GetChildEntities();
		}

		/// <summary>
		/// Get the child entity.
		/// </summary>
		/// <param name="parentEntityId"> The entity number of the parent entity to get the child entity. </param>
		/// <param name="results"> An array of child entities. </param>
		public void GetChildEntities(int parentEntityId, List<IEntity> results)
		{
			EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
			if (parentEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find parent entity '{0}'.", parentEntityId.ToString()));
			}

			parentEntityInfo.GetChildEntities(results);
		}

		/// <summary>
		/// Get the child entity.
		/// </summary>
		/// <param name="parentEntity"> Get the parent of the child entity. </param>
		/// <returns> an array of child entities. </returns>
		public IEntity[] GetChildEntities(IEntity parentEntity)
		{
			if (parentEntity == null)
			{
				throw new GameFrameworkException("Parent entity is invalid.");
			}

			return GetChildEntities(parentEntity.Id);
		}

		/// <summary>
		/// Get the child entity.
		/// </summary>
		/// <param name="parentEntity"> Get the parent of the child entity. </param>
		/// <param name="results"> An array of child entities. </param>
		public void GetChildEntities(IEntity parentEntity, List<IEntity> results)
		{
			if (parentEntity == null)
			{
				throw new GameFrameworkException("Parent entity is invalid.");
			}

			GetChildEntities(parentEntity.Id, results);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the sub-entity to be attached. </param>
		/// <param name="parentEntityId"> The entity number of the parent entity to be attached. </param>
		public void AttachEntity(int childEntityId, int parentEntityId)
		{
			AttachEntity(childEntityId, parentEntityId, null);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the sub-entity to be attached. </param>
		/// <param name="parentEntityId"> The entity number of the parent entity to be attached. </param>
		/// <param name="userData"> User-defined data. </param>
		public void AttachEntity(int childEntityId, int parentEntityId, object userData)
		{
			if (childEntityId == parentEntityId)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not attach entity when child entity id equals to parent entity id '{0}'.", parentEntityId.ToString()));
			}

			EntityInfo childEntityInfo = GetEntityInfo(childEntityId);
			if (childEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find child entity '{0}'.", childEntityId.ToString()));
			}

			if (childEntityInfo.Status >= EntityStatus.WillHide)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not attach entity when child entity status is '{0}'.", childEntityInfo.Status.ToString()));
			}

			EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
			if (parentEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find parent entity '{0}'.", parentEntityId.ToString()));
			}

			if (parentEntityInfo.Status >= EntityStatus.WillHide)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not attach entity when parent entity status is '{0}'.", parentEntityInfo.Status.ToString()));
			}

			IEntity childEntity = childEntityInfo.Entity;
			IEntity parentEntity = parentEntityInfo.Entity;
			DetachEntity(childEntity.Id, userData);
			childEntityInfo.ParentEntity = parentEntity;
			parentEntityInfo.AddChildEntity(childEntity);
			parentEntity.OnAttached(childEntity, userData);
			childEntity.OnAttachTo(parentEntity, userData);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the sub-entity to be attached. </param>
		/// <param name="parentEntity"> The parent entity to be attached. </param>
		public void AttachEntity(int childEntityId, IEntity parentEntity)
		{
			AttachEntity(childEntityId, parentEntity, null);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the sub-entity to be attached. </param>
		/// <param name="parentEntity"> The parent entity to be attached. </param>
		/// <param name="userData"> User-defined data. </param>
		public void AttachEntity(int childEntityId, IEntity parentEntity, object userData)
		{
			if (parentEntity == null)
			{
				throw new GameFrameworkException("Parent entity is invalid.");
			}

			AttachEntity(childEntityId, parentEntity.Id, userData);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntity"> The child entity to attach. </param>
		/// <param name="parentEntityId"> The entity number of the parent entity to be attached. </param>
		public void AttachEntity(IEntity childEntity, int parentEntityId)
		{
			AttachEntity(childEntity, parentEntityId, null);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntity"> The child entity to attach. </param>
		/// <param name="parentEntityId"> The entity number of the parent entity to be attached. </param>
		/// <param name="userData"> User-defined data. </param>
		public void AttachEntity(IEntity childEntity, int parentEntityId, object userData)
		{
			if (childEntity == null)
			{
				throw new GameFrameworkException("Child entity is invalid.");
			}

			AttachEntity(childEntity.Id, parentEntityId, userData);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntity"> The child entity to attach. </param>
		/// <param name="parentEntity"> The parent entity to be attached. </param>
		public void AttachEntity(IEntity childEntity, IEntity parentEntity)
		{
			AttachEntity(childEntity, parentEntity, null);
		}

		/// <summary>
		/// Add a child entity.
		/// </summary>
		/// <param name="childEntity"> The child entity to attach. </param>
		/// <param name="parentEntity"> The parent entity to be attached. </param>
		/// <param name="userData"> User-defined data. </param>
		public void AttachEntity(IEntity childEntity, IEntity parentEntity, object userData)
		{
			if (childEntity == null)
			{
				throw new GameFrameworkException("Child entity is invalid.");
			}

			if (parentEntity == null)
			{
				throw new GameFrameworkException("Parent entity is invalid.");
			}

			AttachEntity(childEntity.Id, parentEntity.Id, userData);
		}

		/// <summary>
		/// Release the child entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the child entity to be released. </param>
		public void DetachEntity(int childEntityId)
		{
			DetachEntity(childEntityId, null);
		}

		/// <summary>
		/// Release the child entity.
		/// </summary>
		/// <param name="childEntityId"> The entity number of the child entity to be released. </param>
		/// <param name="userData"> User-defined data. </param>
		public void DetachEntity(int childEntityId, object userData)
		{
			EntityInfo childEntityInfo = GetEntityInfo(childEntityId);
			if (childEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find child entity '{0}'.", childEntityId.ToString()));
			}

			IEntity parentEntity = childEntityInfo.ParentEntity;
			if (parentEntity == null)
			{
				return;
			}

			EntityInfo parentEntityInfo = GetEntityInfo(parentEntity.Id);
			if (parentEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find parent entity '{0}'.", parentEntity.Id.ToString()));
			}

			IEntity childEntity = childEntityInfo.Entity;
			childEntityInfo.ParentEntity = null;
			parentEntityInfo.RemoveChildEntity(childEntity);
			parentEntity.OnDetached(childEntity, userData);
			childEntity.OnDetachFrom(parentEntity, userData);
		}

		/// <summary>
		/// Release the child entity.
		/// </summary>
		/// <param name="childEntity"> The child entity to be released. </param>
		public void DetachEntity(IEntity childEntity)
		{
			DetachEntity(childEntity, null);
		}

		/// <summary>
		/// Release the child entity.
		/// </summary>
		/// <param name="childEntity"> The child entity to be released. </param>
		/// <param name="userData"> User-defined data. </param>
		public void DetachEntity(IEntity childEntity, object userData)
		{
			if (childEntity == null)
			{
				throw new GameFrameworkException("Child entity is invalid.");
			}

			DetachEntity(childEntity.Id, userData);
		}

		/// <summary>
		/// Remove all child entities.
		/// </summary>
		/// <param name="parentEntityId"> The entity number of the parent entity being dismissed. </param>
		public void DetachChildEntities(int parentEntityId)
		{
			DetachChildEntities(parentEntityId, null);
		}

		/// <summary>
		/// Remove all child entities.
		/// </summary>
		/// <param name="parentEntityId"> The entity number of the parent entity being dismissed. </param>
		/// <param name="userData"> User-defined data. </param>
		public void DetachChildEntities(int parentEntityId, object userData)
		{
			EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
			if (parentEntityInfo == null)
			{
				throw new GameFrameworkException(Utility.Text.Format("Can not find parent entity '{0}'.", parentEntityId.ToString()));
			}

			IEntity[] childEntities = parentEntityInfo.GetChildEntities();
			foreach (IEntity childEntity in childEntities)
			{
				DetachEntity(childEntity.Id, userData);
			}
		}

		/// <summary>
		/// Remove all child entities.
		/// </summary>
		/// <param name="parentEntity"> The parent entity that was dismissed. </param>
		public void DetachChildEntities(IEntity parentEntity)
		{
			DetachChildEntities(parentEntity, null);
		}

		/// <summary>
		/// Remove all child entities.
		/// </summary>
		/// <param name="parentEntity"> The parent entity that was dismissed. </param>
		/// <param name="userData"> User-defined data. </param>
		public void DetachChildEntities(IEntity parentEntity, object userData)
		{
			if (parentEntity == null)
			{
				throw new GameFrameworkException("Parent entity is invalid.");
			}

			DetachChildEntities(parentEntity.Id, userData);
		}

		/// <summary>
		/// Get entity information.
		/// </summary>
		/// <param name="entityId"> entity number. </param>
		/// <returns> entity information. </returns>
		private EntityInfo GetEntityInfo(int entityId)
		{
			EntityInfo entityInfo = null;
			if (m_EntityInfos.TryGetValue(entityId, out entityInfo))
			{
				return entityInfo;
			}

			return null;
		}

		private void InternalShowEntity(int entityId, string entityAssetName, EntityGroup entityGroup, object entityInstance, bool isNewInstance, float duration, object userData)
		{
			try
			{
				IEntity entity = m_EntityHelper.CreateEntity(entityInstance, entityGroup, userData);
				if (entity == null)
				{
					throw new GameFrameworkException("Can not create entity in helper.");
				}

				EntityInfo entityInfo = EntityInfo.Create(entity);
				m_EntityInfos.Add(entityId, entityInfo);
				entityInfo.Status = EntityStatus.WillInit;
				entity.OnInit(entityId, entityAssetName, entityGroup, isNewInstance, userData);
				entityInfo.Status = EntityStatus.Inited;
				entityGroup.AddEntity(entity);
				entityInfo.Status = EntityStatus.WillShow;
				entity.OnShow(userData);
				entityInfo.Status = EntityStatus.Showed;

				if (m_ShowEntitySuccessEventHandler != null)
				{
					ShowEntitySuccessEventArgs showEntitySuccessEventArgs = ShowEntitySuccessEventArgs.Create(entity, duration, userData);
					m_ShowEntitySuccessEventHandler(this, showEntitySuccessEventArgs);
					ReferencePool.Release(showEntitySuccessEventArgs);
				}
			}
			catch (Exception exception)
			{
				if (m_ShowEntityFailureEventHandler != null)
				{
					ShowEntityFailureEventArgs showEntityFailureEventArgs = ShowEntityFailureEventArgs.Create(entityId, entityAssetName, entityGroup.Name, exception.ToString(), userData);
					m_ShowEntityFailureEventHandler(this, showEntityFailureEventArgs);
					ReferencePool.Release(showEntityFailureEventArgs);
					return;
				}

				throw;
			}
		}

		private void InternalHideEntity(EntityInfo entityInfo, object userData)
		{
			IEntity entity = entityInfo.Entity;
			IEntity[] childEntities = entityInfo.GetChildEntities();
			foreach (IEntity childEntity in childEntities)
			{
				HideEntity(childEntity.Id, userData);
			}

			if (entityInfo.Status == EntityStatus.Hidden)
			{
				return;
			}

            DetachEntity(entity.Id, userData);
            entityInfo.Status = EntityStatus.WillHide;
            entity.OnHide(m_IsShutdown, userData);
            entityInfo.Status = EntityStatus.Hidden;

			EntityGroup entityGroup = (EntityGroup)entity.EntityGroup;
			if (entityGroup == null)
			{
				throw new GameFrameworkException("Entity group is invalid.");
			}

			entityGroup.RemoveEntity(entity);
			if (!m_EntityInfos.Remove(entity.Id))
			{
				throw new GameFrameworkException("Entity info is unmanaged.");
			}

			if (m_HideEntityCompleteEventHandler != null)
			{
				HideEntityCompleteEventArgs hideEntityCompleteEventArgs = HideEntityCompleteEventArgs.Create(entity.Id, entity.EntityAssetName, entityGroup, userData);
				m_HideEntityCompleteEventHandler(this, hideEntityCompleteEventArgs);
				ReferencePool.Release(hideEntityCompleteEventArgs);
			}

			m_RecycleQueue.Enqueue(entityInfo);
		}

        private void LoadAssetSuccessCallback(string entityAssetName, object entityAsset, float duration, object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            if (showEntityInfo == null)
            {
                throw new GameFrameworkException("Show entity info is invalid.");
            }

			if (m_EntitiesToReleaseOnLoad.Contains(showEntityInfo.SerialId))
			{
				m_EntitiesToReleaseOnLoad.Remove(showEntityInfo.SerialId);
				ReferencePool.Release(showEntityInfo);
				m_EntityHelper.ReleaseEntity(entityAsset, null);
				return;
			}

			m_EntitiesBeingLoaded.Remove(showEntityInfo.EntityId);
			EntityInstanceObject entityInstanceObject = EntityInstanceObject.Create(entityAssetName, entityAsset, m_EntityHelper.InstantiateEntity(entityAsset), m_EntityHelper);
			showEntityInfo.EntityGroup.RegisterEntityInstanceObject(entityInstanceObject, true);

			InternalShowEntity(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup, entityInstanceObject.Target, true, duration, showEntityInfo.UserData);
			ReferencePool.Release(showEntityInfo);
		}

        private void LoadAssetFailureCallback(string entityAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            if (showEntityInfo == null)
            {
                throw new GameFrameworkException("Show entity info is invalid.");
            }

            if (m_EntitiesToReleaseOnLoad.Contains(showEntityInfo.SerialId))
            {
                m_EntitiesToReleaseOnLoad.Remove(showEntityInfo.SerialId);
                return;
            }

            m_EntitiesBeingLoaded.Remove(showEntityInfo.EntityId);
            string appendErrorMessage = Utility.Text.Format("Load entity failure, asset name '{0}', status '{1}', error message '{2}'.", entityAssetName, status.ToString(), errorMessage);
            if (m_ShowEntityFailureEventHandler != null)
            {
                ShowEntityFailureEventArgs showEntityFailureEventArgs = ShowEntityFailureEventArgs.Create(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup.Name, appendErrorMessage, showEntityInfo.UserData);
                m_ShowEntityFailureEventHandler(this, showEntityFailureEventArgs);
                ReferencePool.Release(showEntityFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string entityAssetName, float progress, object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            if (showEntityInfo == null)
            {
                throw new GameFrameworkException("Show entity info is invalid.");
            }

			if (m_ShowEntityUpdateEventHandler != null)
			{
				ShowEntityUpdateEventArgs showEntityUpdateEventArgs = ShowEntityUpdateEventArgs.Create(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup.Name, progress, showEntityInfo.UserData);
				m_ShowEntityUpdateEventHandler(this, showEntityUpdateEventArgs);
				ReferencePool.Release(showEntityUpdateEventArgs);
			}
		}

        private void LoadAssetDependencyAssetCallback(string entityAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            if (showEntityInfo == null)
            {
                throw new GameFrameworkException("Show entity info is invalid.");
            }

			if (m_ShowEntityDependencyAssetEventHandler != null)
			{
				ShowEntityDependencyAssetEventArgs showEntityDependencyAssetEventArgs = ShowEntityDependencyAssetEventArgs.Create(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup.Name, dependencyAssetName, loadedCount, totalCount, showEntityInfo.UserData);
				m_ShowEntityDependencyAssetEventHandler(this, showEntityDependencyAssetEventArgs);
				ReferencePool.Release(showEntityDependencyAssetEventArgs);
			}
		}
	}
}