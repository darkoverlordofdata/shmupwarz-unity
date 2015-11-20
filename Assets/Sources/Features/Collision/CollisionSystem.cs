using Entitas;
using UnityEngine;
using System;
using System.Collections.Generic;
public class CollisionSystem : ISetPool, IExecuteSystem, IInitializeSystem {

    Pool _pool;
    Group _group;
    CollisionPair[] collisionPairs;

    public void SetPool(Pool pool) {
        _pool = pool;
    }

    public void Execute() {
        foreach (var pair in collisionPairs) {
            pair.CheckForCollisions();
        }
    }

    public void Initialize() {
        var bullets = _pool.GetGroup(Matcher.Bullet);
        var enemies = _pool.GetGroup(Matcher.Enemy);
        collisionPairs = new CollisionPair[1];
        collisionPairs[0] = new EnemyBulletCollision(bullets, enemies);
        
    }

}

class EnemyBulletCollision : CollisionPair {

    public EnemyBulletCollision(Group bullets, Group enemies) : base(bullets, enemies) {}
        
    public override void HandleCollision(Entity bullet, Entity ship) {
    
        var pos = bullet.position;
        
        Pools.pool.CreateExplosion(pos.x, pos.y, .1f);
        int i = 5;
        while (--i > 0) Pools.pool.CreateParticle(pos.x, pos.y);
        bullet.IsDestroy(true);
        ship.health.health -= 1;
        if (ship.health.health <= 0) {
            ship.IsDestroy(true);
            Pools.pool.CreateExplosion(ship.position.x, ship.position.y, .5f);
        }
    } 
    
}
interface CollisionHandler {
    void HandleCollision(Entity a, Entity b);
}

abstract class CollisionPair : CollisionHandler {

    private Group group1;
    private Group group2;
    private CollisionHandler handler;
    
    public CollisionPair(Group group1, Group group2) {
        this.group1 = group1;
        this.group2 = group2;
        this.handler = handler;
    }
    
    public abstract void HandleCollision(Entity a, Entity b);
    
    public void CheckForCollisions() {
    
        foreach (var a in group1.GetEntities()) {
            foreach (var b in group2.GetEntities()) {
                if (CollisionExists(a, b)) {
                    HandleCollision(a, b);
                }
            }
        }
    }
    
    private bool CollisionExists(Entity e1, Entity e2) {
        float a = e1.position.x - e2.position.x;
        float b = e1.position.y - e2.position.y;
        return (Math.Sqrt(a * a + b * b) - e1.bounds.radius) < e2.bounds.radius;
    }
}