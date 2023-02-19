# Python program to explain os.system() method
	
# importing os module
import os

# Command to execute
# Using Windows OS command
command = "cqlsh localhost -u cassandra -p 'cassandra' -e \" COPY azuredata.azurecost (resourceid,usagedate,resourcename,region,resourcetype,cost,projectname,projectowneremail,resourcegroupname,subscriptionid,BU,Division,CostCenter) FROM '/mnt/blob/output/outputdata.csv' with header = true; \" "

# Using os.system() method
os.system(command)
os.remove("/mnt/blob/output/outputdata.csv")