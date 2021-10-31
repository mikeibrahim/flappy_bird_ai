using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
	private AgentBrain brain = null;
	private float 	gravity = -40f,
					jumpSpeed = 12f,
					bodySize = 0.85f;
	private Vector2 velocity = Vector2.zero;

	private void Start() {
		transform.localScale = new Vector3(bodySize, bodySize, 1f);
		GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5f);
		// change alpha
	}

	public AgentBrain GetBrain() => this.brain;
	public void SetBrain(AgentBrain brain) => this.brain = brain;
	public void MutateBrain(float mutationRate) => brain.Mutate(mutationRate);

	// private void Update() {
	// 	Jump(); // Temporary
	// }

	// private void Jump() {
	// 	if (Input.GetKeyDown(KeyCode.Space))
	// 		velocity.y = jumpSpeed;
	// }

	private void FixedUpdate() {
		Fall();
		Physics();
		Collisions();
		BrainDecisions();
    }

	private void Fall() => velocity.y += gravity * Time.deltaTime;

	private void Physics() => transform.Translate(velocity * Time.deltaTime);

	private void Collisions() {
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, bodySize / 2f, Vector2.zero);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.GetComponent<Agent>())
				continue;
			if (hit.collider.GetComponent<Pipe>() || hit.collider.tag == "Boundary")
				ReturnAgent();
		}
		// if (hit.collider == null)
		// 	return;
		// if (hit.collider.GetComponent<Pipe>() != null)
		// 	ReturnAgent();
	}

	private void ReturnAgent() {
		AgentManager.inst.ReturnAgent(this);
	}

	public void ResetPosition() {
		transform.position = Vector3.zero;
		velocity = Vector2.zero;
	}

	// The inputs to the brain
	public Matrix Getinputs() {
		(float, float) pipePos = PipeManager.inst.GetClosestPipeHeights();
		return new Matrix(new float[,] {
			{ PipeManager.inst.GetClosestPipeDistance() },
			{ -transform.position.y + pipePos.Item1 },
			{ -pipePos.Item2 + transform.position.y }
		});
	}

	private void BrainDecisions() {
		if (brain == null)
			return;
		Matrix inputs = Getinputs();
		bool jump = brain.Jump(inputs);
		if (jump)
			velocity.y = jumpSpeed;
	}
}

// Uses forward propagation to determine outputs
public class AgentBrain {
	private static readonly int[] structure = {3, 8, 1};
	Matrix[] weights = new Matrix[structure.Length - 1], // List of weights
			 biases = new Matrix[structure.Length - 1]; // List of biases
	public int id;

	public AgentBrain() {
		for (int i = 0; i < structure.Length - 1; i++) {
			weights[i] = new Matrix(structure[i + 1], structure[i]);
			biases[i] = new Matrix(structure[i + 1], 1);
		}
	}

	public void Randomize(float min, float max) {
		id = Random.Range(0, int.MaxValue);
		for (int i = 0; i < structure.Length - 1; i++) {
			weights[i].Randomize(min, max);
			biases[i].Randomize(min, max);
		}
	}
	public void Mutate(float mutationRate) {
		for (int i = 0; i < structure.Length - 1; i++) {
			weights[i].Mutate(mutationRate);
			biases[i].Mutate(mutationRate);
		}
	}

	public bool Jump(Matrix x) {
		Matrix output = Netmath.Forward(x, weights, biases);
		return output.data[0, 0] > 0.5f;
	}
}