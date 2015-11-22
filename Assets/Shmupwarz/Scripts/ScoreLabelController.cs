using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLabelController : MonoBehaviour {
    public Text label;
    private Pool pool;
    private int score = 0;

    void Start() {
        label = GetComponent<Text>();

        pool = Pools.pool;
        updateScore(pool.score.value);
    }

    void Update() {
        if (score != pool.score.value) {
            score = pool.score.value;
            updateScore(score);
        }
    }
    
    void updateScore(int score) {
        label.text = string.Format("Score: {0:0000}", score);
    }
}
