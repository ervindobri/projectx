using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsPanelController : MonoBehaviour
{
	[Header("Player Data")]
	public string currentPanelName;
	public Color color;
	public string playerName;
	private Text textObject;
	private InputField inputTextObject;
	private GameObject selectColorButton;
	private Color outlineColor;
	private GameObject colorSelectorPanel;
	[Header("Display Message")]
	private MessagePanelController messagePanelController;

	public static SettingsPanelController Instance { set; get; }
	private void Start()
	{
		Instance = this;
		currentPanelName = gameObject.name;
		textObject = GameObject.Find(currentPanelName + "VibratingPanel/Name/InputField/Text").GetComponent<Text>();
		inputTextObject = GameObject.Find(currentPanelName + "VibratingPanel/Name/InputField").GetComponent<InputField>();

		selectColorButton = GameObject.Find("SelectColorButton");
		//Debug.Log(textObject.name);
		colorSelectorPanel = GameObject.Find(currentPanelName + "/ColorSelectorPanel");
		messagePanelController = FindObjectOfType<MessagePanelController>();
	}
	private void Update()
	{
		selectColorButton.GetComponent<Outline>().effectColor = colorSelectorPanel.GetComponent<ChangeColor>().handle.color;
		color = selectColorButton.GetComponent<Outline>().effectColor;
		playerName = textObject.text;
	}
	public void EnableColorSelectorPanel()
	{
		colorSelectorPanel.GetComponent<Canvas>().sortingLayerName = "SetColors";

	}
	public void DisableColorSelectorPanel()
	{
		colorSelectorPanel.GetComponent<Canvas>().sortingLayerName = "Default";
	}

	public void SavePlayer()
	{
		//Displays a message so that the user knows the playerdata has been saved
		//Calls the function which creates a file with the data saved
		SaveSystem.SavePlayer(this);
		messagePanelController.SetMessageAndNotify("PLAYER DATA HAS BEEN SAVED!");
	}
	public void SavePlayerPrefs()
	{
		Color playerColor = colorSelectorPanel.GetComponent<ChangeColor>().handle.color;
		PlayerPrefs.SetString("playername", playerName );
		PlayerPrefs.SetFloat("playercolor1", playerColor.r);
		PlayerPrefs.SetFloat("playercolor2", playerColor.g);
		PlayerPrefs.SetFloat("playercolor3", playerColor.b);
		//Debug.Log("Playerprefs Saved for " + name + " " + playerColor.r + playerColor.g + playerColor.b + playerColor.a);
	}
	public void LoadPlayer()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if ( data == null)
		{
			messagePanelController.SetMessageAndNotify("COULDN'T LOAD PLAYER DATA!");
		}
		else
		{
			inputTextObject.text = data.playerName;
			Color loadColor;
			loadColor.r = data.cursorColor[0];
			loadColor.g = data.cursorColor[1];
			loadColor.b = data.cursorColor[2];
			loadColor.a = 1;
			colorSelectorPanel.GetComponent<ChangeColor>().handle.color = loadColor;
			//Displays a message so that the user knows the playerdata has been saved
			messagePanelController.SetMessageAndNotify("PLAYER DATA HAS BEEN LOADED!");
		}
		
	}
	public void ResetPlayer()
	{
		inputTextObject.text = null;
		colorSelectorPanel.GetComponent<ChangeColor>().handle.color = outlineColor;
	}
	
}
