# Use the official SQL Server 2022 image (Developer edition) from Microsoft
FROM mcr.microsoft.com/mssql/server:2022-latest

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=StrongP@ssw0rd123

# Copy the DDL file into the container
COPY personadb.ddl /tmp/personadb.ddl

# Create an entrypoint script in /tmp to execute SQL commands after SQL Server starts
RUN echo '#!/bin/bash\n\
/opt/mssql/bin/sqlservr &\n\
sleep 30\n\
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /tmp/personadb.ddl\n\
wait' > /tmp/entrypoint.sh

# Make the entrypoint script executable
RUN chmod +x /tmp/entrypoint.sh

# Expose SQL Server port
EXPOSE 1433

# Switch to the mssql user
USER mssql

# Set the entrypoint script to run when the container starts
ENTRYPOINT ["/tmp/entrypoint.sh"]
