using UnityEngine;
using Assets.Sources.Contracts;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

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
        public HudCharacter ClientHud;
        public CharacterTarget ObjectTarget;
        public SoundLink SoundCharacterLink;
        public Vector3 UpdatePositionInServer;
        public bool ObjectIsLoadData = false;
        public StateAnimationAttackMagic _stateAnimationAttackMagic;
        public StateAnimationAttack _stateAnimationAttack;
        public StateAnimationCastSpell1 _stateAnimationCastSpell1;
        public StateAnimationDeath _stateAnimationDeath;
        public StateAnimationIdle _stateAnimationIdle;
        public StateAnimationRun _stateAnimationRun;
        public VisualModelOfAbilityExecution ClientVisualModelOfAbilityExecution;
    }
}