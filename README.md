# coffeemachine
A mono repo dedicated to showing the power of kubernetes in a simple micro service application

This application consists of three micro services, described below, which showcase common kubernetes scenarios including but not limited to pods, deployments, and services with emphasis on rolling deployments, blue green deployments, and canary deployments.  Each micro service is dedicated to a simple task as to not over complicate or detract from the primary focus of the project and repo.

The production application is running in AWS EKS and is located (can be found) at <here>

Microservices
1.  Web UI.  Will show the current cups of coffee awaiting pickup, cups being created, and number of coffee machines with availability.
2.  Coffee Machine (1).  Workhorses of the application, creating coffee, and delivering these to the application.
3.  Barista.  Represents a order queue.  Only a single one is needed but more can be added.

All folders in this repo are labeled with obvious intent.

(more to come)
