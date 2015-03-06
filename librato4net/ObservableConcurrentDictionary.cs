﻿using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace librato4net
{
	public sealed class ObservableConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, INotifyCollectionChanged
	{
		public ObservableConcurrentDictionary()
		{ 
		}

		public ObservableConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
			: base(collection)
		{ 
		}

		public ObservableConcurrentDictionary(IEqualityComparer<TKey> comparer)
			: base(comparer)
		{ 
		}

		public ObservableConcurrentDictionary(int concurrencyLevel, int capacity)
			: base(concurrencyLevel, capacity)
		{ 
		}

		public ObservableConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
			: base(collection, comparer)
		{
		}

		public ObservableConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
			: base(concurrencyLevel, capacity, comparer)
		{ 
		}

		public ObservableConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
			: base(concurrencyLevel, collection, comparer)
		{
		}

		public new TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
		{
			TValue value;

			if (ContainsKey(key))
			{
				value = base.AddOrUpdate(key, addValue, updateValueFactory);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value)));
			}
			else
			{
				value = base.AddOrUpdate(key, addValue, updateValueFactory);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
			}

			return value;
		}

		public new TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
		{
			TValue value;

			if (ContainsKey(key))
			{
				value = base.AddOrUpdate(key, addValueFactory, updateValueFactory);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value)));
			}
			else
			{
				value = base.AddOrUpdate(key, addValueFactory, updateValueFactory);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
			}

			return value;
		}

		public new void Clear()
		{
			base.Clear();
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public new TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			TValue value;

			if (ContainsKey(key)) 
			{
				value = base.GetOrAdd(key, valueFactory);
			}
			else
			{
				value = base.GetOrAdd(key, valueFactory);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
			}

			return value;
		}

		public new TValue GetOrAdd(TKey key, TValue value)
		{
			if (ContainsKey(key)) 
			{
				base.GetOrAdd(key, value);
			}
			else
			{
				base.GetOrAdd(key, value);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
			}

			return value;
		}

		public new bool TryAdd(TKey key, TValue value)
		{
			bool tryAdd;

			if (tryAdd = base.TryAdd(key, value)) 
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
			}

			return tryAdd;
		}

		public new bool TryRemove(TKey key, out TValue value)
		{
			bool tryRemove;

			if (tryRemove = base.TryRemove(key, out value)) 
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value)));
			}

			return tryRemove;
		}

		public new bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
		{
			bool tryUpdate;

			if (tryUpdate = base.TryUpdate(key, newValue, comparisonValue)) 
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, newValue)));
			}

			return tryUpdate;
		}

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (CollectionChanged != null) 
			{
				CollectionChanged(this, e);
			}
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
