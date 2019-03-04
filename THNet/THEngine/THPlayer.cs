using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THEngine
{
    public class THPlayer : THNetEntity
    {
        public UnityEngine.Vector3 location;

        public void SetLocation(UnityEngine.Vector3 position)
        {
            location = position;
        }
    }
}
