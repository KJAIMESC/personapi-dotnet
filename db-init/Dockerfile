# Use the official SQL Server 2022 image (Developer edition) from Microsoft
FROM mcr.microsoft.com/mssql/server:2022-latest

# Ensure running as root
USER root

# Clean and prepare apt-get environment
RUN apt-get clean \
    && rm -rf /var/lib/apt/lists/* \
    && mkdir -p /var/lib/apt/lists/partial \
    && chmod -R 755 /var/lib/apt/lists

# Install basic utilities and prepare for ODBC installation
RUN apt-get update \
    && apt-get install -y curl apt-transport-https gnupg

# Add the Microsoft package repository for SQL Server tools
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update

# Remove conflicting older packages
RUN apt-get remove -y libodbc2 libodbcinst2 unixodbc-common

# Install ODBC dependencies and force overwrites during installation
RUN DEBIAN_FRONTEND=noninteractive apt-get install -y --allow-downgrades --allow-remove-essential --allow-change-held-packages odbcinst1debian2 odbcinst libodbc1 --no-install-recommends

# Install mssql-tools and unixodbc-dev
RUN ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev --no-install-recommends \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Add mssql-tools to the path
ENV PATH="$PATH:/opt/mssql-tools/bin"

# Copy the DDL file and script into the container
COPY personadb.ddl /tmp/personadb.ddl
COPY wait-for-sqlserver.sh /usr/src/app/wait-for-sqlserver.sh

# Make the script executable
RUN chmod +x /usr/src/app/wait-for-sqlserver.sh

# Expose SQL Server port
EXPOSE 1433

# Run SQL Server and execute the DDL file after SQL Server starts
CMD /bin/bash /usr/src/app/wait-for-sqlserver.sh && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -i /tmp/personadb.ddl
