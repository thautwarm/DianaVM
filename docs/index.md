# DianaScript(Preview)

## Introduction

DianaScript是一个高性能、API安全可控的动态脚本语言。它运行在.net standard 2.0及更新的运行时之上。
DianaScript的实现**无需反射机制的介入**，因此，在与Unity一同使用时，支持IL2CPP、 WebGL等任意构建target。

DianaScript是DianaVM的一个主要前端，该虚拟机的概要描述见[`codegen/lang.spec`](https://github.com/thautwarm/DianaScript/blob/master/codegen/lang.spec)文件。

```diana
# 类型检查；数字运算
func is_even_number(x)
    return x.Class == int and x % 2 == 0
end

/* 传递函数；基于.NET IEnumerable的foreach */
func filter_list(f, sequence)
    var i = low
    var s = 0
    var result = []
    each element of sequence do
        if f(element) is True then
            result.append(element)
        end
    end

    return result
end

print("过滤偶数", [1, "23", 2, "4", 6]))
```