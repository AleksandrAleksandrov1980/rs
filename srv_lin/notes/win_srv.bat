rem sc.exe stop srv_lin
rem dotnet publish "C:\projects\git_main\rs\srv_lin\srv_lin.csproj" -c Release -o C:\projects\git_main\rs\srv_lin\publish -r win-x64 --self-contained -p:PublishTrimmed=true
dotnet publish "C:\projects\rastrservice\srv_lin\srv_lin.csproj" -c Release -o C:\projects\git_main\rs\srv_lin\publish -r win-x64 --self-contained -p:PublishTrimmed=true
rem sc.exe start srv_lin
rem sc.exe queryex srv_lin