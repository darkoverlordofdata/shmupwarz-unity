namespace Entitas {
    public partial class Pool {
        public ISystem CreateEntitySpawningTimerSystem() {
            return this.CreateSystem<EntitySpawningTimerSystem>();
        }
    }
}