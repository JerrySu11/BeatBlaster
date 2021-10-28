using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialUI : MonoBehaviour
{
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorial;
    public GameObject startGameUI;
    public GameController game;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        game.player.GetComponent<PlayableDirector>().Stop();
    }

    // Update is called once per frame
    public void GotoSecondTutorial()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
    }
    public void GotoThirdTutorial()
    {
        tutorial2.SetActive(false);
        tutorial3.SetActive(true);
    }

    public void exitTutorial()
    {
        tutorial.SetActive(false);
        Time.timeScale = 1;
        startGameUI.SetActive(true);
        game.player.GetComponent<PlayableDirector>().Play();
    }
}
