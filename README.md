# ComposeBuilderDotNet
Generate docker-compose YAML in C# using the builder design pattern

```csharp
var dbUser = "root";
var dbPass = "pass";
var dbName = "wordpress"; 

var network1 = Builder.MakeNetwork("my-net")
    .SetExternal(true)                       
    .Build();

var network2 = Builder.MakeNetwork("my-net2") 
    .Build();

var mysql = Builder.MakeService("db")
    .WithImage("mysql:5.7")
    .WithNetworks(network1)
    .WithExposed("3306") 
    .WithEnvironment(mb => mb
        .WithProperty("MYSQL_ROOT_PASSWORD", dbPass)
        .WithProperty("MYSQL_DATABASE", dbName)
        .WithProperty("MYSQL_USER", dbUser)
        .WithProperty("MYSQL_PASSWORD", dbPass)
    )
    .WithRestartPolicy(ERestartMode.Always) 
    .WithSwarm()
    .WithDeploy(d => d
        .WithMode(EReplicationMode.Replicated)
        .WithReplicas(3))
    .Build();

var wordpress = Builder.MakeService("wordpress")
    .WithImage("wordpress:latest")
    .WithNetworks(network1, network2)
    .WithPortMapping("8000:80") 
    .WithEnvironment(mb => mb
        .WithProperty("WORDPRESS_DB_HOST", $"{mysql.Name}:3306")
        .WithProperty("WORDPRESS_DB_USER", dbUser)
        .WithProperty("WORDPRESS_DB_PASSWORD", dbPass)
        .WithProperty("WORDPRESS_DB_NAME", dbName)
    )
    .WithDependencies(mysql) 
    .WithRestartPolicy(ERestartMode.UnlessStopped) 
    .WithSwarm()
    .WithDeploy(d => d
        .WithMode(EReplicationMode.Global) 
    )
    .Build();

var compose = Builder.MakeCompose()
    .WithServices(mysql, wordpress)
    .WithNetworks(network1, network2)
    .Build();

// serialize our object graph to yaml for writing to a docker-compose file
var result = compose.Serialize();

```