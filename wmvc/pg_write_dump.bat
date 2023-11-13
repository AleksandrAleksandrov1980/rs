rem pg_dump -h [host address] -Fc -o -U [database user] <database name> > [dump file]
rem Host=192.168.1.84;Database=rastrwin;Username=postgres;Password=pgadmin
rem SET PGPASSWORD="pgadmin"
"C:\Program Files\PostgreSQL\16\bin\pg_dump" -h 192.168.1.84 -Fc  -U postgres rastrwin > pg_dump.bin