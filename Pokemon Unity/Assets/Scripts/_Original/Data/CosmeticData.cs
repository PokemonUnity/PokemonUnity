using System;

[System.Serializable]
public class CosmeticData
{
	public enum Gender
	{
		MALE,
		FEMALE,
		BOTH
	}

	public string name;

	private string file_name;
	private Gender gender;


	public CosmeticData(string name, string path, Gender gender)
	{
		this.name = name;
		file_name = path;
		this.gender = gender;
	}


	public string getFileName()
	{
		return file_name;
	}

	public Gender getGender()
	{
		return gender;
	}
}
