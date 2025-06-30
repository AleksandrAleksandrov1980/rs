import sys
import os
import logging
import json
from logging import StreamHandler, Formatter
from time import gmtime, strftime, localtime


print(sys.argv)

#path_etalon_fjson = 'C:\\mdp-test\\TESTS\\SMZU\\!res\\res_24_06_2024__09_40_39.737_new_ref\\_calc_.json'
#path_etalon_fjson = r'C:\mdp-test\TESTS/SMZU\!res\res_24_06_2024__09_40_39.737_new_ref\_calc_.json'
#path_new_fjson    = r'C:/mdp-test/TESTS/SMZU/!res/res_24_06_2024__10_18_25.201/_calc_.json'

#path_etalon_fjson = r'C:\mdp-test\TESTS/SMZU\!res\res_20_06_2024__15_36_36.726\_calc_.json'
#path_new_fjson    = r'C:/mdp-test/TESTS/SMZU/!res/res_20_06_2024__15_36_36.726/_calc_.json'
path_etalon_fjson = sys.argv[1]
path_new_fjson    = sys.argv[2]

path_new_dir = os.path.dirname(path_new_fjson)
logging.basicConfig(filename=path_new_dir+"/_cmp_.txt", encoding = "utf-8", filemode='w', format='[%(asctime)s: %(name)s %(levelname)s ] %(message)s', datefmt='%Y-%m-%d %H:%M:%S' )
logging.basicConfig(level=logging.NOTSET)
logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)
handler = StreamHandler(stream=sys.stdout)
handler.setFormatter(Formatter(fmt='[%(asctime)s: %(levelname)s] %(message)s'))
logger.addHandler(handler)
logger.info("<------------------------- START! ------------------------->")
logger.info(f'path_to_etalon: {path_etalon_fjson}')
logger.info(f'path_to_new   : {path_new_fjson}')
#-shared_fun--------------------------------------------------------------------------------------------------------
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
def parse_results_to_arr_dict(results):
    arr_dict_out = []
    i=0
    for arr in results[1]:
        if(i>=4):
            dict = {}
            for indx_field, name_field in enumerate(results[1][0]):
                value_field = results[1][i][indx_field]
                dict[name_field] = value_field
            arr_dict_out.append(dict)
        i = i + 1
    return arr_dict_out
#---------------------------------------------------------------------------------------------------------
def cmp_results(arr_cmp, arr_dict_etalon, arr_dict_new):
    arr_diffs = []
    for dict_etalon in arr_dict_etalon:
        find = False
        for dict_new in arr_dict_new:
            if( (dict_etalon["ns"]   == dict_new["ns"]  ) and
                (dict_etalon["nvir"] == dict_new["nvir"]) and
                (dict_etalon["npor"] == dict_new["npor"]) 
                ):
                find = True
                for cmp in arr_cmp:
                    diff = {}
                    diff['ns']         = dict_etalon["ns"]
                    diff['nvir']       = dict_etalon["nvir"]
                    diff['npor']       = dict_etalon["npor"]
                    diff['name']       = cmp['name']
                    diff['val_etalon'] = dict_etalon[cmp['name']]
                    diff['val_new']    = dict_new   [cmp['name']]
                    diff['error_cmp']  = ""
                    arr_diffs.append(diff)
        if(find == False) :
            diff = {}
            diff['ns']         = dict_etalon["ns"]
            diff['nvir']       = dict_etalon["nvir"]
            diff['npor']       = dict_etalon["npor"]
            diff['error_cmp']  = "no in new results!"
            arr_diffs.append(diff)
    return arr_diffs
#---------------------------------------------------------------------------------------------------------
def print_results(arr_cmp, arr_diffs):
    logger = logging.getLogger(__name__)
    #logger.info('<---------------- diffs -------------------------->')
    logger.info('<----------------  SECHS SUMMARY  -------------------------->')
    for cmp in arr_cmp:
        for diff in arr_diffs:
            if(len(diff['error_cmp'])>0):
                continue
            if(cmp['name']==diff['name']):
                if( (diff['nvir']==0) and
                    (diff['npor']==0) 
                    ):
                    val_etalon    = diff['val_etalon']
                    val_new       = diff['val_new']
                    val_diff_proc = 0.
                    if(val_etalon!=0):
                        val_diff_proc = 100*(val_new-val_etalon)/val_etalon
                    str_log = f'{diff['ns']:10} : {cmp['caption']:>10} : {val_etalon:8.1f}->{val_new:8.1f} = {val_etalon-val_new:4.1f} : {val_diff_proc:7.2f} %'
                    if(abs(val_diff_proc) < cmp['max_diff_proc']):
                        logger.info(str_log)    
                    else:
                        logger.error(str_log)
    logger.info('<----------------  COMPARE ERRORS  -------------------------->')
    for diff in arr_diffs:
        if(len(diff['error_cmp'])>0):
            logger.error(f'{diff['ns']:10}:s {diff['nvir']:5}:v {diff['npor']:5}:p error: {diff['error_cmp']}')  
    logger.info('<------------------------------------------------------------>')
#---------------------------------------------------------------------------------------------------------
arr_cmp = [ 
        { 'name': 'itog_mdp',       'max_diff_proc': 1   , "caption" : "МДП"        },
        { 'name': 'itog_mdp_pa',    'max_diff_proc': 1   , "caption" : "МДП+ПА"     },
        { 'name': 'padp_real',      'max_diff_proc': 1   , "caption" : "АДП"        },
        { 'name': 'itog_vdp_pa',    'max_diff_proc': 1   , "caption" : "ВДП"        },
        { 'name': 'itog_mdp_ap',    'max_diff_proc': 0.1 , "caption" : "АП->МДП"    },
        { 'name': 'itog_mdp_pa_ap', 'max_diff_proc': 0.1 , "caption" : "АП->МДП+ПА" },
        { 'name': 'kod_mdp',        'max_diff_proc': 0.1 , "caption" : "код МДП"    },
        { 'name': 'kod_mdp_pa',     'max_diff_proc': 0.1 , "caption" : "код МДП+ПА" },
        { 'name': 'kod_adp',        'max_diff_proc': 0.1 , "caption" : "код АДП"    },
        { 'name': 'kod_vdp_pa',     'max_diff_proc': 0.1 , "caption" : "код ВДП"    }
]
with open(path_etalon_fjson,encoding="utf-8") as file_etalon: 
    j_ress_etalon = json.load(file_etalon)
with open(path_new_fjson,encoding="utf-8") as file_new: 
    j_ress_new = json.load(file_new)
for j_res_etalon in j_ress_etalon:
    etalon_tst = j_res_etalon['tst']
    find = False
    for j_res_new in j_ress_new:
        new_tst = j_res_new['tst']
        if etalon_tst == new_tst :
            find = True
            logger.info(f'<------------------------------------------------------------------------------------------------>')
            logger.info(f'<---------------- tst[{etalon_tst}] -------------------------->')
            logger.info(f'<------------------------------------------------------------------------------------------------>')
            arr_dict_etalon = parse_results_to_arr_dict( j_res_etalon['results'] )
            arr_dict_new    = parse_results_to_arr_dict( j_res_new   ['results'] )
            arr_difss       = cmp_results( arr_cmp, arr_dict_etalon, arr_dict_new )
            print_results(arr_cmp, arr_difss)
    if find == False:
        logger.error("not found new results of test: {etalon_tst} !!!")
#---------------------------------------------------------------------------------------------------------
