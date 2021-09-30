## 创语言

该语言是Diana虚拟机的一个简单前端。作用于只有local和global两种(虚拟机还支持cell和free)


CheatSheet:

```
/* 
multi-line comment
*/

var x # global level declaration makes no sense
function f()
    var x, y, z # local variable
    # ... single-line comment
    
    function g() # a locally visible function
        x + 1 # x is global variable!
    end

    #
    loop
        # multi-target assignment
        expr.[expr] := expr.attr := name := expr

        #
        if expr then
            block
        end

        if expr then
            block
        else
            block
        end
    
        break
        continue
        return 1
        return # return None
    end
end


if cond1 and cond2 or cond3 and cond4 
then
    block
end


each expr of iter_expr
do
    block
end
```