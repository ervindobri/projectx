using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This means we can save it in a file
[System.Serializable]
public class PlayerData
{
	public string playerName;
	public float[] cursorColor;

	public PlayerData(SettingsPanelController player )
	{
		playerName = player.playerName;
		//Convert a Color structure to a float array so we can serialize it
		cursorColor = new float[4];
		cursorColor[0] = player.color.r;
		cursorColor[1] = player.color.g;
		cursorColor[2] = player.color.b;
		cursorColor[3] = player.color.a;
	}
}
