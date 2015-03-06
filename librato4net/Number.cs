﻿using Newtonsoft.Json;

namespace librato4net
{
	[JsonConverter(typeof(NumberConverter))]
	public class Number
	{
		internal int? IntegerValue { get; private set; }
		internal float? FloatValue { get; private set; }

		public Number(int value)
		{
			IntegerValue = value;
		}

		public Number(float value)
		{
			FloatValue = value;
		}

		public static implicit operator Number(float value)
		{
			return new Number(value);
		}

		public static implicit operator Number(int value)
		{
			return new Number(value);
		}

	}
}

