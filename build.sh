python -m lark_action ch.lark --package dianascript --module parser
python codegen/lang-generator.py
python codegen/datatype_gen.py
python codegen/binding-generator.py

# unity does not support default interface implementation..
python codegen/default-method-gen.py  
