#docker build -t ornekwebapihealthcheck .
#docker run -p 5010:80 -d --name myapp ornekwebapihealthcheck:latest
#docker inspect --format='' myapp  


//https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks


//IdentithServer'ı  contorl etmek için  HealthCheck.OpenIdConnectServer kullan.