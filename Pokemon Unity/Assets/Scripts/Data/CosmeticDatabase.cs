using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CosmeticDatabase
{

	public static CosmeticData[] haircut = new CosmeticData[]
	{
		new CosmeticData("default", "default_f", CosmeticData.Gender.FEMALE),
		new CosmeticData("default", "default_m", CosmeticData.Gender.MALE)
	};

	public static CosmeticData[] outfit = new CosmeticData[]
	{
		//empty for now / not implemented
	};

	static CosmeticData getDataByName(CosmeticData[] table, string name, bool isMale)
	{
		int i = 0;
		while(i < table.Length)
		{
			if (isMale)
			{
				if (table[i].name == name && table[i].getGender() == CosmeticData.Gender.MALE || table[i].getGender() == CosmeticData.Gender.BOTH) return table[i];
			}
			else
			{
				if (table[i].name == name && table[i].getGender() == CosmeticData.Gender.FEMALE || table[i].getGender() == CosmeticData.Gender.BOTH) return table[i];
			}

			i++;
		}

		return null;
	}
	
	public static CosmeticData getHaircut(string name, bool isMale)
	{
		return getDataByName(haircut, name, isMale);
	}
}
