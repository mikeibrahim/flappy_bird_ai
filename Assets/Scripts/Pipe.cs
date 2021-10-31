using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
	float speed;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetSpeed(float speed) => this.speed = speed;

	public void SetHeight(float height) => spriteRenderer.size = new Vector2(1, height);
	public float GetHeight() => spriteRenderer.size.y;

	void FixedUpdate() {
        this.transform.Translate(-transform.right * speed * Time.deltaTime);
    }

	public IEnumerator DeathTimer(float time) {
		yield return new WaitForSeconds(time);
		ReturnPipe();
	}

	public void ReturnPipe() {
		PipeManager.inst.ReturnPipe(this);
	}

	public void Reset() {
		transform.localScale = Vector3.one;
		transform.position = Vector3.zero;
		spriteRenderer.size = Vector2.one;
		transform.rotation = Quaternion.identity;
	}
}