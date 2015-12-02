using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Bosco;
using Bosco.Json;
using Bosco.Utils;


public class LeaderboardController : MonoBehaviour {
	
	string score="";
	JSONArray data;

	void Start () {
		try {
			Properties.Init("shmupwarz", @"[
				{""name"":""playSfx"", ""value"":true},
				{""name"":""playMusic"", ""value"":true}
			]");
			data = Properties.GetLeaderboard(1);
		} catch (Exception e) {
			Debug.Log("Error: "+e.ToString());
		}
	}
	
	
	void ButtonClicked(GameObject _obj) {
		Debug.Log("Clicked button:"+_obj.name);
	}
	
	void Update () {
		
	}
	
	void OnGUI()
	{
		GUIStyle style = new GUIStyle();
		style.font = (Font)Resources.Load("OpenDyslexic-Regular", typeof(Font));
		style.normal.textColor = Color.white;
		style.fontSize = 20;


		GUILayout.Space(60);

		GUILayout.BeginArea(new Rect(20, 3*(Screen.height/16), Screen.width-20, Screen.height-(Screen.height/4)));
		GUILayout.BeginHorizontal();
		GUILayout.Label("          Date", style, GUILayout.Width((Screen.width-40)/2));
		GUILayout.Label("Score", style, GUILayout.Width((Screen.width-40)/2));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("");
		GUILayout.EndHorizontal();


		foreach (JSONObject row in data) {
			GUILayout.BeginHorizontal();
			string yyyymmdd = Convert.ToString(row["date"]);
			yyyymmdd = yyyymmdd.Substring(4, 2)+"/"+yyyymmdd.Substring(6)+"/"+yyyymmdd.Substring(0, 4);
			GUILayout.Label("    "+yyyymmdd, style, GUILayout.Width((Screen.width-40)/2));
			GUILayout.Label(Convert.ToString(row["score"]), style, GUILayout.Width((Screen.width-40)/2));
			GUILayout.EndHorizontal();
		}



		GUILayout.EndArea();
		
		GUILayout.Space(25);
		/*
		foreach(Scores _score in highscore)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(_score.name,GUILayout.Width(Screen.width/2));
			GUILayout.Label(""+_score.score,GUILayout.Width(Screen.width/2));
			GUILayout.EndHorizontal();
		}*/
	}
}