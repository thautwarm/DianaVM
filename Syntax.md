

func : "func" name "(" arglist ")"  block "end" { block }
block : stmt+ 

stmt :   "var" seplist{id, ","}      -> decl
       | id "=" expr  -> assign
       | expr         -> expr_stmt
       | "while" expr "do" block "end"        -> while
       | "for" lhs "in" expr "do" block "end" -> for_loop
       | "del" seplist{",", id}               -> delete
       | "if" expr "then" block "end"         -> if_then
       | "if" expr "then" block "else" block "end" -> if_then_else
       | "break"                                   -> break_
       | "continue"                                -> continue_

expr :  bool_expr

or_expr : or_expr "or" and_expr -> or_expr
        | and_expr              -> just

and_expr : and_expr "and" not   -> and_expr
         | not                  -> just


cmpop : "<" | ">" |">=" | "<=" | "==" | "!=" | "in" | "not" "in"

not : 'not' cmp -> not_
    | cmp       -> just

cmp : cmp cmpop bin
    | bin

