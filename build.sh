cd UI
./build.sh

cd ..

cd Barista
./build.sh

cd ..

cd CoffeeMachine
./build.sh

cd ..

echo "building docker image..."
docker-compose build
echo "tag images"
docker tag coffeemachine_ui:latest coffeemachine_ui:1.0.0
docker tag coffeemachine_barista:latest coffeemachine_barista:1.0.0
docker tag coffeemachine_coffeemachine:latest coffeemachine_coffeemachine:1.0.0
echo "tagged images"