import logging
import pathlib
import sys
from logging import StreamHandler, Formatter
from csv import reader

from decimal import Decimal

path_etalon = r"C:\cmp_ti\ia_Срез измерений за 02_01_00 19062024 .csv"
path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 04_01_00 19062024.csv"

#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 04_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 06_01_00 19062024.csv"

#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 12_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 14_01_00 19062024.csv"

#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 14_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 16_01_00 19062024.csv"

#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 16_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 18_01_00 19062024.csv"

#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 18_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 20_01_00 19062024.csv"

#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 20_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 22_01_00 19062024.csv"

#fun
#path_etalon = r"C:\cmp_ti\ia_Срез измерений за 02_01_00 19062024 .csv"
#path_source = r"C:\cmp_ti\ur_sfil_Срез измерений за 22_01_00 19062024.csv"


cmp_what       = 'Pij ВЛ' # какой параметр сравнивается
#cmp_what       = 'Qij ВЛ' # какой параметр сравнивается
#cmp_what       = 'U' # какой параметр сравнивается
#cmp_what       = 'ОТС' # какой параметр сравнивается
#cmp_what       = '№ анцапфы' # какой параметр сравнивается
cmp_length     = 60       # длина строки используемой для создания ключа ТИ
#cmp_length     = 25       # длина строки используемой для создания ключа ТИ 50=Pij 25=U 35 -№ анц
tol_proc       = 4.        # процент при превышении которого разница считается недопустимой
cmp_val_border = 10       # отсечение величины сравниваемого паметра, так как на Урале значения округлены, что приводит ложным срабатываниям

path_to_log = pathlib.Path(__file__).parent.resolve().as_posix()+"/_diff_ti.txt"
logging.basicConfig(filename=path_to_log, encoding = "utf-8", filemode='w', format='[%(asctime)s: %(name)s %(levelname)s ] %(message)s', datefmt='%Y-%m-%d %H:%M:%S' )
logging.basicConfig(level=logging.NOTSET)
logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)
handler = StreamHandler(stream=sys.stdout)
handler.setFormatter(Formatter(fmt='[%(asctime)s: %(levelname)s] %(message)s'))
logger.addHandler(handler)
logger.info(f"<------------------------- START! log [{path_to_log}] ------------------------->")

def get_key_from_tag(str_tag,indx):
    return str_tag[indx:indx+cmp_length]
def get_val(line):
    if(line[4] == 'Включено'):
        return 0
    if(line[4] == 'Отключено'):
        return 1
    val = Decimal(line[4].replace(',','.'))
    return val
source = {}
with open(path_source, encoding='utf-8') as in_file:
    csv_reader = reader(in_file,delimiter=';') 
    next(csv_reader) # skip name of sheet
    headers = [x.strip() for x in next(csv_reader)]# extract headers
    for line in csv_reader:
        if line: # if line is not empty
            str_tag = line[1]
            indx = str_tag.find(cmp_what)
            if indx >-1 :
                str_key = get_key_from_tag(str_tag,indx)
                source[str_key] = line
coincide = []
diff = []
absent = []
with open(path_etalon, encoding='utf-8') as in_file:
    csv_reader = reader(in_file,delimiter=';') 
    next(csv_reader) # skip name of sheet
    headers = [x.strip() for x in next(csv_reader)]# extract headers
    for line in csv_reader:
        if line: # if line is not empty
            # create dict for line            d = dict(zip(headers, map(str.strip, line)))
            str_tag = line[1]
            d_etalon_val = get_val(line)
            indx = str_tag.find(cmp_what)
            if indx >-1 :
                str_key = get_key_from_tag(str_tag,indx)
                if(str_key in source):
                    line_source = source[str_key]
                    d_source_val = get_val(line_source)
                    coincide.append([line, line_source])
                    diff_proc = 0
                    if( d_etalon_val != 0 and 
                        abs(d_etalon_val) > cmp_val_border 
                        ):
                        diff_proc = 100* (d_source_val - d_etalon_val) / d_etalon_val
                    if abs(diff_proc) > tol_proc:
                        diff.append({'diff_proc':diff_proc, 'etalon': line, 'source': line_source})
                    #logger.info(f'{str_key} : -> : {source[str_key]}')
                else:
                    absent.append(line)
                   #logger.warning(f'{str_key} : -> : NO!')
logger.info(f"etalon : {path_etalon}")
logger.info(f"source : {path_source}")
logger.info(f"compare: {cmp_what}")            
logger.info(f'coincide.count: {len(coincide)}')
logger.info(f'absent.count:   {len(absent)}')
logger.info(f'diff.count:     {len(diff)}')
i = 1  
for dif in diff:
    logger.error(f'---------------------------- {i} ------------------------------------------------')
    d_etalon = round(get_val(dif['etalon']), 3 )
    d_source = round(get_val(dif['source']), 3 )
    logger.error(f'{round(dif['diff_proc'],1)} % etalon [{d_etalon}] source [{d_source}] : [{dif['etalon'][2]}]')
    logger.error(f'etalon: {dif['etalon'][1]} ')
    logger.error(f'source: {dif['source'][1]} ')
    logger.error(f'etalon: {dif['etalon']} ')
    logger.error(f'source: {dif['source']} ')
    i = i + 1 
#logger.error(diff)

