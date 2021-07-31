Start-Process -FilePath 'dotnet' -WorkingDirectory '.\' -ArgumentList 'run --project ./mqtask.WebApi/mqtask.WebApi.csproj  --configuration Release --urls="http://localhost:5002;https://localhost:5003'
Start-Sleep 1
Start-Process -FilePath 'dotnet' -WorkingDirectory '.\' -ArgumentList 'run --project ./mqtask.WebApi/mqtask.WebApi.csproj  --configuration Release --urls="http://localhost:5004;https://localhost:5005'
Start-Sleep 1
Start-Process -FilePath 'dotnet' -WorkingDirectory '.\' -ArgumentList 'run --project ./mqtask.WebApi/mqtask.WebApi.csproj  --configuration Release --urls="http://localhost:5006;https://localhost:5007'
Start-Sleep 1
Start-Process -FilePath 'dotnet' -WorkingDirectory '.\' -ArgumentList 'run --project ./mqtask.WebApp/mqtask.WebApp.csproj  --configuration Release --urls="http://localhost:5008;https://localhost:5009'
Start-Sleep 1
Start-Process -FilePath 'dotnet' -WorkingDirectory '.\' -ArgumentList 'run --project ./mqtask.ReverseProxy/mqtask.ReverseProxy.csproj  --configuration Release --urls="http://localhost:5000;https://localhost:5001"'

