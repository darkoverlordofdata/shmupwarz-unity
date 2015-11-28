using Entitas;
using UnityEngine;
using System;
using System.Collections.Generic;
public class HealthRenderSystem : ISetPool, IExecuteSystem {

    Pool pool;
    Group group;

    public void SetPool(Pool pool) {
        pool = pool;
        group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Health, Matcher.Enemy));

    }


    public void Execute() {
/*        foreach (var e in group.GetEntities()) {
			GameObject o = e.view.gameObject;
			var m = (TextMesh)o.GetComponent("TextMesh");
			Debug.Log("mesh = "+m.text);

        }*/
    }
}