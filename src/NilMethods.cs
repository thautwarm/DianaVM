using System;
namespace DianaScript
{
    public partial class DNil
    {

        public bool __bool__()
        {
            return false;
        }

        public bool __not__()
        {
            return true;

        }

        public bool __eq__(DObj o)
        {
            return Object.ReferenceEquals(this, o);
            
        }

        public bool __ne__(DObj o)
        {
            return !Object.ReferenceEquals(this, o);
            
        }
    }
}