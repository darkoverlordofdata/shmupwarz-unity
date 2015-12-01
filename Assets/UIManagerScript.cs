using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

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
