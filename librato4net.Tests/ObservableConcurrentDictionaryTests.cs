using NUnit.Framework;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace librato4net.Tests
{
	[TestFixture]
	public class ObservableConcurrentDictionaryTests
	{
		[Test]
		public void when_adding_new_key_for_the_first_time_then_changed_event_contains_the_added_item() 
		{
			NotifyCollectionChangedEventArgs raisedEventArgs = null;
			var dictionary = new ObservableConcurrentDictionary<string, string>();
			dictionary.CollectionChanged += (sender, e) => raisedEventArgs = e;

			dictionary.AddOrUpdate("some.key", "some.value", (key, value) => "some.updated.value");

			Assert.That(raisedEventArgs.NewItems, Has.Count.EqualTo(1));

			var addedItem = (KeyValuePair<string, string>)raisedEventArgs.NewItems[0];
			Assert.That(addedItem.Key, Is.EqualTo("some.key"));
			Assert.That(addedItem.Value, Is.EqualTo("some.value"));
		}

		[Test]
		public void when_updating_existing_key_then_changed_event_contains_the_update_item_as_new() 
		{
			NotifyCollectionChangedEventArgs raisedEventArgs = null;
			var dictionary = new ObservableConcurrentDictionary<string, string>();
			dictionary.CollectionChanged += (sender, e) => raisedEventArgs = e;

			//add then update
			dictionary.AddOrUpdate("some.key", "some.value", (key, value) => "some.updated.value");
			dictionary.AddOrUpdate("some.key", "some.value", (key, value) => "some.updated.value");

			Assert.That(raisedEventArgs.NewItems, Has.Count.EqualTo(1));

			var addedItem = (KeyValuePair<string, string>)raisedEventArgs.NewItems[0];
			Assert.That(addedItem.Key, Is.EqualTo("some.key"));
			Assert.That(addedItem.Value, Is.EqualTo("some.updated.value"));
		}

		[Test]
		public void when_updating_existing_key_then_changed_event_contains_the_former_item_as_old() 
		{
			NotifyCollectionChangedEventArgs raisedEventArgs = null;
			var dictionary = new ObservableConcurrentDictionary<string, string>();
			dictionary.CollectionChanged += (sender, e) => raisedEventArgs = e;

			//add then update
			dictionary.AddOrUpdate("some.key", "some.value", (key, value) => "some.updated.value");
			dictionary.AddOrUpdate("some.key", "some.value", (key, value) => "some.updated.value");

			Assert.That(raisedEventArgs.OldItems, Has.Count.EqualTo(1));

			var formerItem = (KeyValuePair<string, string>)raisedEventArgs.OldItems[0];
			Assert.That(formerItem.Key, Is.EqualTo("some.key"));
			Assert.That(formerItem.Value, Is.EqualTo("some.value"));
		}
	}
}

