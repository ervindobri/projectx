using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerData : MonoBehaviour
{
	SettingsPanelController settings;

	private List<GameObject> playerNames;

	public static bool playersLoaded;
	private void Start()
	{
		playerNames = new List<GameObject>();
		settings = GetComponent<SettingsPanelController>();
		GameObject.Find(this.gameObject.name + "/Player1Panel/Text/Name/");
		playerNames.Add(GameObject.Find(this.gameObject.name +"/PlayerPanel"));
		playerNames.Add(GameObject.Find(this.gameObject.name +"/PlayerPanel"));
		
		// Load player data on Load
		LoadPlayersOnLoad();
		playersLoaded = true;
	}
	private void LoadPlayersOnLoad()
	{
		PlayerData data1 = SaveSystem.LoadPlayer(1);
		PlayerData data2 = SaveSystem.LoadPlayer(2);

		playerNames[0].transform.Find("Text/Name").GetComponent<Text>().text = data1.playerName;
		playerNames[1].transform.Find("Text/Name").GetComponent<Text>().text = data2.playerName;
		Color loadColor;
		loadColor.r = data1.cursorColor[0];
		loadColor.g = data1.cursorColor[1];
		loadColor.b = data1.cursorColor[2];
		loadColor.a = data1.cursorColor[3];
		playerNames[0].transform.Find("Text").GetComponent<Text>().color = loadColor;
		loadColor.r = data2.cursorColor[0];
		loadColor.g = data2.cursorColor[1];
		loadColor.b = data2.cursorColor[2];
		loadColor.a = data2.cursorColor[3];
		playerNames[1].transform.Find("Text").GetComponent<Text>().color = loadColor;
	}
}
