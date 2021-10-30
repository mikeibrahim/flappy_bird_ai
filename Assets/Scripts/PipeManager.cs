using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {
	[SerializeField] private Pipe pipePrefab;
	private float 	spawninterval = 2.5f,
					pipeSpeed = 2f,
					holeSize = 1f;
	float currentSpawnInterval;
	float screenHeight, screenWidth;
	
    void Start() {
		screenHeight = Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		currentSpawnInterval = spawninterval;
    }

    void FixedUpdate() {
        if (currentSpawnInterval <= 0) {
			currentSpawnInterval = spawninterval;
			SpawnPipeGroup();
		} else {
			currentSpawnInterval -= Time.deltaTime;
		}
    }

	private void SpawnPipeGroup() {
		Pipe p1 = Instantiate(pipePrefab);
		p1.transform.position = new Vector3(screenWidth + 1, -screenHeight, 0);
		p1.SetSpeed(pipeSpeed);

		Pipe p2 = Instantiate(pipePrefab);
		p2.transform.localScale = new Vector3(-1, 1, 1);
		p2.transform.position = new Vector3(screenWidth + 1, screenHeight, 0);
		p2.transform.Rotate(0, 0, 180);
		p2.SetSpeed(pipeSpeed);

		float holePos = Random.Range(-screenHeight + 1 + holeSize, screenHeight - 1 - holeSize);

		p1.SetHeight(screenHeight - holePos - holeSize);
		p2.SetHeight(screenHeight + holePos - holeSize);
	}
}
