/**
 *--------------------------------------------------------------------+
 * MersenneTwister.cs
 *--------------------------------------------------------------------+
 * Copyright DarkOverlordOfData (c) 2015
 *--------------------------------------------------------------------+
 *
 * This file is a part of Bosco
 *
 * Bosco is free software; you can copy, modify, and distribute
 * it under the terms of the MIT License
 *
 * MersenneTwister:
 *  Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
 *  All rights reserved.
 *
 *--------------------------------------------------------------------+
 *  MT19937 - An alternative PRNG
 *
 */
using System;
namespace Bosco.Utils {
	public class MersenneTwister : IRandum {

		private static long N = 624;
		private static long M = 397;
		private static long MATRIX_A = -1727483681;
		private static long UPPER_MASK = -2147483648;
		private static int LOWER_MASK = 2147483647;

		public long[] mt;
		public long mti = N+1;

		public MersenneTwister (long seed) {
			if (seed == -1) {
				// time since epoch: 1/1/1970
				seed = ((new DateTime()).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
			}
			init_genrand(seed);

		}

		/*
	     * Generates a random boolean value.
	    */
		public bool NextBool() {
			return (genrand_int32() & 1) == 1;
		}
		
		/*
	     * Generates a random real value from 0.0, inclusive, to 1.0, exclusive.
	    */
		public double NextDouble() {
			return genrand_res53();
		}
		
		/*
	     * Generates a random int value from 0, inclusive, to max, exclusive.
	    */
		public long NextInt(long max) {
			return (long)Math.Abs(genrand_res53() * max);
		}


		public void init_genrand(long s) {
			mt[0] = s & -1;
			mti = 1;
			while (mti < N) {
				mt[mti] = 1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti;
				/*
	            # See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. #
	            # In the previous versions, MSBs of the seed affect   #
	            # only MSBs of the array mt[].                        #
	            # 2002/01/09 modified by Makoto Matsumoto             #
	            */
				mt[mti] = (mt[mti] & -1) >> 0;
				/*
	            # for >32 bit machines #
	            */
				mti++;

			}
		}

		public void init_by_array(int[] init_key, int key_length) {
			long i, j, k;
			init_genrand(19650218);
			i = 1;
			j = 0;
			k = N > key_length ? N : key_length;
			while (k > 0) {
				mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525)) + init_key[j] + j;
				mt[i] &= -1;
				i++;
				j++;
				if (i >= N) {
					mt[0] = mt[N - 1];
					i = 1;
				}
				if (j >= key_length) {
					j = 0;
				}
				k--;
			}
			
			k = N - 1;
			while (k > 0) {
				mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941)) - i;
				mt[i] &= -1;
				i++;
				if (i >= N) {
					mt[0] = mt[N - 1];
					i = 1;
				}
				k--;
			}
			mt[0] = UPPER_MASK;

		}

		public long genrand_int32() {
			long kk, y;
			var mag01 = new long[]{0, MATRIX_A};

			if (mti >= N) {
				if (mti == N + 1) {
					init_genrand(5489);
				}
				
				kk = 0;
				while (kk < N - M) {
					y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
					mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 1];
					kk++;
				}
				
				while (kk < N - 1) {
					y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
					mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 1];
					kk++;
				}
				
				y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
				mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 1];
				
				mti = 0;
			}
			
			y = mt[mti++];
			y ^= y >> 11;
			y ^= (y << 7) & -1658038656;
			y ^= (y << 15) & -272236544;
			y ^= y >> 18;
			
			return y >> 0;
		}

		/*
	    * generates a random number on [0,0x7fffffff]-interval
	    */
		public long genrand_int31() {
			return genrand_int32() >> 1;
		}
		
		/*
	     * generates a random number on [0,1]-real-interval
	    */
		public double genrand_real1() {
			return genrand_int32() * 2.32830643653869629e-10;
		}
		
		/*
	     * generates a random number on [0,1)-real-interval
	    */
		public double genrand_real2() {
			return genrand_int32() * 2.32830643653869629e-10;
		}
		
		/*
	     * generates a random number on (0,1)-real-interval
	    */
		public double genrand_real3() {
			return (genrand_int32() + 0.5) * 2.32830643653869629e-10;
		}
		
		/*
	     * generates a random number on [0,1] 53-bit resolution
	    */
		public double genrand_res53() {
			long a, b;
			a = genrand_int32() >> 5;
			b = genrand_int32() >> 6;
			return (a * 67108864.0 + b) * 1.11022302462515654e-16;
		}

	}
}
/*
# These real versions are due to Isaku Wada, 2002/01/09 added
*/
/**
 A C-program for MT19937, with initialization improved 2002/1/26.
 Coded by Takuji Nishimura and Makoto Matsumoto.
 Before using, initialize the state by using init_genrand(seed)
 or init_by_array(init_key, key_length).
 Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions
 are met:
 1. Redistributions of source code must retain the above copyright
 notice, this list of conditions and the following disclaimer.
 2. Redistributions in binary form must reproduce the above copyright
 notice, this list of conditions and the following disclaimer in the
 documentation and/or other materials provided with the distribution.
 3. The names of its contributors may not be used to endorse or promote
 products derived from this software without specific prior written
 permission.
 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 Any feedback is very welcome.
 http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
 email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
 */

