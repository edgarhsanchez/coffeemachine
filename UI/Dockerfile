
FROM node:latest as build
WORKDIR /app
COPY . . 
WORKDIR /app/client
RUN npm install --loglevel=error --no-update-modifier
RUN npm install -g @angular/cli --loglevel=error --no-update-modifier --unsafe
RUN npm run build

FROM nginx:stable
COPY --from=build /app/client/dist/client/ /usr/share/nginx/html