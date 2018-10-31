echo "performing a local k8s deployment"
kubectl apply -f UI/deploy.yml --record
kubectl apply -f Barista/deploy.yml --record
kubectl apply -f CoffeeMachine/deploy.yml --record