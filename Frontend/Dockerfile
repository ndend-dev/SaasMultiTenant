FROM node:22 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

RUN npm run build -- -c production

FROM nginx:alpine

COPY --from=build /app/dist/SaasMultiTenant/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]