using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {
	public static PipeManager inst;
	[SerializeField] private Pipe pipePrefab;
	private static int pipeBufferSize = 20;
	private List<Pipe> pipeBuffer = new List<Pipe>();
	private List<Pipe> closestPipes = new List<Pipe>();
	private float 	spawninterval = 2f,
					pipeSpeed = 4f,
					holeSize = 1.5f,
					pipeWidth = 1.5f;
	float 	currentSpawnInterval,
			screenHeight, screenWidth,
			pipeDeath;

	void Awake() { inst = this; }
	
    void Start() {
		screenHeight = Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		currentSpawnInterval = spawninterval;
		pipeDeath = (screenWidth * 2 + 2) / pipeSpeed;
		LoadPipes();
		SpawnPipeGroup();
    }

    void FixedUpdate() {
        if (currentSpawnInterval <= 0) {
			currentSpawnInterval = spawninterval;
			SpawnPipeGroup();
		} else {
			currentSpawnInterval -= Time.deltaTime;
		}
    }

	// Puts objects into a buffer
	private void LoadPipes() {
		for (int i = 0; i < pipeBufferSize; i++) {
			Pipe newPipe = Instantiate(pipePrefab);
			pipeBuffer.Add(newPipe);
			newPipe.gameObject.SetActive(false);
		}
	}

	// Returns a pipe from the buffer
	private Pipe GetPipe() {
		Pipe p = pipeBuffer[0];
		pipeBuffer.RemoveAt(0);
		p.gameObject.SetActive(true);
		return p;
	}

	public void ReturnPipe(Pipe p) {
		p.gameObject.SetActive(false);
		pipeBuffer.Add(p);
		p.Reset();
	}

	public void ResetSpawnInterval() {
		currentSpawnInterval = 0;
		StopAllCoroutines();
		closestPipes.Clear();
	}

	private void SpawnPipeGroup() {
		// Object Pooling 
		Pipe p1 = GetPipe();
		Pipe p2 = GetPipe();

		// Positioning the pipes at the top and bottom of the screen
		p1.transform.position = new Vector3(screenWidth + 1, -screenHeight, 0); // Position
		p1.transform.localScale = new Vector3(1 * pipeWidth, 1, 1); // Scale

		p2.transform.localScale = new Vector3(-1 * pipeWidth, 1, 1); // Scale
		p2.transform.position = new Vector3(screenWidth + 1, screenHeight, 0);
		p2.transform.Rotate(0, 0, 180);

		// Getting a random point for the hole
		float holePos = Random.Range(-screenHeight + 1 + holeSize, screenHeight - 1 - holeSize);
		// float holePos = 0;

		// Setting the pipe heights
		p1.SetHeight(screenHeight - holePos - holeSize);
		p2.SetHeight(screenHeight + holePos - holeSize);

		// Setting the pipe speeds
		p1.SetSpeed(pipeSpeed);
		p2.SetSpeed(pipeSpeed);

		// Calculate the time at which the pipes will go off the screen
		StartCoroutine(p1.DeathTimer(pipeDeath));
		StartCoroutine(p2.DeathTimer(pipeDeath));

		// The timer for score
		StartCoroutine(ScoreTimer(pipeDeath / 2f + 0.2f));

		// For the inputs to the agents
		closestPipes.Add(p1);
		closestPipes.Add(p2);
	}

	public IEnumerator ScoreTimer(float time) {
		yield return new WaitForSeconds(time);
		GameManager.inst.IncrementScore();
		closestPipes.RemoveAt(0);
		closestPipes.RemoveAt(0);
	}

	public float GetClosestPipeDistance() {
		if (closestPipes.Count > 0) {
			return closestPipes[0].transform.position.x;
		} else {
			return 0;
		}
	}

	public (float, float) GetClosestPipeHeights() {
		if (closestPipes.Count > 0) {
			return (-screenHeight + closestPipes[0].GetHeight(), screenHeight - closestPipes[1].GetHeight());
		} else {
			return (0, 0);
		}
	}
}
