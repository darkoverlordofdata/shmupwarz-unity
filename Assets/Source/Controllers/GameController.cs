using Entitas;
using Entitas.Unity.VisualDebugging;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Bosco;
using Bosco.Json;

public class GameController : MonoBehaviour {

    Systems _systems;
	PlayerPrefsDB db;
    public static Dictionary<string, AudioSource> AudioSources;

    void Start() {
    
         Type type = Type.GetType("Mono.Runtime");
         if (type != null) {
             MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
             if (displayName != null)
                 Debug.Log("Using Unity version "+displayName.Invoke(null, null));
         }
        
        //string source = @"{""name"": ""fred"", ""active"": true, ""id"": 42, ""scores"": [0, 1, 2], ""flags"": {""first"": false}}";
		string source = @"{""tables"":{""settings"":{""fields"":[""ID"",""name"",""value""],""auto_increment"":8},""leaderboard"":{""fields"":[""ID"",""date"",""score""],""auto_increment"":9}},""data"":{""settings"":{""1"":{""ID"":1,""name"":""leaderboard"",""value"":""off""},""2"":{""ID"":2,""name"":""player"",""value"":""""},""3"":{""ID"":3,""name"":""userId"",""value"":""""},""4"":{""ID"":4,""name"":""playMusic"",""value"":""false""},""5"":{""ID"":5,""name"":""playSfx"",""value"":""true""},""6"":{""ID"":6,""name"":""skip"",""value"":""false""},""7"":{""ID"":7,""name"":""skip"",""value"":""false""}},""leaderboard"":{""1"":{""ID"":1,""date"":""20151001"",""score"":400},""2"":{""ID"":2,""date"":""20151002"",""score"":1850},""3"":{""ID"":3,""date"":""20151003"",""score"":980},""4"":{""ID"":4,""date"":""20151004"",""score"":1100},""5"":{""ID"":5,""date"":""20151007"",""score"":60},""6"":{""ID"":6,""date"":""20151027"",""score"":1020},""7"":{""ID"":7,""date"":""20151028"",""score"":680},""8"":{""ID"":8,""date"":""20151029"",""score"":710}}}}";
        //var jso = (JSONObject)JSON.Parse(source);
        //Debug.Log(JSON.Stringify(jso, 2));
        
		try {

			/** testing... */ //PlayerPrefs.SetString("db_d16a", source);
			/** testing... */ //PlayerPrefs.Save();

			PlayerPrefs.DeleteKey("db_d16a");
			db = new PlayerPrefsDB("d16a");
			//Debug.Log("DB "+db.Serialize());
			/*
			Debug.Log("Get Table Count");
			Debug.Log("DB tables count = "+db.TableCount());
			Debug.Log("Get Row Counts");
			Debug.Log("DB settings count = "+db.RowCount("settings"));
			Debug.Log("DB leaderboard count = "+db.RowCount("leaderboard"));

			db.CreateTableWithData("preferences", new JSONObject [] {
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
			*/
			db.CreateTableWithData("settings", @"[
				{""name"":""leaderboard"", ""value"": ""off""},
				{""name"":""playMusic"", ""value"": false},
				{""name"":""playSfx"", ""value"": true}
			]");

			/*
			db.CreateTable("settings", @"[""name"", ""value""]");
			//db.CreateTable(tableName: "settings", fields: new string [] {"name", "value"});

			db.Insert("settings", @"{""name"":""leaderboard"", ""value"": ""off""}");
			db.Insert("settings", @"{""name"":""playMusic"", ""value"": false}");
			db.Insert("settings", @"{""name"":""playSfx"", ""value"": true}");

			/*
			db.Insert("settings", new JSONObject() {
				{"name", "leaderboard"},
				{"value", "off"}
			});

			db.Insert("settings", new JSONObject() {
				{"name", "playMusic"},
				{"value", false}
			});
			db.Insert("settings", new JSONObject() {
				{"name", "playSfx"},
				{"value", true}
			});
			*/

			db.Commit();

			Debug.Log("DB "+db.Serialize());


			db.Update("settings", @"{""name"": ""playMusic""}", (JSONObject row) => {
				row["value"] = true;
				return row;
			});
			/*
			db.Update("settings", new JSONObject() {
				{"name", "playMusic"}
			}, 	
			(JSONObject row) => {
					row["value"] = true;
					return row;
			});*/

			Debug.Log("DB "+db.Serialize());

		} catch (Exception e) {
			Debug.Log("Error: "+e.ToString());
		}

        UnityEngine.Random.seed = 42;
        
        _systems = createSystems(Pools.pool);
        _systems.Initialize();
        Pools.pool.CreatePlayer();
    }

    void Update() {
        _systems.Execute();
    }

    Systems createSystems(Pool pool) {
        #if (UNITY_EDITOR)
        return new DebugSystems()
        #else
        return new Systems()
        #endif

            // Input
            .Add(pool.CreateSystem<ProcessInputSystem>())

            // Update
            
            .Add(pool.CreateSystem<EntitySpawningTimerSystem>())
            .Add(pool.CreateSystem<MovementSystem>()) 
            .Add(pool.CreateSystem<CollisionSystem>()) 
            .Add(pool.CreateSystem<ExpiringSystem>())
            .Add(pool.CreateSystem<ColorAnimationSystem>())
            .Add(pool.CreateSystem<ScaleAnimationSystem>())
            .Add(pool.CreateSystem<ScoreSystem>())
            //.Add(pool.CreateSystem<SoundEffectSystem>())
            // Render 
            .Add(pool.CreateSystem<RemoveViewSystem>())
            .Add(pool.CreateSystem<AddViewSystem>())
            .Add(pool.CreateSystem<RenderPositionSystem>())

            // Destroy
            .Add(pool.CreateSystem<RemoveOffscreenShipsSystem>())
            .Add(pool.CreateSystem<DestroySystem>());
    }
    
}
