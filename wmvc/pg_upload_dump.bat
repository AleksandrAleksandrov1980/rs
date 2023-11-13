rem pg_dump -h [host address] -Fc -o -U [database user] <database name> > [dump file]
rem Host=192.168.1.84;Database=rastrwin;Username=postgres;Password=pgadmin
rem SET PGPASSWORD="pgadmin"
rem "C:\Program Files\PostgreSQL\16\bin\pg_dump" -h 192.168.1.84 -Fc  -U postgres rastrwin > pg_dump.bin
rem "C:\Program Files\PostgreSQL\16\bin\pg_restore" -h msk-N9E-PSU5.ntc.ntcees.ru -p 5432 -Fc -U postgres -d rastrwin  "C:\projects\git_main\rs\wmvc\pg_dump.bin" -v
SET new_db="rastrwin"
SET db_dump_file="C:\projects\git_main\rs\wmvc\pg_dump.bin"
"C:\Program Files\PostgreSQL\16\bin\dropdb"   -h msk-N9E-PSU5.ntc.ntcees.ru -p 5432 -U postgres --if-exists  %new_db%
"C:\Program Files\PostgreSQL\16\bin\createdb" -h msk-N9E-PSU5.ntc.ntcees.ru -p 5432 -U postgres -T template0 %new_db%
"C:\Program Files\PostgreSQL\16\bin\pg_restore" -h msk-N9E-PSU5.ntc.ntcees.ru -p 5432 -U postgres -d %new_db% %db_dump_file%