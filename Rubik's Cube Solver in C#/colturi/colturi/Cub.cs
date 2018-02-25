using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace colturi
{
    class Cub
    {
        public Colt[] clt;
        public Muchie[] mch;
        public Cub()
        {
            clt = new Colt[10];
            for (int i = 0; i <= 9; i++)
                clt[i] = new Colt();
            mch = new Muchie[15];
            for (int i = 0; i <= 14; ++i)
                mch[i] = new Muchie();
        }

        public void Mutarea_F()
        {
            Colt aux = new Colt();
            aux = clt[8];
            MessageBox.Show("F");
            clt[8].cul[0] = clt[7].cul[2];
            clt[8].cul[1] = clt[7].cul[0];
            clt[8].cul[2] = clt[7].cul[1];

            clt[7].cul[0] = clt[3].cul[1];
            clt[7].cul[1] = clt[3].cul[2];
            clt[7].cul[2] = clt[3].cul[0];

            clt[3].cul[0] = clt[4].cul[2];
            clt[3].cul[1] = clt[4].cul[0];
            clt[3].cul[2] = clt[4].cul[1];

            clt[4].cul[0] = aux.cul[1];
            clt[4].cul[1] = aux.cul[2];
            clt[4].cul[2] = aux.cul[0];
        }

        /**
         * if (sticker == "DR") return "D2 S D2 S";
            if (sticker == "UL") return "S\' D2 S\' D2";
            if (sticker == "RD") return "R E\' F D F\' E F D\' F\' R\' S2";
            if (sticker == "LU") return "S2 L\' D\' F\' D S S' F D S\' L";
         * */
    }
}
