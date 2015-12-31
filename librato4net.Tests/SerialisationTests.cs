using System;
using System.Collections.Generic;
using librato4net.Annotations;
using librato4net.Metrics;
using Newtonsoft.Json;
using NUnit.Framework;

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

        private string Serialise<T>(T payload)
        {
            var jsonData = JsonConvert.SerializeObject(payload, LibratoJson.Settings);

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

        [Test]
        public void annotation_serialises_as_expected()
        {
            var fullAnnotation = new Annotation
            {
                    Type = "some.type", Title = "some.title", Description = "some.description",
                    Source = "some.source", StartTime = new DateTime(2015, 12, 12), EndTime = null  
            };

            var json = Serialise(fullAnnotation);

            Assert.That(json, Is.EqualTo(@"{""source"":""some.source"",""title"":""some.title"",""description"":""some.description"",""start_time"":1449878400}"));
        }
    }
}

