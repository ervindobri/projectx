using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	public GameObject clientObject;
	public Client client;
	private GameObject parent;
	private GameObject playerMessages;
	private GameObject messagePanelObject;
	public MessagePanelController messagePanel;
	public GameObject textPrefab;

	public GameObject contentParent;
	public InputField inputField;

	private GameObject sendButton;
	private bool wasPressed;

	private void Awake()
	{
		sendButton = GameObject.Find("SendButton");
		clientObject = GameObject.FindGameObjectWithTag("Client");
		parent = GameObject.FindGameObjectWithTag("Content");

		playerMessages = GameObject.Find("PlayerMessages/Scrollview/Viewport/Content");

		client = clientObject.GetComponent<Client>();
		//displayPlayerName(client);

		// Display clients in scrollview
	}

	private void Update()
	{
		if (Input.GetAxis("Submit") == 1 && !wasPressed)
		{
			sendButton.GetComponent<Button>().onClick.Invoke();
			Debug.Log("invoked");
			wasPressed = true;
			StartCoroutine(ResetBool());
		}
	}
	public void SendMessage()
	{
		textPrefab.GetComponent<Text>().text = client.GetComponent<Client>().clientName + " says: " + inputField.text;
		//Send to server
		client.Send(textPrefab.GetComponent<Text>().text);
		inputField.Select();
		inputField.text = "";
	}
	IEnumerator ResetBool()
	{
		yield return new WaitForSeconds(0.5f);
		wasPressed = false;
	}
}
