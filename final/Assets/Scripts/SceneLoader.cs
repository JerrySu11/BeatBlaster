using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader singleton;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        //Ensure only one instance of scene loader exists at a time.
        // This allows you to call functions in this script without needing to find a reference to it.
        if (singleton == null)
        {
            singleton = this;
            singleton.health = 4;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // Allows this GameObject to persist through scene changes
    }

    public void NextScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void StartOver()
    {
        
        SceneManager.LoadScene(2);
        
    }
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    // For the optional Scene Transitions portion
    private IEnumerator NextSceneCoroutine()
    {
        // Your code here
        yield return new WaitForSeconds(0f);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        
    }
}
