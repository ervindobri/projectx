using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
	// Drag & drop slider
	public Slider _slider;

	// Drag & drop handle
	public Image _handle;

	public void Start()
	{
		_slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{
		_handle.color = Color.HSVToRGB(_slider.value, 1, 1);
	}
	public void getSelectedColor()
	{
		Debug.Log(_handle.color);
	}
}
