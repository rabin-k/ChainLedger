#!/bin/bash

# Define the package version
VERSION=$(cat .version)
OUTPUT_DIR="./nupkg"

echo "Building version: $VERSION"

# Build
dotnet clean
dotnet build --configuration Release

# Run tests
TEST_RESULT="SUCCESS"
if dotnet test ChainLedger.sln --configuration Release --no-build --logger "console;verbosity=normal"; then
	TEST_RESULT="SUCCESS";
else
	TEST_RESULT="FAIL";
fi

echo "------------------"
echo "Test run completed"
echo "-------------------"

if [[ $TEST_RESULT=="SUCCESS" ]]; then
	if [ -d $OUTPUT_DIR ];
	then
		rm -r $OUTPUT_DIR
	fi

	# Pack the NuGet package
	dotnet pack ChainLedger.sln --configuration Release --output $OUTPUT_DIR -p:PackageVersion=$VERSION

	# Add the local package source if not already added
	#dotnet nuget add source "$\nupkg" --name LocalNuGetSource #--configfile NuGet.config

	# Display success message
	echo "Package built and added to local NuGet source. To use it, add the following source in your .csproj or NuGet.config:"
	echo "<add key='LocalNuGetSource' value='$(pwd)/nupkg' />"
else
	echo "Test failed. Please fix the error and build again"
fi