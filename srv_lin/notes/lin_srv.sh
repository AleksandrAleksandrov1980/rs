#!/bin/bash
sudo systemctl stop srv_lin.service # stop service to release any file locks which could conflict with dotnet publish
dotnet publish /home/ustas/projects/git_main/rs/srv_lin/srv_lin.csproj -c Release -o /home/ustas/projects/git_main/rs/srv_lin/publish -r linux-x64 --self-contained -p:PublishTrimmed=true
sudo cp /home/ustas/projects/git_main/rs/srv_lin/srv_lin.service /etc/systemd/system/srv_lin.service
sudo systemctl daemon-reload
systemctl start srv_lin.service  
sudo systemctl status srv_lin.service

