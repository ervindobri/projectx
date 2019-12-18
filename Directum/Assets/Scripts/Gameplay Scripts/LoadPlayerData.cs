using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerData : MonoBehaviour
{
	Client client;
	public GameObject playerPrefab;
	private GameObject content;
	private void Awake()
	{
		//Get the current client
		client = FindObjectOfType<Client>();
		content = GameObject.FindGameObjectWithTag("Content");
		if (content == null)
		{
			UnityEngine.Debug.Log("Content not found!");
		}

		//Instantiate player panels
		GameObject p1 = Instantiate(playerPrefab, content.transform) as GameObject;
		p1.transform.Find("TitleImage/Title").GetComponent<Text>().text = "PLAYER 1";
		p1.transform.Find("Name").GetComponent<Text>().text = client.players[0].playerName;
		string[] pcolor = client.players[0].playerColor.Split('-');
		Color color = new Color(float.Parse(pcolor[0]), float.Parse(pcolor[1]), float.Parse(pcolor[2]), float.Parse(pcolor[3]));
		p1.transform.Find("Glow").GetComponent<Image>().color = color;
		p1.transform.Find("TitleImage").GetComponent<Image>().color = color;
		p1.transform.Find("Glow").GetComponent<Image>().enabled = true;
		client.players[0].moveTimer = p1.GetComponent<MoveTimer>();
		client.players[0].playerPanel = p1;

		GameObject p2 = Instantiate(playerPrefab, content.transform) as GameObject;
		p2.transform.Find("TitleImage/Title").GetComponent<Text>().text = "PLAYER 2";
		p2.transform.Find("Name").GetComponent<Text>().text = client.players[1].playerName;
		pcolor = client.players[1].playerColor.Split('-');
		color = new Color(float.Parse(pcolor[0]), float.Parse(pcolor[1]), float.Parse(pcolor[2]), float.Parse(pcolor[3]));
		p2.transform.Find("Glow").GetComponent<Image>().color = color;
		p2.transform.Find("TitleImage").GetComponent<Image>().color = color;
		p2.transform.Find("Glow").GetComponent<Image>().enabled = false;
		client.players[1].moveTimer = p2.GetComponent<MoveTimer>();
		client.players[1].playerPanel = p2;
		if (p1 == null || p2 == null)
		{
			UnityEngine.Debug.Log("Couldn't instantiate players!");
		}
		//---------------------------------------------------------

	}
}
