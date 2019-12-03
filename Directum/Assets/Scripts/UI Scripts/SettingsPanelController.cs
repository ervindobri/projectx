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
	private Color outlineColor;

	[Header("Display Message")]
	private GameObject messagePanelDisplayText;
	public Animator messagePanelAnimator;

	public static SettingsPanelController Instance { set; get; }
	private void Start()
	{
		Instance = this;
		currentPanelName = this.gameObject.name;
		GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<Canvas>().sortingOrder = 0;
		textObject = GameObject.Find(currentPanelName + "VibratingPanel/Name/InputField/Text").GetComponent<Text>();
		inputTextObject = GameObject.Find(currentPanelName + "VibratingPanel/Name/InputField").GetComponent<InputField>();

		outlineColor = GameObject.Find(currentPanelName + "VibratingPanel/Color/SelectColorButton").GetComponent<Outline>().effectColor;
		//Debug.Log(textObject.name);
		messagePanelAnimator = GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<Animator>();
		messagePanelDisplayText = GameObject.Find("MessagePanel/DisplayText");
	}
	private void Update()
	{
		color = GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<ChangeColor>()._handle.color;
		GameObject.Find(currentPanelName + "VibratingPanel/Color/SelectColorButton").GetComponent<Outline>().effectColor = color;
		playerName = textObject.text;

		//Debug.Log(playerName);
	}
	public void enableColorSelectorPanel()
	{
		GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<Canvas>().sortingOrder = 2;

	}
	public void disableColorSelectorPanel()
	{
		GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<Canvas>().sortingOrder = 0;
	}

	public void SavePlayer()
	{
		//Displays a message so that the user knows the playerdata has been saved
		messagePanelAnimator.SetTrigger("dispMessage");
		messagePanelDisplayText.GetComponent<Text>().text = "Player data has been saved!";
		//Calls the function which creates a file with the data saved
		//Debug.Log( "Player" + index + " data Saved!");
		SaveSystem.SavePlayer(this);
	}
	public void SavePlayerPrefs(int index)
	{
		string name = playerName;
		Color playerColor = GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<ChangeColor>()._handle.color;
		PlayerPrefs.SetString("p"+index+"name", name );
		PlayerPrefs.SetFloat("p" + index + "c1", playerColor.r);
		PlayerPrefs.SetFloat("p" + index + "c2", playerColor.g);
		PlayerPrefs.SetFloat("p" + index + "c3", playerColor.b);
		PlayerPrefs.SetFloat("p" + index + "c4", playerColor.a);

		Debug.Log("Playerprefs Saved for " + name + " " + playerColor.r + playerColor.g + playerColor.b + playerColor.a);

	}
	public void LoadPlayer()
	{
		PlayerData data = SaveSystem.LoadPlayer();

		inputTextObject.text = data.playerName;
		Color loadColor;
		loadColor.r = data.cursorColor[0];
		loadColor.g = data.cursorColor[1];
		loadColor.b = data.cursorColor[2];
		loadColor.a = data.cursorColor[3];

		if (GameObject.Find(currentPanelName + "/ColorSelectorPanel"))
		{
			GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<ChangeColor>()._handle.color = loadColor;
		}
		//Debug.Log(data.playerName + " " + loadColor);

		//Displays a message so that the user knows the playerdata has been saved
		messagePanelAnimator.SetTrigger("dispMessage");
		messagePanelDisplayText.GetComponent<Text>().text = "Player data has been loaded!";


	}
	public void ResetPlayer(int index)
	{
		inputTextObject.text = null;
		GameObject.Find(currentPanelName + "/ColorSelectorPanel").GetComponent<ChangeColor>()._handle.color = outlineColor;
	}
	
}
