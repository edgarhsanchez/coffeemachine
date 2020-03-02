helm uninstall coffeemachine
cd ./UI/client
npm run build
cd ../../
docker-compose build
docker push edgarhsanchez/coffeemachine-coffeemachine:1.0.0
docker push edgarhsanchez/coffeemachine-barista:1.0.0
docker push edgarhsanchez/coffeemachine-krakend:1.0.0
docker push edgarhsanchez/coffeemachine-ui:1.0.0
docker push edgarhsanchez/coffeemachine-brokerj:1.0.0
helm install coffeemachine ./k8s/coffeemachine/.