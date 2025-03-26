import numpy as np
import win32com.client as c32
import logging
import os
from pathlib import *


g_logger_protocol = logging.getLogger("logger_protocol")

msg = "Hello from Test_PlainTI.py" 
print(msg)

#код загрузки файла для Rastr.Load()
def Get_RG_REPL():
    return 1

#обработка событий протокола объекта растра
class RastrEvents:
    def OnLog(self, code, level, id, name, index, description, formName):
        print ("[%d] %s"%(code, description))
		
    def Onprot(self, message):
        print (message)
        
    def OnCommandMain(self, comm: int, p1: str, p2: str, pp: int, pVal):
        print("cmmand [%d] %s"%(comm, p1))


#Тествый файл из документов пользователя
TestFile = 'cx195.rg2'
PathTest = Path('Documents', 'RastrWin3',  'test-rastr', TestFile)
path_test_file = Path.home() / PathTest 

print(f"Test file:  {path_test_file}")

prastr = c32.Dispatch('astra.rastr')  
comck = c32.Dispatch("COM.CK")	
TI = c32.Dispatch("COMCK.TI")	
c32.WithEvents(prastr, RastrEvents)    # подписываемся на события объекта растра    
prastr.Load(Get_RG_REPL(), "E:\\Documents\\RastrWin3\\test-rastr\\cx195.rg2", "")  # при загрузке вылазит ошибка, но загрузка проходит
'''
rastr_tables = prastr.Tables
node_table_test = rastr_tables('node')
node_columns_test = node_table_test.Cols
node_ny = node_columns_test('ny')
node_uhom = node_columns_test('uhom')
node_na = node_columns_test('na')
node_pn = node_columns_test('pn')
pn1 = node_pn.Z(2)
print(f"pn[1]={pn1}")"
'''


res = TI.Init(prastr,0)


print("End Test_PlainTI.py")

