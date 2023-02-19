# Python program to explain os.system() method
	
# importing os module
import os

# Command to execute
# Using Windows OS command
command = f"""cqlsh localhost -u cassandra -p 'cassandra' -k 'azuredata' -e \" COPY azuredata.azurecost FROM '/mnt/blob/output/output.csv' with header = true; \" """

# Using os.system() method
os.system(command)
os.remove("/mnt/blob/output/output.csv")