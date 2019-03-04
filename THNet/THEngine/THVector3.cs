using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THEngine
{
    public class THVector3
    {
        private float[] val = new float[3] {0,0,0};

        public float x
        {
            get
            {
                return val[0];
            }
        }

        public float y
        {
            get
            {
                return val[1];
            }
        }

        public float z
        {
            get
            {
                return val[2];
            }
        }

        public THVector3()
        {

        }

        public THVector3(float x, float y, float z)
        {
            val[0] = x;
            val[1] = y;
            val[2] = z;
        }
    }
}
