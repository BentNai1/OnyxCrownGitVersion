using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour

{
	bool loadingStarted = false;
	float secondsLeft = 0;

	void Start()
	{
		StartCoroutine(DelayLoadLevel(107));
	}
	IEnumerator DelayLoadLevel(float seconds)
	{
		secondsLeft = seconds;
		loadingStarted = true;
		do
		{
			yield return new WaitForSeconds(1);
		} 

		while (--secondsLeft > 0);
			Application.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}
	void OnGUI()
	{
	if (loadingStarted)
	GUI.Label(new Rect(0, 0, 100, 20), secondsLeft.ToString());
	}
}