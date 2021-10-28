using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Enemy : MonoBehaviour {
	private float moveDist = 0.7f;
	private float xBounds = 8.5f, yBounds = 4.5f;
	public int level;
	public int health = 100;
	public Slider healthBar;
	public GameController game;
    public GameObject player;
    public GameObject shadow;
    public Texture2D shadowTexture;
    public bool moveable = false;

	void Start() {
		PlayableDirector director = GetComponent<PlayableDirector>();
		director.Stop();
		director.initialTime = player.GetComponent<PlayableDirector>().time;
		director.Play();
	}

	void Update() {
		if (level == 3) {
			chase();
		}
	}

    IEnumerator level3_respawnAnimation() {
        for (int i = 0; i < 10; i++) {
            transform.localScale *= 0.8f;
            yield return new WaitForSeconds(.01f);
        }

        if (health > 0) {
            transform.position = game.spawnPosition();

    		for (int i = 0; i < 10; i++) {
    			transform.localScale *= 1.25f;
    			yield return new WaitForSeconds(.01f);
    		}
        }
    }

    public void die()
    {
        StartCoroutine("deathAnimation");
    }

    IEnumerator deathAnimation()
    {
        for (int i = 0; i < 15; i++)
        {
            transform.localScale *= 0.9f;
            yield return new WaitForSeconds(.01f);
        }
    }

    public void gotHit(int damage)
    {
        StartCoroutine("hitAnimation");
        health -= damage;
        healthBar.value = health;

        if (level == 3) {
        	StartCoroutine("level3_respawnAnimation");
        }
    }

    IEnumerator hitAnimation()
    {
        Color temp = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        
        for (int i = 0; i < 10; i++)
        {
            Color current = gameObject.GetComponent<Renderer>().material.color;
            gameObject.GetComponent<Renderer>().material.color = new Color(current.r+(temp.r-1)/10f, current.g + (temp.g/10), current.b + (temp.b/10), 1f);
            yield return new WaitForSeconds(.05f);
        }
    }

	public void randomMove() {
        if (health > 0 && moveable)
        {
            float x = Random.Range(-1.0f, 1.0f), y = Random.Range(-1.0f, 1.0f);
            if (transform.position.x + x < -1 * xBounds || transform.position.x + x > xBounds)
            {
                x = 0;
            }
            if (transform.position.y + y < -1 * yBounds || transform.position.y + y > yBounds)
            {
                y = 0;
            }

            Vector3 dir = new Vector3(x, y, 0);
            if ((player.transform.position - transform.position).magnitude < 3.0f)
            {
                dir = (player.transform.position - transform.position).normalized;
            }
            Vector3 targetPos = transform.position + dir * moveDist;

            StartCoroutine(teleportGradual(transform.position, targetPos));
        }
	}

    IEnumerator teleportGradual(Vector3 startPos, Vector3 endPos)
    {

        for (int i = 1; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(startPos, endPos, 0.1f * i);
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    public void chase() {
        if (health > 0) {
            transform.Translate((player.transform.position - transform.position).normalized * 1.5f * Time.deltaTime);
        }
	}
}
