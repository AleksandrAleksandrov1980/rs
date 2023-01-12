sc.exe stop srv_lin
dotnet publish "srv_lin.csproj" -c Release -o C:\projects\git_main\rs\srv_lin\publish -r win-x64 --self-contained -p:PublishTrimmed=true
sc.exe start srv_lin
sc.exe queryex srv_lin