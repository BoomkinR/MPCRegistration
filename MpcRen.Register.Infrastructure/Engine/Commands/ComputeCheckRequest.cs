using System.Numerics;

namespace MpcRen.Register.Infrastructure.Engine.Commands;

public class ComputeCheckRequest
{
    public (BigInteger[], BigInteger[], BigInteger[]) Shares { get; set; }
}