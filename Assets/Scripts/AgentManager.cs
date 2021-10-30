using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {
	public static AgentManager inst;
	[SerializeField] private Agent agentPrefab;
	public static int batchSize = 1;
	private List<Agent> agents = new List<Agent>();

    void Awake() { inst = this; }

	private void Start() {
		LoadAgents();
		InitBatch();
	}

	public void InitBatch() {
		for (int i = 0; i < batchSize; i++) {
			Agent a = GetAgent();
		}
	}

	public void NewBatch() {
		for (int i = 0; i < agents.Count; i++) {
			GetAgent();
		}
	}

	private void LoadAgents() {
		for (int i = 0; i < batchSize; i++) {
			Agent newAgent = Instantiate(agentPrefab);
			agents.Add(newAgent);
			newAgent.gameObject.SetActive(false);
		}
	}

	private Agent GetAgent() {
		Agent a = agents[0];
		agents.RemoveAt(0);
		a.gameObject.SetActive(true);
		return a;
	}

	public void ReturnAgent(Agent a) {
		a.gameObject.SetActive(false);
		agents.Add(a);
		if (agents.Count == batchSize) {
			GameManager.inst.ResetScene();
		}
		a.ResetPosition();
	}
}