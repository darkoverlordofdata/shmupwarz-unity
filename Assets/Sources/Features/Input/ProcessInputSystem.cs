using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ProcessInputSystem : IExecuteSystem, ISetPool {


    Pool _pool;
    Group _group;
    private Vector3 mousePosition;
    private bool isFiring;
    private float timeToFire = 0;
    private Entity player;

    public void SetPool(Pool pool) {
        _pool = pool;
        _group = pool.GetGroup(Matcher.Player);
    }

    public void Execute() {

        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Entity player = _group.GetSingleEntity();
        player.position.x = mousePosition.x;
        player.position.y = mousePosition.y;

        isFiring = Input.GetMouseButton(0); 
        _pool.isFiring = isFiring;
        if (isFiring) {
            if (timeToFire <=0) {
                _pool.CreateBullet(mousePosition.x+.6f, mousePosition.y);
                _pool.CreateBullet(mousePosition.x-.6f, mousePosition.y);
                timeToFire = .1f;
            }
        }
        if (timeToFire>0) {
            timeToFire -= Time.deltaTime;
            if (timeToFire < 0) timeToFire = 0;
        }
    }
}

