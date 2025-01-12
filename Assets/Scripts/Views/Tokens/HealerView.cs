using Core.Interfaces.Entities;
using Core.Models.Entities.SO;
using UnityEngine;

namespace Views.Tokens
{
    public class HealerView : TokenView
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite specialSprite;
        
        public override void UpdateVisuals()
        {
            if (tokenImage == null) return;
            TokensStates state = _token.Model.State;

            tokenImage.sprite = state switch
            {
                TokensStates.Special => specialSprite,
                _ => defaultSprite
            };
        }
    }
}