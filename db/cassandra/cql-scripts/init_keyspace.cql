CREATE KEYSPACE azuredata WITH REPLICATION = {'class': 'SimpleStrategy', 'replication_factor': 1};

CREATE USER IF NOT EXISTS aksuser WITH PASSWORD 'password123';


GRANT ALL PERMISSONS ON KEYSPACE azuredata TO aksuser;