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
using Bosco.Utils;

public class GameController : MonoBehaviour {

    Systems _systems;
	PlayerPrefsDB db;

    public static Dictionary<string, AudioSource> AudioSources;

    void Start() {
    
         Type type = Type.GetType("Mono.Runtime");
         if (type != null) {
             MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
             if (displayName != null)
                 Debug.Log("Unity "+displayName.Invoke(null, null));
         }
        
		try {
			Properties.Init("shmupwarz", @"[
				{""name"":""playSfx"", ""value"":true},
				{""name"":""playMusic"", ""value"":true}
			]");
		} catch (Exception e) {
			Debug.Log("Error: "+e.ToString());
		}

		UnityEngine.Random.seed = (int)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds*100);
        
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
			//.Add(pool.CreateSystem<HealthRenderSystem>())

            // Destroy
            .Add(pool.CreateSystem<RemoveOffscreenShipsSystem>())
            .Add(pool.CreateSystem<DestroySystem>());
    }
    
}
