cd UI
./build.sh

cd ..

cd Barista
./build.sh

cd ..

cd CoffeeMachine
./build.sh

cd ..

VERSION=$(git for-each-ref refs/tags --sort=-taggerdate --format='%(refname:short)' --count=1)

echo "building docker image: ${VERSION}..."
docker-compose build
echo "tag images..."
echo "tag ui local image"
docker tag coffeemachine_ui:latest coffeemachine_ui:${VERSION}
echo "tag ui aws image"
docker tag coffeemachine_ui:latest 087812398137.dkr.ecr.us-east-1.amazonaws.com/coffeemachine/ui:${VERSION}
docker tag coffeemachine_ui:latest 087812398137.dkr.ecr.us-east-1.amazonaws.com/coffeemachine/ui:latest
deocker push 
echo "tag barista local image"
docker tag coffeemachine_barista:latest coffeemachine_barista:${VERSION}
echo "tag barista aws image"
docker tag coffeemachine_barista:latest 087812398137.dkr.ecr.us-east-1.amazonaws.com/coffeemachine/barista:${VERSION}
docker tag coffeemachine_barista:latest 087812398137.dkr.ecr.us-east-1.amazonaws.com/coffeemachine/barista:latest
echo "tag coffeemachine local image"
docker tag coffeemachine_coffeemachine:latest coffeemachine_coffeemachine:${VERSION}
echo "tag coffeemachine aws image"
docker tag coffeemachine_coffeemachine:latest 087812398137.dkr.ecr.us-east-1.amazonaws.com/coffeemachine/coffeemachine:${VERSION}
docker tag coffeemachine_coffeemachine:latest 087812398137.dkr.ecr.us-east-1.amazonaws.com/coffeemachine/coffeemachine:latest
echo "tagged images"

echo "push images to aws ecr"
PROFILE=us-east-1
REPOSITORY=087812398137.dkr.ecr.us-east-1.amazonaws.com
echo "aws login to ecr"
aws ecr get-login --no-include-email --region ${REGION} --profile ${PROFILE} | awk '{printf $6}' | docker login -u AWS ${REPOSITORY} --password-stdin
echo "aws push ui"
docker push ${REPOSITORY}/coffeemachine/ui:${VERSION}
echo "aws push barista"
docker push ${REPOSITORY}/coffeemachine/barista:${VERSION}
echo "aws push coffeemachine"
docker push ${REPOSITORY}/coffeemachine/coffeemachine:${VERSION}