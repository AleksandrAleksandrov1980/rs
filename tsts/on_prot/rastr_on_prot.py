import sys
import win32com.client as c32
from win32com.client import WithEvents

if(sys.maxsize > 2**32):
    print("Python x64 and Rastr must be x64 ")
else:
    print("Python x32 and Rastr must be x32 ")
rastr = c32.Dispatch('astra.rastr')  
class MyEvents:
    #C# proto: ( LogErrorCodes Code,  int Level, int StageId,  string TableName, int TableIndex,  string Description, string FormName)
    def OnLog (self, a1,a2,a3,a4,a5,a6,a7):
        print(f"{a1} : {a2} : {a3} :{a4} :{a5} :{a6} :{a7} !")
r_events = c32.WithEvents(rastr, MyEvents)
rastr.Load(1,r'C:\Users\ustas\Documents\RastrWin3\test-rastr\cx195.rg2','')
rastr.Printp('test')
rastr.rgm('')