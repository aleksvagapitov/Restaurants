#!/bin/bash
if [ "$EUID" -ne 0 ]; then
    echo "You need to run this script as root"
    exit 1
fi

# Default Installations
apt autoremove -y
add-apt-repository -y ppa:keithw/mosh 
apt-get update && apt-get upgrade -y

apt-get install -y man
apt-get install -y vim
apt-get install -y wget
apt-get install -y curl
apt-get install -y nginx
# python3-pip \
# apt-transport-https \
# ca-certificates \
# software-properties-common \
# python-software-properties \
# mosh \

# OpenVPN setup
# wget https://git.io/vpn -O vpn.sh
chmod +x vpn.sh

# Setup Dnsmasq
systemctl disable systemd-resolved
sudo systemctl stop systemd-resolved
rm /etc/resolv.conf
cat > /etc/resolv.conf << EOL
nameserver 127.0.0.1
nameserver 8.8.8.8
EOL
apt-get install -y dnsmasq
cat > /etc/dnsmasq.conf << EOL
port=53
domain-needed
bogus-priv
strict-order
expand-hosts
domain=restaurants.tools
listen-address=127.0.0.1
listen-address=10.8.0.1
EOL
systemctl restart dnsmasq
sed -i "/127.0.0.1/ a 172.17.0.1 www.restaurants.tools" /etc/hosts
sed -i "/127.0.0.1/ a 172.17.0.1 backend.restaurants.tools" /etc/hosts
sed -i "/127.0.0.1/ a 172.17.0.1 teamcity.restaurants.tools" /etc/hosts
sed -i "/127.0.0.1/ a 172.17.0.1 registry.restaurants.tools" /etc/hosts
sed -i "/127.0.0.1/ a 172.17.0.1 gitea.restaurants.tools" /etc/hosts
sed -i "/127.0.0.1/ a 172.17.0.1 mailhog.restaurants.tools" /etc/hosts
sed -i "/127.0.0.1/ a 172.17.0.1 minio.restaurants.tools" /etc/hosts
# systemctl restart bind9
systemctl restart dnsmasq
# dig A dev.tools | grep "172.17.0.1"

# Setup Nginx
mv nginx.conf /etc/nginx/sites-available/default
systemctl restart nginx

# Install Docker
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu bionic stable"
apt update
apt install -y docker-ce

usermod -aG docker user

# Install Docker-Compose
curl -L "https://github.com/docker/compose/releases/download/1.24.0/docker-compose-$(uname -s)-$(uname -m)"  -o /usr/local/bin/docker-compose
mv /usr/local/bin/docker-compose /usr/bin/docker-compose
chmod +x /usr/bin/docker-compose

# Docker setup
systemctl stop docker #or service docker stop
cat > /etc/docker/daemon.json << EOL
{
    "iptables" : false,
    "experimental": true,
    "insecure-registries" : ["my-registry:5000"]
}
EOL
sed -i 's/#DOCKER_OPTS="--dns 8.8.8.8 --dns 8.8.4.4"/DOCKER_OPTS="--dns 8.8.8.8 --dns 8.8.4.4 --iptables=false"/' /etc/default/docker
echo 1 > /proc/sys/net/ipv4/ip_forward
iptables-restore < rules.v4
systemctl start docker #or service docker start
docker-compose up -d

# Install Neofetch

# add-apt-repository ppa:dawidd0811/neofetch
# apt update
# apt install neofetch

# Configure zsh

# timedatectl set-timezone America/New_York
# apt-get install zsh git curl -y
# chsh -s $(which zsh)
# sudo apt-get install powerline fonts-powerline
# git clone https://github.com/robbyrussell/oh-my-zsh.git ~/.oh-my-zsh
# cp ~/.oh-my-zsh/templates/zshrc.zsh-template ~/.zshrc
# git clone https://github.com/romkatv/powerlevel10k.git $ZSH_CUSTOM/themes/powerlevel10k
# git clone https://github.com/zsh-users/zsh-autosuggestions ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-autosuggestions

# plugins=(git zsh-autosuggestions)

#Check the exit status of the last command

if (( "${?}" != 0 )); then
   echo "Updates were not successful."
   exit 1
fi

exit 0