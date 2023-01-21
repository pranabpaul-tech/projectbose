cqlsh -u aksuser -p password123 --execute=copy azuredata.azurecost from '/mnt/blob/output/output.csv' with header = true
rm -rf /mnt/blob/output/output.csv || true