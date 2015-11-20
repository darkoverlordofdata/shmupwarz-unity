using Entitas;
using UnityEngine;
using System.Collections.Generic;
public class ScaleAnimationSystem : ISetPool, IExecuteSystem {

    Group _group;

    public void SetPool(Pool pool) {
        _group = pool.GetGroup(Matcher.AllOf(Matcher.Scale, Matcher.ScaleAnimation, Matcher.View));
    }

    public void Execute() {
        foreach (var e in _group.GetEntities()) {
        
            if (e.scaleAnimation.active) {
            
                var scale = e.scale;
                scale.x += e.scaleAnimation.speed * Time.deltaTime;
                //if (scale.x < 0) scale.x = 0-scale.x;
                
                scale.y = scale.x;
                if (scale.x > e.scaleAnimation.max) {
                    scale.x = e.scaleAnimation.max;
                    scale.y = scale.x;
                    e.scaleAnimation.active = false;
                } else if (scale.x < e.scaleAnimation.min) {
                    scale.x = e.scaleAnimation.min;
                    scale.y = scale.x;
                    e.scaleAnimation.active = false;
                }
                
                var transform = e.view.gameObject.transform;

                transform.localScale = new Vector3(scale.x, scale.y, transform.localScale.z);
            }
        }
    }
}