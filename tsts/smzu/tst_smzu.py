import sys
import os
import win32com.client as c32
import psutil

import json
import logging

from pathlib import Path
from datetime import datetime
import pythoncom
from pathlib import Path


def GetPathToScanDir():
    return r'D:\Vms\SHARA\crosses\ODU_SB\2022_11_18_archm\up'
def GetPathToScanDir7z():
    return r'D:\Vms\SHARA\crosses\ODU_SB\2022_11_18_archm'
def GetCentr():
    return 45000
def GetCentrs():
    return (15000,)
def GetScnaFName():
    return 'roc_debug_before_OC'
def Get_RG_REPL():
    return 1
def GetMaxOtv():
    return 18
def Get_max_otv(tables):
    table_node = tables('node')
    node_col_sta = table_node.Cols('sta')
    node_col_ny = table_node.Cols('ny')
    node_col_otv = table_node.Cols('otv')
    table_node.SetSel('sta=0')
    max_otv = 0
    max_otv_node_num = -1000
    curent_row_node = table_node.FindNextSel(-1)
    while curent_row_node != -1:
        sta = node_col_sta.Z(curent_row_node)
        ny = node_col_ny.Z(curent_row_node)
        otv = node_col_otv.Z(curent_row_node)
        if abs(max_otv) < abs(otv):
            max_otv = otv
            max_otv_node_num = ny
        #print('sta= '+str(sta)+ ' dv='+str(otv))
        curent_row_node = table_node.FindNextSel(curent_row_node)
    return (max_otv, max_otv_node_num)
def rfile_analyze(rastr,path_rfile,centr):
    rastr.Load(Get_RG_REPL(), path_rfile, '')
    try:
        tables = rastr.Tables
        table_com_opf = tables('com_opf')
        com_opf_col_centr = table_com_opf.Cols('centr')
        res1 = rastr.opf('s')
        (max_otv1, max_otv_node_num1) = Get_max_otv(tables)
        com_opf_col_centr.SetZ(0,centr)
        res2 = rastr.opf('s')
        rastr.rgm('')
        (max_otv2, max_otv_node_num2) = Get_max_otv(tables)
        str_res1 = ''
        str_res2 = ''
        if(abs(max_otv1) > GetMaxOtv() ) : str_res1 = "FAIL1" 
        if(abs(max_otv2) > GetMaxOtv() ) : str_res2 = "FAIL2" 
        print (str(path_rfile)+ ' : ' + str(res1) + ' - ' + str(res2) +'  '+ ' max_otv1= '+ str(max_otv1)+ ' max_otv2= '+ str(max_otv2)  + ' <> '+ str_res1 + ' - '+ str_res2  
        + ' : ny1=' + str(max_otv_node_num1)+ '  ny2=' + str(max_otv_node_num2) ) 
    except OSError as err:
        print("OS error:", err)
    except ValueError:
        print("Could not convert data to an integer.")
    except Exception as ex:
        print('type{type(ex)}')
    except :
        print('xz')
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
def scan_dir(path_2_scan_dir):
    print( "%x" % sys.maxsize, " x644? =", sys.maxsize > 2**32 )
    if(sys.maxsize > 2**32):
        print("64")
    else:
        print("32")
    #app = c32.gencache.EnsureDispatch('astra.rastr')
    CLSCTX_LOCAL_SERVER = 4
    rastr=c32.Dispatch('astra.rastr')      
    print(rastr.__module__)  
    blFindAstra = enum_loaded_dlls("astra.dll")
    if(blFindAstra == False):
        print("NotFind astra.dll")
        return
    print( 'scan-> ' + path_2_scan_dir )
    folder_names = os.listdir(path_2_scan_dir)
    #print(folder_names)
    for centr in GetCentrs() :
        print(centr)
        print('--------------------------- '+str(centr)+' -----------------------------------')
        for folder_name in folder_names:
            path_2_folder = path_2_scan_dir+'\\'+ folder_name
            #print(path_2_folder)
            if os.path.isdir(path_2_folder):
                #print(path_2_folder)
                path_2_file = path_2_folder+ '\\'+GetScnaFName()
                if os.path.isfile(path_2_file) :
                    #print (path_2_file)
                    rfile_analyze(rastr,path_2_file,centr)
                    continue
                else:
                    print ( f'no file: {path_2_file}' )
#---------------------------------------------------------------------------------------------------------
def rastr_set_val( rastr, namet, namec, val ):
    tables   = rastr.tables
    table    = tables(namet)
    cols     = table.cols
    col      = cols(namec)
    newVal   = c32.VARIANT(pythoncom.VT_VARIANT,val)
    nRow     = 0 # 
    #24                           = 0x00000018 in decimal: this is the Id of the property
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
        print(f"set_path_log:{path_file_mdp}")
        rastr_set_val(rastr, "ut_vir_common", "log_path2file", path_file_mdp )
        print(f"call Emergencies")
        newVal = c32.VARIANT(pythoncom.VT_VARIANT,0)
        nRes = rastr.Emergencies(newVal)
        path_file_save = os.path.dirname(path_file_log) + "/" + Path(path_file_log).stem + "____22222__.os"
        #rastr.Save( path_file_mdp+"_ress", '' )
        print(f"save {path_file_save}")
        rastr.Save( path_file_save, '' )
    except OSError as err:
        print("OS error:", err)
    except ValueError:
        print("Could not convert data to an integer.")
    except Exception as ex:
        print(f"EXCEPTION: {ex}")
    except :
        print('xz')
    print("thats all folks!")
#!!!!! win-service !!! path to json!!!
#---------------------------------------------------------------------------------------------------------
def read_conf():
   # with open("config.json") as json_data_file:
    p = psutil.Process( os.getpid() )
    p.cmdline()
    print(f"current scriptname: {os.path.basename(__file__)}")
    #with open("D:/rs/py_db_wrt/config.json") as json_data_file: # for WIN-SERVICE !!!
    path_json = os.getcwd() + "/"+"config.json";
    with open(path_json) as json_data_file: # for WIN-SERVICE !!!
        global JSON_CONFIG 
        JSON_CONFIG = json.load(json_data_file)
        return JSON_CONFIG
    return
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
        path_file_log= path_tst_dir_res + "/" + Path(path_file_tst).stem + ".xml"
        tst_mdp_file( rastr, path_file_tst, path_file_log )
    print("")
tsts_main()

#scan_dir( r'D:\Vms\SHARA\crosses\ODU_SB\2022_11_18_archm\up')
