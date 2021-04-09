#!/bin/bash

if [ "$EUID" -ne 0 ]; then
    echo "You need to run this script as root"
    exit 1
fi

# Add user and add to group wheel
useradd -m user -s /bin/bash
echo root:root | chpasswd
echo user:user | chpasswd
usermod -aG sudo user

# Copy ssh keys to user
cd /home/user && mkdir .ssh && touch .ssh/authorized_keys && cat ~/.ssh/authorized_keys > .ssh/authorized_keys
cd /home/user && chmod 700 .ssh && chown user:user .ssh && chmod 600 .ssh/authorized_keys && chown user:user .ssh/authorized_keys

#To directly modify sshd_config.

# sed -i 's/#\?\(Port\s*\).*$/\1 2231/' /etc/ssh/sshd_config
sed -i '$ a AllowUsers user' /etc/ssh/sshd_config
sed -i 's/#\?\(PermitRootLogin\s*\).*$/\1 no/' /etc/ssh/sshd_config
sed -i 's/#\?\(PubkeyAuthentication\s*\).*$/\1 yes/' /etc/ssh/sshd_config
sed -i 's/#\?\(PermitEmptyPasswords\s*\).*$/\1 no/' /etc/ssh/sshd_config
sed -i 's/#\?\(PasswordAuthentication\s*\).*$/\1 no/' /etc/ssh/sshd_config

#Check the exit status of the last command
systemctl restart sshd

exit 0