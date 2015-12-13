using UnityEngine;
using System;
using System.Collections;
using HelloWorld;

public class UIManagerScript : MonoBehaviour {

	public void Start() {
        HelloWorldClass x = new HelloWorldClass();
        Debug.Log(x.SayHello()+" From Nemerle");
	}

	public void StartGame() {
		Application.LoadLevel("Game Scene");
	}

	public void LoadLeaderboard() {
		Application.LoadLevel("LeaderboardScene");
	}

	public void LoadCredits() {
		Application.LoadLevel("CreditsScene");
	}
	
	public void LoadMenu() {
		Application.LoadLevel("MenuScene");
	}

}
