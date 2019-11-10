using UnityEngine.UI;
using UnityEngine;

public class BackButtonController : MonoBehaviour
{
	private bool isVisible;
	[Header("Back or cancel button")]
	public GameObject button;

	private void Start()
	{

	}
	void Update()
    {
		if ( this.gameObject.GetComponent<RectTransform>().offsetMin == new Vector2(0,0) && this.gameObject.GetComponent<RectTransform>().offsetMax == new Vector2(0, 0))
		{
			isVisible = true;
		}
		else if ( this.gameObject.name == "Canvas")
		{
			isVisible = true;
		}
		else
		{
			isVisible = false;
		}
		if (Input.GetKeyDown("escape") && isVisible)
		{
			button.GetComponent<Button>().onClick.Invoke();
		}
        
    }
}
