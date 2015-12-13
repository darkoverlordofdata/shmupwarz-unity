/**
 *--------------------------------------------------------------------+
 * IRandum.cs
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
using System;
namespace Bosco.Utils {
	public interface IRandum {
		bool NextBool();
		double NextDouble();
		int NextInt(int max);
	}

}

