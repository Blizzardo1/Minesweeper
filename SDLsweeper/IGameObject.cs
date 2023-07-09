﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLsweeper; 

internal interface IGameObject {
    int X { get; }
    int Y { get; }
    
    string Name { get; }
    void Draw();
    void Update();
    
}