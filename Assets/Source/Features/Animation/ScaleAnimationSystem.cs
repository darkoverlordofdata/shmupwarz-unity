using Entitas;
using UnityEngine;
using System.Collections.Generic;
public class ScaleAnimationSystem : ISetPool, IExecuteSystem {

    Group group;

    public void SetPool(Pool pool) {
        group = pool.GetGroup(Matcher.AllOf(Matcher.Scale, Matcher.ScaleAnimation, Matcher.View));
    }

    public void Execute() {
        foreach (var e in group.GetEntities()) {
        
            ScaleAnimationComponent scaleAnimation = e.scaleAnimation;
            
            if (scaleAnimation.active) {
            
                var scale = e.scale;
                scale.x += scaleAnimation.speed * Time.deltaTime;
                
                scale.y = scale.x;
                if (scale.x > scaleAnimation.max) {
                    scale.x = scaleAnimation.max;
                    scale.y = scale.x;
                    scaleAnimation.active = false;
                } else if (scale.x < scaleAnimation.min) {
                    scale.x = scaleAnimation.min;
                    scale.y = scale.x;
                    scaleAnimation.active = false;
                }
                
                var transform = e.view.gameObject.transform;

                transform.localScale = new Vector3(scale.x, scale.y, transform.localScale.z);
            }
        }
    }
}