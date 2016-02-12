using Kappa.utilities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace Kappa.world {
    class WorldGenerator {
        // Settings to allow us some level of rigidity and predictability, check out the presets in LoadPreset().
        private ushort _worldWidth;
        private ushort _worldHeight;
        private ushort _startX;
        private ushort _startY;
        private ushort _towerColumn1X;
        private ushort _towerColumn2X;
        private byte _numberOfLanes;
        private bool _generateJungle;

        // Randomized settings.
        private List<ushort> _towerColumn1Ys;
        private List<ushort> _towerColumn2Ys;

        // WorldGenerator() expects a byte preset param which will be used here. Using presets instead of having 500 params in the call.
        public void loadPreset(byte preset) {
            switch(preset) {
                // Small map with no jungle
                case 0:
                    _worldWidth = 10;
                    _worldHeight = 10;
                    _startX = 0;
                    _startY = 5;
                    _towerColumn1X = 3;
                    _towerColumn2X = 7;
                    _numberOfLanes = 3;
                    _generateJungle = false;
                    break;
            }
        }

        public WorldGenerator(byte preset) {
            // Presets to allow multiple kind of maps
            loadPreset(preset);
        }

        public void GenerateWorld() {
            _towerColumn1Ys = new List<ushort>();
            _towerColumn2Ys = new List<ushort>();

            Random random = new Random();

            while(_towerColumn1Ys.Count != _numberOfLanes) {
                ushort n = (ushort) random.Next(0, _worldHeight);
                if(!_towerColumn1Ys.Contains(n)) {
                    _towerColumn1Ys.Add(n);
                }
            }
            _towerColumn1Ys.Sort(); _towerColumn1Ys.Reverse();
            while(_towerColumn2Ys.Count != _numberOfLanes) {
                ushort n = (ushort)random.Next(0, _worldHeight);
                if(!_towerColumn2Ys.Contains(n)) {
                    _towerColumn2Ys.Add(n);
                }
            }
            _towerColumn2Ys.Sort(); _towerColumn2Ys.Reverse();

            Pathfinder P = new Pathfinder(Pathfinder.GenerateGrid(_worldWidth, _worldHeight));

            List<Pathfinder.PathfinderNode> World = new List<Pathfinder.PathfinderNode>();

            byte i = (byte) _numberOfLanes;
            while(i-- > 0) {
                // Pretty sure this section is wrong.
                World.Union(P.FindPath(P.GetNodeAt(_startX, _startY), P.GetNodeAt(_towerColumn1X, _towerColumn1Ys[i]), true));
                World.Union(P.FindPath(P.GetNodeAt(_towerColumn1X, _towerColumn1Ys[i]), P.GetNodeAt(_towerColumn2X, _towerColumn2Ys[i]), true));
            }

            for(i = 0; i < World.Count; i++) {
                Debug.WriteLine(World[i].X + ", " + World[i].Y);
            }
        }
    }
}
