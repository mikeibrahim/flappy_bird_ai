using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager inst;

	private void Awake() {
		inst = this;
	}

    public void ResetScene() {
		Pipe[] pipes = FindObjectsOfType<Pipe>();
		foreach (Pipe pipe in pipes) {
			pipe.ReturnPipe();
		}
		AgentManager.inst.NewBatch();
	}
}
