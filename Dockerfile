# Use the official Ubuntu base image
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Copy files from the host to the container
COPY ./ ./TCP

# Start the container in interactive mode
CMD ["bash"]
