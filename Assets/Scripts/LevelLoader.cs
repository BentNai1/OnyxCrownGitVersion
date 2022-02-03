using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float trainsitionTime = 2f;

    // Update is called once per frame
    void Update()
    {
        
    }


public void OnTriggerEnter(Collider other)
    {

        
        if(other.CompareTag("Player"))
        {

            LoadNextLevel();


        }



    }


public void LoadNextLevel()
{
    
    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    
}


IEnumerator LoadLevel(int levelIndex)
{
    transition.SetTrigger("Start");

    yield return new WaitForSeconds(trainsitionTime);

    SceneManager.LoadScene(levelIndex);

}


}
