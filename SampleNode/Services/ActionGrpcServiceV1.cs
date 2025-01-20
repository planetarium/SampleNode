using Bencodex.Types;
using Grpc.Core;
using Libplanet.Crypto;
using Libplanet.Node.Options;
using Libplanet.Node.Services;
using Microsoft.Extensions.Options;
using SampleAction;

namespace SampleNode.Services;

public class ActionGrpcServiceV1(
    IOptions<SoloOptions> soloProposeOption,
    IBlockChainService blockChainService) : Action.ActionBase
{
    private readonly Libplanet.Blockchain.BlockChain _blockChain = blockChainService.BlockChain;
    private readonly PrivateKey _privateKey = soloProposeOption.Value.PrivateKey == string.Empty
        ? new PrivateKey()
        : PrivateKey.FromString(soloProposeOption.Value.PrivateKey);

    public override Task<AddNumberReply> AddNumber(
        AddNumberRequest request,
        ServerCallContext context)
    {
        var action = new AddNumber { Value = request.Value };
        var tx = _blockChain.MakeTransaction(_privateKey, [action]);
        return Task.FromResult(
            new AddNumberReply
            {
                TxId = tx.Id.ToHex()
            });
    }

    public override Task<GetNumberReply> GetNumber(
        GetNumberRequest request,
        ServerCallContext context)
    {
        var value = _blockChain.GetWorldState()
            .GetAccountState(SampleAction.AddNumber.AddNumberAccount)
            .GetState(SampleAction.AddNumber.AddNumberAddress);
        if (value is not Integer intValue)
        {
            throw new Exception("State is empty.");
        }
        
        return Task.FromResult(new GetNumberReply
        {
            Value = intValue
        });
    }
}