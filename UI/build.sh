#! /bin/bash
echo "get go libs"
go get -v ./...

echo "building go ui application..."
CGO_ENABLED=0 GOOS=linux go build -tags=jsoniter -a -installsuffix cgo -o main .
echo "setting permission on file..."
chmod +x ./main

echo "Get Client Deps"
yarn install
echo "building client..."
ng version || yarn global add @angular/cli
ng build --prod