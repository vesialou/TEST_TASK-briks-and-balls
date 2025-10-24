using BricksAndBalls.Core.Interfaces;

namespace BricksAndBalls.Configs.Sources
{
    public class DynamicLevelConfigSourceSelector : ILevelConfigSource
    {
        //todo
        //private readonly IServerService _server;

        public DynamicLevelConfigSourceSelector()
        {
        }

        public ILevelConfig LoadLevel(int index)
        {
            //if (_server.IsConnected)
            {
            }

            return new ResourceLevelConfigSource().LoadLevel(index);
        }
    }
}