using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {
	public static AgentManager inst;
	[SerializeField] private Agent agentPrefab;
	public static int agentBufferSize = 250;
	private List<Agent> agentBuffer = new List<Agent>();
	private float 	cloningChance = 0.05f,
					mutationChance = 0.5f,
					mutationRate = 0.1f;

    void Awake() { inst = this; }

	// Load and initialize agents
	private void Start() {
		LoadAgents();
		InitBatch();
	}

	// Initialize the agents in the buffer, no mutaing nescessary here
	public void InitBatch() {
		for (int i = 0; i < agentBufferSize; i++) {
			Agent a = GetAgent(0);
			AgentBrain brain = new AgentBrain();
			brain.Randomize(-1, 1);
			a.SetBrain(brain);
		}
	}

	// Destroys the brains of the worst agents,
	// replaces them with mutations of the best brains of the agents
	// Best agents are in the last positions of the buffer (they survived for the longest)
	public void NewBatch() {
		// print(agentBuffer[agentBuffer.Count - 1].GetBrain().id);
		for (int i = agentBuffer.Count; i > 0; i --) {
			if (i > 2) {
				float chance = cloningChance * (i / (float)agentBufferSize);
				if (Random.Range(0f, 1f) < chance) {
					Agent good = GetAgent(i - 1);
					Agent bad = GetAgent(0);
					// Replace bad brain with good brain
					bad.SetBrain(good.GetBrain());
					// Mutate bad brain
					bad.MutateBrain(mutationRate, mutationChance);
					i--;
					continue;
				}
			}
			GetAgent(i - 1);
		}
	}

	// Loads agents into a buffer
	private void LoadAgents() {
		for (int i = 0; i < agentBufferSize; i++) {
			Agent newAgent = Instantiate(agentPrefab);
			agentBuffer.Add(newAgent);
			newAgent.gameObject.SetActive(false);
		}
	}

	// Returns an agent from the buffer
	private Agent GetAgent(int index) {
		Agent a = agentBuffer[index];
		agentBuffer.RemoveAt(index);
		a.gameObject.SetActive(true);
		return a;
	}

	// Puts an agent into the buffer,
	// If the buffer is full, all the agents are gone
	// If so, reset the scene and start new batches
	public void ReturnAgent(Agent a) {
		a.gameObject.SetActive(false);
		agentBuffer.Add(a);
		a.ResetPosition();

		if (agentBuffer.Count == agentBufferSize) {
			GameManager.inst.NewEpoche();
			NewBatch();
		}
	}
}