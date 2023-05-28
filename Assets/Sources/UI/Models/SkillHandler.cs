using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Models
{
    public sealed class SkillHandler : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _experienceText;

        private int[] _experience;
        private int _currentexperience;
        private int _level;

        public void SetSpriteToSkill(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetSkillExperience(params int[] experience)
        {
            _experience = experience;
        }

        public void SetSkillCurrentExperience(int experience)
        {
            _currentexperience = experience;
        }

        public void SetSkillLevel(int level)
        {
            _level = level;
        }

        public void ParseSkill()
        {
            _experienceText.gameObject.SetActive(true);
            _experienceText.text = $"{_currentexperience}/{_experience[_level]}";
        }
    }
}