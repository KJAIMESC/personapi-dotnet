# Use the official SQL Server 2022 image (Developer edition) from Microsoft
FROM mcr.microsoft.com/mssql/server:2022-latest

# Switch to root for installations
USER root

# Install dependencies and remove old ODBC packages
RUN apt-get update && \
    apt-get remove -y libodbc2 libodbcinst2 unixodbc-common && \
    apt-get install -y --no-install-recommends curl apt-transport-https gnupg && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y --no-install-recommends mssql-tools unixodbc-dev && \
    apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/*

# Copy the DDL file into the container
COPY personadb.ddl /tmp/personadb.ddl

# Create an entrypoint script to execute SQL commands after SQL Server starts
RUN echo '#!/bin/bash\n\
/opt/mssql/bin/sqlservr &\n\
sleep 40\n\
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /tmp/personadb.ddl\n\
wait' > /tmp/entrypoint.sh

# Make the entrypoint script executable
RUN chmod +x /tmp/entrypoint.sh

# Expose SQL Server port
EXPOSE 1433

# Switch back to the mssql user
USER mssql

# Set the entrypoint script to run when the container starts
ENTRYPOINT ["/tmp/entrypoint.sh"]
