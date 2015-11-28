using System;
using Entitas;
using UnityEngine;
using Bosco.Utils;

public static class PoolExtensions {

    static readonly int EFFECT_PEW = 0;
    static readonly int EFFECT_ASPLODE = 1;
    static readonly int EFFECT_SMALLASPLODE = 1;
    
    
    static System.Random rnd = new System.Random();

    public static Entity CreatePlayer(this Pool pool) {

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, 100, 0));
        
        return pool.CreateEntity()
            .AddBounds(4)
            .AddHealth(100, 100)
            .AddPosition(pos.x, pos.y, pos.z)
            .IsPlayer(true)
            .AddResource(Res.Fighter);
            
    }

    public static Entity CreateBullet(this Pool pool, float x, float y) {
        return pool.CreateEntity()
            .AddBounds(.1f)
            .AddVelocity(0f, 800*3, 0)
            .AddPosition(x, y, 0)
            .AddExpires(2)
            .AddSoundEffect(EFFECT_PEW)
            .IsBullet(true)
            .AddResource(Res.Bullet);
    }

    public static Entity CreateEnemy1(this Pool pool) {
        int x = rnd.Next(Screen.width);
        int y = Screen.height-100;
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
        
        return pool.CreateEntity()
            .AddBounds(1)
            .AddPosition(pos.x, pos.y, pos.z)
            .AddVelocity(0f, -40*3, 0f)
            .AddHealth(10, 10)
            .IsEnemy(true)
			.AddResource(Res.Enemy1);
    }

    public static Entity CreateEnemy2(this Pool pool) {
        int x = rnd.Next(Screen.width);
        int y = Screen.height-200;
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));

        return pool.CreateEntity()
            .AddBounds(2)
            .AddPosition(pos.x, pos.y, pos.z)
            .AddVelocity(0, -30*3, 0f)
            .AddHealth(20, 20)
            .IsEnemy(true)
            .AddResource(Res.Enemy2);
    }

    public static Entity CreateEnemy3(this Pool pool) {
        int x = rnd.Next(Screen.width);
        int y = Screen.height-300;
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));

        return pool.CreateEntity()
            .AddBounds(3)
            .AddVelocity(0, -20*3, 0f)
            .AddPosition(pos.x, pos.y, pos.z)
            .AddHealth(60, 60)
            .IsEnemy(true)
            .AddResource(Res.Enemy3);
    }
    

    public static Entity CreateBigExplosion(this Pool pool, float x, float y) {
        float scale = 0.5f;
        return pool.CreateEntity()
            .AddExpires(0.5f)
            .AddScale(scale, scale)
            .AddScaleAnimation(scale/100, scale, -3, false, true)
            .AddPosition(x, y, 0)
            .AddResource(Res.BigExplosion);
    }

    public static Entity CreateSmallExplosion(this Pool pool, float x, float y) {
        float scale = 0.1f;
        return pool.CreateEntity()
            .AddExpires(0.5f)
            .AddScale(scale, scale)
            .AddScaleAnimation(scale/100, scale, -3, false, true)
            .AddPosition(x, y, 0)
            .AddResource(Res.SmallExplosion);
    }

}

