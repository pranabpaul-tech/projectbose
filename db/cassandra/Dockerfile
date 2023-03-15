FROM cassandra:3.11
ENV LANG C.UTF-8
COPY db/cassandra/cql-scripts/ /docker-entrypoint-initdb.d/
COPY db/cassandra/entrypoint.sh /
COPY db/cassandra/insertdata.py /
RUN ["chmod", "+x", "/entrypoint.sh"]
ENTRYPOINT ["/entrypoint.sh"]
CMD ["cassandra", "-f"]