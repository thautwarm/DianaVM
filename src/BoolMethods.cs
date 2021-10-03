namespace DianaScript
{
    public partial class DBool
    {

        public bool __eq__(DObj o)
        {
            return (o is DBool b) && b.value == value;
        }

        public bool __ne__(DObj o)
        {
            return !__eq__(o);
        }
    }
}