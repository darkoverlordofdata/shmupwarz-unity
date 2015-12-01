/**
 *--------------------------------------------------------------------+
 * Properties.cs
 *--------------------------------------------------------------------+
 * Copyright DarkOverlordOfData (c) 2015
 *--------------------------------------------------------------------+
 *
 * This file is a part of Bosco
 *
 * Bosco is free software; you can copy, modify, and distribute
 * it under the terms of the MIT License
 *
 *--------------------------------------------------------------------+
 */
using System;
using Bosco.Json;
using UnityEngine;
namespace Bosco {
	public class Properties
	{
		private static PlayerPrefsDB db;
		private static string dbName;
	
		/**
		* Init
		* 
		* @param {string} name
		* @param {string} properties
		* 
		*/
		public static void Init(string name, string properties) {
	
			if (db != null) return;
			db = new PlayerPrefsDB(name);
			if (db.IsNew()) {
				db.CreateTableWithData("settings", properties);
				db.CreateTable("leaderboard", @"[""date"", ""score""]");
				db.Commit();
			}
		}
	
		/**
		* Get Game Property from PlayerPrefs
		*
		* @param property name
		* @return property value
		*/
		public static string GetProperty(string name) {
			var jsonQueryAll = string.Format(@"{""query"": {""name"": ""{0}""}", name);
			
			var property = db.QueryAll("settings", jsonQueryAll);
			return (string)JSON.Object(JSON.Array(property)[0])[name];
		}
	
		/**
		* Set Game Property in PlayerPrefs
		*
		* @param property name
		* @param property value
		*/
		public static void SetProperty(string name, string value) {
			var jsonQueryAll = string.Format(@"{""query"": {""name"": ""{0}""}", name);
			
			db.Update("settings", jsonQueryAll, (JSONObject row) => {
				row["value"] = value;
				return row;
			});
			db.Commit();
		}
	
		/**
		* Set Score in leaderboard
		* 
		* @param {int} score
		* 
		*/
		public static void SetScore(int score) {
	
			DateTime today = DateTime.Today;
			var mm = today.Month.ToString();
			Debug.Log("mm = "+mm);
			var dd = today.Day.ToString();
			Debug.Log("dd = "+dd);
			var yyyy = today.Year.ToString();
			Debug.Log("yyyy = "+yyyy);
			var yyyymmdd = yyyy+mm+dd;
			Debug.Log("yyyymmdd = "+yyyymmdd);

			var jsonQuery = string.Format(@"{{""query"":{{""date"": ""{0}""}}}}", yyyymmdd);
			Debug.Log("jsonQuery = "+jsonQuery);
			var jsonInsert = string.Format(@"{{""date"": ""{0}"", ""score"":{1}}}", yyyymmdd, score);
			Debug.Log("jsonInsert = "+jsonInsert);
			var jsonUpdate = string.Format(@"{{""date"": ""{0}""}}", yyyymmdd);
			Debug.Log("jsonUpdate = "+jsonUpdate);

			if (0 == db.QueryAll("leaderboard", jsonQuery).Count) {
				Debug.Log("Insert score");
				db.Insert("leaderboard", jsonInsert);
			} else {
				Debug.Log("Update score");
				db.Update("leaderboard", jsonUpdate,  (JSONObject row) => {
					if (score > (int)row["score"]) {
						row["score"] = score;
					}
					return row;
				});
			}
			db.Commit();
		}
	
		/**
		* Get Leaderboard
		* 
		* @param {int} count
		* @returns {JSONArray} the top count scores.
		*/
		public static JSONArray GetLeaderboard(int count) {
			var jsonQueryAll = string.Format(@"{""limit"":{0}, ""sort"": [[""score"", ""DESC""]]}", count);
			
			return db.QueryAll("leaderboard", jsonQueryAll);
		}
	
	}
	
}