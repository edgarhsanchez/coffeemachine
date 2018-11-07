echo "performing a deletion of k8s deployments"
kubectl delete deployment coffeemachine-ui-deploy
kubectl delete deployment coffeemachine-barista-deploy
kubectl delete deployment coffeemachine-coffeemachine-deploy