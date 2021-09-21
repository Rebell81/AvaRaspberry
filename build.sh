#!/usr/bin/env bash

echo "Started build and run project script."
echo "$SECRET_PASSPHRASE"
echo $SECRET_PASSPHRASE
# Configure linux machine

# Explicitly setting DISPLAY=:0 is usually a way to access a machine's local display from outside the local session
export DISPLAY=:0

# Restore and build
dotnet restore
dotnet build --configuration Release

cd AvaRaspberry/bin/Release/net5.0/ || exit

# Decrypt AppSettings config with help Github Secrets 
gpg --quiet --batch --yes --decrypt --passphrase="$SECRET_PASSPHRASE" --output appsettings.json appsettings.json.gpg

# Run
( dotnet AvaRaspberry.dll & )
echo "Build and run project script is executed."

