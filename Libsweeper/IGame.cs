using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libsweeper
{
    public interface IGame {
        public void Begin();
        public void Draw();
        public void Update();
        public void GameOver();
    }
}
