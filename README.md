[![Build Status](https://github.com/aviationexam/docker-compose-builder/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/aviationexam/docker-compose-builder/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/DockerComposeBuilder.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DockerComposeBuilder/)
[![MyGet](https://img.shields.io/myget/docker-compose-builder/vpre/DockerComposeBuilder?label=MyGet)](https://www.myget.org/feed/docker-compose-builder/package/nuget/DockerComposeBuilder)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Faviationexam%2Fdocker-compose-builder%2Fshield%2FDockerComposeBuilder%2Flatest&label=DockerComposeBuilder)](https://f.feedz.io/aviationexam/docker-compose-builder/packages/DockerComposeBuilder/latest/download)

# DockerComposeBuilder

## The Goal
The goal of this project is to generate docker-compose literate YAML in C# using the builder design pattern

## About ComposeBuilderDotNet
This project was inspired by The need to create dynamically generated docker-compose files the right way,
without using templates and replacement techniques; as well as providing a wide range of developer friendly
methods and enums for quick access without the need to constantly check documentation.

## Example Code

```cs
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
