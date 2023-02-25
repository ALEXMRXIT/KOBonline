using UnityEngine;
using Assets.Sources.Contracts;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Models.Base
{
    public sealed class ObjectData
    {
        public long ObjId;
        public bool IsBot = true;
        public bool IsDeath = false;
        public GameObject GameObjectModel;
        public TextView ClientTextView;
        public PlayerContract ObjectContract;
        public CharacterState ClientAnimationState;
        public CharacterTarget ObjectTarget;
        public HUDCharacterComponent ObjectHUD;
        public BaseAttackEffect ObjectBaseEffectWhereAttack;
        public Vector3 UpdatePositionInServer;
        public bool ObjectIsLoadData = false;
    }
}