/* test parsrer only */

func f(a, b, c)

end

func f()

end


    
func f()
    loop
        a.[1] := a := c.attr := x + 1
        print()
    end
end

each x of expr do
    x + 3
    x =: x - 3
end

if 1 > 2 or 2 < 3 and test(func(), func()) then
    p(1)
    p(a(x), b(x))
end

each of [1, 2, 3] do
    break
end

each of [1, 2, 3] do
    if a() then
        continue
    end
    return 1
    return
end

1 and 2 or 3 and 4


1 < 2
1 > 2
1 <= 2
1 >= 3
1 != 2
1 in [2]
1 + 2
1 - 2
1 * 2
1 / 2
1 // 2
1 ** 2
1 % 2
1 & 2
1 | 2
1 << 2
1 >> 2
1 not in [2]
f(1, 2)
{1: 2, f(2): 3 + 2}
f
{(1, a.[2]): 5, }
{}
()
[]
(1, )
(1)
[1, 1.0, 0x223, 0b110010]

/*
[
    dianascript.chstmt.SFunc(
        name='f',
        args=['a', 'b', 'c'],
        body=[],
        loc=(1, 1)
    ),
    dianascript.chstmt.SFunc(name='f', args=[], body=[], loc=(5, 1)),
    dianascript.chstmt.SFunc(
        name='f',
        args=[],
        body=[
            dianascript.chstmt.SLoop(
                block=[
                    dianascript.chstmt.SAssign(
                        targets=[
                            dianascript.chlhs.LIt(
                                value=dianascript.chexpr.EVar(
                                    var='a',
                                    loc=(13, 9)
                                ),
                                item=dianascript.chexpr.EVal(
                                    val=1,
                                    loc=(13, 12)
                                ),
                                loc=(13, 10)
                            ),
                            dianascript.chlhs.LVar(var='a', loc=(13, 18)),
                            dianascript.chlhs.LAttr(
                                value=dianascript.chexpr.EVar(
                                    var='c',
                                    loc=(13, 23)
                                ),
                                attr='attr',
                                loc=(13, 24)
                            )
                        ],
                        value=dianascript.chexpr.EOp(
                            left=dianascript.chexpr.EVar(
                                var='x',
                                loc=(13, 33)
                            ),
                            op='+',
                            right=dianascript.chexpr.EVal(val=1, loc=(13, 37))
                        ),
                        loc=(13, 30)
                    )
                ],
                loc=(12, 5)
            )
        ],
        loc=(11, 1)
    )
]
*/