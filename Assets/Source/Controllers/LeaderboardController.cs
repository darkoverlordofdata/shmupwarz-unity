using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Bosco;
using Bosco.Json;
using Bosco.Utils;


public class LeaderboardController : MonoBehaviour {
	
	JSONArray data;
	
	void Start () {

		const int MAX = 5; // MAX leaderboard entries

		Properties.Init("shmupwarz", @"[
			{""name"":""playSfx"", ""value"":true},
			{""name"":""playMusic"", ""value"":true}
		]");
		data = Properties.GetLeaderboard(MAX);

		for (var r=0; r<MAX; r++) {
			string score = "";
			string yyyymmdd = "";
			if (r<data.Count) {
				var row = JSON.Object(data[r]);
				score = Convert.ToString(row["score"]);
				yyyymmdd = Convert.ToString(row["date"]);
				yyyymmdd = yyyymmdd.Substring(4, 2)+"/"+yyyymmdd.Substring(6)+"/"+yyyymmdd.Substring(2, 2);
			}

			GameObject col1 = GameObject.Find("Canvas/Panel/TextRow"+(r+1)+"Date");
			var text1 = (Text)col1.GetComponent("Text");
			text1.text = yyyymmdd;

			GameObject col2 = GameObject.Find("Canvas/Panel/TextRow"+(r+1)+"Score");
			var text2 = (Text)col2.GetComponent("Text");
			text2.text = score;

		}
	}
	
	
	void Update () {
	}

}