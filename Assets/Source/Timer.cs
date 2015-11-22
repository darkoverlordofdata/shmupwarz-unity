using UnityEngine;

public abstract class Timer {

	float delay;
	float acc;
	bool repeat;
	bool done;
	bool stopped;
	
	public Timer(float delay, bool repeat) {
		this.delay = delay;
		this.repeat = repeat;
		this.acc = 0;
	}
	
	public void Update() {
		if (!done && !stopped) {
			acc += Time.deltaTime;
		}
		if (acc >= delay) {
			acc -= delay;
	
			if (repeat) {
				Reset();
			} else {
				done = true;
			}
	
			Execute();
		}
	}
	
	public void Reset() {
		stopped = false;
		done = false;
		acc = 0;
	}
	
	public bool IsDone() {
		return done;
	}
	
	public bool IsRunning() {
		return !done && acc < delay && !stopped;
	}
	
	public void Stop() {
		stopped = true;
	}
	
	public void SetDelay(float delay) {
		this.delay = delay;
	}
	
	public abstract void Execute();
	
	public int GetPercentageRemaining() {
		if (done) return 100;
		else if (stopped) return 0;
		else return (int)(1 - (delay - acc) / delay);
	}
	
	public float GetDelay() {
		return delay;
	}

}
