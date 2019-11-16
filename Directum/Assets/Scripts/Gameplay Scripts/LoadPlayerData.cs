using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadPlayerData : MonoBehaviour
{
	SettingsPanelController settings;

	private GameObject playerNameObject;

	private string playerName;
	private Color loadColor;
	public static bool playersLoaded;
	private void Start()
	{
		settings = GetComponent<SettingsPanelController>();
		if ( SceneManager.GetActiveScene().name == "GameMain")
		{
			playerNameObject = GameObject.Find(this.gameObject.name +"/PlayerPanel");
		}
		//playerNames.Add(GameObject.Find(this.gameObject.name +"/Player2Panel"));
		
		// Load player data on Load
		LoadPlayerOnLoad();
		playersLoaded = true;
	}
	private void LoadPlayerOnLoad()
	{
		try
		{
			PlayerData data = SaveSystem.LoadPlayer();
			playerName = data.playerName;
			loadColor.r = data.cursorColor[0];
			loadColor.g = data.cursorColor[1];
			loadColor.b = data.cursorColor[2];
			loadColor.a = data.cursorColor[3];
			if (SceneManager.GetActiveScene().name == "GameMain")
			{
				playerNameObject.transform.Find("Text/Name").GetComponent<Text>().text = data.playerName;
				loadColor = transform.Find("Text").GetComponent<Text>().color = loadColor;
			}
		}
		catch (System.Exception)
		{
			Debug.Log("Save file not found");
			throw;
		}



	}
}
