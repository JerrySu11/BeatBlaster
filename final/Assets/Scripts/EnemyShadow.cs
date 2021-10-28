using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadow : MonoBehaviour {
	public float speed = 1.0f;
	public float flashDirection = -1.0f;
	public float alpha = 0.5f;

	void Start() {

	}

	void Update() {
		if (alpha >= 0.5f) {
			flashDirection = -1.0f;
		} else if (alpha <= 0f) {
			flashDirection = 1.0f;
		}

		alpha += Time.deltaTime * flashDirection * speed;
		GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, alpha);
	}
}
