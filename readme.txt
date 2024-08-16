# Configuración del Entorno de Producción en CentOS

Este documento describe los pasos necesarios para configurar el entorno de producción en CentOS, incluyendo la instalación de .NET, SQLCMD y Nginx.

## Requisitos Previos

- CentOS Stream 8

## Pasos de Instalación

### 1. Actualizar el Sistema

Primero, asegúrate de que tu sistema está actualizado:

bash
sudo yum update -y
# o
sudo dnf update -y

### 2. Intalar dotnet para ejecutar la api

Agrega el repositorio de Microsoft:

bash
sudo rpm -Uvh https://packages.microsoft.com/config/centos/8/packages-microsoft-prod.rpm

Instala el SDK de .NET 8.0.4:

bash
sudo yum install dotnet-sdk-8.0 -y
# o
sudo dnf install dotnet-sdk-8.0 -y

### 3. Instalar SQLCMD
Para instalar SQLCMD, sigue estos pasos:

Importa la clave GPG de Microsoft:

bash
sudo rpm --import https://packages.microsoft.com/keys/microsoft.asc

Agrega el repositorio de Microsoft para SQL Server:

bash
sudo curl -o /etc/yum.repos.d/msprod.repo https://packages.microsoft.com/config/rhel/8/prod.repo

Instala SQLCMD:

bash
sudo yum install mssql-tools unixODBC-devel -y
# o
sudo dnf install mssql-tools unixODBC-devel -y

Agrega SQLCMD al PATH:

bash
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
source ~/.bash_profile

### 4. Instalar Nginx
Para instalar y configurar Nginx, sigue estos pasos:

Instala Nginx:

bash
sudo yum install nginx -y
# o
sudo dnf install nginx -y
Inicia y habilita el servicio de Nginx:

bash
sudo systemctl start nginx
sudo systemctl enable nginx
Configura Nginx para que redirija las solicitudes a tu aplicación .NET:

Crea un archivo de configuración para tu aplicación en /etc/nginx/conf.d/todoapi.conf:

nginx
Copy code
server {
    listen 80;
    server_name _;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
Reinicia Nginx para aplicar los cambios:

bash
sudo systemctl restart nginx

###5. Configuración del Firewall

Asegúrate de que el firewall permite el tráfico en el puerto 80:

bash
sudo firewall-cmd --zone=public --add-service=http --permanent
sudo firewall-cmd --reload

### 6. Verificación

Para verificar que todo está configurado correctamente, puedes probar acceder a tu API desde otro equipo en la misma red usando la dirección IP de tu servidor. Por ejemplo:

bash
curl -X POST http://<tu_ip_servidor>/api/O_MAIL_OBJECTS/buscar -H "Content-Type: application/json" -d '{"id":"CY575438213DE"}'

### 7. Listado de Paquetes Instalados
Para obtener una lista completa de los paquetes instalados, puedes usar el siguiente comando:

bash
Copy code
yum list installed > paquetes_instalados.txt
# o
dnf list installed > paquetes_instalados.txt
Puedes consultar este archivo para revisar todos los paquetes instalados:

bash
Copy code
cat paquetes_instalados.txt

######################################################################
Resumen de Comandos Ejecutados
Para comodidad, aquí tienes un resumen de todos los comandos ejecutados:

bash
# Actualización del sistema
sudo yum update -y
# o
sudo dnf update -y

# Instalación de .NET
sudo rpm -Uvh https://packages.microsoft.com/config/centos/8/packages-microsoft-prod.rpm
sudo yum install dotnet-sdk-8.0 -y
# o
sudo dnf install dotnet-sdk-8.0 -y


# Instalación de SQLCMD
sudo rpm --import https://packages.microsoft.com/keys/microsoft.asc
sudo curl -o /etc/yum.repos.d/msprod.repo https://packages.microsoft.com/config/rhel/8/prod.repo
sudo yum install mssql-tools unixODBC-devel -y
# o
sudo dnf install mssql-tools unixODBC-devel -y
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
source ~/.bash_profile

# Instalación de Nginx
sudo yum install nginx -y
# o
sudo dnf install nginx -y
sudo systemctl start nginx
sudo systemctl enable nginx

# Configuración de Nginx
sudo nano /etc/nginx/conf.d/todoapi.conf

# Reinicio de Nginx
sudo systemctl restart nginx

# Configuración del Firewall
sudo firewall-cmd --zone=public --add-service=http --permanent
sudo firewall-cmd --reload

# Listado de paquetes instalados
yum list installed > paquetes_instalados.txt
# o
dnf list installed > paquetes_instalados.txt
cat paquetes_instalados.txt

Con estos pasos, deberías tener tu entorno de producción configurado y listo para usar en CentOS. Asegúrate de revisar y ajustar cualquier comando o configuración según tus necesidades específicas.






