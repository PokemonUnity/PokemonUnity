//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-02-11 15:21:41.719
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	/// <summary>
	/// 声音配置表。
	/// </summary>
	public class DRSound : DataRowBase
	{
		private int m_Id = 0;

		/// <summary>
		/// 获取声音编号。
		/// </summary>
		public override int Id
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
		/// 获取优先级（默认0，128最高，-128最低）。
		/// </summary>
		public int Priority
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取是否循环。
		/// </summary>
		public bool Loop
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取音量（0~1）。
		/// </summary>
		public float Volume
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取声音空间混合量（0为2D，1为3D，中间值混合效果）。
		/// </summary>
		public float SpatialBlend
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取声音最大距离。
		/// </summary>
		public float MaxDistance
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
			Priority = int.Parse(columnTexts[index++]);
			Loop = bool.Parse(columnTexts[index++]);
			Volume = float.Parse(columnTexts[index++]);
			SpatialBlend = float.Parse(columnTexts[index++]);
			MaxDistance = float.Parse(columnTexts[index++]);

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
					Priority = binaryReader.ReadInt32();
					Loop = binaryReader.ReadBoolean();
					Volume = binaryReader.ReadSingle();
					SpatialBlend = binaryReader.ReadSingle();
					MaxDistance = binaryReader.ReadSingle();
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
