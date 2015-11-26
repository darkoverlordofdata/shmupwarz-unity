using System;
using Json;
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
		db = new PlayerPrefsDB("shmupwarz");
		if (db.IsNew()) {
			db.CreateTableWithData("settings", properties);
			db.CreateTable("leaderboard", @"[""date"", ""score""]");
			db.Commit();
		}
	}

	/**
	 * query property name
	 * 
	 * @param {string} name
	 * @returns {string} json query
	 * 
	 */
	private static string query(string name) {
		return string.Format(@"{""query"": {""name"": ""{0}""}", name);
	}

	/**
     * Get Game Property from PlayerPrefs
     *
     * @param property name
     * @return property value
     */
	public static string GetProperty(string name) {
		var property = db.QueryAll("settings", query(name));
		return (string)JSON.Object(JSON.Array(property)[0])[name];
	}

	/**
     * Set Game Property in PlayerPrefs
     *
     * @param property name
     * @param property value
     */
	public static void SetProperty(string name, string value) {
		db.Update("settings", query(name), (JSONObject row) => {
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
		var dd = today.Day.ToString();
		var yyyy = today.Year.ToString();
		var yyyymmdd = yyyy+mm+dd;
	
		if (0 == db.QueryAll("leaderboard", string.Format(@"{""query"":{""date"": ""{0}""}}", yyyymmdd)).Count) {
			db.Insert("leaderboard", string.Format(@"{""date"": ""{0}}, ""score"":{1}}", yyyymmdd, score));
		} else {
			db.Update("leaderboard", string.Format(@"{""date"":""{0}", yyyymmdd),  (JSONObject row) => {
				if (score > (int)row["score"]) {
					row["score"] = score;
				}
				return row;
			});
		}
		db.Commit();
	}

	public static int GetLeaderboard(int count) {
		return db.QueryAll("leaderboard", string.Format(@"{""limit"":{0}, ""sort"": [[""score"", ""DESC""]]}", count)).Count;
	}

}

