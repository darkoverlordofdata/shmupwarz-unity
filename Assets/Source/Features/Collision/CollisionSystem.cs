using Entitas;
using UnityEngine;
using System;
using System.Collections.Generic;
public class CollisionSystem : ISetPool, IExecuteSystem, IInitializeSystem {

    Pool pool;
    CollisionPair[] collisionPairs;

    public void SetPool(Pool pool) {
        this.pool = pool;
		pool.SetStatus(100, 0);
	}

    public void Execute() {
        foreach (var pair in collisionPairs) {
            pair.CheckForCollisions();
        }
    }

    public void Initialize() {

        var bullets = pool.GetGroup(Matcher.Bullet);
        var enemies = pool.GetGroup(Matcher.Enemy);
		var player = pool.GetGroup(Matcher.Player);
		var mines = pool.GetGroup(Matcher.Mine);
		collisionPairs = new CollisionPair[2];
        collisionPairs[0] = new EnemyBulletCollision(bullets, enemies);
		collisionPairs[1] = new PlayerMineCollision(mines, player);

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
		} else {
			if (ship.hasView) {
				GameObject o = ship.view.gameObject;
				var text = (TextMesh)o.GetComponent("TextMesh");
				var percentage = (int)Math.Round(health.health / health.maximumHealth * 100);
				text.text = percentage+"%";
			}
		}
    } 
    
}

class PlayerMineCollision : CollisionPair {

	GameObject statusBar;
	GameObject[] lives = new GameObject[3];
	int lifeCounter;

	public PlayerMineCollision(Group mines, Group player) : base(mines, player) {
		statusBar = GameObject.Find("Status/display");
		for (var life=0; life<3; life++) {
			lives[life] = GameObject.Find("Lives/life"+(life+1));
		}
		lifeCounter = 3;

	}
	
	public override void HandleCollision(Entity mine, Entity player) {
		
		mine.IsDestroy(true);
		var status = Pools.pool.status;
		if (status.immunity > 0) {
			status.immunity -= 1;
			if (status.immunity <= 0) {
				//TODO: reset sprite display to normal
			}
			return;
		}

		player.health.health -= mine.health.health;
		if (player.health.health > 0) {
			var pc = player.health.health / player.health.maximumHealth;
			var scale = statusBar.transform.localScale;
			statusBar.transform.localScale = new Vector3(pc, scale.y, scale.z);

		} else {
			//player.IsDestroy(true);
			Pools.pool.CreateHugeExplosion(player.position.x, player.position.y);
			lifeCounter--;
			if (lifeCounter < 0) {
				//TODO: game over!
				GameObject gameOver = GameObject.Find("Canvas/GameOver");
				gameOver.transform.localScale = new Vector3(1, 1, 1);

			} else {
				lives[lifeCounter].SetActive(false);
				statusBar.transform.localScale = new Vector3(1, 1, 1);
				player.health.health = player.health.maximumHealth;
			}
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