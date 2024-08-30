#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p d9567e4c-2741-40fa-98bf-d56580f8b948 -t
    fi
    cd ../
fi

docker-compose up -d
