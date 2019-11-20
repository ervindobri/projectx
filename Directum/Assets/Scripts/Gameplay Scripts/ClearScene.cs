using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScene : MonoBehaviour
{
	public void BackButton()
	{
		GameplayManager gm = FindObjectOfType<GameplayManager>();
		if ( gm != null)
		{
			Destroy(gm);
		}
		Server s = FindObjectOfType<Server>();
		if ( s != null)
		{
			Destroy(s);
		}
		Client c = FindObjectOfType<Client>();
		if (c != null)
		{
			Destroy(c);
		}
	}
}
