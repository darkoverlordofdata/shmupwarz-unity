namespace Entitas {
    public partial class Pool {
        public ISystem CreateMovementSystem() {
            return this.CreateSystem<MovementSystem>();
        }
    }
}