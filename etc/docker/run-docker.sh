#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p d1ee8a91-ffb6-448d-a38e-bc2420419bc0 -t
    fi
    cd ../
fi

docker-compose up -d
