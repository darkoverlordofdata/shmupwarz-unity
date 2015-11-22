using Entitas;
using UnityEngine;
using System.Collections.Generic;
public class ColorAnimationSystem : ISetPool, IExecuteSystem {

    Group group;

    public void SetPool(Pool pool) {
        group = pool.GetGroup(Matcher.AllOf(Matcher.ColorAnimation));
    }

    public void Execute() {
        foreach (var e in group.GetEntities()) {
        
            ColorAnimationComponent c = e.colorAnimation; 
            if (c.alphaAnimate) {
                if (e.hasView) {
                    var color = e.view.gameObject.GetComponent<Renderer>().material.color;
                    
                    color.a += c.alphaSpeed * Time.deltaTime;
            
                    if (color.a > c.alphaMax || color.a < c.alphaMin) {
                        if (c.repeat) {
                            c.alphaSpeed = -c.alphaSpeed;
                        } else {
                            c.alphaAnimate = false;
                        }
                    }
                }
            }
        }
    }
}

