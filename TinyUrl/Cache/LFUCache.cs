using Microsoft.Extensions.Options;

namespace TinyUrl.Cache
{
    public class LFUCache : ICache
    {
        private int _minCount;
        private readonly int _capacity;


        private readonly Dictionary<int, LinkedList<Uri>> _countMap;

        public Dictionary<Uri, (LinkedListNode<Uri> node, Uri value, int count)> _cache { get; private set; }

        public LFUCache(IOptions<CacheSettings> CacheSettings)
        {
            _capacity = CacheSettings.Value.MaxCacheSize ?? throw new ArgumentNullException(nameof(CacheSettings.Value.MaxCacheSize));
            
            _countMap = new Dictionary<int, LinkedList<Uri>> 
            { 
                [1] = new() 
            };

            _cache = new Dictionary<Uri, (LinkedListNode<Uri> node, Uri value, int count)>(_capacity);
        }

        public Uri? Get(Uri uriKey)
        {
            if (!_cache.ContainsKey(uriKey))
                return null;

            (LinkedListNode<Uri> node, Uri value, int count) = _cache[uriKey];
            PromoteItem(uriKey, value, count, node);

            return value;
        }

        public void Put(Uri key, Uri value)
        {
            if (_cache.ContainsKey(key))
            {
                (LinkedListNode<Uri> node, Uri _, int count) = _cache[key];
                PromoteItem(key, value, count, node);
            }
            else
            {
                if (_cache.Count >= _capacity)
                {
                    LinkedList<Uri>? minList = _countMap[_minCount];
                    _cache.Remove(minList.Last!.Value);
                    minList.RemoveLast();
                }

                _cache.Add(key, (_countMap[1].AddFirst(key), value, 1));
                _minCount = 1;
            }
        }

        private void PromoteItem(Uri key, Uri value, int count, LinkedListNode<Uri> node)
        {
            LinkedList<Uri>? list = _countMap[count];
            list.Remove(node);

            if (_minCount == count && list.Count == 0)
                _minCount++;

            var newCount = count + 1;
            if (!_countMap.ContainsKey(newCount))
                _countMap[newCount] = new LinkedList<Uri>();

            _countMap[newCount].AddFirst(node);
            _cache[key] = (node, value, newCount);
        }
    }
}
