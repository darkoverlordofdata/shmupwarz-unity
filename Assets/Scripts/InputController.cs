using UnityEngine;

public class InputController : MonoBehaviour {

    public bool burstMode;

    void Start() {
        Pools.pool.CreatePlayer();
    }
    
    void Update() {}
}
