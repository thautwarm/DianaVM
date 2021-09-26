using System;
using System.IO;
using System.Collections.Generic;
namespace DianaScript
{
public class FunctionDef
{
    public int[] frees;

    public int code;

    public static FunctionDef Make(int[] frees, int code) => new FunctionDef
    {
        frees = frees,
        code = code,
    };
}

public class Return
{
    public Ptr<Expr> value;

    public static Return Make(Ptr<Expr> value) => new Return
    {
        value = value,
    };
}

public class DelGlobalName
{
    public int slot;

    public static DelGlobalName Make(int slot) => new DelGlobalName
    {
        slot = slot,
    };
}

public class DelLocalName
{
    public int slot;

    public static DelLocalName Make(int slot) => new DelLocalName
    {
        slot = slot,
    };
}

public class DelDerefName
{
    public int slot;

    public static DelDerefName Make(int slot) => new DelDerefName
    {
        slot = slot,
    };
}

public class DeleteItem
{
    public Ptr<Expr> value;

    public Ptr<Expr> item;

    public static DeleteItem Make(Ptr<Expr> value, Ptr<Expr> item) => new DeleteItem
    {
        value = value,
        item = item,
    };
}

public class Assign
{
    public Ptr<Expr> targets;

    public Ptr<Expr> value;

    public static Assign Make(Ptr<Expr> targets, Ptr<Expr> value) => new Assign
    {
        targets = targets,
        value = value,
    };
}

public class AddAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static AddAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new AddAssign
    {
        target = target,
        value = value,
    };
}

public class SubAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static SubAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new SubAssign
    {
        target = target,
        value = value,
    };
}

public class MutAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static MutAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new MutAssign
    {
        target = target,
        value = value,
    };
}

public class TrueDivAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static TrueDivAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new TrueDivAssign
    {
        target = target,
        value = value,
    };
}

public class FloorDivAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static FloorDivAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new FloorDivAssign
    {
        target = target,
        value = value,
    };
}

public class ModAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static ModAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new ModAssign
    {
        target = target,
        value = value,
    };
}

public class PowAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static PowAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new PowAssign
    {
        target = target,
        value = value,
    };
}

public class LShiftAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static LShiftAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new LShiftAssign
    {
        target = target,
        value = value,
    };
}

public class RShiftAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static RShiftAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new RShiftAssign
    {
        target = target,
        value = value,
    };
}

public class BitOrAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static BitOrAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new BitOrAssign
    {
        target = target,
        value = value,
    };
}

public class BitAndAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static BitAndAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new BitAndAssign
    {
        target = target,
        value = value,
    };
}

public class BitXorAssign
{
    public Ptr<Expr> target;

    public Ptr<Expr> value;

    public static BitXorAssign Make(Ptr<Expr> target, Ptr<Expr> value) => new BitXorAssign
    {
        target = target,
        value = value,
    };
}

public class For
{
    public Ptr<Expr> target;

    public Ptr<Expr> iter;

    public Ptr<Stmt>[] body;

    public static For Make(Ptr<Expr> target, Ptr<Expr> iter, Ptr<Stmt>[] body) => new For
    {
        target = target,
        iter = iter,
        body = body,
    };
}

public class While
{
    public Ptr<Expr> cond;

    public Ptr<Stmt>[] body;

    public static While Make(Ptr<Expr> cond, Ptr<Stmt>[] body) => new While
    {
        cond = cond,
        body = body,
    };
}

public class If
{
    public Ptr<Expr> cond;

    public Ptr<Stmt>[] then;

    public Ptr<Stmt>[] orelse;

    public static If Make(Ptr<Expr> cond, Ptr<Stmt>[] then, Ptr<Stmt>[] orelse) => new If
    {
        cond = cond,
        then = then,
        orelse = orelse,
    };
}

public class With
{
    public Ptr<Expr> expr;

    public Ptr<Stmt>[] body;

    public static With Make(Ptr<Expr> expr, Ptr<Stmt>[] body) => new With
    {
        expr = expr,
        body = body,
    };
}

public class Raise
{
    public Ptr<Expr> value;

    public Ptr<Expr> from;

    public static Raise Make(Ptr<Expr> value, Ptr<Expr> from) => new Raise
    {
        value = value,
        from = from,
    };
}

public class Try
{
    public Ptr<Stmt>[] body;

    public Ptr<ExceptHandler>[] except_handlers;

    public Ptr<Stmt>[] final_body;

    public static Try Make(Ptr<Stmt>[] body, Ptr<ExceptHandler>[] except_handlers, Ptr<Stmt>[] final_body) => new Try
    {
        body = body,
        except_handlers = except_handlers,
        final_body = final_body,
    };
}

public class Assert
{
    public Ptr<Expr> value;

    public static Assert Make(Ptr<Expr> value) => new Assert
    {
        value = value,
    };
}

public class ExprStmt
{
    public Ptr<Expr> value;

    public static ExprStmt Make(Ptr<Expr> value) => new ExprStmt
    {
        value = value,
    };
}

public class Control
{
    public int kind;

    public static Control Make(int kind) => new Control
    {
        kind = kind,
    };
}

public class AddOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static AddOp Make(Ptr<Expr> left, Ptr<Expr> right) => new AddOp
    {
        left = left,
        right = right,
    };
}

public class SubOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static SubOp Make(Ptr<Expr> left, Ptr<Expr> right) => new SubOp
    {
        left = left,
        right = right,
    };
}

public class MutOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static MutOp Make(Ptr<Expr> left, Ptr<Expr> right) => new MutOp
    {
        left = left,
        right = right,
    };
}

public class TrueDivOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static TrueDivOp Make(Ptr<Expr> left, Ptr<Expr> right) => new TrueDivOp
    {
        left = left,
        right = right,
    };
}

public class FloorDivOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static FloorDivOp Make(Ptr<Expr> left, Ptr<Expr> right) => new FloorDivOp
    {
        left = left,
        right = right,
    };
}

public class ModOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static ModOp Make(Ptr<Expr> left, Ptr<Expr> right) => new ModOp
    {
        left = left,
        right = right,
    };
}

public class PowOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static PowOp Make(Ptr<Expr> left, Ptr<Expr> right) => new PowOp
    {
        left = left,
        right = right,
    };
}

public class LShiftOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static LShiftOp Make(Ptr<Expr> left, Ptr<Expr> right) => new LShiftOp
    {
        left = left,
        right = right,
    };
}

public class RShiftOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static RShiftOp Make(Ptr<Expr> left, Ptr<Expr> right) => new RShiftOp
    {
        left = left,
        right = right,
    };
}

public class BitOrOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static BitOrOp Make(Ptr<Expr> left, Ptr<Expr> right) => new BitOrOp
    {
        left = left,
        right = right,
    };
}

public class BitAndOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static BitAndOp Make(Ptr<Expr> left, Ptr<Expr> right) => new BitAndOp
    {
        left = left,
        right = right,
    };
}

public class BitXorOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static BitXorOp Make(Ptr<Expr> left, Ptr<Expr> right) => new BitXorOp
    {
        left = left,
        right = right,
    };
}

public class GlobalBinder
{
    public int slot;

    public static GlobalBinder Make(int slot) => new GlobalBinder
    {
        slot = slot,
    };
}

public class LocalBinder
{
    public int slot;

    public static LocalBinder Make(int slot) => new LocalBinder
    {
        slot = slot,
    };
}

public class DerefBinder
{
    public int slot;

    public static DerefBinder Make(int slot) => new DerefBinder
    {
        slot = slot,
    };
}

public class AndOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static AndOp Make(Ptr<Expr> left, Ptr<Expr> right) => new AndOp
    {
        left = left,
        right = right,
    };
}

public class OrOp
{
    public Ptr<Expr> left;

    public Ptr<Expr> right;

    public static OrOp Make(Ptr<Expr> left, Ptr<Expr> right) => new OrOp
    {
        left = left,
        right = right,
    };
}

public class InvertOp
{
    public Ptr<Expr> value;

    public static InvertOp Make(Ptr<Expr> value) => new InvertOp
    {
        value = value,
    };
}

public class NotOp
{
    public Ptr<Expr> value;

    public static NotOp Make(Ptr<Expr> value) => new NotOp
    {
        value = value,
    };
}

public class Lambda
{
    public int[] frees;

    public int code;

    public static Lambda Make(int[] frees, int code) => new Lambda
    {
        frees = frees,
        code = code,
    };
}

public class IfExpr
{
    public Ptr<Expr> cond;

    public Ptr<Stmt>[] then;

    public Ptr<Stmt>[] orelse;

    public static IfExpr Make(Ptr<Expr> cond, Ptr<Stmt>[] then, Ptr<Stmt>[] orelse) => new IfExpr
    {
        cond = cond,
        then = then,
        orelse = orelse,
    };
}

public class Dict
{
    public Ptr<Expr>[] keys;

    public Ptr<Expr>[] values;

    public static Dict Make(Ptr<Expr>[] keys, Ptr<Expr>[] values) => new Dict
    {
        keys = keys,
        values = values,
    };
}

public class Set
{
    public Ptr<Expr>[] elts;

    public static Set Make(Ptr<Expr>[] elts) => new Set
    {
        elts = elts,
    };
}

public class List
{
    public Ptr<Expr>[] elts;

    public static List Make(Ptr<Expr>[] elts) => new List
    {
        elts = elts,
    };
}

public class Generator
{
    public Ptr<Expr> target;

    public Ptr<Expr> iter;

    public Ptr<Expr> body;

    public static Generator Make(Ptr<Expr> target, Ptr<Expr> iter, Ptr<Expr> body) => new Generator
    {
        target = target,
        iter = iter,
        body = body,
    };
}

public class Comprehension
{
    public Ptr<Expr> adder;

    public Ptr<Expr> target;

    public Ptr<Expr> iter;

    public Ptr<Expr> body;

    public static Comprehension Make(Ptr<Expr> adder, Ptr<Expr> target, Ptr<Expr> iter, Ptr<Expr> body) => new Comprehension
    {
        adder = adder,
        target = target,
        iter = iter,
        body = body,
    };
}

public class Call
{
    public Ptr<Expr> f;

    public Ptr<Arg> args;

    public static Call Make(Ptr<Expr> f, Ptr<Arg> args) => new Call
    {
        f = f,
        args = args,
    };
}

public class Format
{
    public int format;

    public Ptr<Expr> args;

    public static Format Make(int format, Ptr<Expr> args) => new Format
    {
        format = format,
        args = args,
    };
}

public class Const
{
    public int constInd;

    public static Const Make(int constInd) => new Const
    {
        constInd = constInd,
    };
}

public class Attr
{
    public Ptr<Expr> value;

    public int attr;

    public static Attr Make(Ptr<Expr> value, int attr) => new Attr
    {
        value = value,
        attr = attr,
    };
}

public class GlobalName
{
    public int slot;

    public static GlobalName Make(int slot) => new GlobalName
    {
        slot = slot,
    };
}

public class LocalName
{
    public int slot;

    public static LocalName Make(int slot) => new LocalName
    {
        slot = slot,
    };
}

public class DerefName
{
    public int slot;

    public static DerefName Make(int slot) => new DerefName
    {
        slot = slot,
    };
}

public class Item
{
    public Ptr<Expr> value;

    public Ptr<Expr> item;

    public static Item Make(Ptr<Expr> value, Ptr<Expr> item) => new Item
    {
        value = value,
        item = item,
    };
}

public class Tuple
{
    public Ptr<Expr>[] elts;

    public static Tuple Make(Ptr<Expr>[] elts) => new Tuple
    {
        elts = elts,
    };
}

public class GlobalNameOut
{
    public int ind;

    public static GlobalNameOut Make(int ind) => new GlobalNameOut
    {
        ind = ind,
    };
}

public class LocalNameOut
{
    public int ind;

    public static LocalNameOut Make(int ind) => new LocalNameOut
    {
        ind = ind,
    };
}

public class DerefNameOut
{
    public int ind;

    public static DerefNameOut Make(int ind) => new DerefNameOut
    {
        ind = ind,
    };
}

public class ItemOut
{
    public Ptr<Expr> value;

    public Ptr<Expr> item;

    public static ItemOut Make(Ptr<Expr> value, Ptr<Expr> item) => new ItemOut
    {
        value = value,
        item = item,
    };
}

public class AttrOut
{
    public Ptr<Expr> value;

    public int attr;

    public static AttrOut Make(Ptr<Expr> value, int attr) => new AttrOut
    {
        value = value,
        attr = attr,
    };
}

public class Val
{
    public Ptr<Expr> value;

    public static Val Make(Ptr<Expr> value) => new Val
    {
        value = value,
    };
}

public class ArbitraryCatch
{
    public int assign_slot;

    public Ptr<Stmt>[] body;

    public static ArbitraryCatch Make(int assign_slot, Ptr<Stmt>[] body) => new ArbitraryCatch
    {
        assign_slot = assign_slot,
        body = body,
    };
}

public class TypeCheckCatch
{
    public Ptr<Expr> type;

    public int assign_slot;

    public Ptr<Stmt>[] body;

    public static TypeCheckCatch Make(Ptr<Expr> type, int assign_slot, Ptr<Stmt>[] body) => new TypeCheckCatch
    {
        type = type,
        assign_slot = assign_slot,
        body = body,
    };
}

public enum Stmt: int
{
    FunctionDef,
    Return,
    DelGlobalName,
    DelLocalName,
    DelDerefName,
    DeleteItem,
    Assign,
    AddAssign,
    SubAssign,
    MutAssign,
    TrueDivAssign,
    FloorDivAssign,
    ModAssign,
    PowAssign,
    LShiftAssign,
    RShiftAssign,
    BitOrAssign,
    BitAndAssign,
    BitXorAssign,
    For,
    While,
    If,
    With,
    Raise,
    Try,
    Assert,
    ExprStmt,
    Control,
}

public enum Expr: int
{
    AddOp,
    SubOp,
    MutOp,
    TrueDivOp,
    FloorDivOp,
    ModOp,
    PowOp,
    LShiftOp,
    RShiftOp,
    BitOrOp,
    BitAndOp,
    BitXorOp,
    GlobalBinder,
    LocalBinder,
    DerefBinder,
    AndOp,
    OrOp,
    InvertOp,
    NotOp,
    Lambda,
    IfExpr,
    Dict,
    Set,
    List,
    Generator,
    Comprehension,
    Call,
    Format,
    Const,
    Attr,
    GlobalName,
    LocalName,
    DerefName,
    Item,
    Tuple,
}

public enum Arg: int
{
    GlobalNameOut,
    LocalNameOut,
    DerefNameOut,
    ItemOut,
    AttrOut,
    Val,
}

public enum ExceptHandler: int
{
    ArbitraryCatch,
    TypeCheckCatch,
}

public partial class DFlatGraphCode
{
    public FunctionDef[] functiondefs;

    public Return[] returns;

    public DelGlobalName[] delglobalnames;

    public DelLocalName[] dellocalnames;

    public DelDerefName[] delderefnames;

    public DeleteItem[] deleteitems;

    public Assign[] assigns;

    public AddAssign[] addassigns;

    public SubAssign[] subassigns;

    public MutAssign[] mutassigns;

    public TrueDivAssign[] truedivassigns;

    public FloorDivAssign[] floordivassigns;

    public ModAssign[] modassigns;

    public PowAssign[] powassigns;

    public LShiftAssign[] lshiftassigns;

    public RShiftAssign[] rshiftassigns;

    public BitOrAssign[] bitorassigns;

    public BitAndAssign[] bitandassigns;

    public BitXorAssign[] bitxorassigns;

    public For[] fors;

    public While[] whiles;

    public If[] ifs;

    public With[] withs;

    public Raise[] raises;

    public Try[] trys;

    public Assert[] asserts;

    public ExprStmt[] exprstmts;

    public Control[] controls;

    public AddOp[] addops;

    public SubOp[] subops;

    public MutOp[] mutops;

    public TrueDivOp[] truedivops;

    public FloorDivOp[] floordivops;

    public ModOp[] modops;

    public PowOp[] powops;

    public LShiftOp[] lshiftops;

    public RShiftOp[] rshiftops;

    public BitOrOp[] bitorops;

    public BitAndOp[] bitandops;

    public BitXorOp[] bitxorops;

    public GlobalBinder[] globalbinders;

    public LocalBinder[] localbinders;

    public DerefBinder[] derefbinders;

    public AndOp[] andops;

    public OrOp[] orops;

    public InvertOp[] invertops;

    public NotOp[] notops;

    public Lambda[] lambdas;

    public IfExpr[] ifexprs;

    public Dict[] dicts;

    public Set[] sets;

    public List[] lists;

    public Generator[] generators;

    public Comprehension[] comprehensions;

    public Call[] calls;

    public Format[] formats;

    public Const[] consts;

    public Attr[] attrs;

    public GlobalName[] globalnames;

    public LocalName[] localnames;

    public DerefName[] derefnames;

    public Item[] items;

    public Tuple[] tuples;

    public GlobalNameOut[] globalnameouts;

    public LocalNameOut[] localnameouts;

    public DerefNameOut[] derefnameouts;

    public ItemOut[] itemouts;

    public AttrOut[] attrouts;

    public Val[] vals;

    public ArbitraryCatch[] arbitrarycatchs;

    public TypeCheckCatch[] typecheckcatchs;

}
public partial class AIRParser
{
    public Stmt ReadStmtTag()
    {
        fileStream.Read(cache_4byte, 0, 1);
        return (Stmt)cache_4byte[0];
    }

    public Ptr<Stmt> Read(THint<Ptr<Stmt>> _)
    {
        var tag = ReadStmtTag();
        switch(tag)
        {
            case Stmt.FunctionDef: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.Return: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.DelGlobalName: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.DelLocalName: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.DelDerefName: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.DeleteItem: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.Assign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.AddAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.SubAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.MutAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.TrueDivAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.FloorDivAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.ModAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.PowAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.LShiftAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.RShiftAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.BitOrAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.BitAndAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.BitXorAssign: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.For: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.While: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.If: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.With: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.Raise: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.Try: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.Assert: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.ExprStmt: return new Ptr<Stmt>(tag, ReadInt());
            case Stmt.Control: return new Ptr<Stmt>(tag, ReadInt());
            default: throw new InvalidDataException($"invalid tag {tag} for Stmt.");
        }
    }
    public Expr ReadExprTag()
    {
        fileStream.Read(cache_4byte, 0, 1);
        return (Expr)cache_4byte[0];
    }

    public Ptr<Expr> Read(THint<Ptr<Expr>> _)
    {
        var tag = ReadExprTag();
        switch(tag)
        {
            case Expr.AddOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.SubOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.MutOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.TrueDivOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.FloorDivOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.ModOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.PowOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.LShiftOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.RShiftOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.BitOrOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.BitAndOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.BitXorOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.GlobalBinder: return new Ptr<Expr>(tag, ReadInt());
            case Expr.LocalBinder: return new Ptr<Expr>(tag, ReadInt());
            case Expr.DerefBinder: return new Ptr<Expr>(tag, ReadInt());
            case Expr.AndOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.OrOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.InvertOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.NotOp: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Lambda: return new Ptr<Expr>(tag, ReadInt());
            case Expr.IfExpr: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Dict: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Set: return new Ptr<Expr>(tag, ReadInt());
            case Expr.List: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Generator: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Comprehension: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Call: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Format: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Const: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Attr: return new Ptr<Expr>(tag, ReadInt());
            case Expr.GlobalName: return new Ptr<Expr>(tag, ReadInt());
            case Expr.LocalName: return new Ptr<Expr>(tag, ReadInt());
            case Expr.DerefName: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Item: return new Ptr<Expr>(tag, ReadInt());
            case Expr.Tuple: return new Ptr<Expr>(tag, ReadInt());
            default: throw new InvalidDataException($"invalid tag {tag} for Expr.");
        }
    }
    public Arg ReadArgTag()
    {
        fileStream.Read(cache_4byte, 0, 1);
        return (Arg)cache_4byte[0];
    }

    public Ptr<Arg> Read(THint<Ptr<Arg>> _)
    {
        var tag = ReadArgTag();
        switch(tag)
        {
            case Arg.GlobalNameOut: return new Ptr<Arg>(tag, ReadInt());
            case Arg.LocalNameOut: return new Ptr<Arg>(tag, ReadInt());
            case Arg.DerefNameOut: return new Ptr<Arg>(tag, ReadInt());
            case Arg.ItemOut: return new Ptr<Arg>(tag, ReadInt());
            case Arg.AttrOut: return new Ptr<Arg>(tag, ReadInt());
            case Arg.Val: return new Ptr<Arg>(tag, ReadInt());
            default: throw new InvalidDataException($"invalid tag {tag} for Arg.");
        }
    }
    public ExceptHandler ReadExceptHandlerTag()
    {
        fileStream.Read(cache_4byte, 0, 1);
        return (ExceptHandler)cache_4byte[0];
    }

    public Ptr<ExceptHandler> Read(THint<Ptr<ExceptHandler>> _)
    {
        var tag = ReadExceptHandlerTag();
        switch(tag)
        {
            case ExceptHandler.ArbitraryCatch: return new Ptr<ExceptHandler>(tag, ReadInt());
            case ExceptHandler.TypeCheckCatch: return new Ptr<ExceptHandler>(tag, ReadInt());
            default: throw new InvalidDataException($"invalid tag {tag} for ExceptHandler.");
        }
    }
    public FunctionDef Read(THint<FunctionDef> _) => new FunctionDef
    {
        frees = Read(THint<int[]>.val),
        code = Read(THint<int>.val),
    };

    public Return Read(THint<Return> _) => new Return
    {
        value = Read(THint<Ptr<Expr>>.val),
    };

    public DelGlobalName Read(THint<DelGlobalName> _) => new DelGlobalName
    {
        slot = Read(THint<int>.val),
    };

    public DelLocalName Read(THint<DelLocalName> _) => new DelLocalName
    {
        slot = Read(THint<int>.val),
    };

    public DelDerefName Read(THint<DelDerefName> _) => new DelDerefName
    {
        slot = Read(THint<int>.val),
    };

    public DeleteItem Read(THint<DeleteItem> _) => new DeleteItem
    {
        value = Read(THint<Ptr<Expr>>.val),
        item = Read(THint<Ptr<Expr>>.val),
    };

    public Assign Read(THint<Assign> _) => new Assign
    {
        targets = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public AddAssign Read(THint<AddAssign> _) => new AddAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public SubAssign Read(THint<SubAssign> _) => new SubAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public MutAssign Read(THint<MutAssign> _) => new MutAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public TrueDivAssign Read(THint<TrueDivAssign> _) => new TrueDivAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public FloorDivAssign Read(THint<FloorDivAssign> _) => new FloorDivAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public ModAssign Read(THint<ModAssign> _) => new ModAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public PowAssign Read(THint<PowAssign> _) => new PowAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public LShiftAssign Read(THint<LShiftAssign> _) => new LShiftAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public RShiftAssign Read(THint<RShiftAssign> _) => new RShiftAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public BitOrAssign Read(THint<BitOrAssign> _) => new BitOrAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public BitAndAssign Read(THint<BitAndAssign> _) => new BitAndAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public BitXorAssign Read(THint<BitXorAssign> _) => new BitXorAssign
    {
        target = Read(THint<Ptr<Expr>>.val),
        value = Read(THint<Ptr<Expr>>.val),
    };

    public For Read(THint<For> _) => new For
    {
        target = Read(THint<Ptr<Expr>>.val),
        iter = Read(THint<Ptr<Expr>>.val),
        body = Read(THint<Ptr<Stmt>[]>.val),
    };

    public While Read(THint<While> _) => new While
    {
        cond = Read(THint<Ptr<Expr>>.val),
        body = Read(THint<Ptr<Stmt>[]>.val),
    };

    public If Read(THint<If> _) => new If
    {
        cond = Read(THint<Ptr<Expr>>.val),
        then = Read(THint<Ptr<Stmt>[]>.val),
        orelse = Read(THint<Ptr<Stmt>[]>.val),
    };

    public With Read(THint<With> _) => new With
    {
        expr = Read(THint<Ptr<Expr>>.val),
        body = Read(THint<Ptr<Stmt>[]>.val),
    };

    public Raise Read(THint<Raise> _) => new Raise
    {
        value = Read(THint<Ptr<Expr>>.val),
        from = Read(THint<Ptr<Expr>>.val),
    };

    public Try Read(THint<Try> _) => new Try
    {
        body = Read(THint<Ptr<Stmt>[]>.val),
        except_handlers = Read(THint<Ptr<ExceptHandler>[]>.val),
        final_body = Read(THint<Ptr<Stmt>[]>.val),
    };

    public Assert Read(THint<Assert> _) => new Assert
    {
        value = Read(THint<Ptr<Expr>>.val),
    };

    public ExprStmt Read(THint<ExprStmt> _) => new ExprStmt
    {
        value = Read(THint<Ptr<Expr>>.val),
    };

    public Control Read(THint<Control> _) => new Control
    {
        kind = Read(THint<int>.val),
    };

    public AddOp Read(THint<AddOp> _) => new AddOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public SubOp Read(THint<SubOp> _) => new SubOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public MutOp Read(THint<MutOp> _) => new MutOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public TrueDivOp Read(THint<TrueDivOp> _) => new TrueDivOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public FloorDivOp Read(THint<FloorDivOp> _) => new FloorDivOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public ModOp Read(THint<ModOp> _) => new ModOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public PowOp Read(THint<PowOp> _) => new PowOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public LShiftOp Read(THint<LShiftOp> _) => new LShiftOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public RShiftOp Read(THint<RShiftOp> _) => new RShiftOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public BitOrOp Read(THint<BitOrOp> _) => new BitOrOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public BitAndOp Read(THint<BitAndOp> _) => new BitAndOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public BitXorOp Read(THint<BitXorOp> _) => new BitXorOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public GlobalBinder Read(THint<GlobalBinder> _) => new GlobalBinder
    {
        slot = Read(THint<int>.val),
    };

    public LocalBinder Read(THint<LocalBinder> _) => new LocalBinder
    {
        slot = Read(THint<int>.val),
    };

    public DerefBinder Read(THint<DerefBinder> _) => new DerefBinder
    {
        slot = Read(THint<int>.val),
    };

    public AndOp Read(THint<AndOp> _) => new AndOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public OrOp Read(THint<OrOp> _) => new OrOp
    {
        left = Read(THint<Ptr<Expr>>.val),
        right = Read(THint<Ptr<Expr>>.val),
    };

    public InvertOp Read(THint<InvertOp> _) => new InvertOp
    {
        value = Read(THint<Ptr<Expr>>.val),
    };

    public NotOp Read(THint<NotOp> _) => new NotOp
    {
        value = Read(THint<Ptr<Expr>>.val),
    };

    public Lambda Read(THint<Lambda> _) => new Lambda
    {
        frees = Read(THint<int[]>.val),
        code = Read(THint<int>.val),
    };

    public IfExpr Read(THint<IfExpr> _) => new IfExpr
    {
        cond = Read(THint<Ptr<Expr>>.val),
        then = Read(THint<Ptr<Stmt>[]>.val),
        orelse = Read(THint<Ptr<Stmt>[]>.val),
    };

    public Dict Read(THint<Dict> _) => new Dict
    {
        keys = Read(THint<Ptr<Expr>[]>.val),
        values = Read(THint<Ptr<Expr>[]>.val),
    };

    public Set Read(THint<Set> _) => new Set
    {
        elts = Read(THint<Ptr<Expr>[]>.val),
    };

    public List Read(THint<List> _) => new List
    {
        elts = Read(THint<Ptr<Expr>[]>.val),
    };

    public Generator Read(THint<Generator> _) => new Generator
    {
        target = Read(THint<Ptr<Expr>>.val),
        iter = Read(THint<Ptr<Expr>>.val),
        body = Read(THint<Ptr<Expr>>.val),
    };

    public Comprehension Read(THint<Comprehension> _) => new Comprehension
    {
        adder = Read(THint<Ptr<Expr>>.val),
        target = Read(THint<Ptr<Expr>>.val),
        iter = Read(THint<Ptr<Expr>>.val),
        body = Read(THint<Ptr<Expr>>.val),
    };

    public Call Read(THint<Call> _) => new Call
    {
        f = Read(THint<Ptr<Expr>>.val),
        args = Read(THint<Ptr<Arg>>.val),
    };

    public Format Read(THint<Format> _) => new Format
    {
        format = Read(THint<int>.val),
        args = Read(THint<Ptr<Expr>>.val),
    };

    public Const Read(THint<Const> _) => new Const
    {
        constInd = Read(THint<int>.val),
    };

    public Attr Read(THint<Attr> _) => new Attr
    {
        value = Read(THint<Ptr<Expr>>.val),
        attr = Read(THint<int>.val),
    };

    public GlobalName Read(THint<GlobalName> _) => new GlobalName
    {
        slot = Read(THint<int>.val),
    };

    public LocalName Read(THint<LocalName> _) => new LocalName
    {
        slot = Read(THint<int>.val),
    };

    public DerefName Read(THint<DerefName> _) => new DerefName
    {
        slot = Read(THint<int>.val),
    };

    public Item Read(THint<Item> _) => new Item
    {
        value = Read(THint<Ptr<Expr>>.val),
        item = Read(THint<Ptr<Expr>>.val),
    };

    public Tuple Read(THint<Tuple> _) => new Tuple
    {
        elts = Read(THint<Ptr<Expr>[]>.val),
    };

    public GlobalNameOut Read(THint<GlobalNameOut> _) => new GlobalNameOut
    {
        ind = Read(THint<int>.val),
    };

    public LocalNameOut Read(THint<LocalNameOut> _) => new LocalNameOut
    {
        ind = Read(THint<int>.val),
    };

    public DerefNameOut Read(THint<DerefNameOut> _) => new DerefNameOut
    {
        ind = Read(THint<int>.val),
    };

    public ItemOut Read(THint<ItemOut> _) => new ItemOut
    {
        value = Read(THint<Ptr<Expr>>.val),
        item = Read(THint<Ptr<Expr>>.val),
    };

    public AttrOut Read(THint<AttrOut> _) => new AttrOut
    {
        value = Read(THint<Ptr<Expr>>.val),
        attr = Read(THint<int>.val),
    };

    public Val Read(THint<Val> _) => new Val
    {
        value = Read(THint<Ptr<Expr>>.val),
    };

    public ArbitraryCatch Read(THint<ArbitraryCatch> _) => new ArbitraryCatch
    {
        assign_slot = Read(THint<int>.val),
        body = Read(THint<Ptr<Stmt>[]>.val),
    };

    public TypeCheckCatch Read(THint<TypeCheckCatch> _) => new TypeCheckCatch
    {
        type = Read(THint<Ptr<Expr>>.val),
        assign_slot = Read(THint<int>.val),
        body = Read(THint<Ptr<Stmt>[]>.val),
    };

    public DFlatGraphCode Read(THint<DFlatGraphCode> _) => new DFlatGraphCode
    {
        functiondefs = Read(THint<FunctionDef[]>.val),
        returns = Read(THint<Return[]>.val),
        delglobalnames = Read(THint<DelGlobalName[]>.val),
        dellocalnames = Read(THint<DelLocalName[]>.val),
        delderefnames = Read(THint<DelDerefName[]>.val),
        deleteitems = Read(THint<DeleteItem[]>.val),
        assigns = Read(THint<Assign[]>.val),
        addassigns = Read(THint<AddAssign[]>.val),
        subassigns = Read(THint<SubAssign[]>.val),
        mutassigns = Read(THint<MutAssign[]>.val),
        truedivassigns = Read(THint<TrueDivAssign[]>.val),
        floordivassigns = Read(THint<FloorDivAssign[]>.val),
        modassigns = Read(THint<ModAssign[]>.val),
        powassigns = Read(THint<PowAssign[]>.val),
        lshiftassigns = Read(THint<LShiftAssign[]>.val),
        rshiftassigns = Read(THint<RShiftAssign[]>.val),
        bitorassigns = Read(THint<BitOrAssign[]>.val),
        bitandassigns = Read(THint<BitAndAssign[]>.val),
        bitxorassigns = Read(THint<BitXorAssign[]>.val),
        fors = Read(THint<For[]>.val),
        whiles = Read(THint<While[]>.val),
        ifs = Read(THint<If[]>.val),
        withs = Read(THint<With[]>.val),
        raises = Read(THint<Raise[]>.val),
        trys = Read(THint<Try[]>.val),
        asserts = Read(THint<Assert[]>.val),
        exprstmts = Read(THint<ExprStmt[]>.val),
        controls = Read(THint<Control[]>.val),
        addops = Read(THint<AddOp[]>.val),
        subops = Read(THint<SubOp[]>.val),
        mutops = Read(THint<MutOp[]>.val),
        truedivops = Read(THint<TrueDivOp[]>.val),
        floordivops = Read(THint<FloorDivOp[]>.val),
        modops = Read(THint<ModOp[]>.val),
        powops = Read(THint<PowOp[]>.val),
        lshiftops = Read(THint<LShiftOp[]>.val),
        rshiftops = Read(THint<RShiftOp[]>.val),
        bitorops = Read(THint<BitOrOp[]>.val),
        bitandops = Read(THint<BitAndOp[]>.val),
        bitxorops = Read(THint<BitXorOp[]>.val),
        globalbinders = Read(THint<GlobalBinder[]>.val),
        localbinders = Read(THint<LocalBinder[]>.val),
        derefbinders = Read(THint<DerefBinder[]>.val),
        andops = Read(THint<AndOp[]>.val),
        orops = Read(THint<OrOp[]>.val),
        invertops = Read(THint<InvertOp[]>.val),
        notops = Read(THint<NotOp[]>.val),
        lambdas = Read(THint<Lambda[]>.val),
        ifexprs = Read(THint<IfExpr[]>.val),
        dicts = Read(THint<Dict[]>.val),
        sets = Read(THint<Set[]>.val),
        lists = Read(THint<List[]>.val),
        generators = Read(THint<Generator[]>.val),
        comprehensions = Read(THint<Comprehension[]>.val),
        calls = Read(THint<Call[]>.val),
        formats = Read(THint<Format[]>.val),
        consts = Read(THint<Const[]>.val),
        attrs = Read(THint<Attr[]>.val),
        globalnames = Read(THint<GlobalName[]>.val),
        localnames = Read(THint<LocalName[]>.val),
        derefnames = Read(THint<DerefName[]>.val),
        items = Read(THint<Item[]>.val),
        tuples = Read(THint<Tuple[]>.val),
        globalnameouts = Read(THint<GlobalNameOut[]>.val),
        localnameouts = Read(THint<LocalNameOut[]>.val),
        derefnameouts = Read(THint<DerefNameOut[]>.val),
        itemouts = Read(THint<ItemOut[]>.val),
        attrouts = Read(THint<AttrOut[]>.val),
        vals = Read(THint<Val[]>.val),
        arbitrarycatchs = Read(THint<ArbitraryCatch[]>.val),
        typecheckcatchs = Read(THint<TypeCheckCatch[]>.val),
    };

    public static readonly THint<int> int_hint = THint<int>.val;
    public int[] Read(THint<int[]> _)
    {
        int[] src = new int[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(int_hint);
        }
        return src;
    }
    public static readonly THint<string> string_hint = THint<string>.val;
    public string[] Read(THint<string[]> _)
    {
        string[] src = new string[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(string_hint);
        }
        return src;
    }
    public static readonly THint<float> float_hint = THint<float>.val;
    public float[] Read(THint<float[]> _)
    {
        float[] src = new float[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(float_hint);
        }
        return src;
    }
    public static readonly THint<bool> bool_hint = THint<bool>.val;
    public bool[] Read(THint<bool[]> _)
    {
        bool[] src = new bool[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(bool_hint);
        }
        return src;
    }
    public static readonly THint<DFlatGraphCode> DFlatGraphCode_hint = THint<DFlatGraphCode>.val;
    public DFlatGraphCode[] Read(THint<DFlatGraphCode[]> _)
    {
        DFlatGraphCode[] src = new DFlatGraphCode[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DFlatGraphCode_hint);
        }
        return src;
    }
    public static readonly THint<FunctionDef> FunctionDef_hint = THint<FunctionDef>.val;
    public FunctionDef[] Read(THint<FunctionDef[]> _)
    {
        FunctionDef[] src = new FunctionDef[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(FunctionDef_hint);
        }
        return src;
    }
    public static readonly THint<Return> Return_hint = THint<Return>.val;
    public Return[] Read(THint<Return[]> _)
    {
        Return[] src = new Return[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Return_hint);
        }
        return src;
    }
    public static readonly THint<DelGlobalName> DelGlobalName_hint = THint<DelGlobalName>.val;
    public DelGlobalName[] Read(THint<DelGlobalName[]> _)
    {
        DelGlobalName[] src = new DelGlobalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DelGlobalName_hint);
        }
        return src;
    }
    public static readonly THint<DelLocalName> DelLocalName_hint = THint<DelLocalName>.val;
    public DelLocalName[] Read(THint<DelLocalName[]> _)
    {
        DelLocalName[] src = new DelLocalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DelLocalName_hint);
        }
        return src;
    }
    public static readonly THint<DelDerefName> DelDerefName_hint = THint<DelDerefName>.val;
    public DelDerefName[] Read(THint<DelDerefName[]> _)
    {
        DelDerefName[] src = new DelDerefName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DelDerefName_hint);
        }
        return src;
    }
    public static readonly THint<DeleteItem> DeleteItem_hint = THint<DeleteItem>.val;
    public DeleteItem[] Read(THint<DeleteItem[]> _)
    {
        DeleteItem[] src = new DeleteItem[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DeleteItem_hint);
        }
        return src;
    }
    public static readonly THint<Assign> Assign_hint = THint<Assign>.val;
    public Assign[] Read(THint<Assign[]> _)
    {
        Assign[] src = new Assign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Assign_hint);
        }
        return src;
    }
    public static readonly THint<AddAssign> AddAssign_hint = THint<AddAssign>.val;
    public AddAssign[] Read(THint<AddAssign[]> _)
    {
        AddAssign[] src = new AddAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(AddAssign_hint);
        }
        return src;
    }
    public static readonly THint<SubAssign> SubAssign_hint = THint<SubAssign>.val;
    public SubAssign[] Read(THint<SubAssign[]> _)
    {
        SubAssign[] src = new SubAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(SubAssign_hint);
        }
        return src;
    }
    public static readonly THint<MutAssign> MutAssign_hint = THint<MutAssign>.val;
    public MutAssign[] Read(THint<MutAssign[]> _)
    {
        MutAssign[] src = new MutAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(MutAssign_hint);
        }
        return src;
    }
    public static readonly THint<TrueDivAssign> TrueDivAssign_hint = THint<TrueDivAssign>.val;
    public TrueDivAssign[] Read(THint<TrueDivAssign[]> _)
    {
        TrueDivAssign[] src = new TrueDivAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(TrueDivAssign_hint);
        }
        return src;
    }
    public static readonly THint<FloorDivAssign> FloorDivAssign_hint = THint<FloorDivAssign>.val;
    public FloorDivAssign[] Read(THint<FloorDivAssign[]> _)
    {
        FloorDivAssign[] src = new FloorDivAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(FloorDivAssign_hint);
        }
        return src;
    }
    public static readonly THint<ModAssign> ModAssign_hint = THint<ModAssign>.val;
    public ModAssign[] Read(THint<ModAssign[]> _)
    {
        ModAssign[] src = new ModAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ModAssign_hint);
        }
        return src;
    }
    public static readonly THint<PowAssign> PowAssign_hint = THint<PowAssign>.val;
    public PowAssign[] Read(THint<PowAssign[]> _)
    {
        PowAssign[] src = new PowAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(PowAssign_hint);
        }
        return src;
    }
    public static readonly THint<LShiftAssign> LShiftAssign_hint = THint<LShiftAssign>.val;
    public LShiftAssign[] Read(THint<LShiftAssign[]> _)
    {
        LShiftAssign[] src = new LShiftAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(LShiftAssign_hint);
        }
        return src;
    }
    public static readonly THint<RShiftAssign> RShiftAssign_hint = THint<RShiftAssign>.val;
    public RShiftAssign[] Read(THint<RShiftAssign[]> _)
    {
        RShiftAssign[] src = new RShiftAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(RShiftAssign_hint);
        }
        return src;
    }
    public static readonly THint<BitOrAssign> BitOrAssign_hint = THint<BitOrAssign>.val;
    public BitOrAssign[] Read(THint<BitOrAssign[]> _)
    {
        BitOrAssign[] src = new BitOrAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(BitOrAssign_hint);
        }
        return src;
    }
    public static readonly THint<BitAndAssign> BitAndAssign_hint = THint<BitAndAssign>.val;
    public BitAndAssign[] Read(THint<BitAndAssign[]> _)
    {
        BitAndAssign[] src = new BitAndAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(BitAndAssign_hint);
        }
        return src;
    }
    public static readonly THint<BitXorAssign> BitXorAssign_hint = THint<BitXorAssign>.val;
    public BitXorAssign[] Read(THint<BitXorAssign[]> _)
    {
        BitXorAssign[] src = new BitXorAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(BitXorAssign_hint);
        }
        return src;
    }
    public static readonly THint<For> For_hint = THint<For>.val;
    public For[] Read(THint<For[]> _)
    {
        For[] src = new For[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(For_hint);
        }
        return src;
    }
    public static readonly THint<While> While_hint = THint<While>.val;
    public While[] Read(THint<While[]> _)
    {
        While[] src = new While[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(While_hint);
        }
        return src;
    }
    public static readonly THint<If> If_hint = THint<If>.val;
    public If[] Read(THint<If[]> _)
    {
        If[] src = new If[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(If_hint);
        }
        return src;
    }
    public static readonly THint<With> With_hint = THint<With>.val;
    public With[] Read(THint<With[]> _)
    {
        With[] src = new With[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(With_hint);
        }
        return src;
    }
    public static readonly THint<Raise> Raise_hint = THint<Raise>.val;
    public Raise[] Read(THint<Raise[]> _)
    {
        Raise[] src = new Raise[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Raise_hint);
        }
        return src;
    }
    public static readonly THint<Try> Try_hint = THint<Try>.val;
    public Try[] Read(THint<Try[]> _)
    {
        Try[] src = new Try[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Try_hint);
        }
        return src;
    }
    public static readonly THint<Assert> Assert_hint = THint<Assert>.val;
    public Assert[] Read(THint<Assert[]> _)
    {
        Assert[] src = new Assert[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Assert_hint);
        }
        return src;
    }
    public static readonly THint<ExprStmt> ExprStmt_hint = THint<ExprStmt>.val;
    public ExprStmt[] Read(THint<ExprStmt[]> _)
    {
        ExprStmt[] src = new ExprStmt[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ExprStmt_hint);
        }
        return src;
    }
    public static readonly THint<Control> Control_hint = THint<Control>.val;
    public Control[] Read(THint<Control[]> _)
    {
        Control[] src = new Control[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Control_hint);
        }
        return src;
    }
    public static readonly THint<AddOp> AddOp_hint = THint<AddOp>.val;
    public AddOp[] Read(THint<AddOp[]> _)
    {
        AddOp[] src = new AddOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(AddOp_hint);
        }
        return src;
    }
    public static readonly THint<SubOp> SubOp_hint = THint<SubOp>.val;
    public SubOp[] Read(THint<SubOp[]> _)
    {
        SubOp[] src = new SubOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(SubOp_hint);
        }
        return src;
    }
    public static readonly THint<MutOp> MutOp_hint = THint<MutOp>.val;
    public MutOp[] Read(THint<MutOp[]> _)
    {
        MutOp[] src = new MutOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(MutOp_hint);
        }
        return src;
    }
    public static readonly THint<TrueDivOp> TrueDivOp_hint = THint<TrueDivOp>.val;
    public TrueDivOp[] Read(THint<TrueDivOp[]> _)
    {
        TrueDivOp[] src = new TrueDivOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(TrueDivOp_hint);
        }
        return src;
    }
    public static readonly THint<FloorDivOp> FloorDivOp_hint = THint<FloorDivOp>.val;
    public FloorDivOp[] Read(THint<FloorDivOp[]> _)
    {
        FloorDivOp[] src = new FloorDivOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(FloorDivOp_hint);
        }
        return src;
    }
    public static readonly THint<ModOp> ModOp_hint = THint<ModOp>.val;
    public ModOp[] Read(THint<ModOp[]> _)
    {
        ModOp[] src = new ModOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ModOp_hint);
        }
        return src;
    }
    public static readonly THint<PowOp> PowOp_hint = THint<PowOp>.val;
    public PowOp[] Read(THint<PowOp[]> _)
    {
        PowOp[] src = new PowOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(PowOp_hint);
        }
        return src;
    }
    public static readonly THint<LShiftOp> LShiftOp_hint = THint<LShiftOp>.val;
    public LShiftOp[] Read(THint<LShiftOp[]> _)
    {
        LShiftOp[] src = new LShiftOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(LShiftOp_hint);
        }
        return src;
    }
    public static readonly THint<RShiftOp> RShiftOp_hint = THint<RShiftOp>.val;
    public RShiftOp[] Read(THint<RShiftOp[]> _)
    {
        RShiftOp[] src = new RShiftOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(RShiftOp_hint);
        }
        return src;
    }
    public static readonly THint<BitOrOp> BitOrOp_hint = THint<BitOrOp>.val;
    public BitOrOp[] Read(THint<BitOrOp[]> _)
    {
        BitOrOp[] src = new BitOrOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(BitOrOp_hint);
        }
        return src;
    }
    public static readonly THint<BitAndOp> BitAndOp_hint = THint<BitAndOp>.val;
    public BitAndOp[] Read(THint<BitAndOp[]> _)
    {
        BitAndOp[] src = new BitAndOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(BitAndOp_hint);
        }
        return src;
    }
    public static readonly THint<BitXorOp> BitXorOp_hint = THint<BitXorOp>.val;
    public BitXorOp[] Read(THint<BitXorOp[]> _)
    {
        BitXorOp[] src = new BitXorOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(BitXorOp_hint);
        }
        return src;
    }
    public static readonly THint<GlobalBinder> GlobalBinder_hint = THint<GlobalBinder>.val;
    public GlobalBinder[] Read(THint<GlobalBinder[]> _)
    {
        GlobalBinder[] src = new GlobalBinder[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(GlobalBinder_hint);
        }
        return src;
    }
    public static readonly THint<LocalBinder> LocalBinder_hint = THint<LocalBinder>.val;
    public LocalBinder[] Read(THint<LocalBinder[]> _)
    {
        LocalBinder[] src = new LocalBinder[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(LocalBinder_hint);
        }
        return src;
    }
    public static readonly THint<DerefBinder> DerefBinder_hint = THint<DerefBinder>.val;
    public DerefBinder[] Read(THint<DerefBinder[]> _)
    {
        DerefBinder[] src = new DerefBinder[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DerefBinder_hint);
        }
        return src;
    }
    public static readonly THint<AndOp> AndOp_hint = THint<AndOp>.val;
    public AndOp[] Read(THint<AndOp[]> _)
    {
        AndOp[] src = new AndOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(AndOp_hint);
        }
        return src;
    }
    public static readonly THint<OrOp> OrOp_hint = THint<OrOp>.val;
    public OrOp[] Read(THint<OrOp[]> _)
    {
        OrOp[] src = new OrOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(OrOp_hint);
        }
        return src;
    }
    public static readonly THint<InvertOp> InvertOp_hint = THint<InvertOp>.val;
    public InvertOp[] Read(THint<InvertOp[]> _)
    {
        InvertOp[] src = new InvertOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(InvertOp_hint);
        }
        return src;
    }
    public static readonly THint<NotOp> NotOp_hint = THint<NotOp>.val;
    public NotOp[] Read(THint<NotOp[]> _)
    {
        NotOp[] src = new NotOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(NotOp_hint);
        }
        return src;
    }
    public static readonly THint<Lambda> Lambda_hint = THint<Lambda>.val;
    public Lambda[] Read(THint<Lambda[]> _)
    {
        Lambda[] src = new Lambda[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Lambda_hint);
        }
        return src;
    }
    public static readonly THint<IfExpr> IfExpr_hint = THint<IfExpr>.val;
    public IfExpr[] Read(THint<IfExpr[]> _)
    {
        IfExpr[] src = new IfExpr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(IfExpr_hint);
        }
        return src;
    }
    public static readonly THint<Dict> Dict_hint = THint<Dict>.val;
    public Dict[] Read(THint<Dict[]> _)
    {
        Dict[] src = new Dict[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Dict_hint);
        }
        return src;
    }
    public static readonly THint<Set> Set_hint = THint<Set>.val;
    public Set[] Read(THint<Set[]> _)
    {
        Set[] src = new Set[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Set_hint);
        }
        return src;
    }
    public static readonly THint<List> List_hint = THint<List>.val;
    public List[] Read(THint<List[]> _)
    {
        List[] src = new List[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(List_hint);
        }
        return src;
    }
    public static readonly THint<Generator> Generator_hint = THint<Generator>.val;
    public Generator[] Read(THint<Generator[]> _)
    {
        Generator[] src = new Generator[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Generator_hint);
        }
        return src;
    }
    public static readonly THint<Comprehension> Comprehension_hint = THint<Comprehension>.val;
    public Comprehension[] Read(THint<Comprehension[]> _)
    {
        Comprehension[] src = new Comprehension[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Comprehension_hint);
        }
        return src;
    }
    public static readonly THint<Call> Call_hint = THint<Call>.val;
    public Call[] Read(THint<Call[]> _)
    {
        Call[] src = new Call[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Call_hint);
        }
        return src;
    }
    public static readonly THint<Format> Format_hint = THint<Format>.val;
    public Format[] Read(THint<Format[]> _)
    {
        Format[] src = new Format[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Format_hint);
        }
        return src;
    }
    public static readonly THint<Const> Const_hint = THint<Const>.val;
    public Const[] Read(THint<Const[]> _)
    {
        Const[] src = new Const[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Const_hint);
        }
        return src;
    }
    public static readonly THint<Attr> Attr_hint = THint<Attr>.val;
    public Attr[] Read(THint<Attr[]> _)
    {
        Attr[] src = new Attr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Attr_hint);
        }
        return src;
    }
    public static readonly THint<GlobalName> GlobalName_hint = THint<GlobalName>.val;
    public GlobalName[] Read(THint<GlobalName[]> _)
    {
        GlobalName[] src = new GlobalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(GlobalName_hint);
        }
        return src;
    }
    public static readonly THint<LocalName> LocalName_hint = THint<LocalName>.val;
    public LocalName[] Read(THint<LocalName[]> _)
    {
        LocalName[] src = new LocalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(LocalName_hint);
        }
        return src;
    }
    public static readonly THint<DerefName> DerefName_hint = THint<DerefName>.val;
    public DerefName[] Read(THint<DerefName[]> _)
    {
        DerefName[] src = new DerefName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DerefName_hint);
        }
        return src;
    }
    public static readonly THint<Item> Item_hint = THint<Item>.val;
    public Item[] Read(THint<Item[]> _)
    {
        Item[] src = new Item[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Item_hint);
        }
        return src;
    }
    public static readonly THint<Tuple> Tuple_hint = THint<Tuple>.val;
    public Tuple[] Read(THint<Tuple[]> _)
    {
        Tuple[] src = new Tuple[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Tuple_hint);
        }
        return src;
    }
    public static readonly THint<GlobalNameOut> GlobalNameOut_hint = THint<GlobalNameOut>.val;
    public GlobalNameOut[] Read(THint<GlobalNameOut[]> _)
    {
        GlobalNameOut[] src = new GlobalNameOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(GlobalNameOut_hint);
        }
        return src;
    }
    public static readonly THint<LocalNameOut> LocalNameOut_hint = THint<LocalNameOut>.val;
    public LocalNameOut[] Read(THint<LocalNameOut[]> _)
    {
        LocalNameOut[] src = new LocalNameOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(LocalNameOut_hint);
        }
        return src;
    }
    public static readonly THint<DerefNameOut> DerefNameOut_hint = THint<DerefNameOut>.val;
    public DerefNameOut[] Read(THint<DerefNameOut[]> _)
    {
        DerefNameOut[] src = new DerefNameOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DerefNameOut_hint);
        }
        return src;
    }
    public static readonly THint<ItemOut> ItemOut_hint = THint<ItemOut>.val;
    public ItemOut[] Read(THint<ItemOut[]> _)
    {
        ItemOut[] src = new ItemOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ItemOut_hint);
        }
        return src;
    }
    public static readonly THint<AttrOut> AttrOut_hint = THint<AttrOut>.val;
    public AttrOut[] Read(THint<AttrOut[]> _)
    {
        AttrOut[] src = new AttrOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(AttrOut_hint);
        }
        return src;
    }
    public static readonly THint<Val> Val_hint = THint<Val>.val;
    public Val[] Read(THint<Val[]> _)
    {
        Val[] src = new Val[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Val_hint);
        }
        return src;
    }
    public static readonly THint<ArbitraryCatch> ArbitraryCatch_hint = THint<ArbitraryCatch>.val;
    public ArbitraryCatch[] Read(THint<ArbitraryCatch[]> _)
    {
        ArbitraryCatch[] src = new ArbitraryCatch[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ArbitraryCatch_hint);
        }
        return src;
    }
    public static readonly THint<TypeCheckCatch> TypeCheckCatch_hint = THint<TypeCheckCatch>.val;
    public TypeCheckCatch[] Read(THint<TypeCheckCatch[]> _)
    {
        TypeCheckCatch[] src = new TypeCheckCatch[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(TypeCheckCatch_hint);
        }
        return src;
    }
    public static readonly THint<Ptr<Stmt>> Ptr__Stmt___hint = THint<Ptr<Stmt>>.val;
    public Ptr<Stmt>[] Read(THint<Ptr<Stmt>[]> _)
    {
        Ptr<Stmt>[] src = new Ptr<Stmt>[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Ptr__Stmt___hint);
        }
        return src;
    }
    public static readonly THint<Ptr<Expr>> Ptr__Expr___hint = THint<Ptr<Expr>>.val;
    public Ptr<Expr>[] Read(THint<Ptr<Expr>[]> _)
    {
        Ptr<Expr>[] src = new Ptr<Expr>[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Ptr__Expr___hint);
        }
        return src;
    }
    public static readonly THint<Ptr<Arg>> Ptr__Arg___hint = THint<Ptr<Arg>>.val;
    public Ptr<Arg>[] Read(THint<Ptr<Arg>[]> _)
    {
        Ptr<Arg>[] src = new Ptr<Arg>[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Ptr__Arg___hint);
        }
        return src;
    }
    public static readonly THint<Ptr<ExceptHandler>> Ptr__ExceptHandler___hint = THint<Ptr<ExceptHandler>>.val;
    public Ptr<ExceptHandler>[] Read(THint<Ptr<ExceptHandler>[]> _)
    {
        Ptr<ExceptHandler>[] src = new Ptr<ExceptHandler>[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Ptr__ExceptHandler___hint);
        }
        return src;
    }
}
}
