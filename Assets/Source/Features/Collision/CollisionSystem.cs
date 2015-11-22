using Entitas;
using UnityEngine;
using System;
using System.Collections.Generic;
public class CollisionSystem : ISetPool, IExecuteSystem, IInitializeSystem {

    Pool pool;
    CollisionPair[] collisionPairs;

    public void SetPool(Pool pool) {
        this.pool = pool;
    }

    public void Execute() {
        foreach (var pair in collisionPairs) {
            pair.CheckForCollisions();
        }
    }

    public void Initialize() {
        var bullets = pool.GetGroup(Matcher.Bullet);
        var enemies = pool.GetGroup(Matcher.Enemy);
        collisionPairs = new CollisionPair[1];
        collisionPairs[0] = new EnemyBulletCollision(bullets, enemies);
        
    }

}

class EnemyBulletCollision : CollisionPair {

    public EnemyBulletCollision(Group bullets, Group enemies) : base(bullets, enemies) {}
        
    public override void HandleCollision(Entity bullet, Entity ship) {
    
        var pos = bullet.position;
        
        Pools.pool.CreateSmallExplosion(pos.x, pos.y);
        Shrapnel.Instance.Hit(pos.x, pos.y);
        bullet.IsDestroy(true);
        
        HealthComponent health = ship.health;
        
        health.health -= 1;
        if (health.health <= 0) {
            Pools.pool.score.value += (int)health.maximumHealth;
            ship.IsDestroy(true);
            PositionComponent position = ship.position;
            Pools.pool.CreateBigExplosion(position.x, position.y);
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
        PositionComponent position1 = e1.position;
        PositionComponent position2 = e2.position;
        
        float a = position1.x - position2.x;
        float b = position1.y - position2.y;
        return (Math.Sqrt(a * a + b * b) - e1.bounds.radius) < e2.bounds.radius;
    }
}