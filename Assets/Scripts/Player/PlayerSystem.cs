using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player
{
    public class PlayerSystem
    {
        private readonly PlayerEntity _playerEntity;
        private readonly PlayerBrain _playerBrain;

        public PlayerSystem(PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            _playerEntity = playerEntity;
            _playerBrain = new PlayerBrain(playerEntity, inputSources);
        }
    }
}
