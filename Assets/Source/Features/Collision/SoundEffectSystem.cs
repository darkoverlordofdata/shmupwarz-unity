using Entitas;
using UnityEngine;
using System.Collections.Generic;
public class SoundEffectSystem : ISetPool, IExecuteSystem, IInitializeSystem {

    Pool pool;
    Group group;

    public void SetPool(Pool pool) {
        //_pool = pool;
        group = pool.GetGroup(Matcher.AllOf(Matcher.SoundEffect));
    }

    public void Execute() {
    //            GameController.AudioSources["pew"].Play();

        foreach (var e in group.GetEntities()) {
            switch ((int)e.soundEffect.effect) {
                case 0:
                    GameController.AudioSources["pew"].PlayOneShot(GameController.AudioSources["pew"].clip, 0.5f);
                    break;
                case 1:
                    GameController.AudioSources["asplode"].PlayOneShot(GameController.AudioSources["pew"].clip, 0.5f);
                    break;
                case 2:
                    GameController.AudioSources["smallasplode"].PlayOneShot(GameController.AudioSources["pew"].clip, 0.5f);
                    break;
            }
        }
    }

    public void Initialize() {
    }

}