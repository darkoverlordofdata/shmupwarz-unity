using System;
using Json;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/**
 * A spork of https://github.com/knadh/localStorageDB to CSharp/Unity5
 * 
 * PlayerPrefsDB 0.0.1
 * 
 * PlayerPrefsDB is a simple layer over PlayerPrefs that provides a set of functions to store
 * structured data like databases and tables. It provides basic insert/update/delete/query
 * capabilities. 
 * 
 * Underneath it all, the structured data is stored as serialized JSON in PlayerPrefs.
 * 
 * Database API:
 * 
 * AlterTable(string tableName, string newField, string defaultValue)
 * ColumnExists(string tableName, string fieldName)
 * Commit()
 * CreateTable(string tableName, string fields)
 * CreateTable(string tableName, string[] fields)
 * CreateTableWithData(string tableName, string data)
 * CreateTableWithData(string tableName, object[] data) 
 * DeleteRows(string tableName, string query) 
 * DeleteRows(string tableName, object query)
 * Drop()
 * DropTable(string tableName) 
 * Insert(string tableName, string data) 
 * Insert(string tableName, object data) 
 * InsertOrUpdate(string tableName, object query, object data)
 * IsNew()
 * Query(string tableName, object query=null, int limit=-1, int start=-1, object[] sort=null, object[] distinct=null)
 * QueryAll(string tableName, string query) 
 * RowCount(string tableName) 
 * Serialize() 
 * TableCount() 
 * TableExists(string tableName)
 * TableFields(string tableName)
 * Truncate(string tableName) 
 * Update(string tableName, string query, Func<JSONObject, JSONObject> updateFunction)
 * Update(string tableName, object query, Func<JSONObject, JSONObject> updateFunction) 
 */
public class PlayerPrefsDB {
	private string db_prefix = "db_";
	private string db_id;
	private bool db_new;
	private JSONObject db;
	private Regex rx_validate_name = new Regex(@"[^A-Za-z_0-9]");

	public PlayerPrefsDB (string db_name) {
		db_id = db_prefix + db_name;
		db_new = false;

		if (PlayerPrefs.HasKey(db_id)) {
			db = (JSONObject)JSON.Parse(PlayerPrefs.GetString(db_id));
		} else {
			db = (JSONObject)JSON.Parse(@"{""tables"": {}, ""data"": {}}");
			Commit();
			db_new = true;
		}
	}

	/*
	 * Commit the database to localStorage
	 * 
	 */
	public void Commit() {
		commit();
	}
	
	/*
	 * Is this instance a newly created database?
	 * 
	 * @returns {bool} true if db is new
	 */
	public bool IsNew() {
		return db_new;
	}
	
	/*
	 * Delete the database
	 * 
	 */
	public void Drop() {
		drop();
	}
	
	/*
	 * Serialize the database
	 * 
	 * @returns {string} db as a json string
	 * 
	 */
	public string Serialize() {
		return serialize();
	}
	
	/*
	 * Check whether a table exists
	 * 
	 * @param {string} tableName
	 * @returns {bool} true if the table exists
	 * 
	 */
	public bool TableExists(string tableName) {
		return tableExists(tableName);
	}
	
	/*
	 * List of keys in a table
	 * 
	 * @param {string} tableName
	 * @returns {JSONArray} array of field names in the table
	 * 
	 */
	public JSONArray TableFields(string tableName) {
		return tableFields(tableName);
	}
	
	/*
	 * Number of tables in the database
	 * 
	 * @returns {int} table count
	 * 
	 */
	public int TableCount() {
		return tableCount();
	}
	
	
	/**
	 * Column Exists
	 * 
	 * @param {string} tableName
	 * @param {string} fieldName
	 * @returns {bool} true if the table contains the field.
	 * 
	 */
	public bool ColumnExists(string tableName, string fieldName) {
		return columnExists(tableName, fieldName);
	}
	
	/**
	 * Create Table
	 * @param {string} tableName
	 * @param {string} fields
	 * @returns true on success
	 * 
	 * Fields is a json formatted string representing
	 * a string array of field names.
	 * 
	 * Usage:
	 * 
	 * db.CreateTable("settings", @"[""name"",""value""]");
	 * 
	 */
	public bool CreateTable(string tableName, string fields) {
		var args = JSON.Array(JSON.Parse(fields));
		var fields_array = new List<string>();
		foreach (var arg in args) {
			fields_array.Add(arg.ToString());
		}
		return CreateTable(tableName, fields_array.ToArray());
	}
	
	/**
	 * Create Table
	 * @param {string} tableName
	 * @param {string[]} fields
	 * @returns true on success
	 * 
	 * Fields is a string array of field names.
	 * 
	 * Usage:
	 * 
	 * db.CreateTable("settings", new string [] {"name", "value"});
	 * 
	 */
	public bool CreateTable(string tableName, string[] fields) {
		var result = false;
		if (!validateName(tableName)) {
			throw new Exception("The database name '" + tableName + "' contains invalid characters.");
		} else if (tableExists(tableName)) {
			throw new Exception("The table name '" + tableName + "' already exists.");
		} else {
			var is_valid = true;
			for (var i=0; i<fields.Length; i++) {
				if (!validateName(fields[i])) {
					is_valid = false;
					throw new Exception("The field name '" + tableName + ":"+ fields[i] + "' contains invalid characters.");
					break;
				}
			}
			if (is_valid) {
				// de-duplicate the field list
				var fields_literal = new Dictionary<string, bool>();
				for (var i=0; i<fields.Length; i++) {
					if (!fields_literal.ContainsKey(fields[i])) {
						fields_literal.Add(fields[i], true);
					}
				}
				if (fields_literal.ContainsKey("ID")) {
					fields_literal.Remove("ID");
				}
				var new_fields = new JSONArray();
				new_fields.Add("ID");
				foreach (var field in fields_literal) {
					new_fields.Add(field.Key);
				}
				createTable(tableName, new_fields);
				result = true;
			}
		}
		return result;
	}
	
	
	/**
	 * Create Table With Data 
	 * 
	 * create a table using an array of Objects
	 * 
	 * @param {string} tableName
	 * @param {string} fields
	 * @returns true on success
	 * 
	 * Fields is a json formatted string representing
	 * an array of data records.
	 * 
	 * Usage:
	 * 
	 * db.CreateTableWithData("settings", 
	 * 	@"[
	 * 		{""name"":""leaderboard"", ""value"": false},
	 * 		{""name"":""playSfx"", ""value"": false}
	 * 	]");
	 *
	 * 
	 */
	public bool CreateTableWithData(string tableName, string data) {
		return CreateTableWithData(tableName, JSON.Array(JSON.Parse(data)).ToArray());
	}
	
	/**
	 * Create Table With Data 
	 * 
	 * create a table using an array of Objects
	 * 
	 * @param {string} tableName
	 * @param {object[]} fields
	 * @returns true on success
	 * 
	 * Fields is an array of data records.
	 * 
	 * Usage:
	 * 
	 * db.CreateTableWithData("settings", new JSONObject [] {
	 * 	new JSONObject() {
	 * 		{"name", "leaderboard"},
	 *		{"value", "off"}
	 * 	},
	 *	new JSONObject() {
	 * 		{"name", "playMusic"},
	 *		{"value", "false"}
	 * 	}
	 * });
	 * 
	 */
	public bool CreateTableWithData(string tableName, object[] data) {
		var fields = new List<string>();
		foreach (var name in JSON.Object(data[0]).Keys) {
			fields.Add(name);
		}
		
		// create the table
		if (CreateTable(tableName, fields.ToArray())) {
			Commit();
			
			// populate
			for (var i=0; i<data.Length; i++) {
				if (insert(tableName, JSON.Object(data[i])) < 0) {
					throw new Exception("Failed to insert record: [" + JSON.Stringify(data[i]) + "]");
				}
			}
			Commit();
		} 
		return true;
	}
	
	/**
	 * Drop a Table
	 * 
	 * @param {string} tableName
	 * 
	 */
	public void DropTable(string tableName) {
		tableExistsWarn(tableName);
		dropTable(tableName);
	}
	
	/**
	 * Empty a Table
	 * 
	 * @param {string} tableName
	 * 
	 */
	public void Truncate(string tableName) {
		tableExistsWarn(tableName);
		truncate(tableName);
	}
	
	/**
	 * Alter a Table
	 * 
	 * @param {string} tableName
	 * @param {string} newField name of the new column
	 * @param {string} defaultValue 
	 * 
	 * 
	 */
	public bool AlterTable(string tableName, string newField, string defaultValue) {
		var result = false;
		if (!validateName(tableName)) {
			throw new Exception("The database name '" + tableName + "' contains invalid characters");
		} else {
			if (validateName(newField)) {
				alterTable(tableName, newField, defaultValue);
				result = true;
			} else {
				throw new Exception("One or more field names in the table definition contains invalid characters");
			}
		}
		return result;
	}
	
	/**
	 * Row Count
	 * 
	 * @param {string} tableName
	 * @returns {int} number of rows in a table
	 * 
	 */
	public int RowCount(string tableName) {
		tableExistsWarn(tableName);
		return rowCount(tableName);
	}
	
	/**
	 * Insert a row
	 * 
	 * @param {string} tableName
	 * @param {string} data
	 * @returns number of rows inserted
	 * 
	 * Data is a json formatted string representing
	 * an array of data records to insert
	 * 
	 * db.Insert("settings", @"{""name"":""leaderboard"", ""value"": false}");
	 * 
	 */
	public int Insert(string tableName, string data) {
		return Insert (tableName, JSON.Parse(data));
	}
	
	/**
	 * Insert a row
	 * 
	 * @param {string} tableName
	 * @param {object} data
	 * @returns number of rows inserted
	 * 
	 * Data is an array of data records to insert
	 * 
	 * db.Insert("settings", @"{""name"":""leaderboard"", ""value"": false}");
	 * 
	 */
	public int Insert(string tableName, object data) {
		tableExistsWarn(tableName);
		return insert(tableName, (JSONObject)data);
	}
	
	// insert or update based on a given condition
	/**
	 * Insert Or Update
	 * 
	 * insert or update based on a given condition
	 * 
	 * @param {string} tableName
	 * @param {object} query
	 * @param {object} data
	 * @returns number of rows inserted
	 * 
	 * 
	 * 
	 */
	public int InsertOrUpdate(string tableName, object query, object data) {
		tableExistsWarn(tableName);
		var resultIds = query == null 
			? getIDs(tableName) 
			: queryByValues(tableName, (JSONObject)query);

		// no existing records matched, so insert a new row
		if (resultIds.Count == 0) {
			insert(tableName, validateData(tableName, JSON.Object (data)));
			return 1;
		} else {
			var ids = new List<int>();
			for (var n=0; n<resultIds.Count; n++) {
				update(tableName, resultIds, (JSONObject row) => {
					ids.Add(Convert.ToInt32(row["ID"]));
					return JSON.Object(data);
				});
			}
			return ids.Count;
		}
	}
	
	/**
	 * Update Rows
	 * 
	 * @param {string} tableName
	 * @param {string} query
	 * @param {Func} updateFunction callback
	 * @returns number of rows updated
	 * 
	 * Query is a json formatted string representing selection creiteria
	 * 
	 * db.Update("settings", @"{""name"": ""playMusic""}", (JSONObject row) => {
	 * 	row["value"] = true;
	 * 	return row;
	 * });
	 * 
	 */
	public int Update(string tableName, string query, Func<JSONObject, JSONObject> updateFunction) {
		return Update (tableName, JSON.Parse(query), updateFunction);
	}
	
	/**
	 * Update Rows
	 * 
	 * @param {string} tableName
	 * @param {object} query
	 * @param {Func} updateFunction callback
	 * @returns number of rows updated
	 * 
	 * Query is a hash of selection creiteria
	 * 
	 * db.Update("settings", new JSONObject() {
	 * 	{"name", "playMusic"}
	 * }, 	
	 * (JSONObject row) => {
	 * 	row["value"] = true;
	 * 	return row;
	 * });
	 * 
	 */
	public int Update(string tableName, object query, Func<JSONObject, JSONObject> updateFunction) {
		tableExistsWarn(tableName);
		var resultIds = query == null 
			? getIDs(tableName) 
			: queryByValues(tableName, (JSONObject)query);
		return update(tableName, resultIds, updateFunction);
	}
	
	/**
	 * Query
	 * 
	 * @param {string} tableName
	 * @param {object} query
	 * @param {int} limit
	 * @param {int} start
	 * @param {object[]} sort
	 * @param {object[]} distinct
	 * @returns array of rows selected
	 * 
	 * 
	 * ddb.Query("settings", new JSONObject() {
	 * 	{"name", "playMusic"}
	 * }, new JSONArray() {
	 * 	{"name", "asc"}
	 * }, limit: 10)
	 * 
	 */
	public JSONArray Query(string tableName, object query=null, int limit=-1, int start=-1, object[] sort=null, object[] distinct=null) {
		tableExistsWarn(tableName);
		var resultIds = query == null 
			? getIDs(tableName) 
			: queryByValues(tableName, (JSONObject)query);
		return select(tableName, resultIds, start, limit, sort, distinct);
	}
	// alias for query() that takes a dict of params instead of positional arrguments
	
	/**
	 * QueryAll
	 * 
	 * @param {string} tableName
	 * @param {string} query
	 * @returns array of rows selected
	 * 
	 * 
	 * db.Query("settings", @"{""name"": ""playMusic""}", sort:@"[[""name"", ""asc""]]", limit:10);
	 */
	public JSONArray QueryAll(string tableName, string query) {
		var args = JSON.Object(JSON.Parse(query));
		if (args.Count == 0) {
			return Query(tableName);
		} else {
			return Query(tableName, 
			     args.ContainsKey("query") ? args["query"] : null,
				 args.ContainsKey("limit") ? (int)args["limit"] : -1,
				 args.ContainsKey("start") ? (int)args["start"] : -1,
			     args.ContainsKey("sort") ? (object[])args["sort"] : null,
			     args.ContainsKey("distinct") ? (object[])args["distinct"] : null
			);
		}
	}
	
	
	/**
	 * DeleteRows
	 * 
	 * @param {string} tableName
	 * @param {string} query
	 * @returns array of rows selected
	 * 
	 * 
	 * db.DeleteRows("settings", @"{""name"": ""playMusic""}")
	 */
	public int DeleteRows(string tableName, string query) {
		return DeleteRows(tableName, JSON.Parse(query));
	}
	
	/**
	 * DeleteRows
	 * 
	 * @param {string} tableName
	 * @param {object} query
	 * @returns array of rows selected
	 * 
	 * 
	 * db.DeleteRows("settings", new JSONObject() {
	 * 	{"name", "playMusic"}
	 * });
	 * 
	 */
	public int DeleteRows(string tableName, object query) {
		tableExistsWarn(tableName);
		var resultIds = query == null 
			? getIDs(tableName) 
			: queryByValues(tableName, (JSONObject)query);
		return deleteRows(tableName, resultIds);
	}
	
	// ______________________ private methods

	// _________ database functions
	// drop the database
	private void drop() {
		if (PlayerPrefs.HasKey(db_id)) {
			PlayerPrefs.DeleteKey(db_id);
		}
		db = null;
	}

	// number of tables in the database
	private int tableCount() {
		return JSON.Object(db["tables"]).Count;
	}

	// _________ table functions

	// returns all fields in a table.
	private JSONArray tableFields(string table_name) {
		var tables = JSON.Object(db["tables"]);
		var table = JSON.Object(tables[table_name]);
		return (JSONArray)table["fields"];
	}

	// check whether a table exists
	private bool tableExists(string table_name) {
		return JSON.Object(db["tables"]).ContainsKey(table_name);
	}

	// check whether a table exists, and if not, throw an error
	private void tableExistsWarn(string table_name) {
		if (!tableExists(table_name)) {
			throw new Exception("The table '" + table_name + "' does not exist");
		}
	}

	// check whether a table column exists
	private bool columnExists(string table_name, string field_name) {
		var fields = JSON.Array(tableFields(table_name));
		for (var i=0; i<fields.Count; i++) {
			if (fields[i] == field_name) return true;
		}
		return false;
	}

	// create a table
	private void createTable(string table_name, JSONArray fields) {
		var tables = JSON.Object(db["tables"]);
		tables.Add(table_name, JSON.Parse(@"{""auto_increment"": 1}"));
		JSON.Object(tables[table_name]).Add("fields", fields);
		JSON.Object(db["data"]).Add(table_name, JSON.Parse(@"{}"));
	}

	// drop a table
	private void dropTable(string table_name) {
		JSON.Object(db["tables"]).Remove(table_name);
		JSON.Object(db["data"]).Remove(table_name);
	}

	// empty a table
	private void truncate(string table_name) {
		var tables = JSON.Object(db["tables"]);
		JSON.Object(tables[table_name])["auto_increment"] = 1;
		JSON.Object(db["data"]).Add(table_name, JSON.Parse(@"{}"));
	}

	// alter a table
	private void alterTable(string table_name, string new_field, string default_value="") {
		tableFields(table_name).Add(new_field);

		// insert default values in existing table
		var data = JSON.Object(db["data"]);
		var rows = JSON.Object(data[table_name]);
		
		for (var id=0; id<rows.Count; id++) {
			var row = JSON.Object(rows[""+id]);
			if (!row.ContainsKey("ID")) continue;
			row[new_field] = default_value;
		}
	}

	// number of rows in a table
	private int rowCount(string table_name) {
		var data = JSON.Object(db["data"]);
		return JSON.Object(data[table_name]).Count;

	}

	// insert a new row
	private int insert(string table_name, JSONObject row) {
		var tables = JSON.Object(db["tables"]);
		var table = JSON.Object(tables[table_name]);
		int id = Convert.ToInt32(table["auto_increment"]);
		row["ID"] = id;
		var data = JSON.Object(db["data"]);
		JSON.Object(data[table_name])[""+id] = row;

		table["auto_increment"] = id+1;
		return id;
	}

	// select rows, given a list of IDs of rows in a table
	private JSONArray select(string table_name, List<int> ids, int start, int limit, object[] sort, object[] distinct) {
		var id = 0;
		var results = new JSONArray();

		for (var i=0; i<ids.Count; i++) {
			id = ids[i];
			var data = JSON.Object(db["data"]);
			var o1 = JSON.Array(data[table_name]);
			var o = clone(JSON.Object(o1[id]));
			results.Add(o);
		}

		// there are sorting params 
		if (sort != null) {
			//foreach (var field in sort) {
			for (var i=0; i<sort.Length; i++) {
				var field = JSON.Array(sort[i]);

				results.Sort(delegate(object o1, object o2){

					var field1 = JSON.Object(o1)[field[0].ToString()];
					var field2 = JSON.Object(o2)[field[0].ToString()];

					if (field1 is String) {
						return ((string)(field1)).CompareTo(field2)*(field[1].ToString() == "DESC" ? -1 : 1);
					} else if (field1 is Boolean) {
						return ((bool)(field1)).CompareTo(field2)*(field[1].ToString() == "DESC" ? -1 : 1);
					} else {
						return ((float)(field1)).CompareTo(field2)*(field[1].ToString() == "DESC" ? -1 : 1);
					}
				});
			}
		}

		// distinct params
		if (distinct != null) {
			for (var j=0; j<distinct.Length; j++) {
				var seen = new JSONObject();
				var d = (string)distinct[j];
				for (var i=0; i<results.Count; i++) {
					if (JSON.Object(results[i]).ContainsKey(d)) {// && seen.ContainsKey(JSON.Object(results[i])[d])) {
						if (seen.ContainsKey(JSON.Object(results[i])[d].ToString())) {
							results.RemoveAt(i);
						}
					} else {
						seen.Add(JSON.Object(results[i])[d].ToString(), 1);
					}
				}
			}
		}

		// limit and offset
		if (start != -1 && limit != -1) {
			results = JSON.Array(results.GetRange(start, limit));
		} else if (start != -1) {
			results = JSON.Array(results.GetRange(start, results.Count));
		} else if (limit != -1) {
			results = JSON.Array(results.GetRange(0, limit));
		}
		return results;


	}

	// select rows in a table by field-value pairs, returns the IDs of matches
	private List<int> queryByValues(string table_name, JSONObject query) {
		var result_ids = new List<int>();
		var exists = false;

		var data = JSON.Object(db["data"]);
		foreach (var pair in JSON.Object(data[table_name])) {

			var row = JSON.Object(pair.Value);
			exists = true;
			foreach (var field in query) {
				if (query[field.Key] is String) {
					if (row[field.Key].ToString().ToLower() != query[field.Key].ToString().ToLower()) {
						exists = false;
						break;
					} 
				} else {
					if (row[field.Key] != query[field.Key]) {
						exists = false;
						break;
					}
				}
			}
			if (exists) {
				result_ids.Add(Convert.ToInt32(pair.Key));
			}
		}
		return result_ids;
	}
		
	 // return all the IDs in a table
	private List<int> getIDs(string table_name) {
		var result_ids = new List<int>();
		var data = JSON.Object(db["data"]);
		var rows = JSON.Object(data[table_name]);

		for (var id=0; id<rows.Count; id++) {
			var row = JSON.Object(rows[""+id]);
			if (row.ContainsKey("ID")) {
				if ((int)row["ID"] == id) {
					result_ids.Add(id);
				}
			}
		}
		return result_ids;
	}

	// delete rows, given a list of their IDs in a table
	private int deleteRows(string table_name, List<int> ids) {
		for(var i=0; i<ids.Count; i++) {
			var id = ids[i];
			var data = JSON.Object(db["data"]);
			var rows = JSON.Object(data[table_name]);
			rows[""+id] = new JSONObject();
		}
		return ids.Count;
	}

	// update rows
	private int update(string table_name, List<int> ids, Func<JSONObject, JSONObject> update_function) {
		var data = JSON.Object(db["data"]);
		var rows = JSON.Object(data[table_name]);
		var num = 0;
		for (var i=0; i<ids.Count; i++) {
			var id = Convert.ToString(ids[i]);
			JSONObject updated_data = update_function(clone(JSON.Object(rows[id])));
			if (updated_data != null) {
				updated_data.Remove("ID");
				var new_data = JSON.Object(rows[id]);
				// merge updated data with existing data
				foreach (var field in updated_data) {
					new_data[field.Key] = field.Value;
				}
				rows[id] = validFields(table_name, new_data);
				num++;
			}
		}
		return num;
	}

	// commit the database to localStorage
	private void commit() {
		PlayerPrefs.SetString(db_id, JSON.Stringify(db));
		PlayerPrefs.Save();
	}


	// serialize the database
	private string serialize() {
		return JSON.Stringify(db);
	}

	// clone an object
	private JSONObject clone(JSONObject obj) {
		var new_obj = new JSONObject();
		foreach (KeyValuePair< string, object > pair in obj) {
			new_obj.Add(pair.Key, pair.Value);
		}
		return new_obj;
	}

	// validate db, table, field names (alpha-numeric only)
	private bool validateName(string name) {
		return !rx_validate_name.IsMatch(name);
	}

	// given a data list, only retain valid fields in a table
	private JSONObject validFields(string table_name, JSONObject data) {
		return data;
	}

	// given a data list, populate with valid field names of a table
	private JSONObject validateData(string table_name, JSONObject data) {
		return data;
	}
}
