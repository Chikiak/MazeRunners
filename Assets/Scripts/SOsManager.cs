using System;
using System.Collections.Generic;
using Core.Interfaces.Entities;
using Core.Models.Entities.SO;
using UnityEngine;

public class SOsManager : MonoBehaviour
{
    public static SOsManager Instance { get; private set; }
    public static Action<GameStates> Ready;
    
    [SerializeField] private List<TokenModel> tokenModels;
    private Dictionary<TokensNames, IToken> tokenDictionary = new Dictionary<TokensNames, IToken>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeTokenDictionary();
        Ready?.Invoke(GameStates.Starting);
    }

    private void InitializeTokenDictionary()
    {
        foreach (var token in tokenModels)
        {
            tokenDictionary[token.Name] = token;
        }
    }

    public IToken GetToken(TokensNames tokenName)
    {
        if (tokenDictionary.TryGetValue(tokenName, out var token))
        {
            return token;
        }
        
        Debug.LogError($"Token with name {tokenName} not found!");
        return null;
    }
}