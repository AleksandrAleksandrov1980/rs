import sys
import os
import win32com.client as c32
import psutil
import json
from pathlib import Path
from datetime import datetime
import pythoncom
from pathlib import Path
import time
import logging
from logging import StreamHandler, Formatter
from tabulate import tabulate
tabulate.WIDE_CHARS_MODE = False

#set-executionpolicy remotesigned
#python -m pip install --upgrade pywin32
#pip install psutil
#pip install tabulate
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
def rastr_set_val( rastr, logg, namet, namec, val ):
    tables   = rastr.tables
    table    = tables(namet)
    cols     = table.cols
    col      = cols(namec)
    newVal   = c32.VARIANT(pythoncom.VT_VARIANT,val)
    nRow     = 0 # 
    logg.info(f"{namet}.{namec}: {val}")
    #4                            = 0x00000004 in decimal: this is the Id of the property
    #0                            = the LCID, the Locale Id ... I always set it to 0
    #pythoncom.INVOKE_PROPERTYPUT = the type of call.
    #0                            = whether you care about the return type (you probably don't = False)
    #0                            = first parameter, lAcq, as in CurrentDevice(0)
    #newVal                       = second paramter,pVal, the new device name as a VARIANT
    col._oleobj_.Invoke( 4, 0, pythoncom.INVOKE_PROPERTYPUT, 0, nRow, val )
#---------------------------------------------------------------------------------------------------------
def tst_mdp_file( rastr, path_file_mdp : str, path_dir_out : str, path_file_log :str, logg ):
    try:
        logg.info(f"read_file_mdp:{path_file_mdp}")
        logg.info(f"\n <B> <a href=\"{os.path.dirname(path_file_mdp)}\" >  { os.path.dirname(path_file_mdp) } </a> </B>");
        rastr.Load( Get_RG_REPL(), path_file_mdp, 'C:/Program Files (x86)/RastrWin3/RastrWin3/SHABLON/poisk.os' ) #Загружаем по шаблону poisk.os, чтобы новые поля добавлялись в расчет

        rastr_set_val(rastr, logg, "com_regim",     "neb_p",     0.01  ) #
        rastr_set_val(rastr, logg, "com_regim",     "start",      1    ) # 0-yes 1-no    ,0,1,0,1
        rastr_set_val(rastr, logg, "com_regim",     "flot",       0    ) # 0-no  1-yes   ,1,1,0,0 
        rastr_set_val(rastr, logg, "ut_vir_common", "ParNumProc", 10   ) # 

        rastr_set_val(rastr, logg, "ut_vir_common", "kod", 22 )
        logg.info(f"dir_out: {path_dir_out} ")
        logg.info(f"set_path_log:{path_file_log}")
        rastr_set_val(rastr, logg, "ut_vir_common", "log_path2file", path_file_log )
        path_file_save_1 =  path_dir_out + "/" + Path(path_file_log).stem + "____11111__.os"
        rastr.Save( path_file_save_1, '' )
        logg.info(f"call Emergencies")
        newVal = c32.VARIANT(pythoncom.VT_VARIANT,0)
        results = rastr.Emergencies(newVal)
        path_file_save_2 = path_dir_out + "/" + Path(path_file_log).stem + "____22222__.os"
        logg.info(f"results   [{results}]")
        logg.info(f"save file {path_file_save_2}")
        rastr.Save( path_file_save_2, '' )
        return results
    except OSError as err:
        logg.info("OS error:", err)
    except ValueError:
        logg.info("Could not convert data to an integer.")
    except Exception as ex:
        logg.info(f"EXCEPTION: {ex}")
    except :
        logg.info('exception xz')
    logg.info("ERROR!")
    return
#---------------------------------------------------------------------------------------------------------
def get_name__current_script():
    return os.path.basename(__file__)
#---------------------------------------------------------------------------------------------------------
def read_conf():
    dir_current_script = os.getcwd()
    p = psutil.Process( os.getpid() )
    p.cmdline()
    name_current_script = get_name__current_script()
    #for cmdline in p.cmdline():
    #    if(cmdline.endswith(name_current_script)):
    #        dir_current_script = os.path.dirname(cmdline)
     #!!!!! win-service !!! path to json!!!
    print(f"current scriptname: {os.path.basename(__file__)}")
    print(f"directory of current script: {dir_current_script}")
    #with open("D:/rs/py_db_wrt/config.json") as json_data_file: # for WIN-SERVICE !!!
    #path_json = os.getcwd() + "/"+"config.json";
    path_json = dir_current_script + "/"+"config.json"
    with open(path_json,encoding="utf-8") as json_data_file: # for WIN-SERVICE !!!
        jcnf = json.load(json_data_file)
    return jcnf
#---------------------------------------------------------------------------------------------------------
def find_field_indx(results, namef ):
    found = False
    i = 0
    for name_f in results[1][0]:
        if(name_f == namef):
            found = True
            break
        i += 1
    if(found == False):
         raise Exception( "not find index for "+ namef)
    return i
#---------------------------------------------------------------------------------------------------------
def results_trace(results, logg):
    try:
        logg.info( f"kod={results[0]}" )
        indx_ns          = find_field_indx( results, "ns"          )
        indx_kod         = find_field_indx( results, "kod"         )
        indx_nvir        = find_field_indx( results, "nvir"        )
        indx_npor        = find_field_indx( results, "npor"        )
        indx_name_schem  = find_field_indx( results, "name_schem"  )
        indx_itog_mdp    = find_field_indx( results, "itog_mdp"    )
        indx_itog_mdp_pa = find_field_indx( results, "itog_mdp_pa" )
        indx_padp_real   = find_field_indx( results, "padp_real"   )
        indx_itog_mdp_ap = find_field_indx( results, "itog_mdp_ap" )
        indx_itog_mdp_pa_ap = find_field_indx( results, "itog_mdp_pa_ap" )
        indx_kod_mdp     = find_field_indx( results, "kod_mdp"     )
        indx_kod_mdp_pa  = find_field_indx( results, "kod_mdp_pa"  )
        data = [[ "ns", "kod", "nvir","npor","name_schem","itog_mdp", "itog_mdp_pa", "padp_real","itog_mdp_ap", "itog_mdp_pa_ap", "kod_mdp",  "kod_mdp_pa"],]
        i = 0
        for arr in results[1]:
            if(i>=4):
                ns          = arr[ indx_ns          ]
                kod         = arr[ indx_kod         ]
                nvir        = arr[ indx_nvir        ]
                npor        = arr[ indx_npor        ]
                name_schem  = arr[ indx_name_schem  ]
                itog_mdp    = arr[ indx_itog_mdp    ]
                itog_mdp_pa = arr[ indx_itog_mdp_pa ]
                padp_real   = arr[ indx_padp_real   ]
                itog_mdp_ap = arr[ indx_itog_mdp_ap ]
                itog_mdp_pa_ap = arr[indx_itog_mdp_pa_ap]
                kod_mdp     = arr[ indx_kod_mdp     ]
                kod_mdp_pa  = arr[ indx_kod_mdp_pa  ]
                if( (ns>0) and (nvir==0) and (npor==0) ):
                    data.append( [ ns, kod, nvir, npor, name_schem, itog_mdp, itog_mdp_pa, padp_real, itog_mdp_ap, itog_mdp_pa_ap, kod_mdp, kod_mdp_pa ] )
                    round_to_dig = 1
                    logg.info( f"[{ns}] [{name_schem}] kod=[{kod}]  mdp=[{ round(itog_mdp,round_to_dig) }]  mdp_pa=[{ round(itog_mdp_pa,round_to_dig) }]  adp=[{ round(padp_real, round_to_dig) }] itog_mdp_ap=[{itog_mdp_ap}] itog_mdp_pa_ap=[{itog_mdp_pa_ap}] kod_mdp=[{kod_mdp}] kod_mdp_pa=[{kod_mdp_pa}]" )
            i += 1
        #print(tabulate(data, headers="firstrow", tablefmt='rounded_grid', stralign='center', colalign=("left",) ))
        print(tabulate( data, headers="firstrow", tablefmt='rounded_grid' ))
        str_table = tabulate( data, headers='firstrow', tablefmt='html')
        logg.info( f"\n{ str_table }" )
        #print(tabulate( data, headers="firstrow", tablefmt='rounded_grid' ))
    except Exception as ex:
        logg.info(f"EXCEPTION: {ex}")
    except :
        logg.info('exception xz')
#---------------------------------------------------------------------------------------------------------
def tsts_main():
    tm_start_all      = time.time()
    tm_start_proc_all = time.process_time()
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
    path_tst_dir_ress = path_tst_dir + "/!res/"+dt_string+"/"
    os.makedirs(path_tst_dir_ress)
    #logging.basicConfig(filename=path_tst_dir_ress+"/_calc_.log", filemode='w', format='[%(asctime)s %(name)s %(levelname)s ] %(message)s', datefmt='%Y-%m-%d %H:%M:%S' )
    logging.basicConfig(filename=path_tst_dir_ress+"/_calc_.html", filemode='w', format='[%(name)s %(levelname)s ] %(message)s<br>', datefmt='%Y-%m-%d %H:%M:%S' )
    logging.basicConfig(level=logging.NOTSET)
    logger = logging.getLogger(__name__)
    logger.setLevel(logging.DEBUG)
    handler = StreamHandler(stream=sys.stdout)
    handler.setFormatter(Formatter(fmt='[%(asctime)s: %(levelname)s] %(message)s'))
    logger.addHandler(handler)
    logger.info("<h2>")
    logger.info(f"************************************ {dt_string} ****************************************************")
    logger.info("</h2>")
    path_json_ress = path_tst_dir_ress+"/_calc_.json"
    tsts = cnf['tsts']
    i = 0
    for tst in tsts:
        logger.info("<p>")
        i += 1
        tm_start      = time.time()
        tm_start_proc = time.process_time()
        logger.info("<h3>")
        logger.info("****************************************************************************************************")
        logger.info(f"[{i}] from [{len(tsts)}] : {tst}")
        logger.info("****************************************************************************************************")
        logger.info("</h3>")
        path_file_tst     = path_tst_dir + "/" + tst                  # in dmp
        path_tst_dir_out  = path_tst_dir_ress  + os.path.dirname(tst) # out dumps
        path_tst_dir_res  = path_tst_dir_out + "/calc/"               # wrk folder
        os.makedirs(path_tst_dir_res)
        path_file_log     = os.path.dirname(path_tst_dir_res) + "/" + Path(path_file_tst).stem + ".log"
        results           = None
        results           = tst_mdp_file( rastr, path_file_tst, path_tst_dir_out, path_file_log, logger )
        time_elapsed      = time.time() - tm_start
        time_elapsed_proc = time.process_time() - tm_start_proc
        logger.info("----------------------------------------------------------------------------------------------------")
        logger.info( f"test execution time: { time.strftime( '%H:%M:%S', time.gmtime(time_elapsed) )}   CPU execution time:  {time_elapsed_proc}  " )
        logger.info("----------------------------------------------------------------------------------------------------")
        path_file_xml     = os.path.dirname(path_tst_dir_res) + "/" + Path(path_file_tst).stem + ".xml" # simple parse out array!
        logger.info(f"\n <B> <a href=\"{os.path.dirname(path_file_tst)}\" >  { os.path.dirname(path_file_tst) } </a> </B>");
        if results is None:
            logger.error("GET NO RESULTS!! HALT!")
            break
        else:
            results_trace( results, logger ) #parse out array
            j_ress = []
            if os.path.isfile(path_json_ress):
                with open(path_json_ress,encoding="utf-8") as j_file: 
                    j_ress = json.load(j_file)
            j_ress.append( {
                "tst": tst, 
                "results": results 
            })
            with open(path_json_ress, 'w', encoding='utf-8') as f:
                json.dump(j_ress, f, ensure_ascii=False, indent=1)
        logger.info("</p>")
    logger.info("************************************** TOTAL **************************************************************")
    time_elapsed_all      = time.time() - tm_start_all
    time_elapsed_proc_all = time.process_time() - tm_start_proc_all
    logger.info( f"TOTAL: execution time: { time.strftime( '%H:%M:%S', time.gmtime(time_elapsed_all) )}   CPU execution time:  {time_elapsed_proc_all}  " )
    logger.info("***********************************************************************************************************")
    path_tst_dir = ""
    last_results_file = open("lastresults.txt", "w", encoding="utf-8") # !!!! + , encoding="utf-8"
    last_results_file.write(dt_string)
    last_results_file.close()
    return
#---------------------------------------------------------------------------------------------------------
tsts_main()



