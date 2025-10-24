using System.Collections.Generic;
using BricksAndBalls.Views;

namespace BricksAndBalls.Systems.Grid
{
    public class BrickViewRegistry
    {
        private readonly Dictionary<int, BrickView> _views = new();

        public void Register(int id, BrickView view)
        {
            _views[id] = view;
        }

        public void Unregister(int id)
        {
            _views.Remove(id);
        }

        public bool TryGet(int id, out BrickView view)
            => _views.TryGetValue(id, out view);

        public IEnumerable<BrickView> AllViews => _views.Values;
    }
}