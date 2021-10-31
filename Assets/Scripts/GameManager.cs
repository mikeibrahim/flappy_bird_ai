using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour {
	public static GameManager inst;
	int epoche = 0,
		score  = 0;

	private void Awake() {
		inst = this;
	}

	public int GetScore() => score;

	// Increments score and UI
	public void IncrementScore() {
		score++;
		UI.inst.UpdateScore();
	}

	// Resets score and UI
	public void ResetScore() {
		score = 0;
		UI.inst.UpdateScore();
	}

    public void NewEpoche() {
		print("Epoche[" + epoche + "]: " + score);
		epoche++;

		Pipe[] pipes = FindObjectsOfType<Pipe>();
		foreach (Pipe pipe in pipes) { pipe.ReturnPipe(); }
		PipeManager.inst.ResetSpawnInterval();

		ResetScore();
	}
}