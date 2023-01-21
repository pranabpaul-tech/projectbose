FROM cassandra:3.11
ENV LANG C.UTF-8
COPY ./cql-scripts/ /docker-entrypoint-initdb.d/
COPY entrypoint.sh /
COPY crontab /
RUN ["chmod", "+x", "/entrypoint.sh"]
ENTRYPOINT ["/entrypoint.sh"]
CMD ["cassandra", "-f"]