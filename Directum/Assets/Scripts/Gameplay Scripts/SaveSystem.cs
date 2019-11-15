using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // this allows us to access binary formatting

//Static class - can't be instantiated!
public static class SaveSystem
{
	public static void SavePlayer ( SettingsPanelController player, int index)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player" + index + ".fun"; // gets a path to the app directory
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new PlayerData(player);
		formatter.Serialize(stream, data);

		stream.Close();
	}
	public static PlayerData LoadPlayer(int index)
	{
		//Debug.Log(Application.persistentDataPath);
		string path = Application.persistentDataPath + "/player" + index + ".fun"; // gets a path to the app directory
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			//Casting formatter as PlayerData type
			PlayerData data = formatter.Deserialize(stream) as PlayerData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}
