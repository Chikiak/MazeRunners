using Core.Interface.Controllers;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Visual.Pieces
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private Sprite SquareSprite;
        [SerializeField] private Sprite PieceSprite;
        private IPieceController _piece;
        public Image PieceImage { get; private set; }
        
        
        public void Initialize(IPieceController piece)
        {
            _piece = piece;
            PieceImage = GetComponentInChildren<Image>();
            PieceImage.sprite = PieceSprite;
        }
    }
}