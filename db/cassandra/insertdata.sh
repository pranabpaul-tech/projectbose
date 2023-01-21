cqlsh cassandraset-0 9042 -u aksuser -p password123
copy azuredata.azurecost (resourceid, resourcename, resourcetype, resourcegroupname, subscriptionid, region, usagedate, project, projectowner, cost, bu, division, costcenter) from '/mnt/blob/output/output.csv' with header = true;
exit
rm -rf /mnt/blob/output/output.csv || true