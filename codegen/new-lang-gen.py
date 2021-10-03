from pathlib import Path
from jinja2 import Environment
from jinja2.loaders import BaseLoader
from gen_lang2 import parser, TName, TArr, TTup
from pathlib import Path
import textwrap

env = Environment(
    loader = BaseLoader(),
    extensions=['jinja2.ext.do'],
    trim_blocks=True,
    lstrip_blocks=True
)

# import prettyprinter
# prettyprinter.install_extras(["dataclasses"])

defs = parser.parse(Path(__file__).with_name("lang.spec").open().read())

def find_paths(p: Path):
    if not p.is_dir():
        if p.suffix == ".in":
            yield p
    else:
        for i in p.iterdir():
            if i == p:
                continue
            yield from find_paths(i)


py_map = {
    'Tuple': 'tuple',
    'string': 'str'
}

def PY_filter(x) -> str:
    if isinstance(x, TName):
        return py_map.get(x.name, x.name)
    elif isinstance(x, TTup):
        return 'tuple[{}]'.format(', '.join(map(PY_filter, x.eltypes)))
    else:
        assert isinstance(x, TArr)
        return 'PVector[{}]'.format(PY_filter(x.eltype))

def NET_filter(x) -> str:
    if isinstance(x, TName):
        return x.name
    elif isinstance(x, TTup):
        return '({})'.format(', '.join(map(NET_filter, x.eltypes)))
    else:
        assert isinstance(x, TArr)
        return '{}[]'.format(NET_filter(x.eltype))

env.filters['PY'] = PY_filter
env.filters['NET'] = NET_filter

dataclass_names = {defi.name for defi in defs if not defi.is_bytecode and not defi.is_external}

def is_dataclass(x):
    return isinstance(x, TName) and x.name in dataclass_names

def assert_(x):
    assert x

import builtins

namespace = {**builtins.__dict__, **globals()}
for FROM, TO in [
    (path, path.with_suffix("")) for path in find_paths(Path(__file__).parent.parent)
]:
    try:
        template = env.from_string(FROM.open(encoding='utf8').read())
        s = template.render(**namespace)
        TO.open('w', encoding='utf8').write(s)
        print(TO, "written")
    except:
        print(FROM)
        raise