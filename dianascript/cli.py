from dianascript.cg import codegen
from dianascript.parser import parser
from dianascript.code_cons import Storage, Builder
from dianascript.logger import logger
from dianascript.mdis import dis
import logging
import struct
def call_main(filename: str, out: str="a.ran", loglevel: str=""):
    if loglevel:
        logger.setLevel(loglevel)
    with open(filename, encoding='utf-8') as f:
        seq = parser.parse(f.read())
    # import prettyprinter
    # prettyprinter.install_extras(["dataclasses"])
    # prettyprinter.pprint(seq)
    barr = bytearray()
    codegen(filename, seq, barr)

    if logging.DEBUG >= logger.level:
        logger.debug("  data count:")
        for k, each in Storage.__dict__.items():
            if isinstance(each, Builder):
                logger.debug(f"  - {k} : {len(each)}")
        logger.debug("  simple CFG:")
        dis()
    #     i1 = struct.unpack('<i', barr[0:4])
    #     i2 = struct.unpack('<i', barr[4:8])
    #     i3 = struct.unpack('<i', barr[8:12])
    #     i4 = struct.unpack('<i', barr[12:16])
    #     i5 = struct.unpack('<i', barr[16:20])
    #     logger.debug(f"data = | {i1} | {i2} | {i3} | {i4} | {i5} ...")
    #     logger.debug("dobjs:" + repr(list(DFlatGraphCode.dobjs)))
    #     logger.debug("intern strings:" + repr(list(DFlatGraphCode.internstrings)))
    #     logger.debug("funcdefs:" + repr(list(DFlatGraphCode.funcmetas)))
    #     b = bytearray()
    #     DFlatGraphCode.strings.serialize_(b)
    #     logger.debug(struct.unpack("<i", b))
    
    
    with open(out, 'wb') as f:
        f.write(barr)
