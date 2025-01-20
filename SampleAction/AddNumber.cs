using Bencodex.Types;
using Libplanet.Action;
using Libplanet.Action.State;
using Libplanet.Crypto;

namespace SampleAction;

[ActionType("AddNumber")]
public class AddNumber : ActionBase
{
    public static readonly Address AddNumberAccount = new("0000000000000000000000000000000000000000");
    public static readonly Address AddNumberAddress = new("0000000000000000000000000000000000000000");

    protected override void LoadPlainValueInternal(IValue plainValue)
    {
        if (plainValue is not Integer value)
        {
            throw new ArgumentException(
                $"Given {nameof(plainValue)} has invalid values: {plainValue}",
                nameof(plainValue));
        }

        Value = value;
    }

    public override IWorld Execute(IActionContext context)
    {
        // Disable gas usage for test action
        //GasTracer.UseGas(1);
        
        var world = context.PreviousState;
        var account = world.GetAccount(AddNumberAccount);
        var state = account.GetState(AddNumberAddress);
        if (state is Integer integer)
        {
            account = account.SetState(AddNumberAddress, (Integer)(integer + Value));
        }
        else
        {
            account = account.SetState(AddNumberAddress, (Integer)Value);
        }

        return world.SetAccount(AddNumberAccount, account);
    }

    protected override IValue PlainValueInternal => (Integer)Value;

    public int Value = 0;
}