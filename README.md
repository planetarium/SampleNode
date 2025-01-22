# SampleNode

Welcome to the project! 🌍 Below are links to the README in various languages:

- [한국어 (Korean)](README.ko.md)


## Prerequisites

You need .NET Core SDK 8.0+ which provides the latest C# compiler and .NET VM. Read and follow the instruction to install .NET Core SDK on the [.NET Core downloads page](https://dotnet.microsoft.com/ko-kr/download).

Make sure that your .NET Core SDK is 8.0 or higher. You could show the version you are using by `dotnet --info` command.


## Build

```
dotnet build
```


## Run

### appsettings.Development.json

```json
    "Action": {
        "ActionLoaderType": "SampleAction.ActionLoader",
        "ModulePath": "<absolute-path-to-the-dll>/SampleAction.dll"
    }
```

Before running, set the `ModulePath` to the absolute path of `SampleAction.dll` built following the steps above.


```
dotnet run --project ./SampleNode/SampleNode.csproj
```


## About the project

SampleNode is a Libplanet blockchain application example using [Libplanet.Node]. All actions are dealt with [Bencodex]'s `IValue` type.

[Libplanet.Node]: https://github.com/planetarium/libplanet/tree/main/sdk/node
[Bencodex]: https://github.com/planetarium/bencodex.net


This example consists of two projects:
- [SampleAction]: Implements the actions for application logic and the loader for those actions.
- [SampleNode]: Configures the application, incorporates the actions implemented in SampleAction, and runs the actual blockchain node.

[SampleAction]: ./SampleAction/
[SampleNode]: ./SampleNode/


### SampleAction

This project consists of the `ActionLoader` class, used for loading actions, and the `ActionBase` class, which serves as the base class for action implementations. Additionally, it includes various action implementations. Both of these classes can be used as-is without modification.

`AddNumber` is an example of an actual action implementation that inherits from `ActionBase` and includes following several essential components.

1. `ActionType` attribute

```csharp
[ActionType("AddNumber")] 
```

An action identifier. An exception will be thrown if the field is missing.

2. Inherit `ActionBase` parent class

```csharp
public class AddNumber : ActionBase
```

Only `ActionBase`-typed classes can be read from `ActionLoader`, all actions must inherit `ActionBase` class to use `ActionLoader` as-is without modification.

3. `PlainValueInternal` property

```csharp
    protected override IValue PlainValueInternal { get; }
```

Actions are serialized as `IValue`-typed value inside Libplanet. This property defines serialization of the action that inherits `ActionBase` classes.

4. `LoadPlainValueInternal` method

```csharp
    protected override void LoadPlainValueInternal(IValue plainValue)
    {
        ...
    }
```

Defines deserialization of the action that inherits `ActionBase` classes.

5. `Execute` method

```csharp
    public override IWorld Execute(IActionContext context)
    {
        ...
    }
```

Implements actual action logic. For implementation detail, check [`IAction` document].

[`IAction` document]:https://docs.libplanet.io/5.5.0/api/Libplanet.Action.IAction.html#Libplanet_Action_IAction_Execute_Libplanet_Action_IActionContext_


### SampleNode

#### Program.cs

```csharp
builder.Services.AddLibplanetNode(builder.Configuration);
```

Injects Libplanet related services.


#### RPC

RPC related protobufs are defined in [./Protos](./SampleNode/Protos/). Basically port number 5260 is opened for RPC communication.