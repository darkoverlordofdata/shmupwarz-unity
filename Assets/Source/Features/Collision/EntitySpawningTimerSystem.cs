using Entitas;
using UnityEngine;
using System;
using System.Collections.Generic;
using Bosco.Utils;

class SpawnEnemy1 : Timer {
    public SpawnEnemy1(float delay, bool repeat) : base(delay, repeat) {}
	public override void Execute() {
        Pools.pool.CreateEnemy1();
    }
}
class SpawnEnemy2 : Timer {
    public SpawnEnemy2(float delay, bool repeat) : base(delay, repeat) {}
	public override void Execute() {
        Pools.pool.CreateEnemy2();
    }
}
class SpawnEnemy3 : Timer {
    public SpawnEnemy3(float delay, bool repeat) : base(delay, repeat) {}
	public override void Execute() {
        Pools.pool.CreateEnemy3();
    }
}
class SpawnMinez : Timer {
	
	static System.Random rnd = new System.Random();
	int zone = 0;
	int mine = 0;
	int offset = 0;
	int[][] pos = new int[][]{new int[]{20, 20}, new int[]{50, 20}, new int[]{80, 20}};
	
	public SpawnMinez(float delay, bool repeat) : base(delay, repeat) {}
	public override void Execute() {
		
		zone = (zone+1)%3;
		mine = (mine+1)%2;
		var m = mine+1;
		
		offset+=100;
		if (offset>Screen.width) offset = 0;
		
		var v = Math.Max((int)(rnd.NextDouble()*500), 300);
		var x = offset+pos[zone][0];
		var y = pos[zone][1];
		Pools.pool.CreateMine(m, x, y, v, 10);
	}
}
public class EntitySpawningTimerSystem : ISetPool, IExecuteSystem, IInitializeSystem {

	static System.Random rnd = new System.Random();
	Pool _pool;
    private Timer timer1;
    private Timer timer2;
    private Timer timer3;
	private Timer timer4;


    public void SetPool(Pool pool) {
        //_pool = pool;
    }

    public void Execute() {
		var r = rnd.NextDouble();
		if (r < .5) r = 1-r;
		float delta = (float)(r*Time.deltaTime);

		timer1.Update(delta);
		timer2.Update(delta);
		timer3.Update(delta);
		timer4.Update(delta);
	}

    public void Initialize() {
    
        timer1 = new SpawnEnemy1(2, true);
        timer2 = new SpawnEnemy2(6, true);
        timer3 = new SpawnEnemy3(12, true);
		timer4 = new SpawnMinez(.85f/(Screen.width/640f), true);

    }

}