
//This means we can save it in a file
[System.Serializable]
public class PlayerData
{
	public string playerName;
	public float[] cursorColor = new float[3];
	public PlayerData(SettingsPanelController player )
	{
		playerName = player.playerName;
		//Convert a Color structure to a float array so we can serialize it
		cursorColor[0] = player.color.r;
		cursorColor[1] = player.color.g;
		cursorColor[2] = player.color.b;
	}
}
