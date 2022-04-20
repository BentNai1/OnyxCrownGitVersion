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
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(trainsitionTime);

        SceneManager.LoadScene(levelIndex);

    }


}