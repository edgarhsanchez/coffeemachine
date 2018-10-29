cd UI
./build.sh

cd ..

cd Barista
./build.sh

cd ..

echo "building docker image..."
docker-compose build
echo "tag images"
docker tag coffeemachine_ui:latest coffeemachine_ui:v1
echo "tagged images"