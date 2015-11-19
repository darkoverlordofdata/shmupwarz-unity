namespace Entitas {
    public partial class Pool {
        public ISystem CreateExpiringSystem() {
            return this.CreateSystem<ExpiringSystem>();
        }
    }
}