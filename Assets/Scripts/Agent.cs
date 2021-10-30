using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
	private float 	gravity = -18f,
					jumpSpeed = 8f,
					bodySize = 0.5f;
	private Vector2 velocity = Vector2.zero;

	private void Start() {
		transform.localScale = new Vector3(bodySize, bodySize, 1f);
	}

    private void Update() {
		Jump(); // Temporary
    }

    private void FixedUpdate() {
		Fall();
		Physics();
		Collisions();
    }

	private void Fall() => velocity.y += gravity * Time.deltaTime;

	private void Jump() {
		if (Input.GetKeyDown(KeyCode.Space))
			velocity.y = jumpSpeed;
	}

	private void Physics() => transform.Translate(velocity * Time.deltaTime);

	private void Collisions() {
		RaycastHit2D hit = Physics2D.CircleCast(transform.position, bodySize / 2f, Vector2.zero);
		if (hit.collider == null)
			return;
		if (hit.collider.GetComponent<Pipe>() != null)
			Die();
	}

	private void Die() {
		AgentManager.inst.ReturnAgent(this);
	}

	public void ResetPosition() {
		transform.position = Vector3.zero;
		velocity = Vector2.zero;
	}
}