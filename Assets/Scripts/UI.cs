using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour {
	public static UI inst;
	[SerializeField] private TMP_Text scoreText;
	private int score = 0;

	private void Awake() {
		inst = this;
	}

    private void Start() {
        UpdateScore();
    }

	public void UpdateScore() {
		scoreText.text = GameManager.inst.GetScore().ToString();
	}
}
