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