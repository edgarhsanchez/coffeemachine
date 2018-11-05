# coffeemachine
A mono repo dedicated to showing the power of kubernetes in a simple micro service application

This application consists of three micro services, described below, which showcase common kubernetes scenarios including but not limited to pods, deployments, and services with emphasis on rolling deployments, blue green deployments, and canary deployments.  Each micro service is dedicated to a simple task as to not over complicate or detract from the primary focus of the project and repo.

Microservices
1.  Web UI.  Will show the current cups of coffee awaiting pickup, cups being created, and number of coffee machines with availability.
2.  Coffee Machine (1).  Workhorses of the application, creating coffee, and delivering these to the application.
3.  Barista.  Represents a order queue.  Only a single one is needed but more can be added.

All folders in this repo are labeled with obvious intent.

This project will grow to highlight particular aspects of microservices with kubernetes as needed to better demonstrate capabilities.  

## Project Setup

1.  Install all required tools
Windows--------
Install chocolatey  (https://chocolatey.org/install)
  choco install golang
  choco install docker

2. Install an ide or editor of choice (this author prefers Visual Studio Code)

3. Setup kubernetes (k8s)
  Enter settings screen in docker.  In windows this can be done by right clicking on the docker whale on the windows status icon.  Once in the settings screen Navigate to the k8s tab and modify the settings to enable k8s.

  ![Screenshot of k8s settings](https://github.com/edgarhsanchez/coffeemachine/blob/master/readmeimages/docker-setup-k8s.PNG)


(more to come)

