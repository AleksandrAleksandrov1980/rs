FOR /L %%A IN (1,1,100000) DO (
ECHO "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
ECHO CYCLE %%A
rem send_to_web.exe  "D:\Vms\SHARA\crosses\ODU_SZ\2023_08_31#1196833\00_06_59\roc_debug_after_OC"
send_to_web.exe  "D:\Vms\SHARA\crosses\ODU_SB\2023_07_13_smzu_dev\00_35_19\roc_debug_after_OC"
rem timeout 1
)