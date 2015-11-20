using System;
using Entitas;
using UnityEngine;

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
            .AddVelocity(0f, 800, 0)
            .AddPosition(x, y, 0)
            .AddExpires(1)
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
            .AddVelocity(0f, -40f, 0f)
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
            .AddVelocity(0, -30, 0f)
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
            .AddVelocity(0, -20, 0f)
            .AddPosition(pos.x, pos.y, pos.z)
            .AddHealth(60, 60)
            .IsEnemy(true)
            .AddResource(Res.Enemy3);
    }
    
    public static Entity CreateParticle(this Pool pool, float x, float y) {
        float radians = (float)(rnd.NextDouble() * Math.PI * 2);
        float magnitude = (float)rnd.Next(200);
        float velocityX = magnitude * (float)Math.Cos(radians);
        float velocityY = magnitude * (float)Math.Sin(radians);
        float scale = (float)rnd.NextDouble() * .5f;
    
        return pool.CreateEntity()
            .AddResource(Res.Particle)
            .AddExpires(1)
            .AddColorAnimation(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, false, false, false, true, true)
            .AddVelocity(velocityX, velocityY, 0)
            .AddScale(scale, scale)
            .AddPosition(x, y, 0);
            
    
    }

    public static Entity CreateExplosion(this Pool pool, float x, float y, float scale) {
        return pool.CreateEntity()
            .AddSoundEffect(scale < .5 ? EFFECT_SMALLASPLODE : EFFECT_ASPLODE)
            .AddExpires(0.5f)
            .AddScale(scale, scale)
            .AddScaleAnimation(scale/100, scale, -3, false, true)
            .AddPosition(x, y, 0)
            .AddResource(Res.Explosion);
    }
    

}

