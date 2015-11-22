using System.Collections.Generic;
using Entitas;
using UnityEngine;

//public class ScoreSystem : IInitializeSystem, IReactiveSystem, ISetPool {
//    public TriggerOnEvent trigger { get { return Matcher.GameBoardElement.OnEntityRemoved(); } }
public class ScoreSystem : IInitializeSystem, ISetPool {

    Pool pool;
    int score;

    public void SetPool(Pool pool) {
        this.pool = pool;
    }

    public void Initialize() {
        pool.SetScore(0);
    }

    public void Execute() {
    
        if (score != pool.score.value) {
            score = pool.score.value;
        }
    }
}

