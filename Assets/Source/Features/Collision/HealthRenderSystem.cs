using Entitas;
using UnityEngine;
using System;
using System.Collections.Generic;
public class HealthRenderSystem : ISetPool, IExecuteSystem {

    Pool pool;
    Group group;
	readonly Transform _viewContainer = new GameObject("View").transform;

	private Dictionary<string,GameObject> texts = new Dictionary<string, GameObject>();

	/**
	 * Create 
	 * 
	 */


    public void SetPool(Pool pool) {
        pool = pool;
        group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Health));

		var enemies = pool.GetGroup(Matcher.Enemy);
		enemies.OnEntityAdded += OnEntityAdded;
		enemies.OnEntityRemoved += OnEntityRemoved;
    }

	void OnEntityAdded(Group group, Entity entity, int index, IComponent component) {
		var res = Resources.Load<GameObject>("health");
		GameObject health = null;
		try {
			health = UnityEngine.Object.Instantiate(res);
		} catch (Exception e) {
			Debug.Log("Cannot instantiate " + res);
		}

		if (health != null) {
			Renderer renderer = health.GetComponent<Renderer>();
			renderer.sortingOrder = 10;
			health.transform.SetParent(_viewContainer, false);
			texts[entity.GetHashCode().ToString()] = health;
		}
	}

	void OnEntityRemoved(Group group, Entity entity, int index, IComponent component) {
		GameObject health = texts[entity.GetHashCode().ToString()];
		texts.Remove(entity.GetHashCode().ToString());
		UnityEngine.Object.Destroy(health);
	}

    public void Execute() {
        foreach (var e in group.GetEntities()) {
			if (texts.ContainsKey(e.GetHashCode().ToString())) {
				GameObject health = texts[e.GetHashCode().ToString()];
				GameObject gameObject = e.view.gameObject;

				var pos = e.position;
				var percentage = Math.Round(e.health.health / e.health.maximumHealth * 100);

				health.transform.position = new Vector3(pos.x, pos.y, 0f);
				TextMesh text = (TextMesh)health.GetComponent("TextMesh");
				text.text = percentage.ToString();


			}
        }
    }

}