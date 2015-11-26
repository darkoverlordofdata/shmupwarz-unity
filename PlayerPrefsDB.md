
db.CreateTable("settings", @"[""name"",""value""]");

db.CreateTable("settings", new string [] {"name", "value"});


db.CreateTableWithData("settings", 
	@"[
		{""name"":""leaderboard"", ""value"": false},
		{""name"":""playSfx"", ""value"": false}

	]");

db.CreateTableWithData("settings", new JSONObject [] {
	new JSONObject() {
		{"name", "leaderboard"},
		{"value", "off"}
	},
	new JSONObject() {
		{"name", "playMusic"},
		{"value", "false"}
	},
	new JSONObject() {
		{"name", "playSfx"},
		{"value", "true"}
	}
});

db.Insert("settings", @"{""name"":""leaderboard"", ""value"": false}");
db.Insert("settings", new JSONObject() {
	{"name", "leaderboard"},
	{"value", "off"}
});


db.Update("settings", @"{""name"": ""playMusic""}", (JSONObject row) => {
	row["value"] = true;
	return row;
});

db.Update("settings", new JSONObject() {
	{"name", "playMusic"}
}, 	
(JSONObject row) => {
		row["value"] = true;
		return row;
});*/

db.Query("settings", @"{""name"": ""playMusic""}", sort:@"{""name"": ""asc""}", limit:10);
db.Query("settings", new JSONObject() {
	{"name", "playMusic"}
}, new JSONObject() {
	{"name", "asc"}
}, limit: 10)

db.DeleteRows("settings", new JSONObject() {
	{"name", "playMusic"}
});
db.DeleteRows("settings", @"{""name"": ""playMusic""}")


