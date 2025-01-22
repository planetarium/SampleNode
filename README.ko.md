# SampleNode

프로젝트에 오신 것을 환영합니다! 🌍 다양한 언어로 제공되는 README 링크는 아래를 참고하세요:

- [English (영어)](README.md)


## 요구 사항

최신 C# 컴파일러와 .NET VM을 제공하는 .NET Core SDK 8.0 이상이 필요합니다. [.NET Core 다운로드 페이지](https://dotnet.microsoft.com/ko-kr/download)의 안내를 따라 .NET Core SDK를 설치하세요.

사용 중인 .NET Core SDK 버전이 8.0 이상인지 확인하세요. 사용 중인 버전을 확인하려면 `dotnet --info` 명령어를 실행하면 됩니다.


## 빌드

```
dotnet build
```


## 실행

### appsettings.Development.json

```json
    "Action": {
        "ActionLoaderType": "SampleAction.ActionLoader",
        "ModulePath": "<absolute-path-to-the-dll>/SampleAction.dll"
    }
```

우선 `ModulePath` 에 위 단계에 따라 빌드된 `SampleAction.dll` 의 절대 경로를 넣어주세요.


```
dotnet run --project ./SampleNode/SampleNode.csproj
```


## 프로젝트 소개

SampleNode는 [Libplanet.Node] 를 활용하여 Libplanet 블록체인 어플리케이션을 만드는 예제입니다. 모든 액션들은 기본적으로 [Bencodex]의 `IValue` 타입으로 다뤄지기 때문에 해당 타입에 대한 이해가 필요합니다.

[Libplanet.Node]: https://github.com/planetarium/libplanet/tree/main/sdk/node
[Bencodex]: https://github.com/planetarium/bencodex.net


이 예제는 다음의 두 프로젝트로 구성되어 있습니다.
- [SampleAction]: 어플리케이션을 로직을 구현하는 액션들과 그 로더가 구현되어있는 프로젝트입니다.
- [SampleNode]: 어플리케이션을 구성하고 SampleAction에서 구현한 액션을 담아 실제 블록체인 노드를 실행하는 프로젝트입니다.

[SampleAction]: ./SampleAction/
[SampleNode]: ./SampleNode/


### SampleAction

이 프로젝트는 크게 액션을 로드하는데 사용되는 `ActionLoader` 클래스와 액션 구현체들을 위한 부모 클래스인 `ActionBase` 클래스, 그리고 액션 구현체들도 이루어져 있습니다. 이 두 클래스는 수정하지 않고 그대로 사용할 수 있습니다.

`AddNumber` 는 `ActionBase` 를 상속받는 실제 액션 구현체의 예시로서 다음 몇 가지 필수 구성요소를 포함하고 있습니다.

1. `ActionType` attribute

```csharp
[ActionType("AddNumber")] 
```

액션의 구분자입니다. 해당 필드 누락시 액션 로드 단계에서 에러가 발생합니다.

2. `ActionBase` 부모 클래스 상속

```csharp
public class AddNumber : ActionBase
```

`ActionLoader` 에서 `ActionBase` 타입의 클래스들만 인식할 수 있기 때문에 로더를 수정 없이 그대로 사용하기 위해서는 이 클래스의 상속이 필요합니다.

3. `PlainValueInternal` 프로퍼티

```csharp
    protected override IValue PlainValueInternal { get; }
```

액션은 Libplanet 내부에서 `IValue` 타입의 직렬화된 값으로 다뤄집니다. 이 프로퍼티는 `ActionBase` 를 상속하는 액션의 직렬화를 구현합니다.

4. `LoadPlainValueInternal` 메서드

```csharp
    protected override void LoadPlainValueInternal(IValue plainValue)
    {
        ...
    }
```

앞서 설명한 `PlainValueInternal` 프로퍼티가 직렬화를 정의했다면, 이 메서드는 `IValue` 값을 `ActionBase` 를 상속하는 액션으로 바꾸는 역직렬화를 수행합니다.

5. `Execute` 메서드
```csharp
    public override IWorld Execute(IActionContext context)
    {
        ...
    }
```

이 메서드는 실제 액션 로직을 구현합니다. 자세한 구현 방법은 [이 문서]를 확인해주세요

[이 문서]:https://docs.libplanet.io/5.5.0/api/Libplanet.Action.IAction.html#Libplanet_Action_IAction_Execute_Libplanet_Action_IActionContext_


### SampleNode

#### Program.cs

```csharp
builder.Services.AddLibplanetNode(builder.Configuration);
```

Libplanet 관련 서비스들을 추가합니다.


#### RPC

[./Protos](./SampleNode/Protos/) 경로에서 RPC 관련 Protobuf 를 확인할 수 있습니다. 코드 수정 없이 기본적으로 5260 번 포트를 통해 접근할 수 있습니다.