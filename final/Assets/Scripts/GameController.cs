using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public GameObject endCanvas;
    public GameObject win;
    public GameObject lose;
    public GameObject player;
    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    private List<GameObject> smallEnemies;
    protected bool end = false;
    public bool isTutorial = false;
    protected List<GameObject> allEnemies;
    public GameObject enemyShadow;
    public int level;
    public int enemyCount;
    public int totalEnemyCount = 10;
    public int maxEnemyAlive = 2;
    private int increment = 1;
    private float xBounds = 8.5f, yBounds = 4.5f;
    public AudioSource destroyEnemySound;
    public GameObject pauseMenu;
    public AudioSource gameOverSound;
    public bool startRightAway = true;
    private bool boss = true;
    private bool start_spawn = true;
    // private bool test = true;

    public void Start()
    {
		smallEnemies = new List<GameObject>();
		if (level == 5) {
			smallEnemies.Add(enemy1);
			smallEnemies.Add(enemy2);
			smallEnemies.Add(enemy3);
			smallEnemies.Add(enemy4);
		}

    	Player p = player.GetComponent<Player>();
    	p.health = isTutorial ? 99999 : SceneLoader.singleton.health;
    	p.healthBar.value = p.health;

        if (startRightAway)
        {
            Time.timeScale = 1;
        }

        enemyCount = 0;
        allEnemies = new List<GameObject>();

        if (level == 5) {
        	Spawn();
        	enemyCount = 1;
        	Invoke("hideBoss", 7.0f);
        } else {
	        while (enemyCount < maxEnemyAlive) {
	        	enemyCount++;
	        	Spawn();
	        }

			maxEnemyAlive += increment++;
		}
    }

    public void Update()
    {   
        // if (test) {
        //     GameWin();
        // }

        if (end) {
        	return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            player.GetComponent<PlayableDirector>().Pause();
        }
        if (totalEnemyCount == 0) {
        	addHealth();
            GameWin();
        } else if (enemyCount < totalEnemyCount) {
            if (enemyCount == 0) {
            	if (level == 5 && boss) {
            		addHealth();
            		showBoss();
        		} else {
	                int spawnNumber = maxEnemyAlive < totalEnemyCount ? maxEnemyAlive : totalEnemyCount;
	                for (int i = 0; i < spawnNumber; i++)
	                {
	                    enemyCount++;
	                    Invoke("Spawn", 2.0f);
	                }

	                if (level == 5) {
	                	boss = true;
	                } else {
	                	addHealth();
	                }

	                if (maxEnemyAlive < 5) {
	                	maxEnemyAlive += increment++;
	                }
	            }
            }
        }
        if (level == 5 && !start_spawn && allEnemies.Count == 0) {
        	GameWin();
        }
    }
    
    public void exitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        player.GetComponent<PlayableDirector>().Resume();
    }

    public void GameWin()
    {
    	SceneLoader.singleton.health = player.GetComponent<Player>().health;
        Time.timeScale = 0;
        endCanvas.SetActive(true);
        win.SetActive(true);
        GameStop();
    }

    public void GameDie()
    {
        gameOverSound.Play(0);
        Time.timeScale = 0;
        endCanvas.SetActive(true);
        lose.SetActive(true);
        GameStop();
    }

    public void GameStop() {
    	end = true;
        player.GetComponent<PlayableDirector>().Stop();
        foreach (GameObject enemy in allEnemies) {
            enemy.GetComponent<PlayableDirector>().Stop();
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        enemyCount--;
        totalEnemyCount--;
        allEnemies.Remove(enemy);
    }

    public void Spawn() {
    	GameObject newEnemy;
        if (level == 5) {
        	if (start_spawn) {
        		newEnemy = Instantiate(enemy, spawnPosition(), Quaternion.identity);
        	} else {
        		newEnemy = Instantiate(smallEnemies[Random.Range(0, 4)], spawnPosition(), Quaternion.identity);
        	}
        } else {
        	newEnemy = Instantiate(enemy, spawnPosition(), Quaternion.identity);
        }

        newEnemy.tag = "Enemy";
        newEnemy.GetComponent<Enemy>().game = this;
        newEnemy.GetComponent<Enemy>().player = player;
        newEnemy.SetActive(false);

        GameObject newEnemyShadow = Instantiate(enemyShadow, newEnemy.transform.position, Quaternion.identity);
        StartCoroutine(DisplayEnemy(newEnemy, newEnemyShadow));
    }

    public IEnumerator DisplayEnemy(GameObject enemy, GameObject enemyShadow) {
        yield return new WaitForSeconds(2.0f);
        enemy.SetActive(true);
        Destroy(enemyShadow);
        yield return new WaitForSeconds(0.1f);
        enemy.GetComponent<Enemy>().moveable = true;
        allEnemies.Add(enemy);
        start_spawn = false;
    }

    public Vector3 spawnPosition() {
        Vector3 pos = player.transform.position;
        int x_sign = Random.Range(1, 3), y_sign = Random.Range(1, 3);
        float x_offset = Random.Range(0f, 3.5f) * Mathf.Pow(-1, x_sign), y_offset = Random.Range(0f, 3.5f) * Mathf.Pow(-1, y_sign);
        pos.x += x_offset;
        pos.y += y_offset;
        if (pos.x < -1 * xBounds || pos.x > xBounds) {
            pos.x -= 2 * x_offset;
        }
        if (pos.y < -1 * yBounds || pos.y > yBounds) {
            pos.y -= 2 * y_offset;
        }

        foreach (GameObject enemy in allEnemies) {
        	if (isOverlap(enemy.transform.position, pos)) {
        		return spawnPosition();
        	} else if (isOverlap(player.transform.position, pos)) {
        		return spawnPosition();
        	}
        }

        return pos;
    }

    public bool isOverlap(Vector3 pos1, Vector3 pos2) {
    	return Mathf.Abs(pos1.x - pos2.x) < 1 && Mathf.Abs(pos1.y - pos2.y) < 1;
    }

    public void addHealth() {
        Player p = player.GetComponent<Player>();
        if (p.health < p.maxHealth) {
        	p.setHealth(p.health + 1);
        }
    }

    public void showBoss() {
    	enemyCount = 1;

    	StartCoroutine("showBossAnimation");
    	Invoke("hideBoss", 8.5f);
    }

    IEnumerator showBossAnimation() {
    	Vector3 pos = spawnPosition();
    	GameObject newEnemyShadow = Instantiate(enemyShadow, pos, Quaternion.identity);
    	allEnemies[0].transform.position = pos;
        yield return new WaitForSeconds(2.0f);
        allEnemies[0].SetActive(true);
        Destroy(newEnemyShadow);
        yield return new WaitForSeconds(0.1f);

		PlayableDirector director = allEnemies[0].GetComponent<PlayableDirector>();
		director.Stop();
		director.initialTime = player.GetComponent<PlayableDirector>().time;
		director.Play();
		
        allEnemies[0].GetComponent<Enemy>().moveable = true;
    }

    public void hideBoss() {
    	StartCoroutine("hideBossAnimation");
    }

    IEnumerator hideBossAnimation() {
    	allEnemies[0].GetComponent<Enemy>().moveable = false;

    	bool active = false;
    	for (int i = 0; i < 7; i++) {
    		allEnemies[0].SetActive(active);
    		active = !active;
    		yield return new WaitForSeconds(0.2f);
    	}

    	enemyCount = 0;
    	boss = false;
    }
}
