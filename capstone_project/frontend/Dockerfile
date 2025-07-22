# Stage 1: Build Angular App
FROM node:24-alpine3.21 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install --force
COPY . .
RUN npm install -g @angular/cli
RUN ng build --configuration production

# Stage 2: Serve via NGINX
FROM nginx:alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]