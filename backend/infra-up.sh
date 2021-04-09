docker container prune -f
docker volume rm -f backend_postgres-data
docker-compose -f infra.yml up
docker container prune -f
docker volume rm -f backend_postgres-data