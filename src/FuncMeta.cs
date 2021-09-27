using System;
namespace DianaScript
{

    public class SourceLocTree
    {
        public SourceLocTree[] children;
        public int lineno;
        public int col_offset;
    }
    // we extract this part from a function, and call it function meta
    // a function meta cannot be shared with other functions, while
    // the instructions can.
    public struct FuncMeta
    {
        public bool is_varg;
        public int narg; // include the varg
        public int nlocal; // and exception slots at the end
                    // and slots temporary vars after local var slots
        // local vars | temporary vars(stack) | exception slots
        public int[] freeslots; // from last callstack
        public InternString name;
        
        public InternString modname;
        public string filename;
        public SourceLocTree sourceLocTree;



    }
}