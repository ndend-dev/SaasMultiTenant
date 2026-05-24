#!/bin/bash

echo "Starting SQL Server connection health check..."

for i in {1..60}; do
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'SaasMultiTenant123%' -Q "SELECT 1" -t 1 -C > /dev/null 2>&1
   
    if [ $? -eq 0 ]; then
        echo "SQL Server is UP and READY! Executing script.sql..."
        /opt/mssql-tools18/bin/sqlcmd -S localhost,1433 -U sa -P "SaasMultiTenant123%" -i /usr/src/app/script.sql -C
        echo "Database creation completed successfully!"
        break
    else
        echo "SQL Server is still starting up... (Attempt $i/60)"
        sleep 2
    fi
done