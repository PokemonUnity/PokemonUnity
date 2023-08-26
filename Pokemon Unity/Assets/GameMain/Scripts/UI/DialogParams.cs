//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace PokemonUnity
{
	/// <summary>
	/// 对话框显示数据。
	/// </summary>
	public class DialogParams2
	{
		/// <summary>
		/// 模式，即按钮数量。取值 1、2、3。
		/// </summary>
		public int Mode
		{
			get;
			set;
		}

		/*// <summary>
		/// 消息内容。
		/// </summary>
		public string Message
		{
			get;
			set;
		}*/

		/// <summary>
		/// 弹出窗口时是否暂停游戏。
		/// </summary>
		public bool PauseGame
		{
			get;
			set;
		}

		/// <summary>
		/// 确认按钮文本。
		/// </summary>
		public string ConfirmText
		{
			get;
			set;
		}

		/// <summary>
		/// 确定按钮回调。
		/// </summary>
		public GameFrameworkAction<object> OnClickConfirm
		{
			get;
			set;
		}

		/// <summary>
		/// 取消按钮回调。
		/// </summary>
		public GameFrameworkAction<object> OnClickCancel
		{
			get;
			set;
		}

		/// <summary>
		/// 用户自定义数据。
		/// </summary>
		public string UserData
		{
			get;
			set;
		}
	}
}
