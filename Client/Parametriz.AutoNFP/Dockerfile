FROM node:22-bookworm-slim AS ng-builder

ARG ENVIRONMENT="production"

WORKDIR /app

COPY package*.json /app

RUN npm install

COPY . /app

RUN npm run build -- -c ${ENVIRONMENT}

FROM nginx:stable-bookworm AS ng-publish

COPY --from=ng-builder /app/dist/parametriz.auto-nfp /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
