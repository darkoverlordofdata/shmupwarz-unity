using Entitas;
using System.Collections.Generic;
public class SoundEffectSystem : ISetPool, IExecuteSystem, IInitializeSystem {

    Pool _pool;
    Group _group;

    public void SetPool(Pool pool) {
        //_pool = pool;
        //_group = pool.GetGroup(Matcher.AllOf(Matcher.SoundEffect));
    }

    public void Execute() {
        //foreach (var e in _group.GetEntities()) {
        //}
    }

    public void Initialize() {
    }

}