using NUnit.Framework;
using Newtonsoft.Json;
using librato4net.Metrics;
using System.Collections.Generic;
using System;

namespace librato4net.Tests
{
	[TestFixture]
	public class SerialisationTests
	{
		[Test]
		public void gauge_serialises_as_expected() 
		{
			var metricWithGauge = new Metric 
			{
				Gauges = new List<Gauge> { new Gauge { Name = "some.metric.name", Value = -56, Source = "some-source" } }
			};

			var json = Serialise(metricWithGauge);

			Assert.That(json, Is.EqualTo(@"{""gauges"":[{""name"":""some.metric.name"",""value"":-56,""source"":""some-source""}]}"));
		}

		public string Serialise(Metric metric) 
		{
			var jsonConfig = new JsonSerializerSettings 
			{
				NullValueHandling = NullValueHandling.Ignore
			};

			var jsonData = JsonConvert.SerializeObject(metric, jsonConfig);

			return jsonData;
		}

		[Test]
		public void counter_serialises_as_expected() 
		{
			var metricWithCounter = new Metric 
			{
				Counters = new List<Counter> { new Counter { Name = "some.metric.name", Value = 12345.67f, Source = "some-source" } }
			};

			var json = Serialise(metricWithCounter);

			Assert.That(json, Is.EqualTo(@"{""counters"":[{""name"":""some.metric.name"",""value"":12345.67,""source"":""some-source""}]}"));
		}
	}
}

