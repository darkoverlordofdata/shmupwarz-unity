namespace Entitas {
    public partial class Pool {
        public ISystem CreateSoundEffectSystem() {
            return this.CreateSystem<SoundEffectSystem>();
        }
    }
}