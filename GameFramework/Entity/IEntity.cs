//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Entity
{
	/// <summary>
	/// Entity interface.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// Get the entity number.
		/// </summary>
		int Id
		{
			get;
		}

		/// <summary>
		/// Get the entity resource name.
		/// </summary>
		string EntityAssetName
		{
			get;
		}

		/// <summary>
		/// Get an entity instance.
		/// </summary>
		object Handle
		{
			get;
		}

		/// <summary>
		/// Get the entity group to which the entity belongs.
		/// </summary>
		IEntityGroup EntityGroup
		{
			get;
		}

		/// <summary>
		/// Entity initialization.
		/// </summary>
		/// <param  name="entityId"> entity number. </param>
		/// <param  name="entityAssetName"> entity resource name. </param>
		/// <param  name="entityGroup"> The entity group to which the entity belongs. </param>
		/// <param  name="isNewInstance"> Whether it is a new instance. </param>
		/// <param  name="userData"> User-defined data. </param>
		void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance, object userData);

		/// <summary>
		/// Entity recycling.
		/// </summary>
		void OnRecycle();

		/// <summary>
		/// The entity is displayed.
		/// </summary>
		/// <param name="userData"> User-defined data. </param>
		void OnShow(object userData);

		/// <summary>
		///The entity is hidden.
		/// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
		/// <param name="userData"> User-defined data. </param>
        void OnHide(bool isShutdown, object userData);

		/// <summary>
		///The entity attaches a child entity.
		/// </summary>
		/// <param name="childEntity"> Additional child entities. </param>
		/// <param name="userData"> User-defined data. </param>
		void OnAttached(IEntity childEntity, object userData);

		/// <summary>
		///The entity releases the child entity.
		/// </summary>
		/// <param name="childEntity"> The released child entity. </param>
		/// <param name="userData"> User-defined data. </param>
		void OnDetached(IEntity childEntity, object userData);

		/// <summary>
		///The entity attaches a child entity.
		/// </summary>
		/// <param name="parentEntity"> The parent entity to be attached. </param>
		/// <param name="userData"> User-defined data. </param>
		void OnAttachTo(IEntity parentEntity, object userData);

		/// <summary>
		///The entity releases the child entity.
		/// </summary>
		/// <param name="parentEntity"> The parent entity that was dismissed. </param>
		/// <param name="userData"> User-defined data. </param>
		void OnDetachFrom(IEntity parentEntity, object userData);

		/// <summary>
		/// entity polling.
		/// </summary>
		/// <param name="elapseSeconds"> The logical elapsed time, in seconds. </param>
		/// <param name="realElapseSeconds"> Real elapsed time in seconds. </param>
		void OnUpdate(float elapseSeconds, float realElapseSeconds);
	}
}