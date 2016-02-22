using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.CacheManager.Purge
{



    public class LFUPurgePolicy<TKey> : PurgePolicyBase<TKey> 
        where TKey : IComparable
    {


        private class LFUNode<TLey> {
            public TKey Key;
            public int AccessCount;
        }

        // TODO MAKE THE LFU AGED
        private int maxSize = 5000;
        private int mintTime = 180;
        private List<LFUNode<TKey>> lfus = null;

        public LFUPurgePolicy()
            : this(5000, 180)
        {

        }

        public LFUPurgePolicy(int maxSize)
            : this(maxSize, 180)
        {

        }

        public LFUPurgePolicy(int maxSize, int mintTime)
        {
            this.maxSize = maxSize;
            this.mintTime = mintTime;
            this.lfus = new List<LFUNode<TKey>>();
        }

        public override void ClearAll()
        {
            lfus.Clear();
        }

        public override bool Store(TKey key, out TKey toRemove)
        {
            toRemove = default(TKey);
            if (lfus.Count > 0)
                toRemove = lfus[lfus.Count - 1].Key;

            LFUNode<TKey> node = new LFUNode<TKey>()
            {
                Key         = key,
                AccessCount = 2
            };
            lfus.Add(node);

            Sort();

            return (lfus.Count >= maxSize);
        }

        public override void Remove(TKey key)
        {
            lfus.RemoveAll(l => l.Key.Equals(key));
        }

        public override void Hit(TKey key)
        {
            LFUNode<TKey> lfuItem = lfus.FirstOrDefault(l => l.Key.Equals(key));
            lfuItem.AccessCount += 2;
            Sort();
        }

        private void Sort()
        {
            Task task = new Task( ()=>{
                Parallel.ForEach(lfus, l => { if (l.AccessCount > 0) l.AccessCount--; });
                lfus.Sort((a, b) => b.AccessCount.CompareTo(a.AccessCount));
            });
            task.Start();
        }
        
    }
}
