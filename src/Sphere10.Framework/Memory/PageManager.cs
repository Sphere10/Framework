//-----------------------------------------------------------------------
// <copyright file="PageManager.cs" company="Sphere 10 Software">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphere10.Framework {
    public class PageManager<T> : IDisposable {
        public event EventHandlerEx<PageManager<T>, Page<T>> PageLoaded;
        public event EventHandlerEx<PageManager<T>, Page<T>> PageUnloaded;
        private readonly Func<T, int> _sizeEstimator;
        private readonly SynchronizedList<Page<T>> _pageHeaders;
        private readonly ICache<Page<T>, Page<T>> _loadedPages;

        private readonly int _pageSize;

        public PageManager(int pageSize) : this(pageSize, UnitSizeEstimator) {
        }

        public PageManager(int pageSize, Func<T, int> sizeEstimator) : this(pageSize, 1, sizeEstimator) {
        }

        public PageManager(int pageSize, int maxOpenPages, Func<T, int> sizeEstimator) {
            _sizeEstimator = sizeEstimator;
            _pageSize = pageSize;
            _pageHeaders = new SynchronizedList<Page<T>>();
            _loadedPages = new ActionCache<Page<T>, Page<T>>(
                p => {
                    if (!p.Loaded) {
                        p.Load();                        
                    }
                    if (p.PageNumber == 0 && p.StartIndex == 0 && p.EndIndex == -1)
                        return p;
                    NotifyPageLoaded(p);
                    return p;
                },
                (p) => 1,
                CacheReapPolicy.LeastUsed,
                ExpirationPolicy.SinceLastAccessedTime,
                (uint)maxOpenPages
            );
            _loadedPages.ItemRemoved += (page, item) => {
                page.Save();
                page.Unload();
                NotifyPageUnloaded(page);
            };
            Clear();
        }

        public int PageCount {
            get { return _pageHeaders.Count; }
        }

        public IEnumerable<Page<T>> Pages {
            get { return _pageHeaders.Select(x => x); }
        }

        public int Count {
            get {
                var lastPage = _pageHeaders[_pageHeaders.Count - 1];
                return lastPage.StartIndex + lastPage.Count;
            }
        }

        public virtual void Add(T item) {
            using (_pageHeaders.EnterWriteScope()) {
                var lastPage = _pageHeaders.Last();
                var itemSize = _sizeEstimator(item);
                if (lastPage.Size + itemSize > _pageSize) {
                    lastPage = AddPage();
                }
                var dataStore = _loadedPages[lastPage];
                dataStore.AddItem(item, itemSize);
            }
        }

        public virtual T Get(int index) {
            using (_pageHeaders.EnterReadScope()) {
                var firstPage = _pageHeaders[0];
                var lastPage = _pageHeaders[_pageHeaders.Count - 1];
                if (index < firstPage.StartIndex || index > lastPage.EndIndex) {
                    throw new SoftwareException("Index out of bounds. Requested index {0} total elements was {1}", index, lastPage.EndIndex);
                }
                var pageIndex = _pageHeaders.BinarySearch(index, (x, p) => {
                    if (index < p.StartIndex)
                        return -1;
                    if (index > p.EndIndex)
                        return +1;
                    return 0;
                });

                var pageHeader = _pageHeaders[pageIndex];
                var loadedPage = _loadedPages[pageHeader];
                return loadedPage.GetItem(index);
            }
        }

        public void Clear() {
            foreach (var page in _pageHeaders) {
                page.Dispose();
            }
            _pageHeaders.Clear();
            AddPage();
        }

        protected virtual Page<T> AddPage() {
            using (_pageHeaders.EnterWriteScope()) {
                Page<T> newPage;
                if (!_pageHeaders.Any()) {
                    newPage = Page<T>.First;
                } else {
                    var lastPage = _pageHeaders.Last();
                    newPage = new Page<T>(lastPage.PageNumber + 1, lastPage.EndIndex + 1);
                }
                _pageHeaders.Add(newPage);
                return newPage;
            }
        }

        protected virtual void OnPageLoaded(Page<T> page) {            
        }

        protected virtual void OnPageUnloaded(Page<T> page) {
        }

        public void Dispose() {
            foreach (var page in _pageHeaders) {
                page.Dispose();
            }
        }

        private void NotifyPageLoaded(Page<T> page) {
            OnPageLoaded(page);
            if (PageLoaded != null) {
                PageLoaded(this, page);
            }
        }

        private void NotifyPageUnloaded(Page<T> page) {
            OnPageUnloaded(page);
            if (PageUnloaded != null) {
                PageUnloaded(this, page);
            }
        }

        private static int UnitSizeEstimator(T item) {
            return 1;
        }

    }
}
