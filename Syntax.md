
```
x.each | x | do
    x + 1
end

x = ref x

x.each | x | do
    x <<< 1
end

x.run | a, b, c | do
    
end
```
