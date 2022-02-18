using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenedelay : MonoBehaviour

{
	bool loadingStarted = false;
	float secondsLeft = 0;
	void Start()
	{
		StartCoroutine(DelayLoadLevel(46));
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
			Application.LoadLevel("Level1");
	}
	void OnGUI()
	{
	if (loadingStarted)
	GUI.Label(new Rect(0, 0, 100, 20), secondsLeft.ToString());
	}
}