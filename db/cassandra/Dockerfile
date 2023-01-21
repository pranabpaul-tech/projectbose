FROM cassandra:3.11
ENV LANG C.UTF-8
COPY ./cql-scripts/ /docker-entrypoint-initdb.d/
COPY crontab /
COPY entrypoint.sh /
COPY insertdata.sh /
RUN ["chmod", "+x", "/entrypoint.sh"]
ENTRYPOINT ["/entrypoint.sh"]
CMD ["cassandra", "-f"]