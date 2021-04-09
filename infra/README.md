# Devtools build

This terraform setup creates a workstation on Hetzner with Teamcity.

## Install


1. Create Server

```
$ terraform plan
$ terraform apply -auto-approve
```
2. Copy output of terraform script and ssh into server
3. Configure the server
```
chmod +x installs.sh && chmod +x vpn.sh && sudo ./installs.sh && sudo ./vpn.sh
```
4. Copy vpn file on local machine 
```
scp -i ~/.ssh/restaurants user@server-ip:client.ovpn ~/Desktop
```
---
Teamcity Configurations:
```
docker-compose down
docker-compose up \
    --force-recreate \
    --build \
    -d
```