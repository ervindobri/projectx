using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
	// Drag & drop slider
	public Slider slider;

	// Drag & drop handle
	public Image handle;

	public void Start()
	{
		slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{
		handle.color = Color.HSVToRGB(slider.value, 1, 1);
	}
}
