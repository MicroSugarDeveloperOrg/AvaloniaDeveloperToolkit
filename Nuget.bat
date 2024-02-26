echo a = %1
echo b = %2

dotnet nuget push nuget\PrismToolkit.SourceGenerators.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json 
dotnet nuget push nuget\Rx.SourceGenerators.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget\Mvvm.SourceGenerators.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json