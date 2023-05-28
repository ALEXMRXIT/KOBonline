using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.UI;
using System;

namespace Assets.Sources.Models.Characters.Smile
{
    public sealed class SmileAPI : MonoBehaviour
    {
        private int[][] _indexes =
        {
            new int[] { 0,   3 },   new int[] { 4,   11 },  new int[] { 12,  16 },
            new int[] { 17,  20 },  new int[] { 21,  24 },  new int[] { 25,  26 },
            new int[] { 27,  30 },  new int[] { 31,  32 },  new int[] { 33,  36 },
            new int[] { 37,  38 },  new int[] { 39,  41 },  new int[] { 42,  45 },
            new int[] { 46,  47 },  new int[] { 48,  49 },  new int[] { 50,  53 },
            new int[] { 54,  57 },  new int[] { 58,  59 },  new int[] { 60,  61 },
            new int[] { 62,  64 },  new int[] { 65,  66 },  new int[] { 67,  70 },
            new int[] { 71,  72 },  new int[] { 73,  74 },  new int[] { 75,  76 },
            new int[] { 77,  79 },  new int[] { 80,  81 },  new int[] { 82,  85 },
            new int[] { 86,  88 },  new int[] { 89,  91 },  new int[] { 92,  95 },
            new int[] { 96,  97 },  new int[] { 98,  100 }, new int[] { 101, 104 },
            new int[] { 105, 106 }, new int[] { 107, 108 }, new int[] { 109, 112 },
            new int[] { 113, 114 }, new int[] { 115, 118 }, new int[] { 119, 120 },
            new int[] { 121, 122 }, new int[] { 123, 125 }, new int[] { 126, 127 },
            new int[] { 128, 129 }, new int[] { 130, 131 }, new int[] { 132, 133 },
            new int[] { 134, 138 }, new int[] { 139, 141 }, new int[] { 142, 143 },
            new int[] { 144, 147 }, new int[] { 148, 149 }, new int[] { 150, 151 }
        };

        private Button[] _buttons;

        public event Action<string> OnSmileClickhandler;

        public void Initialized()
        {
            int count = transform.childCount;
            _buttons = new Button[count];

            for (int iterator = 0; iterator < count; iterator++)
            {
                if (!transform.GetChild(iterator).TryGetComponent(out Button button))
                    throw new MissingComponentException(nameof(Button));

                int hooIteratorId = iterator;
                _buttons[iterator] = button;
                _buttons[iterator].onClick.AddListener(() => InternalOnClickHandler(hooIteratorId));
            }
        }

        private void InternalOnClickHandler(int id)
        {
            gameObject.SetActive(false);
            OnSmileClickhandler?.Invoke($"<sprite anim={_indexes[id][0]},{_indexes[id][1]},5>");
        }

        public void InternalHideSmile()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void ShowSmile()
        {
            gameObject.SetActive(true);
        }

        public void HideSmile()
        {
            gameObject.SetActive(false);
        }
    }
}