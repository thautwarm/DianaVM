print(bool)
print(bool)
x := 1
print(x)

func f(x)
    return x + x
end

print(2 + f(1))


if 1 > 2 then
    print("2")
else
    print("3")
end

func fact(x)
    if x < 1 then
        print("ret")
        return 1
    else
        r := x * fact(x - 1)
        print(r, "<- ret")
        return r
    end
end

print(f(10))

x := 10
print("hhh")

if x < 5 then
    print(1)
else
    print(x)
end


print("y", fact(5))
print([1, 2, 3])

print([1, 2, 3].[0])

if x + 1 = 2 then
else
    print(3)
end

func g()
    var i
    each i of [1, 2, 3, 4] do
        print(i)
    end
end

g()

print(int.parse("233"))

x := time()

func test()
    var x
    loop
        if x < 10000000 then
            break
        end
        x := x + 1
    end
end

print((time() - x))

print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print((time() - time()))
print("aaa")
x := time()

func test2()
    var x
    x := 0
    loop
        if x > 10000000 then
            break
        end
        x := x + 1
    end
    return x
end

x := time()
print(test2())
print(time() - x)

x := time()
print(test2(), "<<test2")
print(time() - x)

