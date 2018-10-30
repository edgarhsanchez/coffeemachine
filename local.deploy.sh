echo "performing a local k8s deployment"
kubectl apply -f ui/deploy.yml --record
kubectl apply -f barista/deploy.yml --record
kubectl apply -f coffeemachine/deploy.yml --record