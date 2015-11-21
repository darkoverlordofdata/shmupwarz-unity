using System.Collections.Generic;
using Entitas;
using UnityEngine;

//public class ScoreSystem : IInitializeSystem, IReactiveSystem, ISetPool {
//    public TriggerOnEvent trigger { get { return Matcher.GameBoardElement.OnEntityRemoved(); } }
public class ScoreSystem : IInitializeSystem, ISetPool {

    Pool _pool;
    int score;

    public void SetPool(Pool pool) {
        _pool = pool;
    }

    public void Initialize() {
        _pool.SetScore(0);
        Debug.Log("Initialize Score System");
    }

    public void Execute() {
    
        if (score != _pool.score.value) {
            score = _pool.score.value;
        }
        //_pool.ReplaceScore(_pool.score.value + entities.Count);
    }
}

