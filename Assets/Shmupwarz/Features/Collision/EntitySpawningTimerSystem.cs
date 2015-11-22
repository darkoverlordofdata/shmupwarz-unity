using Entitas;
using System.Collections.Generic;

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
public class EntitySpawningTimerSystem : ISetPool, IExecuteSystem, IInitializeSystem {

    Pool _pool;
    private Timer timer1;
    private Timer timer2;
    private Timer timer3;
    

    public void SetPool(Pool pool) {
        //_pool = pool;
    }

    public void Execute() {
        timer1.Update();
        timer2.Update();
        timer3.Update();
    }

    public void Initialize() {
    
        timer1 = new SpawnEnemy1(2, true);
        timer2 = new SpawnEnemy2(6, true);
        timer3 = new SpawnEnemy3(12, true);
        
    }

}