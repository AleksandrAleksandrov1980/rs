import sys
import os
import win32com.client as c32
import psutil
import json
from pathlib import Path
from datetime import datetime
import pythoncom
from pathlib import Path

#set-executionpolicy remotesigned
#python -m pip install --upgrade pywin32
#pip install psutil
#---------------------------------------------------------------------------------------------------------
def Get_RG_REPL():
    return 1
#---------------------------------------------------------------------------------------------------------
def enum_loaded_dlls(name_loking_for) -> bool :
    p = psutil.Process( os.getpid() )
    for dll in p.memory_maps():
        if(dll.path.endswith(name_loking_for)):
            print("-FIND-------------------------------------------------- ")
            print(dll.path)
            print("-FIND-------------------------------------------------- ")
            return True
        #print(dll.path)
    return False
#---------------------------------------------------------------------------------------------------------
def rastr_set_val( rastr, namet, namec, val ):
    tables   = rastr.tables
    table    = tables(namet)
    cols     = table.cols
    col      = cols(namec)
    newVal   = c32.VARIANT(pythoncom.VT_VARIANT,val)
    nRow     = 0 # 
    #4                            = 0x00000004 in decimal: this is the Id of the property
    #0                            = the LCID, the Locale Id ... I always set it to 0
    #pythoncom.INVOKE_PROPERTYPUT = the type of call.
    #0                            = whether you care about the return type (you probably don't = False)
    #0                            = first parameter, lAcq, as in CurrentDevice(0)
    #newVal                       = second paramter,pVal, the new device name as a VARIANT
    col._oleobj_.Invoke( 4, 0, pythoncom.INVOKE_PROPERTYPUT, 0, nRow, val )
#---------------------------------------------------------------------------------------------------------
def tst_mdp_file( rastr, path_file_mdp, path_file_log ):
    try:
        print(f"read_file_mdp:{path_file_mdp}")
        rastr.Load( Get_RG_REPL(), path_file_mdp, '' )
        rastr_set_val(rastr, "ut_vir_common", "kod", 22 )
        print(f"set_path_log:{path_file_mdp}")
        rastr_set_val(rastr, "ut_vir_common", "log_path2file", path_file_log )
        path_file_save_1 =  os.path.dirname(path_file_log) + "/" + Path(path_file_log).stem + "____11111__.os"
        rastr.Save( path_file_save_1, '' )
        print(f"call Emergencies")
        newVal = c32.VARIANT(pythoncom.VT_VARIANT,0)
        nRes = rastr.Emergencies(newVal)
        path_file_save_2 = os.path.dirname(path_file_log) + "/" + Path(path_file_log).stem + "____22222__.os"
        print(f"save {path_file_save_2}")
        rastr.Save( path_file_save_2, '' )
    except OSError as err:
        print("OS error:", err)
    except ValueError:
        print("Could not convert data to an integer.")
    except Exception as ex:
        print(f"EXCEPTION: {ex}")
    except :
        print('xz')
    print("thats all folks!")
#---------------------------------------------------------------------------------------------------------
def get_name__current_script():
    return os.path.basename(__file__)
#---------------------------------------------------------------------------------------------------------
def read_conf():
    dir_current_script = os.getcwd()
    p = psutil.Process( os.getpid() )
    p.cmdline()
    name_current_script = get_name__current_script()
    for cmdline in p.cmdline():
        if(cmdline.endswith(name_current_script)):
            dir_current_script = os.path.dirname(cmdline)
     #!!!!! win-service !!! path to json!!!
    print(f"current scriptname: {os.path.basename(__file__)}")
    #with open("D:/rs/py_db_wrt/config.json") as json_data_file: # for WIN-SERVICE !!!
    #path_json = os.getcwd() + "/"+"config.json";
    path_json = dir_current_script + "/"+"config.json"
    with open(path_json) as json_data_file: # for WIN-SERVICE !!!
        jcnf = json.load(json_data_file)
    return jcnf
#---------------------------------------------------------------------------------------------------------
def tsts_main():
    dt_string = datetime.now().strftime("res_%d_%m_%Y__%H_%M_%S.%f")[:-3]
    print(f"************************************ {dt_string} ****************************************************")
    #print( "%x" % sys.maxsize, " x644? =", sys.maxsize > 2**32 )
    if(sys.maxsize > 2**32):
        print("!!!! 64  !!!")
    else:
        print("32")
    #app = c32.gencache.EnsureDispatch('astra.rastr')
    CLSCTX_LOCAL_SERVER = 4
    rastr = c32.Dispatch('astra.rastr')      
    blFindAstra = enum_loaded_dlls("astra.dll")
    if(blFindAstra == False):
        print("NotFind astra.dll")
        return
    cnf = read_conf()
    path_tst_dir = cnf['path_tst_dir']
    print( f"path_tst_dir= {path_tst_dir}" )
    path_tst_dir_ress = path_tst_dir + "/!res/"+dt_string
    os.makedirs(path_tst_dir_ress)
    tsts = cnf['tsts']
    for tst in tsts:
        print(tst)
        path_file_tst    = path_tst_dir + "/"+tst
        path_tst_dir_res = path_tst_dir_ress + "/" + os.path.dirname(tst)
        os.makedirs(path_tst_dir_res)
        #path_file_log= os.path.dirname(path_tst_dir_res) + "/" + Path(path_file_tst).stem + ".xml"
        path_file_log= path_tst_dir_res + "/" + Path(path_file_tst).stem + ".log"
        tst_mdp_file( rastr, path_file_tst, path_file_log )
    print("")
#---------------------------------------------------------------------------------------------------------
tsts_main()


