boxedtypes {
    DObj
    DArray
    DList
    DSet
    DDict
    DInt
    DStr
    DFloat
    DBool
    DNil
    DTuple
    DCode
    DFunc
    DWrap
}

GlobalNamespace GlobalNamespace GlobalNamespace{
    void Print(*DObj) as print
    int Time() as time
    void Assert(bool, string?) as assert
}

NoneType DNil DNil{}

builtin_function DBuiltinFunc Func<Args, DObj>{}

function DFunc DFunc{ }

cell DRef DRef{ }

dictcell DRefGlobal DRefGlobal{ }

// name in script language, name in .net, .net wrap type name
"using System.Collections.Generic;"
dict DDict Dictionary<DObj, DObj> {
    bool this.ContainsKey(DObj) as __contains__
    int this.Count as __len__
    void this.Remove(DObj) as __delitem__
    void this.Clear() as clear
    bool this.TryGetValue(DObj, out DObj) as search
    this[DObj] = DObj as __setitem__
    DObj this[DObj] as __getitem__
}

str DStr String {
    String Join(String, String[] from DObj)  as join
    String Concat(String[] from DObj)  as concat
    bool this.EndsWith(String) as endswith
    bool this.StartsWith(String) as startswith
    int this.Length as __len__
    String this.Trim(Char[] from String) as strip
    String this.TrimEnd(Char[]? from String) as rstrip
    String this.TrimStart(Char[]? from String) as lstrip
    String this.ToLowerInvariant() as lower
    String this.ToUpperInvariant() as upper
    bool this.Contains(String) as __contains__
    String String.Format(String, *Object) as format
    String this.Substring(Int32, Int32?) as substr
    String this.Insert(Int32, String) as insert
    String this.Remove(Int32, Int32?) as remove
    int this.IndexOf(String, Int32?, Int32?) as index
}

int DInt Int32 {
    Int32 Parse(String) as parse
    // need refer type in compiler side
    bool TryParse(String, out Int32) as try_parse
    Int32 MaxValue as max
    Int32 MinValue as min
}

float DFloat Single {
    Single Parse(String) as parse
    bool TryParse(String, out Single) as try_parse
}

bool DBool Boolean {
    Boolean Parse(String) as parse
    bool TryParse(String, out Boolean) as try_parse
}

tuple DTuple DTuple {
    int this.Length as __len__
    bool this.__contains__(DObj) as __contains__
    bool this.__eq__(DTuple) as __eq__
    bool this.__add__(DTuple) as __add__
}

"using System.Collections.Generic;"
array DArray DArray {
    int this.Count as __len__
}

"using System.Collections.Generic;"
list DList List<DObj> {
    bool this.Contains(DObj) as __contains__
    void this.Add(DObj) as append
    void this.AddRange(IEnumerable<DObj> from DObj) as extend
    void this.Insert(Int32, DObj) as insert
    void this.Remove(DObj) as remove
    void this.Find(Predicate<DObj>) as find
    void this.IndexOf(DObj, Int32?) as index
    void this.RemoveAt(Int32) as __delitem__
    void this.Sort() as sort
    DObj[] this.ToArray() as array
    int this.Count as __len__
    void this.Clear() as clear
    this[int] = DObj as __setitem__
    DObj this[int] as __getitem__
}


"using System.Collections.Generic;"
set DSet HashSet<DObj> {
    void this.UnionWith(IEnumerable<DObj>) as __or__
    void this.IntersectWith(IEnumerable<DObj>) as __and__
    bool this.IsSubsetOf(IEnumerable<DObj>) as subset
    bool this.IsSupersetOf(IEnumerable<DObj>) as superset
    bool this.Contains(DObj) as __contains__
    bool this.Add(DObj) as add
    bool this.Remove(DObj) as remove
    int this.Count as __len__
    void this.Clear() as clear
}

// override Equals according to "==" 
// override ToString according to "__str__"
// 


// __add__ __add__ __sub__ - __mul__  * __div__ /
// __mod__ % __and__ __and__ __or__ __or__ __xor__ ^
// __inv__ ~ __lshift__ << __rshift__ >>
// ==  !=

// int mod(int x, int m) {
//    int r = x%m;
//    return r<0 ? r__add__m : r;
// }
