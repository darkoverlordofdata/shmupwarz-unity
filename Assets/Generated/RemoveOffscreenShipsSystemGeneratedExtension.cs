namespace Entitas {
    public partial class Pool {
        public ISystem CreateRemoveOffscreenShipsSystem() {
            return this.CreateSystem<RemoveOffscreenShipsSystem>();
        }
    }
}