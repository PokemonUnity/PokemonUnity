//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-02-11 15:21:41.728
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
//using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	/// <summary>
	/// 界面配置表。
	/// </summary>
	public class DRUIForm //: DataRowBase
	{
		private int m_Id = 0;

		/// <summary>
		/// 获取界面编号。
		/// </summary>
		public int Id
		{
			get
			{
				return m_Id;
			}
		}

		/// <summary>
		/// 获取资源名称。
		/// </summary>
		public string AssetName
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取界面组名称。
		/// </summary>
		public string UIGroupName
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取是否允许多个界面实例。
		/// </summary>
		public bool AllowMultiInstance
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取是否暂停被其覆盖的界面。
		/// </summary>
		public bool PauseCoveredUIForm
		{
			get;
			private set;
		}

		/*public override bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
		{
			// Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
			string[] columnTexts = dataRowSegment.Source.Substring(dataRowSegment.Offset, dataRowSegment.Length).Split(DataTableExtension.DataSplitSeparators);
			for (int i = 0; i < columnTexts.Length; i++)
			{
				columnTexts[i] = columnTexts[i].Trim(DataTableExtension.DataTrimSeparators);
			}

			int index = 0;
			index++;
			m_Id = int.Parse(columnTexts[index++]);
			index++;
			AssetName = columnTexts[index++];
			UIGroupName = columnTexts[index++];
			AllowMultiInstance = bool.Parse(columnTexts[index++]);
			PauseCoveredUIForm = bool.Parse(columnTexts[index++]);

			GeneratePropertyArray();
			return true;
		}

		public override bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
		{
			// Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
			using (MemoryStream memoryStream = new MemoryStream(dataRowSegment.Source, dataRowSegment.Offset, dataRowSegment.Length, false))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
				{
					m_Id = binaryReader.ReadInt32();
					AssetName = binaryReader.ReadString();
					UIGroupName = binaryReader.ReadString();
					AllowMultiInstance = binaryReader.ReadBoolean();
					PauseCoveredUIForm = binaryReader.ReadBoolean();
				}
			}

			GeneratePropertyArray();
			return true;
		}

		public override bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
		{
			Log.Warning("Not implemented ParseDataRow(GameFrameworkSegment<Stream>)");
			return false;
		}*/

		private void GeneratePropertyArray()
		{

		}
	}
}
