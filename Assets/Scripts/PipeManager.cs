using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {
	public static PipeManager inst;
	[SerializeField] private Pipe pipePrefab;
	private static int pipeBuffer = 20;
	private List<Pipe> pipes = new List<Pipe>();
	private float 	spawninterval = 2f,
					pipeSpeed = 2.25f,
					holeSize = 1.5f;
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

	private void LoadPipes() {
		for (int i = 0; i < pipeBuffer; i++) {
			Pipe newPipe = Instantiate(pipePrefab);
			pipes.Add(newPipe);
			newPipe.gameObject.SetActive(false);
		}
	}

	private Pipe GetPipe() {
		Pipe p = pipes[0];
		pipes.RemoveAt(0);
		p.gameObject.SetActive(true);
		return p;
	}

	public void ReturnPipe(Pipe p) {
		p.gameObject.SetActive(false);
		pipes.Add(p);
		p.Reset();
	}

	private void SpawnPipeGroup() {
		// Object Pooling 
		Pipe p1 = GetPipe();
		Pipe p2 = GetPipe();

		// Positioning the pipes at the top and bottom of the screen
		p1.transform.position = new Vector3(screenWidth + 1, -screenHeight, 0); // Position

		p2.transform.localScale = new Vector3(-1, 1, 1);
		p2.transform.position = new Vector3(screenWidth + 1, screenHeight, 0);
		p2.transform.Rotate(0, 0, 180);

		// Getting a random point for the hole
		float holePos = Random.Range(-screenHeight + 1 + holeSize, screenHeight - 1 - holeSize);

		// Setting the pipe heights
		p1.SetHeight(screenHeight - holePos - holeSize);
		p2.SetHeight(screenHeight + holePos - holeSize);

		// Setting the pipe speeds
		p1.SetSpeed(pipeSpeed);
		p2.SetSpeed(pipeSpeed);

		// Calculate the time at which the pipes will go off the screen
		StartCoroutine(p1.DeathTimer(pipeDeath));
		StartCoroutine(p2.DeathTimer(pipeDeath));
	}
}
