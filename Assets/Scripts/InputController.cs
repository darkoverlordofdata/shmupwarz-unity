using UnityEngine;

public class InputController : MonoBehaviour {

    public bool burstMode;

    void Start() {
        Pools.pool.CreatePlayer(3, 2);
    }
    
    void Update() {

        /*    
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Pools.pool.playerEntity.view.gameObject.transform.DOMove(new Vector3(mousePosition.x, mousePosition.y, 0f), 0.3f);
        */
        
        if (Input.GetKeyDown("b")) {
            burstMode = !burstMode;
        }

        var input = burstMode
                    ? Input.GetMouseButton(0)
                    : Input.GetMouseButtonDown(0);

        if (input) {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100);
            if (hit.collider != null) {
                var pos = hit.collider.transform.position;
                Pools.pool.CreateEntity()
                    .AddInput((int)pos.x, (int)pos.y);
            }
        } 
    }
}
