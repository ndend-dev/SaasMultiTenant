FROM mcr.microsoft.com/mssql/server:2022-latest

WORKDIR /usr/src/app

COPY script.sql /usr/src/app/
COPY import-data.sh /usr/src/app/

USER root
RUN chmod +x /usr/src/app/import-data.sh

USER mssql

ENTRYPOINT ["/bin/bash", "-c", "/usr/src/app/import-data.sh & /opt/mssql/bin/sqlservr"]



FROM mcr.microsoft.com/mssql/server:2022-latest

WORKDIR /usr/src/app

COPY script.sql /usr/src/app/
COPY import-data.sh /usr/src/app/

USER root

RUN apt-get update && apt-get install -y dos2unix && rm -rf /var/lib/apt/lists/*
RUN dos2unix /usr/src/app/import-data.sh
RUN chmod +x /usr/src/app/import-data.sh

USER mssql

ENTRYPOINT ["/bin/bash", "-c", "/usr/src/app/import-data.sh & /opt/mssql/bin/sqlservr"]