from dianascript.cg import codegen
from dianascript.parser import parser
def call_main(filename: str, out: str="a.ran"):
    with open(filename, encoding='utf-8') as f:
        seq = parser.parse(f.read())
    import prettyprinter
    prettyprinter.install_extras(["dataclasses"])
    prettyprinter.pprint(seq)
    barr = bytearray()
    codegen(filename, seq, barr)
    with open(out, 'wb') as f:
        f.write(barr)    