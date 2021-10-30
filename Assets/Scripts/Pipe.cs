using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {
	private float speed;

	public void SetSpeed(float speed) => this.speed = speed;

	public void SetHeight(float height) => GetComponent<SpriteRenderer>().size = new Vector2(1, height);

	void FixedUpdate() {
        this.transform.Translate(-transform.right * speed * Time.deltaTime);
    }
}
