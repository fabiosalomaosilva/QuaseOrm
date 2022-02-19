namespace QuaseOrm.Utils
{
    public class Criteria
    {
        public Criteria(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }
        public object Value { get; set; }
    }
}