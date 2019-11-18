using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	public GameObject clientObject;
	private Client client;
	private GameObject parent;
	private GameObject playerMessages;
	private GameObject messagePanelObject;
	public MessagePanelController messagePanel;
	public GameObject textPrefab;
	private void Awake()
	{
		clientObject = GameObject.FindGameObjectWithTag("Client");
		parent = GameObject.FindGameObjectWithTag("Content");

		playerMessages = GameObject.Find("PlayerMessages/Scrollview/Viewport/Content");

		client = clientObject.GetComponent<Client>();
		displayPlayerName(client);

		// Display clients in scrollview
	}
	public void displayPlayerName(Client client)
	{
		textPrefab.GetComponent<Text>().text = client.GetComponent<Client>().clientName;
		Instantiate(textPrefab, parent.transform);
	}
	public void SendMessage()
	{

	}

}
