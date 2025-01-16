using System;
using Controllers;
using Core.Interfaces.Entities;

public static class TokenFactory
{
    public static SOsManager sosManager;
    public static ITokenController NewToken(TokensNames tokenName)
    {
        var tokenModel = sosManager.GetToken(tokenName);
        ITokenController tokenController;
        tokenController = new TokenController(tokenModel, GameManager.Turn);
        return tokenController;
    }
}