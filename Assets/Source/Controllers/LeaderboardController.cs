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

		try {
			Properties.Init("shmupwarz", @"[
				{""name"":""playSfx"", ""value"":true},
				{""name"":""playMusic"", ""value"":true}
			]");
			data = Properties.GetLeaderboard(5);
		} catch (Exception e) {
			Debug.Log("Error: "+e.ToString());
		}

		for (var r=0; r<5; r++) {
			string yyyymmdd = "";
			string score = "";
			if (r<data.Count) {
				var row = JSON.Object(data[r]);
				yyyymmdd = Convert.ToString(row["date"]);
				yyyymmdd = yyyymmdd.Substring(4, 2)+"/"+yyyymmdd.Substring(6)+"/"+yyyymmdd.Substring(2, 2);
				score = Convert.ToString(row["score"]);
			}

			Debug.Log("Canvas/Panel/TextRow"+r+"Date");
			Debug.Log("Canvas/Panel/TextRow"+r+"Score");

			GameObject col1 = GameObject.Find("Canvas/Panel/TextRow"+(r+1)+"Date");
			var text1 = (Text)col1.GetComponent("Text");
			text1.text = yyyymmdd;

			GameObject col2 = GameObject.Find("Canvas/Panel/TextRow"+(r+1)+"Score");
			var text2 = (Text)col2.GetComponent("Text");
			text2.text = score.ToString();

			Debug.Log(""+r+") "+yyyymmdd + " = "+score);
		}
	}
	
	
	void Update () {
	}

}