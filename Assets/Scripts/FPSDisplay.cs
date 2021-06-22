using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
	private float _deltaTime;

	private void Start()
	{
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
		_deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
	}

	private void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		var style = new GUIStyle();

		var rect = new Rect(0, 0, w, h * 2f / 100);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = h * 3 / 100;
		style.normal.textColor = Color.red;
		var msec = _deltaTime * 1000.0f;
		var fps = 1.0f / _deltaTime;
		var text = $"{msec:0.0} ms ({fps:0.} fps)";
		GUI.Label(rect, text, style);
	}
}
