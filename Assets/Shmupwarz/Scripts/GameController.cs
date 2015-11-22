using Entitas;
using Entitas.Unity.VisualDebugging;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    Systems _systems;
    public static Dictionary<string, AudioSource> AudioSources;

    void Start() {
        Random.seed = 42;
        
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
