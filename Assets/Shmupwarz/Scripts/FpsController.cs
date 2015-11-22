using Entitas;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FpsController : MonoBehaviour
{
    public Text label;
	private int totalFrames = 0;
	private int fps = 0;
	private float deltaTime = 0.0f;
	private float elapsedTime = 0.0f;
 
     void Start() {
        label = GetComponent<Text>();
    }

	void Update() {
		totalFrames++;
		elapsedTime += Time.deltaTime;
		if (elapsedTime > 1) {
			fps = totalFrames;
			totalFrames = 0;
			elapsedTime = 0;
		}
        label.text = string.Format("fps: {0:00}", fps);
	}

}