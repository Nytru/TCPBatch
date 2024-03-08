#!/bin/bash
#указываем где у нас хранится bash-интерпретатор
script_name=$0 #присваиваем переменной script_name значение имени скрипта
publish_path=$1
file_path=$2
rm -r -f "$publish_path"/TCP

rm -f "$publish_path"/run_server.sh
rm -f "$publish_path"/run_client.sh

if [[ -z "$file_path" ]]; then
    dotnet publish ./TCPServer -o "$publish_path"/TCP/TCPServer
    dotnet publish ./TCPClient -o "$publish_path"/TCP/TCPClient
else
    dotnet publish "$file_path"/TCPServer -o "$publish_path"/TCP/TCPServer
    dotnet publish "$file_path"/TCPClient -o "$publish_path"/TCP/TCPClient
fi

touch "$publish_path"/run_server.sh
touch "$publish_path"/run_client.sh

echo "$publish_path"/TCP/TCPServer/TCPServer '$1' '$2' '$3' '$4' '$5' '$6' '$7' '$8'>> "$publish_path"/run_server.sh
echo "$publish_path"/TCP/TCPClient/TCPClient '$1 $2' >> "$publish_path"/run_client.sh

chmod +x "$publish_path"/run_server.sh
chmod +x "$publish_path"/run_client.sh
exit 0 #Выход с кодом 0 (удачное завершение работы скрипта)
