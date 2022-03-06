using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Level3SceneLoader : MonoBehaviour
{
    public Animator transition;

    public float trainsitionTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("Level3");
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(trainsitionTime);

        SceneManager.LoadScene(levelIndex);

    }


}