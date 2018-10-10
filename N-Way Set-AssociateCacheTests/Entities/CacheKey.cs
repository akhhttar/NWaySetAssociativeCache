namespace NWaySetAssociativeCache.Tests
{
    public class CacheKey
    {
        private readonly int? _hashCodeToOverride;
        
        public CacheKey(string key, int? hashCodeToOverride)
        {
            _hashCodeToOverride = hashCodeToOverride;
            this.Key = key;
        }

        public CacheKey(string key) : this(key, null)
        {
        }

        public string Key { set; get; }

        public override int GetHashCode()
        {

            return _hashCodeToOverride.GetValueOrDefault(base.GetHashCode());
        }
    }
}