using NPC.UnitData;
using UnityEngine;

namespace Effects
{
    public class DeathAnim : MonoBehaviour
    {
        [SerializeField] private Material _redTexture;
        [SerializeField] private Material _blueTexture;
        [SerializeField] private SkinnedMeshRenderer _renderer;

        private UnitTeam _team;
        public UnitTeam team
        {
            get => _team;
            set
            {
                _team = value;
                switch (value)
                {
                    case UnitTeam.Blue:
                        _renderer.material = _blueTexture;
                        break;
                    case UnitTeam.Red:
                        _renderer.material = _redTexture;
                        break;
                }
            }
        }
    }
}