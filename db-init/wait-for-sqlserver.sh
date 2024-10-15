#!/bin/bash

# Wait until SQL Server is ready
echo "Waiting for SQL Server to be ready..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q 'SELECT 1' &>/dev/null; do
  >&2 echo "SQL Server is starting up..."
  sleep 10
done

>&2 echo "SQL Server is up - executing command"