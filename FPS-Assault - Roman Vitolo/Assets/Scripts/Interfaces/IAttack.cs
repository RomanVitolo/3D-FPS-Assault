using System.Collections.Generic;    

namespace Interfaces
{
    public interface IAttack
    {
        void Shoot();
        void Pursuit();
        void Reload(bool reload);  
        //Queue<bool> NewQuestion();
    }
}