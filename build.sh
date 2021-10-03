python -m lark_action ch.lark --package dianascript --module parser
cd codegen && python -m lark_action gen-lang2.lark --module gen_lang2
cd ..
python codegen/new-lang-gen.py
python codegen/datatype_gen.py
python codegen/binding-generator.py

# unity does not support default interface implementation..
python codegen/default-method-gen.py  

dotnet build