//-----------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !__WP8__
using System.Collections.Concurrent;
#endif



namespace Sphere10.Framework {

	public static class IEnumerableExtensions {

#if !__WP8__
		public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body) {
			return ForEachAsync(source, 1, body);	// one at a time
		}
		
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body) 
		{ 
			return Task.WhenAll( 
				from partition in Partitioner.Create(source).GetPartitions(dop) 
			                 select Task.Run(async delegate { 
				using (partition) 
					while (partition.MoveNext()) 
						await body(partition.Current); 
			})); 
		}

#else
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body)
        {
            foreach (var item in source)
            {
                await body(item);
            }
        }

#endif

		public static IEnumerable<T> TakeUntil<T>(
			this IEnumerable<T> elements,
			Func<T, bool> predicate
		) {
			return 
				elements
				.Select((x, i) => new { Item = x, Index = i })						   
				.TakeUntil((x, i) => predicate(x.Item))
				.Select(x => x.Item);
		}

		public static IEnumerable<T> TakeUntil<T>(
			this IEnumerable<T> elements,
			Func<T, int, bool> predicate
		) {
			int i = 0;
			foreach (T element in elements) {
				yield return element;
				if (predicate(element, i)) {					
					yield break;
				}
				i++;
			}
		}

		public static IEnumerable<IEnumerable<T>> OrderByAll<T>(this IEnumerable<IEnumerable<T>> table) {
            var tableList = table as List<IEnumerable<T>> ?? table.ToList();
            var numCols = tableList[0].Count();
            if (numCols == 0)
                return tableList;

            var result = tableList.OrderBy(r => r.ElementAt(0));
            for (var i = 1; i < numCols; i++) {
                var i1 = i;
                result = result.ThenBy(r => r.ElementAt(i1));
            }

            return result;
	    }

        public static IEnumerable<T> WithoutClones<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<IEnumerable<T>, T> duplicateResolver) {
            return source.WithoutClones((x) => x, (x, resolution) => resolution, keySelector, duplicateResolver);
        }

        public static IEnumerable<T> WithoutClones<T, TMember, TKey>(this IEnumerable<T> source, Func<T, TMember> memberGetter, Func<T, TMember, T> memberCreator, Func<TMember, TKey> keySelector, Func<IEnumerable<TMember>, TMember> duplicateResolver) {
            var sourceArray = source.ToArray();
            var membersByKey = sourceArray.Select(memberGetter).ToLookup(keySelector).ToDictionary(g => g.Key, g => duplicateResolver(g));
            return sourceArray.Select(x => memberCreator(x, membersByKey[keySelector(memberGetter(x))]));
        }

	    public static ObservableList<T> ToObservableList<T>(this IEnumerable<T> source) {
            return new ObservableList<T>(new List<T>(source));
	    }

        /*public static ObseravableCloneableList<T> ToCloneableListWithEvents<T>(this IEnumerable<T> source) where T : ICloneable {
            return new ObseravableCloneableList<T>(source);
        }*/ 


		public static IEnumerable<T> InLinkedListOrder<T, K>(this IEnumerable<T> source, Func<T, K> keySelector, Func<T, K> linkSelector, Func<K, bool> isEndKey = null, bool includeEndKey = true) {
			K startKey;
			#region Find the start key
			// Quickly determine which nodes are linked to
			var existingNodes = new HashSet<K>();
			var linkedNodes = new HashSet<K>();
		    var sourceArr = source as T[] ?? source.ToArray();
		    foreach (var node in sourceArr) {
				var key = keySelector(node);
				var linkedNodeKey = linkSelector(node);
				existingNodes.Add(key);
				linkedNodes.Add(linkedNodeKey);
			}

			var unlinkedNodes = existingNodes.Except(linkedNodes).ToArray();
			var unlinkedNodesCount = unlinkedNodes.Count();
			if (unlinkedNodesCount == 0 && existingNodes.Count > 0) {
				// circular linked list, so pick any item to start with
				startKey = keySelector(sourceArr.First());
			} else if (unlinkedNodesCount == 1) {
				// one item is unlinked, so it's the start item
				startKey = unlinkedNodes.Single();
			} else {
				// many items are unlinked, can't pick start item
				throw new Exception(string.Format("Unable to pick a start node as there were {0} possible candidates. A start node is determined by a node that is not linked to by other nodes", unlinkedNodesCount));
			}
			#endregion

			return InLinkedListOrder(sourceArr, keySelector, linkSelector, startKey, isEndKey, includeEndKey);
		}

		public static IEnumerable<T> InLinkedListOrder<T, K>(this IEnumerable<T> source, Func<T, K> keySelector, Func<T, K> linkSelector, K startKey, Func<K, bool> isEndKey = null, bool includeEndKey = true) {
		    var sourceArr = source as T[] ?? source.ToArray();
		    if (!sourceArr.Any())
				yield break;

			if (isEndKey == null)
				isEndKey = x => false;

			var nodeLookup = sourceArr.ToLookup(keySelector);
			var visited = new HashSet<K>();
			var currentKey = startKey;
			do {
				if (!includeEndKey && isEndKey(currentKey))
					break;
				var linkedNodes = nodeLookup[currentKey];
				var linkedNodesCount = linkedNodes.Count();
				if (linkedNodesCount == 0)
					yield break; // last node
				if (linkedNodesCount > 1)
					throw new Exception(string.Format("Expected 1 node with key {0}, found {1}", currentKey, linkedNodes.Count()));


				var current = linkedNodes.Single();
				visited.Add(currentKey);
				yield return current;
				if (includeEndKey && isEndKey(currentKey))
					break;
				currentKey = linkSelector(current);

			} while (!visited.Contains(currentKey));
		}

		public static IEnumerable<TResult> ZipWith<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) {
			if (first == null) throw new ArgumentNullException("first");
			if (second == null) throw new ArgumentNullException("second");
			if (resultSelector == null) throw new ArgumentNullException("resultSelector");
			return ZipIterator(first, second, resultSelector);
		}

		private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>
			(IEnumerable<TFirst> first,
			IEnumerable<TSecond> second,
			Func<TFirst, TSecond, TResult> resultSelector) {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
				while (e1.MoveNext() && e2.MoveNext())
					yield return resultSelector(e1.Current, e2.Current);
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T element) {
			return source.Except(new[] { element });
		}

		public static IEnumerable<T> Union<T>(this IEnumerable<T> source, T element) {
			return source.Union(new[] { element });
		}

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T element) {
            return source.Concat(new[] { element });
        }

		public static IDictionary<K, List<T>> ToMultiValueDictionary<K, T>(this IEnumerable<T> source, Func<T, K> keySelector) {
			var result = new Dictionary<K, List<T>>();

			foreach (var item in source) {
				var itemKey = keySelector(item);
				if (!result.ContainsKey(itemKey)) {
					result[itemKey] = new List<T>();
				}
				result[itemKey].Add(item);
			}
			return result;
		}

		public static IDictionary<K, List<V>> ToMultiValueDictionary<K, V, T>(this IEnumerable<T> source, Func<T, K> keySelector, Func<T, V> valueSelector) {
			var result = new Dictionary<K, List<V>>();
			foreach (var item in source) {
				var itemKey = keySelector(item);
				if (!result.ContainsKey(itemKey)) {
					result[itemKey] = new List<V>();
				}
				result[itemKey].Add(valueSelector(item));
			}
			return result;
		}

		public static MultiKeyDictionary<K1, K2, V> ToMultiKeyDictionary<S, K1, K2, V>(this IEnumerable<S> items, Func<S, K1> key1, Func<S, K2> key2, Func<S, V> value) {
			var dict = new MultiKeyDictionary<K1, K2, V>();
			foreach (S i in items) {
				dict.Add(key1(i), key2(i), value(i));
			}
			return dict;
		}

		public static MultiKeyDictionary<K1, K2, K3, V> ToMultiKeyDictionary<S, K1, K2, K3, V>(this IEnumerable<S> items, Func<S, K1> key1, Func<S, K2> key2, Func<S, K3> key3, Func<S, V> value) {
			var dict = new MultiKeyDictionary<K1, K2, K3, V>();
			foreach (S i in items) {
				dict.Add(key1(i), key2(i), key3(i), value(i));
			}
			return dict;
		}

		public static MultiKeyDictionary<K1, K2, K3, K4, V> ToMultiKeyDictionary<S, K1, K2, K3, K4, V>(this IEnumerable<S> items, Func<S, K1> key1, Func<S, K2> key2, Func<S, K3> key3, Func<S, K4> key4, Func<S, V> value) {
			var dict = new MultiKeyDictionary<K1, K2, K3, K4, V>();
			foreach (S i in items) {
				dict.Add(key1(i), key2(i), key3(i), key4(i), value(i));
			}
			return dict;
		}

		public static MultiKeyDictionary2<K, T> ToMultiKeyDictionary2<K, T>(this IEnumerable<T> source, params Func<T, K>[] keySelectors) {
		    return ToMultiKeyDictionary2(source, (IEnumerable<Func<T, K>>) keySelectors);
		}

        public static MultiKeyDictionary2<K, T> ToMultiKeyDictionary2<K, T>(this IEnumerable<T> source, IEnumerable<Func<T, K>> keySelectors) {
            return ToMultiKeyDictionary2Ex(source, keySelectors, (x) => x);
        }

        public static MultiKeyDictionary2<K, V> ToMultiKeyDictionary2Ex<T, K, V>(this IEnumerable<T> source, IEnumerable<Func<T, K>> keySelectors, Func<T, V> valueSelector) {
            var result = new MultiKeyDictionary2<K, V>();
            var keySelectorArr = keySelectors as Func<T, K>[] ?? keySelectors.ToArray();
            foreach (var item in source) {                
                var key = keySelectorArr.Select(keySelector => keySelector(item));
                result.Add(key, valueSelector(item));
            }
            return result;
        }

		public static MultiKeyLookup<K, T> ToMultiKeyLookup<T, K>(this IEnumerable<T> source, Func<T, IEnumerable<K>> keySelector, IEqualityComparer<K> comparer = null) {
			return source.ToMultiKeyLookup(keySelector, item => item, comparer);
		}

		public static MultiKeyLookup<K, V> ToMultiKeyLookup<T, K, V>(this IEnumerable<T> source, Func<T, IEnumerable<K>> keySelector, Func<T, V> valueSelector, IEqualityComparer<K> comparer = null) {
			if (source == null) {
				throw new ArgumentNullException("source");
			}
			if (keySelector == null) {
				throw new ArgumentNullException("keySelector");
			}
			if (valueSelector == null) {
				throw new ArgumentNullException("valueSelector");
			}

			var lookup = new MultiKeyLookup<K, V>(comparer ?? EqualityComparer<K>.Default);

			foreach (var item in source) {
				var key = keySelector(item);
				var element = valueSelector(item);
				lookup.Add(key, element);
			}
			return lookup;
		}

		// NOTE: ForReach applies action to all items then return enumerable, Apply applies action during enumeration 
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			foreach (T item in source)
				action(item);

			return source;
		}

		// NOTE: ForReach applies action to all items then return enumerable, Apply applies action during enumeration
		public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Action<T> action) {
			return source.Select(x => {
				action(x);
				return x;
			});
		}

		public static int Update<T>(this IEnumerable<T> source, Action<T> action) {
			int updated = 0;
			foreach (T item in source) {
				action(item);

				updated++;
			}
			return updated;
		}

		public static IEnumerable<IEnumerable<TSource>> Split<TSource>(
	this IEnumerable<TSource> source,
	Func<TSource, bool> predicate) {
			var group = new List<TSource>();
			foreach (var item in source) {
				if (predicate(item)) {
					yield return group.AsEnumerable();
					group = new List<TSource>();
				} else {
					group.Add(item);
				}
			}
			yield return group.AsEnumerable();
		}

		public static IEnumerable<T> Into<T>(this IEnumerable<T> source, ref T value1, ref T value2) {
			var len = source.Count();
			var enumerator = source.GetEnumerator();
			if (len > 0) {
				value1 = enumerator.Current;
				enumerator.MoveNext();
			}

			if (len > 1) {
				value2 = enumerator.Current;
				enumerator.MoveNext();
			}
			return source;
		}

		public static bool ContainSameElements<T>(this IEnumerable<T> source, IEnumerable<T> dest, IEqualityComparer<T> comparer = null ) {
            var c = comparer ?? EqualityComparer<T>.Default;
            return source.Count() == dest.Count() && !source.Except(dest, c).Any();
		}

		public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] matches) {
			return source.ContainsAll(matches.AsEnumerable());
		}

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer, params T[] matches) {
            return source.ContainsAll(matches.AsEnumerable(), comparer);
        }


        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> matches, IEqualityComparer<T> comparer = null) {
		    var c = comparer ?? EqualityComparer<T>.Default;
			return matches.All(x => source.Contains(x,c));
		}


		public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] matches) {
			return source.ContainsAny(matches.AsEnumerable());
		}

        public static bool ContainsAny<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer, params T[] matches) {
            return source.ContainsAny(matches.AsEnumerable(), comparer);
        }


        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> matches, IEqualityComparer<T> comparer = null) {
            var c = comparer ?? EqualityComparer<T>.Default;
            return matches.Any(x => source.Contains(x,c));
		}

		/// <summary>
		/// Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <typeparam name="TSearch">The type of the searched item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem, TSearch>(this IEnumerable<TItem> list, TSearch value, Func<TSearch, TItem, int> comparer) {
			if (list == null) {
				throw new ArgumentNullException("list");
			}
            var listArr = list as TItem[] ?? list.ToArray();

			if (comparer == null) {
				throw new ArgumentNullException("comparer");
			}

			var lower = 0;
		    
		    var upper = listArr.Count() - 1;

			while (lower <= upper) {
				var middle = lower + (upper - lower) / 2;
				var comparisonResult = comparer(value, listArr[middle]);
				if (comparisonResult < 0) {
					upper = middle - 1;
				} else if (comparisonResult > 0) {
					lower = middle + 1;
				} else {
					return middle;
				}
			}

			return ~lower;
		}

		/// <summary>
		/// Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IEnumerable<TItem> list, TItem value) {
			return BinarySearch(list, value, Comparer<TItem>.Default);
		}

		/// <summary>
		/// Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IEnumerable<TItem> list, TItem value, IComparer<TItem> comparer) {
			return list.BinarySearch(value, comparer.Compare);
		}


		public static IEnumerable<t> Randomize<t>(this IEnumerable<t> source) {
			return source.OrderBy(x => (Tools.Maths.RandomNumberGenerator.Next()));
		}


#if __IOS__
		public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, int partitionSize ) {
			if (partitionSize < 1)
				throw new ArgumentOutOfRangeException("partitionSize", partitionSize, "must be equal to or greater than 1");
			// quick hack to get around AOT issues on iOS
			var list = new List<List<T>>();
			for (int i = 0; i < sequence.Count(); i += partitionSize)
				list.Add(sequence.Skip(i).Take (partitionSize).ToList());

			return (IEnumerable<IEnumerable<T>>)list.Cast<IEnumerable<T>>();
		}

        public static IEnumerable<IEnumerable<T>> PartitionBySize<T>(this IEnumerable<T> sequence, Func<T, int> sizeProperty, int partitionSize) {
            if (partitionSize < 1)
                throw new ArgumentOutOfRangeException("partitionSize", partitionSize, "must be equal to or greater than 1");
            long currentSum = 0;
            return
                sequence
                .Select((x, i) => new { Value = x, BatchNo = (currentSum = currentSum + sizeProperty(x)) / partitionSize })
                .GroupAdjacentBy(x => x.BatchNo)
                .Select(g => g.Select(x => x.Value));
        }
#else
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, int partitionSize) {
			if (partitionSize < 1)
				throw new ArgumentOutOfRangeException("partitionSize", partitionSize, "must be equal to or greater than 1");
			return
				sequence
				.Select((x, i) => new { Value = x, BatchNo = i / partitionSize })
				.GroupAdjacentBy(x => x.BatchNo)
				.Select(g => g.Select(x => x.Value));
		}

        public static IEnumerable<IEnumerable<T>> PartitionBySize<T>(this IEnumerable<T> sequence, Func<T, int> sizeProperty, int partitionSize) {
            if (partitionSize < 1)
                throw new ArgumentOutOfRangeException("partitionSize", partitionSize, "must be equal to or greater than 1");
            long currentSum = 0;
            return
                sequence
                .Select((x, i) => new { Value = x, BatchNo = (currentSum = currentSum + sizeProperty(x)) / partitionSize })
                .GroupAdjacentBy(x => x.BatchNo)
                .Select(g => g.Select(x => x.Value));
        }
#endif
        public static IEnumerable<T> Unpartition<T>(this IEnumerable<IEnumerable<T>> sequence) {
			return sequence.SelectMany(partition => partition);
		}

		public static IEnumerable<T> Unpartition<T>(this IEnumerable<T[]> sequence) {
			return sequence.Cast<IEnumerable<T>>().Unpartition();
		}

		public static IEnumerable<IGrouping<TKey, TSource>> GroupAdjacentBy<TSource, TKey>(
				this IEnumerable<TSource> source,
				Func<TSource, TKey> keySelector) {
			var last = default(TKey);
			var haveLast = false;
			var list = new List<TSource>();
			foreach (var s in source) {
				var k = keySelector(s);
				if (haveLast) {
					if (!k.Equals(last)) {
						yield return new GroupOfAdjacent<TSource, TKey>(list, last);
						list = new List<TSource>();
						list.Add(s);
						last = k;
					} else {
						list.Add(s);
						last = k;
					}
				} else {
					list.Add(s);
					last = k;
					haveLast = true;
				}
			}
			if (haveLast)
				yield return new GroupOfAdjacent<TSource, TKey>(list, last);
		}


		public static IEnumerable<T> ForEachThenOnLast<T>(this IEnumerable<T> source, Action<T> eachAction, Action<T> lastAction) {
			source.Reverse().Skip(1).Reverse().ForEach(eachAction);
			var lastItem = source.LastOrDefault();
			if (lastItem != null) {
				lastAction(lastItem);
			}
			return source;
		}

        public static string ToDelimittedString<T>(this IEnumerable<T> source, string delimitter, string nullText = "", Func<T, string> toStringFunc = null) {
		    if (toStringFunc == null)
                toStringFunc = (x) => x.ToString();

			StringBuilder stringBuilder = new StringBuilder();
			foreach (var t in source) {
				if (stringBuilder.Length > 0) {
					stringBuilder.Append(delimitter);
				}
                stringBuilder.Append(t != null ? toStringFunc(t) : nullText);
			}
			return stringBuilder.ToString();
		}

		public static IEnumerable<EnumeratedItem<T>> WithDescriptions<T>(this IEnumerable<T> items) {
			// To avoid evaluating the whole collection up-front (which may be undesirable, for example
			// if the collection contains infinitely many members), read-ahead just one item at a time.

			// Get the first item
			var enumerator = items.GetEnumerator();
			if (!enumerator.MoveNext())
				yield break;
			T currentItem = enumerator.Current;
			int index = 0;

			while (true) {
				// Read ahead so we know whether we're at the end or not
				bool isLast = !enumerator.MoveNext();

				// Describe and yield the current item
				EnumeratedItemDescription description = (index % 2 == 0 ? EnumeratedItemDescription.Odd : EnumeratedItemDescription.Even);
				if (index == 0) description |= EnumeratedItemDescription.First;
				if (isLast) description |= EnumeratedItemDescription.Last;
				if (index > 0 && !isLast) description |= EnumeratedItemDescription.Interior;
				yield return new EnumeratedItem<T>(index, currentItem, description);

				// Terminate or continue
				if (isLast)
					yield break;
				index++;
				currentItem = enumerator.Current;
			}
		}

		// This provides a useful extension-like method to find the index of and item from IEnumerable<T>
		// This was based off of the Enumerable.Count<T> extension method.
		/// <summary>
		/// Returns the index of an item in a sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence containing elements.</param>
		/// <param name="item">The item to locate.</param>        
		/// <returns>The index of the entry if it was found in the sequence; otherwise, -1.</returns>
		public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item) {
			return IndexOf(source, item, EqualityComparer<TSource>.Default);
		}



		// This provides a useful extension-like method to find the index of and item from IEnumerable<T>
		// This was based off of the Enumerable.Count<T> extension method.
		/// <summary>
		/// Returns the index of an item in a sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence containing elements.</param>
		/// <param name="item">The item to locate.</param>
		/// <param name="itemComparer">The item equality comparer to use.  Pass null to use the default comparer.</param>
		/// <returns>The index of the entry if it was found in the sequence; otherwise, -1.</returns>
		public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> itemComparer) {
			if (source == null) {
				throw new ArgumentNullException("source");
			}

			int i = 0;
			foreach (T possibleItem in source) {
				if (itemComparer.Equals(item, possibleItem)) {
					return i;
				}
				i++;
			}
			return -1;
		}

		public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item, Func<TSource, TSource, bool> itemComparer) {
			return source.IndexOf(item, new ActionEqualityComparer<TSource>(itemComparer));
		}

		public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
			if (source == null) {
				throw new ArgumentNullException("source");
			}

			int i = 0;
			foreach (TSource possibleItem in source) {
				if (predicate(possibleItem)) {
					return i;
				}
				i++;
			}
			return -1;
		}

	    public static T RandomElement<T>(this IEnumerable<T> sequence) {
	        if (!sequence.Any())
	            return default(T);

	        return sequence.ElementAt(Tools.Maths.RandomNumberGenerator.Next(0, sequence.Count()));
	    }

		#region AsHierarchy Extensions

		// Stefan Cruysberghs, July 2008, http://www.scip.be
		// <summary>
		// AsHierarchy extension methods for LINQ to Objects IEnumerable
		// </summary>



		/// <summary>
		/// LINQ to Objects (IEnumerable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity">Entity class</typeparam>
		/// <typeparam name="TProperty">Property of entity class</typeparam>
		/// <param name="allItems">Flat collection of entities</param>
		/// <param name="idProperty">Func delegete to Id/Key of entity</param>
		/// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
		/// <returns>Hierarchical structure of entities</returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		  this IEnumerable<TEntity> allItems,
		  Func<TEntity, TProperty> idProperty,
		  Func<TEntity, TProperty> parentIdProperty) where TEntity : class {
			return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, null, 0, 0);
		}

		/// <summary>
		/// LINQ to Objects (IEnumerable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity">Entity class</typeparam>
		/// <typeparam name="TProperty">Property of entity class</typeparam>
		/// <param name="allItems">Flat collection of entities</param>
		/// <param name="idProperty">Func delegete to Id/Key of entity</param>
		/// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
		/// <param name="rootItemId">Value of root item Id/Key</param>
		/// <returns>Hierarchical structure of entities</returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		  this IEnumerable<TEntity> allItems,
		  Func<TEntity, TProperty> idProperty,
		  Func<TEntity, TProperty> parentIdProperty,
		  object rootItemId) where TEntity : class {
			return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, 0, 0);
		}

		/// <summary>
		/// LINQ to Objects (IEnumerable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity">Entity class</typeparam>
		/// <typeparam name="TProperty">Property of entity class</typeparam>
		/// <param name="allItems">Flat collection of entities</param>
		/// <param name="idProperty">Func delegete to Id/Key of entity</param>
		/// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
		/// <param name="rootItemId">Value of root item Id/Key</param>
		/// <param name="maxDepth">Maximum depth of tree</param>
		/// <returns>Hierarchical structure of entities</returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		  this IEnumerable<TEntity> allItems,
		  Func<TEntity, TProperty> idProperty,
		  Func<TEntity, TProperty> parentIdProperty,
		  object rootItemId,
		  int maxDepth) where TEntity : class {
			return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, maxDepth, 0);
		}


		private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity, TProperty>(
			IEnumerable<TEntity> allItems,
			TEntity parentItem,
			Func<TEntity, TProperty> idProperty,
			Func<TEntity, TProperty> parentIdProperty,
			object rootItemId,
			int maxDepth,
			int depth) where TEntity : class {
			IEnumerable<TEntity> childs;

			if (rootItemId != null) {
				childs = allItems.Where(i => idProperty(i).Equals(rootItemId));
			} else {
				if (parentItem == null) {
					childs = allItems.Where(i => parentIdProperty(i).Equals(default(TProperty)));
				} else {
					childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));
				}
			}

			if (childs.Count() > 0) {
				depth++;

				if ((depth <= maxDepth) || (maxDepth == 0)) {
					foreach (var item in childs)
						yield return
						  new HierarchyNode<TEntity>() {
							  Entity = item,
							  ChildNodes =
								CreateHierarchy(allItems.AsEnumerable(), item, idProperty, parentIdProperty, null, maxDepth, depth),
							  Depth = depth,
							  Parent = parentItem
						  };
				}
			}
		}
		#endregion
	}
}
