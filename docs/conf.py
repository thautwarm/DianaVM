# Configuration file for the Sphinx documentation builder.
#
# This file only contains a selection of the most common options. For a full
# list see the documentation:
# https://www.sphinx-doc.org/en/master/usage/configuration.html

# -- Path setup --------------------------------------------------------------

# If extensions (or modules to document with autodoc) are in another directory,
# add these directories to sys.path here. If the directory is relative to the
# documentation root, use os.path.abspath to make it absolute, like shown here.
#
# import os
# import sys
# sys.path.insert(0, os.path.abspath('.'))

# -- Project information -----------------------------------------------------

import sphinx_bootstrap_theme
from pygments.lexer import RegexLexer
from pygments import token
from sphinx.highlighting import lexers
from pygments.style import Style
from re import escape

keywords = [
    "func",
    "each",
    "of",
    "do",
    "if",
    "then",
    "else",
    "loop",
    "end",
    "var",
    "return",
]

keyword_values = [
    "True",
    "False",
    "None",
]

operators = [
    "<",
    ">",
    ">=",
    "<=",
    "==",
    "!=",
    "in",
    "+",
    "-",
    "*",
    "**",
    "/",
    "//",
    "%",
    "&",
    "|",
    "<<",
    ">>",
]

named_operators = [
    "not",
    "in",
    "and",
    "is",
    "or",
]

punctuations = [
    ",", ";", "(", ")", "{", "}", "[", "]"
]

然 = "#E799B0"
晚 = "#9AC8E2"
拉 = "#BD7D74"
乐 = "#B8A6D9"
琳 = "#576690"


class ASOULStyle(Style):
    background_color = "#FFFF99"
    styles = {
        token.Keyword: 然,
        token.String: 乐,
        token.Name: 琳,
        token.Operator.Regular:  拉,  
        token.Operator.Named: 拉,
        token.Number: 然,
        token.Comment.Short: 晚,
        token.Comment.Long: 晚,
        token.Punctuation: 乐,
        token.Literal: 拉,
    }


def pygments_monkeypatch_style(mod_name, cls):
    import sys
    import pygments.styles

    cls_name = cls.__name__
    mod = type(__import__("os"))(mod_name)
    setattr(mod, cls_name, cls)
    setattr(pygments.styles, mod_name, mod)
    sys.modules["pygments.styles." + mod_name] = mod
    from pygments.styles import STYLE_MAP

    STYLE_MAP[mod_name] = mod_name + "::" + cls_name


pygments_monkeypatch_style("asoul", ASOULStyle)
pygments_style = "asoul"

STRING = r"""r?("(?!"").*?(?<!\\)(\\\\)*?"|'(?!'').*?(?<!\\)(\\\\)*?')"""
COMMENT = r"\#[^\n]*"
MULTILINE_COMMENT = r"\/\*(\*(?!\/)|[^*])*\*\/"
INT = r"(\+|\-)?[1-9]\d*|0[oO][0-7]+|0[xX][\da-fA-F]+|0[bB][01]+|0"
FLOAT = r"((\d+\.[\d_]*|\.[\d_]+)([Ee][-+]?\d+)?|\d+([Ee][-+]?\d+))"
NAME = r"[a-zA-Z_][a-zA-Z0-9_]*"
ATTR = r"\.[a-zA-Z_][a-zA-Z0-9_]*"
class DianaLexer(RegexLexer):
    name = "diana"

    tokens = {
        "root": [
            (ATTR, token.Operator.Regular),
            (MULTILINE_COMMENT, token.Comment.Long),
            (COMMENT, token.Comment.Short),
            *[(rf"\b{k}\b", token.Keyword) for k in keywords],
            *[(rf'\B{escape(o)}\B', token.Operator.Regular) for o in operators],
            *[(rf"\b{o}\b", token.Operator.Named) for o in named_operators],
            *[(rf"\b{o}\b", token.Literal) for o in keyword_values],
            *[(escape(o), token.Punctuation) for o in punctuations],
            (FLOAT, token.Number),
            (INT, token.Number),
            (NAME, token.Name),
            (STRING, token.String),
            
            (r"\s+", token.Whitespace),
        ]
    }


lexers[DianaLexer.name] = DianaLexer(startinline=True)

extensions = ["sphinx.ext.mathjax", "recommonmark"]
master_doc = "index"
project = "DianaScript"
copyright = "2021, ASOUL fans"
author = "ASOUL fans"

# The full version, including alpha/beta/rc tags
release = "0.1"

# -- General configuration ---------------------------------------------------

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.

# Add any paths that contain templates here, relative to this directory.
templates_path = ["_templates"]

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = ["_build", "Thumbs.db", ".DS_Store"]

# -- Options for HTML output -------------------------------------------------

# The theme to use for HTML and HTML Help pages.  See the documentation for
# a list of builtin themes.
#

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
html_static_path = ["_static"]

html_theme = "bootstrap"
html_theme_path = sphinx_bootstrap_theme.get_html_theme_path()
html_title = "DianaScript Documentation"

html_theme_options = {
    # Navigation bar title. (Default: ``project`` value)
    "navbar_site_name": "DianaScript",
    "navbar_title": f"{project}",
    # Tab name for entire site. (Default: "Site")
    # A list of tuples containing pages or urls to link to.
    # Valid tuples should be in the following forms:
    #    (name, page)                 # a link to a page
    #    (name, "/aa/bb", 1)          # a link to an arbitrary relative url
    #    (name, "http://example.com", True) # arbitrary absolute url
    # Note the "1" or "True" value above as the third argument to indicate
    # an arbitrary url.
    "navbar_links": [("GitHub", "github")],
    # Render the next and previous page links in navbar. (Default: true)
    "navbar_sidebarrel": False,
    # Render the current pages TOC in the navbar. (Default: true)
    "navbar_pagenav": True,
    # Tab name for the current pages TOC. (Default: "Page")
    "navbar_pagenav_name": "Subsections",
    # Global TOC depth for "site" navbar tab. (Default: 1)
    # Switching to -1 shows all levels.
    "globaltoc_depth": -1,
    # Include hidden TOCs in Site navbar?
    #
    # Note: If this is "false", you cannot have mixed ``:hidden:`` and
    # non-hidden ``toctree`` directives in the same page, or else the build
    # will break.
    #
    # Values: "true" (default) or "false"
    "globaltoc_includehidden": "true",
    # HTML navbar class (Default: "navbar") to attach to <div> element.
    # For black navbar, do "navbar navbar-inverse"
    "navbar_class": "navbar navbar-inverse",
    # Fix navigation bar to top of page?
    # Values: "true" (default) or "false"
    "navbar_fixed_top": "true",
    # Location of link to source.
    # Options are "nav" (default), "footer" or anything else to exclude.
    "source_link_position": "footer",
    # Bootswatch (http://bootswatch.com/) theme.
    #
    # Options are nothing (default) or the name of a valid theme
    # such as "cosmo" or "sandstone".
    #
    # The set of valid themes depend on the version of Bootstrap
    # that's used (the next config option).
    #
    # Currently, the supported themes are:
    # - Bootstrap 2: https://bootswatch.com/2
    # - Bootstrap 3: https://bootswatch.com/3
    "bootswatch_theme": "lumen",
    # Choose Bootstrap version.
    # Values: "3" (default) or "2" (in quotes)
    "bootstrap_version": "3",
}
htmlhelp_basename = "diana_"

# html_favicon = './favicon.ico'

latex_documents = [
    (master_doc, f"{project}.tex", f"{project}", "thautwarm", "manual"),
]

html_sidebars = {"**": []}

html_context = {
    'css_files': ['_static/style.css'],
}

def setup(app):
    app.add_css_file("style.css")