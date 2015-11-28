using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ProcessInputSystem : IExecuteSystem, ISetPool {


    private Pool pool;
    private Group group;
    private Vector3 mousePosition;
    private bool isFiring;
    private float timeToFire = 0;
    private Entity player;

    public void SetPool(Pool pool) {
        this.pool = pool;
        group = pool.GetGroup(Matcher.Player);
    }

    public void Execute() {

        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Entity player = group.GetSingleEntity();
        player.position.x = mousePosition.x;
        player.position.y = mousePosition.y;

        isFiring = Input.GetMouseButton(0) || Input.GetKey("z"); 
        pool.isFiring = isFiring;
        if (isFiring) {
            if (timeToFire <=0) {
                pool.CreateBullet(mousePosition.x+1f, mousePosition.y);
                pool.CreateBullet(mousePosition.x-1f, mousePosition.y);
                timeToFire = .1f;
            }
        }
        if (timeToFire>0) {
            timeToFire -= Time.deltaTime;
            if (timeToFire < 0) timeToFire = 0;
        }
    }
}

