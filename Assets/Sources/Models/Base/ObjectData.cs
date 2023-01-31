using UnityEngine;
using Assets.Sources.Contracts;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Models.Base
{
    public sealed class ObjectData
    {
        public GameObject GameObjectModel;
        public PlayerContract ObjectContract;
        public CharacterTarget ObjectTarget;
        public HUDCharacterComponent ObjectHUD;
        public Vector3 UpdatePositionInServer;
        public bool ObjectIsLoadData = false;
    }
}