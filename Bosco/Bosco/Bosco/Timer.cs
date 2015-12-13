/**
 *--------------------------------------------------------------------+
 * Timer.cs
 *--------------------------------------------------------------------+
 * Copyright DarkOverlordOfData (c) 2015
 *--------------------------------------------------------------------+
 *
 * This file is a part of Bosco
 *
 * Bosco is free software; you can copy, modify, and distribute
 * it under the terms of the MIT License
 *
 *--------------------------------------------------------------------+
 */
using UnityEngine;

namespace Bosco.Utils {
	
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
			Update (Time.deltaTime);
		}
		
		public void Update(float delta) {
			if (!done && !stopped) {
				acc += delta;
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
}