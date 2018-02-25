using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace colturi
{
    public partial class Form1 : Form
    {
        Color culoare = Color.White;
        const string Yperm = "R U\' R\' U\' R U R\' F\' R U R\' U\' R\' F R";
        string SolutieColturi = "", SolutieColturiSucite = "", SolutieMuchii = "", SolutieMuchiiSucite = "", SolutiaCubului = "";
        int cntVerde = 9, cntAlbastru = 9, cntRosu = 9, cntPortocaliu = 9, cntAlb = 9, cntGalben = 9;
        Button[,] matColturi = new Button[10, 5];
        Button[,] matMuchii = new Button[15, 3];
        bool PreaMult, PotRezolva;
        string[] sirColturi = new string[40];
        string[] sirMuchii = new string[60];
        string[] sirColturiSucite = new string[40];
        string[] sirMuchiiSucite = new string[60];
        int sirColturiSize = 0, sirMuchiiSize = 0, sirColturiSuciteSize = 0, sirMuchiiSuciteSize = 0;
        bool[] vizColturi = new bool[10], vizMuchii = new bool[15];//Pentru DFS
        const int BufferMuchii = 2, BufferColturi = 1;
        Cub Rubik = new Cub();

        public Form1()
        {
            InitializeComponent();
        }

        private void button_click(object sender, EventArgs e)//schimba colorile stickerelor
        {
            Button b = (Button)sender;

            if (b.BackColor == Color.Lime) cntVerde--;
            if (b.BackColor == Color.Blue) cntAlbastru--;
            if (b.BackColor == Color.Orange) cntPortocaliu--;
            if (b.BackColor == Color.Red) cntRosu--;
            if (b.BackColor == Color.White) cntAlb--;
            if (b.BackColor == Color.Yellow) cntGalben--;

            b.BackColor = culoare;
            if (culoare == Color.Lime) cntVerde++;
            if (culoare == Color.Blue) cntAlbastru++;
            if (culoare == Color.Orange) cntPortocaliu++;
            if (culoare == Color.Red) cntRosu++;
            if (culoare == Color.White) cntAlb++;
            if (culoare == Color.Yellow) cntGalben++;

            labelRosu.Text = "Rosu:" + (cntRosu);
            labelPortocaliu.Text = "Portocaliu:" + (cntPortocaliu);
            labelAlbastru.Text = "Albastru:" + (cntAlbastru);
            labelVerde.Text = "Verde:" + (cntVerde);
            labelAlb.Text = "Alb:" + (cntAlb);
            labelGalben.Text = "Galben:" + (cntGalben);
        }

        private void Schimba_Culoarea(object sender, EventArgs e)//butoanele din stanga-jos
        {
            Button b = (Button)sender;
            culoare = b.BackColor;
            IndicatorCuloare.BackColor = culoare;
        }

        private void Centru_click(object sender, EventArgs e)//nu poti schimba culoarea centrelor
        {
            MessageBox.Show("Nu poti schimba culoarea centrelor!");
        }

        private void PreaMultCeva()
        {
            PreaMult = true;
            if (cntAlb > 9)
            {
                MessageBox.Show("Prea mult alb! Trebuie sa existe doar 9 patratele albe.");
                return;
            }
            if (cntGalben > 9)
            {
                MessageBox.Show("Prea mult galben! Trebuie sa existe doar 9 patratele galbene.");
                return;
            }
            if (cntVerde > 9)
            {
                MessageBox.Show("Prea mult verde! Trebuie sa existe doar 9 patratele verzi.");
                return;
            }
            if (cntAlbastru > 9)
            {
                MessageBox.Show("Prea mult albastru! Trebuie sa existe doar 9 patratele albastre.");
                return;
            }
            if (cntRosu > 9)
            {
                MessageBox.Show("Prea mult rosu! Trebuie sa existe doar 9 patratele rosii.");
                return;
            }
            if (cntPortocaliu > 9)
            {
                MessageBox.Show("Prea mult portocaliu! Trebuie sa existe doar 9 patratele portocalii.");
                return;
            }
            //daca a ajuns pana aici, nu avem prea mult din nicio culoare
            PreaMult = false;
        }

        void InitMat()
        {
            //----------------------------------------------Colturi----------------------------------------------------------
            //1
            matColturi[1, 0] = button27;
            matColturi[1, 1] = button54;
            matColturi[1, 2] = button34;
            //2
            matColturi[2, 0] = button25;
            matColturi[2, 1] = button36;
            matColturi[2, 2] = button16;
            //3
            matColturi[3, 0] = button19;
            matColturi[3, 1] = button18;
            matColturi[3, 2] = button3;
            //4
            matColturi[4, 0] = button21;
            matColturi[4, 1] = button1;
            matColturi[4, 2] = button52;
            //5
            matColturi[5, 0] = button39;
            matColturi[5, 1] = button28;
            matColturi[5, 2] = button48;
            //6
            matColturi[6, 0] = button37;
            matColturi[6, 1] = button10;
            matColturi[6, 2] = button30;
            //7
            matColturi[7, 0] = button43;
            matColturi[7, 1] = button9;
            matColturi[7, 2] = button12;
            //8
            matColturi[8, 0] = button45;
            matColturi[8, 1] = button46;
            matColturi[8, 2] = button7;
            //----------------------------------------------Muchii----------------------------------------------------------
            //1
            matMuchii[1, 0] = button26;
            matMuchii[1, 1] = button35;
            //2
            matMuchii[2, 0] = button22;
            matMuchii[2, 1] = button17;
            //3
            matMuchii[3, 0] = button20;
            matMuchii[3, 1] = button2;
            //4
            matMuchii[4, 0] = button24;
            matMuchii[4, 1] = button53;
            //5
            matMuchii[5, 0] = button51;
            matMuchii[5, 1] = button31;
            //6
            matMuchii[6, 0] = button13;
            matMuchii[6, 1] = button33;
            //7
            matMuchii[7, 0] = button15;
            matMuchii[7, 1] = button6;
            //8
            matMuchii[8, 0] = button49;
            matMuchii[8, 1] = button4;
            //9
            matMuchii[9, 0] = button38;
            matMuchii[9, 1] = button29;
            //10
            matMuchii[10, 0] = button40;
            matMuchii[10, 1] = button11;
            //11
            matMuchii[11, 0] = button44;
            matMuchii[11, 1] = button8;
            //12
            matMuchii[12, 0] = button42;
            matMuchii[12, 1] = button47;
        }

        int ConvertCuloare(Button b)
        {
            if (b.BackColor == Color.White) return 1;
            if (b.BackColor == Color.Yellow) return 2;
            if (b.BackColor == Color.Red) return 3;
            if (b.BackColor == Color.Orange) return 4;
            if (b.BackColor == Color.Lime) return 5;
            if (b.BackColor == Color.Blue) return 6;
            MessageBox.Show("Eroare in ConvertCuloare()!");
            return 7;
        }

        /// <summary>
        /// Compara doua colturi dupa pozitiile lor. Nu tine cont de orientare
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>

        bool Compara(Colt A, Colt B)//Compara doua colturi dupa pozitiile lor. Nu tine cont de orientare
        {
            if (A.cul[0] == B.cul[0] && A.cul[1] == B.cul[1] && A.cul[2] == B.cul[2]) return true;
            if (A.cul[1] == B.cul[0] && A.cul[2] == B.cul[1] && A.cul[0] == B.cul[2]) return true;
            if (A.cul[2] == B.cul[0] && A.cul[0] == B.cul[1] && A.cul[1] == B.cul[2]) return true;
            return false;
        }

        /// <summary>
        /// Compara doua muchii dupa pozitiile lor. Nu tine cont de orientare
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>

        bool Compara(Muchie A, Muchie B)//Compara doua muchii dupa pozitiile lor. Nu tine cont de orientare
        {
            if (A.cul[0] == B.cul[0] && A.cul[1] == B.cul[1]) return true;
            if (A.cul[0] == B.cul[1] && A.cul[1] == B.cul[0]) return true;
            return false;
        }

        int ReturnHome(Colt c)
        {
            Colt aux = new Colt();
            aux.cul[0] = 1;
            aux.cul[1] = 4;
            aux.cul[2] = 6;
            if (Compara(aux, c))///1
                return 1;
            aux.cul[0] = 1;
            aux.cul[1] = 6;
            aux.cul[2] = 3;
            if (Compara(aux, c))///2
                return 2;
            aux.cul[0] = 1;
            aux.cul[1] = 3;
            aux.cul[2] = 5;
            if (Compara(aux, c))///3
                return 3;
            aux.cul[0] = 1;
            aux.cul[1] = 5;
            aux.cul[2] = 4;
            if (Compara(aux, c))///4
                return 4;
            aux.cul[0] = 2;
            aux.cul[1] = 6;
            aux.cul[2] = 4;
            if (Compara(aux, c))///5
                return 5;
            aux.cul[0] = 2;
            aux.cul[1] = 3;
            aux.cul[2] = 6;
            if (Compara(aux, c))///6
                return 6;
            aux.cul[0] = 2;
            aux.cul[1] = 5;
            aux.cul[2] = 3;
            if (Compara(aux, c))///7
                return 7;
            aux.cul[0] = 2;
            aux.cul[1] = 4;
            aux.cul[2] = 5;
            if (Compara(aux, c))///8
                return 8;
            //MessageBox.Show("Eroare. Nu imi gasesc home-ul.");
            return 9;
        }//returneaza casa coltului

        int ReturnHome(Muchie m)//returneaza casa muchiei
        {
            Muchie aux = new Muchie();
            aux.cul[0] = 1;
            aux.cul[1] = 6;
            if (Compara(aux, m))//1
                return 1;
            aux.cul[0] = 1;
            aux.cul[1] = 3;
            if (Compara(aux, m))//2
                return 2;
            aux.cul[0] = 1;
            aux.cul[1] = 5;
            if (Compara(aux, m))//3
                return 3;
            aux.cul[0] = 1;
            aux.cul[1] = 4;
            if (Compara(aux, m))//4
                return 4;
            aux.cul[0] = 4;
            aux.cul[1] = 6;
            if (Compara(aux, m))//5
                return 5;
            aux.cul[0] = 3;
            aux.cul[1] = 6;
            if (Compara(aux, m))//6
                return 6;
            aux.cul[0] = 3;
            aux.cul[1] = 5;
            if (Compara(aux, m))//7
                return 7;
            aux.cul[0] = 4;
            aux.cul[1] = 5;
            if (Compara(aux, m))//8
                return 8;
            aux.cul[0] = 2;
            aux.cul[1] = 6;
            if (Compara(aux, m))//9
                return 9;
            aux.cul[0] = 2;
            aux.cul[1] = 3;
            if (Compara(aux, m))//10
                return 10;
            aux.cul[0] = 2;
            aux.cul[1] = 5;
            if (Compara(aux, m))//11
                return 11;
            aux.cul[0] = 2;
            aux.cul[1] = 4;
            if (Compara(aux, m))//12
                return 12;
            //MessageBox.Show("Eroare. Nu gasesc home-ul muchiei.");
            return 13;
        }

        int ReturnOrientare(Colt c)//returneaza orientarea coltului
        {
            if (c.cul[0] == 1 || c.cul[0] == 2) return 0;
            if (c.cul[1] == 1 || c.cul[1] == 2) return 1;
            if (c.cul[2] == 1 || c.cul[2] == 2) return -1;
            //MessageBox.Show("Eroare. Nu gasesc o orientare a coltului.");
            return 2;
        }

        int ReturnOrientare(Muchie m)//returneaza orientarea muchiei
        {
            if (m.cul[0] == 1 || m.cul[0] == 2) return 0;
            if ((m.cul[0] == 3 || m.cul[0] == 4) && (m.cul[1] == 5 || m.cul[1] == 6)) return 0;
            return 1;
            //muchia este mereu orientata intr-unul din cele doua moduri posibile
        }

        void FormezColturi()
        {
            int i, casa, ornt;
            //------------------------------------------------------------Formez colturi-------------------------------------------------------------
            for (i = 1; i <= 8; ++i)//formez culorile
            {
                Rubik.clt[i].cul[0] = ConvertCuloare(matColturi[i, 0]);
                Rubik.clt[i].cul[1] = ConvertCuloare(matColturi[i, 1]);
                Rubik.clt[i].cul[2] = ConvertCuloare(matColturi[i, 2]);
            }
            //Home-ul
            for (i = 1; i <= 8; ++i)
            {
                casa = ReturnHome(Rubik.clt[i]);
                Rubik.clt[i].home = casa;
            }
            //Orientarea
            for (i = 1; i <= 8; i++)
            {
                ornt = ReturnOrientare(Rubik.clt[i]);
                Rubik.clt[i].orientare = ornt;
            }
        }

        void FormezMuchii()
        {
            int i, casa, ornt;
            //------------------------------------------------------------Formez muchii-------------------------------------------------------------
            for (i = 1; i <= 12; i++)
            {
                Rubik.mch[i].cul[0] = ConvertCuloare(matMuchii[i, 0]);
                Rubik.mch[i].cul[1] = ConvertCuloare(matMuchii[i, 1]);
            }
            //Home-ul
            for (i = 1; i <= 12; i++)
            {
                casa = ReturnHome(Rubik.mch[i]);
                Rubik.mch[i].home = casa;
            }
            //Orientarea
            for (i = 1; i <= 12; i++)
            {
                ornt = ReturnOrientare(Rubik.mch[i]);
                Rubik.mch[i].orientare = ornt;
            }
        }

        void VerificAmbiguitati()
        {
            //colturile formeaza o permutare?
            int i, casa;
            int[] nrap;
            nrap = new int[15];
            for (i = 1; i <= 8; i++)
                nrap[i] = 0;
            for (i = 1; i <= 8; i++)
            {
                casa = ReturnHome(Rubik.clt[i]);
                nrap[casa]++;
            }
            for (i = 1; i <= 8; ++i)
                if (nrap[i] != 1)
                {
                    PotRezolva = false;
                    MessageBox.Show("Eroare. Colturile nu formeaza o permutare.");
                    return;
                }
            //muchiile formeaza o permutare?
            for (i = 1; i <= 12; i++)
                nrap[i] = 0;
            for (i = 1; i <= 12; i++)
            {
                casa = ReturnHome(Rubik.mch[i]);
                nrap[casa]++;
            }
            for (i = 1; i <= 12; ++i)
                if (nrap[i] != 1)
                {
                    PotRezolva = false;
                    MessageBox.Show("Eroare. Muchiile nu formeaza o permutare.");
                    return;
                }
            //---------------------------------------am piese sucite?------------------------------------------------------------------------------
            //colturi
            int ornt, sum = 0;
            for (int j = 1; j <= 8; ++j)
            {
                ornt = Rubik.clt[j].orientare;
                if (ornt == -1) ornt = 2;
                sum += ornt;
            }
            if (sum % 3 != 0)
            {
                PotRezolva=false;
                MessageBox.Show("Eroare. Exista un colt sucit.\n");
                if (sum % 3 == 1)
                    MessageBox.Show("Roteste unul din colturi in sensul invers acelor de ceasornic.");
                else
                    MessageBox.Show("Roteste unul din colturi in sensul acelor de ceasornic.");
                return;
            }
            //muchii
            sum = 0;
            for (int j = 1; j <= 12; ++j)
            {
                ornt = Rubik.mch[j].orientare;
                sum += ornt;
            }
            if (sum % 2 == 1)
            {
                PotRezolva = false;
                MessageBox.Show("Eroare. Exista o muchie sucita. Scoate una din muchii si asaz-o invers.");
                return;
            }
        }

        string ConjugataMuchie(string sticker)
        {
            //aceste conjugate sunt valabile dupa aplicarea mutarilor y' x2
            if (sticker == "UF") return "U R2 U\'";
            if (sticker == "RF") return "U R U\'";
            if (sticker == "LF") return "U R\' U\'";
            if (sticker == "DF") return "R\' U R U\'";

            if (sticker == "UB") return "U\' L2 U";
            if (sticker == "RB") return "U' L\' U";
            if (sticker == "LB") return "U\' L U";
            if (sticker == "DB") return "L U\' L\' U";

            if (sticker == "FR") return "u R u\'";
            if (sticker == "FD") return "B\' R B";
            if (sticker == "FU") return "B\' R\' B";
            if (sticker == "FL") return "B\' R2 B";

            if (sticker == "BL") return "u' L\' u";
            if (sticker == "BD") return "B L\' B'";
            if (sticker == "BU") return "B L B\'";
            if (sticker == "BR") return "B L2 B\'";

            /*if (sticker == "UB") return "";
            if (sticker == "BU") return "F' D R' F D'";
            if (sticker == "UR") return "R' U R U'";
            if (sticker == "UL") return "L U' L' U";
            if (sticker == "FR") return "U R U'";
            if (sticker == "FL") return "U' L' U";
            if (sticker == "RU") return "B' R B";
            if (sticker == "RB") return "u R u'";
            if (sticker == "RD") return "B' R' B";
            if (sticker == "RF") return "B' R2 B";
            if (sticker == "BL") return "U' L U";
            if (sticker == "BR") return "U R' U'";
            if (sticker == "LU") return "B L' B'";
            if (sticker == "LF") return "B L2 B'";
            if (sticker == "LD") return "B L B'";
            if (sticker == "LB") return "u' L' u";
            if (sticker == "DR") return "U R2 U'";
            if (sticker == "DL") return "U' L2 U";*/

            return "conj" + sticker;
        }

        string InvConjugataMuchie(string sticker)
        {
            if (sticker == "UF") return "U R2 U\'";
            if (sticker == "RF") return "U R\' U\'";
            if (sticker == "LF") return "U R U\'";
            if (sticker == "DF") return "U R\' U\' R";

            if (sticker == "UB") return "U\' L2 U";
            if (sticker == "RB") return "U\' L U";
            if (sticker == "LB") return "U\' L\' U";
            if (sticker == "DB") return "U\' L U L\'";

            if (sticker == "FR") return "u R\' u\'";
            if (sticker == "FD") return "B\' R\' B";
            if (sticker == "FU") return "B\' R B";
            if (sticker == "FL") return "B\' R2 B";

            if (sticker == "BL") return "u' L u";
            if (sticker == "BD") return "B L B'";
            if (sticker == "BU") return "B L\' B\'";
            if (sticker == "BR") return "B L2 B\'";

            /*if (sticker == "UB") return "";
            if (sticker == "BU") return "D F' R D' F";
            if (sticker == "UR") return "U R' U' R";
            if (sticker == "UL") return "U' L U L'";
            if (sticker == "FR") return "U R' U'";
            if (sticker == "FL") return "U' L U";
            if (sticker == "RU") return "B' R' B";
            if (sticker == "RB") return "u R' u'";
            if (sticker == "RD") return "B' R B";
            if (sticker == "RF") return "B' R2 B";
            if (sticker == "BL") return "U' L' U";
            if (sticker == "BR") return "U R U'";
            if (sticker == "LU") return "B L B'";
            if (sticker == "LF") return "B L2 B'";
            if (sticker == "LD") return "B L' B'";
            if (sticker == "LB") return "u' L u";
            if (sticker == "DR") return "U R2 U'";
            if (sticker == "DL") return "U' L2 U";*/

            return "invconj";
        }

        string RezolvareM2(string sticker)
        {
            //intai exceptiile
            if (sticker == "DR") return "U2 M\' U2 M\'";
            if (sticker == "UL") return "M U2 M U2";
            if (sticker == "RD") return "F E R U R\' E\' R U\' R\' M2";
            if (sticker == "LU") return "M2 B\' U\' R\' U M\' U\' R U M B";
           
            if (sticker == "DL")    return "M2";
            return ("(" + ConjugataMuchie(sticker) + ") " + "M2" + " (" + InvConjugataMuchie(sticker) + ")");
        }

        string ConjugataColt(string sticker)
        {
            if (sticker == "ULD") return "error";
            if (sticker == "UBR") return "R D\'";
            if (sticker == "URF") return "F";
            if (sticker == "UFL") return "F R\'";
            if (sticker == "FLU") return "F\' D";
            if (sticker == "FUR") return "F2 D";
            if (sticker == "FRD") return "F D";
            if (sticker == "FDL") return "D";
            if (sticker == "RFU") return "R\'";
            if (sticker == "RUB") return "R2";
            if (sticker == "RBD") return "R";
            if (sticker == "RDF") return "";
            if (sticker == "BRU") return "R\' F";
            if (sticker == "BUL") return "error";
            if (sticker == "BLD") return "D\' R";
            if (sticker == "BDR") return "D'";
            if (sticker == "LBU") return "error";
            if (sticker == "LUF") return "F2";
            if (sticker == "LFD") return "D2 R";
            if (sticker == "LDB") return "D2";
            if (sticker == "DLF") return "F\'";
            if (sticker == "DFR") return "D\' F'";
            if (sticker == "DRB") return "D2 F\'";
            if (sticker == "DBL") return "D F\'";
            return "conj";
        }

        string InvConjugataColt(string sticker)
        {
            if (sticker == "ULD") return "error";
            if (sticker == "UBR") return "D R\'";
            if (sticker == "URF") return "F\'";
            if (sticker == "UFL") return "R F\'";
            if (sticker == "FLU") return "D\' F";
            if (sticker == "FUR") return "D\' F2";
            if (sticker == "FRD") return "D\' F\'";
            if (sticker == "FDL") return "D\'";
            if (sticker == "RFU") return "R";
            if (sticker == "RUB") return "R2";
            if (sticker == "RBD") return "R\'";
            if (sticker == "RDF") return "";
            if (sticker == "BRU") return "F\' R";
            if (sticker == "BUL") return "error";
            if (sticker == "BLD") return "R\' D";
            if (sticker == "BDR") return "D";
            if (sticker == "LBU") return "error";
            if (sticker == "LUF") return "F2";
            if (sticker == "LFD") return "R\' D2";
            if (sticker == "LDB") return "D2";
            if (sticker == "DLF") return "F";
            if (sticker == "DFR") return "F D";
            if (sticker == "DRB") return "F D2";
            if (sticker == "DBL") return "F D\'";
            return "conjprime";
        }

        string RezolvareOldPochman(string sticker)
        {
            if (sticker == "RDF") return Yperm;
            return ("(" + ConjugataColt(sticker) + ") " + Yperm + " (" + InvConjugataColt(sticker) + ")");
        }

        string ConvertToHumanLanguageC(int poz, int sticker)
        {
            if (poz == 1)
            {
                if (sticker == 0) return "ULB";
                if (sticker == 1) return "LBU";
                if (sticker == 2) return "BUL";
            }
            if (poz == 2)
            {
                if (sticker == 0) return "UBR";
                if (sticker == 1) return "BRU";
                if (sticker == 2) return "RUB";
            }
            if (poz == 3)
            {
                if (sticker == 0) return "URF";
                if (sticker == 1) return "RFU";
                if (sticker == 2) return "FUR";
            }
            if (poz == 4)
            {
                if (sticker == 0) return "UFL";
                if (sticker == 1) return "FLU";
                if (sticker == 2) return "LUF";
            }
            if (poz == 5)
            {
                if (sticker == 0) return "DBL";
                if (sticker == 1) return "BLD";
                if (sticker == 2) return "LDB";
            }
            if (poz == 6)
            {
                if (sticker == 0) return "DRB";
                if (sticker == 1) return "RBD";
                if (sticker == 2) return "BDR";
            }
            if (poz == 7)
            {
                if (sticker == 0) return "DFR";
                if (sticker == 1) return "FRD";
                if (sticker == 2) return "RDF";
            }
            if (poz == 8)
            {
                if (sticker == 0) return "DLF";
                if (sticker == 1) return "LFD";
                if (sticker == 2) return "FDL";
            }
            MessageBox.Show("Eroare in ConvertToHumanLanguageC()");
            return "error";
        }

        string ConvertToHumanLanguageM(int poz, int sticker)
        {
            if (poz == 1)
            {
                if (sticker == 0)
                    return "UB";
                if (sticker == 1)
                    return "BU";
            }
            if (poz == 2)
            {
                if (sticker == 0)
                    return "UR";
                if (sticker == 1)
                    return "RU";
            }
            if (poz == 3)
            {
                if (sticker == 0)
                    return "UF";
                if (sticker == 1)
                    return "FU";
            }
            if (poz == 4)
            {
                if (sticker == 0)
                    return "UL";
                if (sticker == 1)
                    return "LU";
            }
            if (poz == 5)
            {
                if (sticker == 0)
                    return "LB";
                if (sticker == 1)
                    return "BL";
            }
            if (poz == 6)
            {
                if (sticker == 0)
                    return "RB";
                if (sticker == 1)
                    return "BR";
            }
            if (poz == 7)
            {
                if (sticker == 0)
                    return "RF";
                if (sticker == 1)
                    return "FR";
            }
            if (poz == 8)
            {
                if (sticker == 0)
                    return "LF";
                if (sticker == 1)
                    return "FL";
            }
            if (poz == 9)
            {
                if (sticker == 0)
                    return "DB";
                if (sticker == 1)
                    return "BD";
            }
            if (poz == 10)
            {
                if (sticker == 0)
                    return "DR";
                if (sticker == 1)
                    return "RD";
            }
            if (poz == 11)
            {
                if (sticker == 0)
                    return "DF";
                if (sticker == 1)
                    return "FD";
            }
            if (poz == 12)
            {
                if (sticker == 0)
                    return "DL";
                if (sticker == 1)
                    return "LD";
            }
            MessageBox.Show("Eroare in ConvertToHumanLanguageM()");
            return "error";
        }

        int ConvertToStickerC(int poz, int culoare)
        {
            if (culoare == 1 || culoare == 2)//alb si galben
                return 0;
            if (culoare == 3)//rosu
            {
                if (poz == 3 || poz == 6)
                    return 1;
                if (poz == 2 || poz == 7)
                    return 2;
                MessageBox.Show("Eroare la ConvertToSticker");
                return 3;
            }
            if (culoare == 4)//portocaliu
            {
                if (poz == 1 || poz == 8)
                    return 1;
                if (poz == 4 || poz == 5)
                    return 2;
                MessageBox.Show("Eroare la ConvertToSticker");
                return 3;
            }
            if (culoare == 5)//verde
            {
                if (poz == 4 || poz == 7)
                    return 1;
                if (poz == 3 || poz == 8)
                    return 2;
                MessageBox.Show("Eroare la ConvertToSticker");
                return 3;
            }
            if (culoare == 6)//albastru
            {
                if (poz == 2 || poz == 5)
                    return 1;
                if (poz == 1 || poz == 6)
                    return 2;
                MessageBox.Show("Eroare la ConvertToSticker");
                return 3;
            }
            MessageBox.Show("Eroare la ConvertToSticker");
            return 3;
        }

        int ConvertToStickerM(int poz, int culoare)
        {
            if (culoare == 1 || culoare == 2)//alb sau galben
                return 0;
            if (culoare == 3)//rosu
            {
                if (poz == 6 || poz == 7)
                    return 0;
                if (poz == 2 || poz == 10)
                    return 1;
                MessageBox.Show("Eroare la ConvertToStickerM");
                return 2;
            }
            if (culoare == 4)//portocaliu
            {
                if (poz == 5 || poz == 8)
                    return 0;
                if (poz == 4 || poz == 12)
                    return 1;
                MessageBox.Show("Eroare la ConvertToStickerM");
                return 2;
            }
            if (culoare == 5 || culoare == 6)//verde si albastru
                return 1;
            MessageBox.Show("Eroare la ConvertToStickerM");
            return 2;
        }

        void DFS_Colturi(int poz, int sticker)
        {
            int casa;
            string nume;
            if (poz != BufferColturi)//adaug in sir. Bufferul nu poate aparea in sir.
            {
                nume = ConvertToHumanLanguageC(poz, sticker);
                sirColturiSize++;
                sirColturi[sirColturiSize] = nume;
            }
            casa = Rubik.clt[poz].home;
            if (!vizColturi[poz])
            {
                vizColturi[poz] = true;
                DFS_Colturi(casa, ConvertToStickerC(casa, Rubik.clt[poz].cul[sticker]));
            }
        }

        void FormezSirColturi()
        {
            int i, casa, ornt;
            for (i = 1; i <= 8; ++i)//init
                vizColturi[i] = false;
            sirColturiSize = 0;
            //prima data bufferul
            if (Rubik.clt[BufferColturi].home != BufferColturi)
                DFS_Colturi(BufferColturi, 0);
            for (i = 8; i >= 1; --i)
                if (!vizColturi[i] && Rubik.clt[i].home != i)//nu e vizitat, si nici nu e deja pe pozitia lui 
                {
                    if (i == 7)
                        DFS_Colturi(i, 2);//economisesc niste miscari conjugate de la metoda Old-Pochmann
                    else
                        DFS_Colturi(i, 0);
                }
            if (sirColturiSize % 2 == 1)//swapul
            {
                Muchie aux = new Muchie();
                aux = Rubik.mch[1];
                Rubik.mch[1] = Rubik.mch[4];
                Rubik.mch[4] = aux;
            }
            //--------------------------------------------------------------sucituri----------------------------------------------------------------
            sirColturiSuciteSize = 0;
            for (i = 1; i <= 8; ++i)
            {
                casa = Rubik.clt[i].home;
                ornt = Rubik.clt[i].orientare;
                if (i!= BufferColturi && casa == i && ornt != 0)
                {
                    if (ornt == -1)
                    {
                        sirColturiSuciteSize++;
                        sirColturiSucite[sirColturiSuciteSize] = ConvertToHumanLanguageC(i, 0);
                        sirColturiSuciteSize++;
                        sirColturiSucite[sirColturiSuciteSize] = ConvertToHumanLanguageC(i, 1);
                    }
                    if (ornt == +1)
                    {
                        sirColturiSuciteSize++;
                        sirColturiSucite[sirColturiSuciteSize] = ConvertToHumanLanguageC(i, 0);
                        sirColturiSuciteSize++;
                        sirColturiSucite[sirColturiSuciteSize] = ConvertToHumanLanguageC(i, 2);
                    }
                }
            }
        }

        void DFS_Muchii(int poz, int sticker)
        {
            int casa;
            string nume;
            if (poz != BufferMuchii)//adaug in sir. Bufferul nu poate aparea in sir.
            {
                nume = ConvertToHumanLanguageM(poz, sticker);
                sirMuchiiSize++;
                sirMuchii[sirMuchiiSize] = nume;
            }
            casa = Rubik.mch[poz].home;
            if (!vizMuchii[poz])
            {
                vizMuchii[poz] = true;
                DFS_Muchii(casa, ConvertToStickerM(casa, Rubik.mch[poz].cul[sticker]));
            }
        }

        void FormezSirMuchii()
        {
            int i, casa, ornt;
            for (i = 1; i <= 12; ++i)//init
                vizMuchii[i] = false;
            sirMuchiiSize = 0;
            //prima data bufferul
            if (Rubik.mch[BufferMuchii].home != BufferMuchii)
                DFS_Muchii(BufferMuchii, 0);
            //apoi restul
            for (i = 1; i <= 12; ++i)
                if (!vizMuchii[i] && Rubik.mch[i].home != i)//nu e vizitata, si nici nu e deja pe pozitia ei 
                {
                    //MessageBox.Show("muchia " + i + " are casa " + Rubik.mch[i].home);
                    DFS_Muchii(i, 0);
                }
            //------------------------------------------------------------sucituri----------------------------------------------------
            sirMuchiiSuciteSize = 0;
            for (i = 1; i <= 12; ++i)
            {
                casa = Rubik.mch[i].home;
                ornt = Rubik.mch[i].orientare;
                if (i != BufferMuchii && casa == i && ornt != 0)
                {
                    sirMuchiiSuciteSize++;
                    sirMuchiiSucite[sirMuchiiSuciteSize] = ConvertToHumanLanguageM(i, 0);
                    sirMuchiiSuciteSize++;
                    sirMuchiiSucite[sirMuchiiSuciteSize] = ConvertToHumanLanguageM(i, 1);
                }
            }
        }

        string ComutatorColturi(string caz)//am scris un program in C++ care sa scrie toate aceste if-uri in C# in locul meu
        {
            if (caz == "(ULB UFL URF)") return "F R\' F L2 F\' R F L2 F2";
            if (caz == "(ULB UFL UBR)") return "B2 L2 B R B\' L2 B R\' B";
            if (caz == "(ULB UFL RFU)") return "F R B\' R\' F\' R B R\'";
            if (caz == "(ULB UFL RUB)") return "B L F L\' B\' L F\' L\'";
            if (caz == "(ULB UFL RDF)") return "U\' L D\' L\' U L D L\'";
            if (caz == "(ULB UFL RBD)") return "U\' F\' D2 F U F\' D2 F";
            if (caz == "(ULB UFL BRU)") return "L U2 L D L\' U2 L D\' L2";
            if (caz == "(ULB UFL BDR)") return "U\' L D2 L\' U L D2 L\'";
            if (caz == "(ULB UFL BLD)") return "U\' F\' D F U F\' D\' F";
            if (caz == "(ULB UFL LDB)") return "B\' L F L\' B L F\' L\'";
            if (caz == "(ULB UFL LFD)") return "U\' L D L\' U L D\' L\'";
            if (caz == "(ULB UFL FUR)") return "L2 D\' L U2 L\' D L U2 L";
            if (caz == "(ULB UFL FDL)") return "U\' F\' D\' F U F\' D F";
            if (caz == "(ULB UFL FRD)") return "L\' D2 L U L\' D2 L U\'";
            if (caz == "(ULB UFL DFR)") return "F2 D B2 D\' F2 D B2 D\'";
            if (caz == "(ULB UFL DRB)") return "B2 L F L\' B2 L F\' L\'";
            if (caz == "(ULB UFL DLF)") return "D F2 D B2 D\' F2 D B2 D2";
            if (caz == "(ULB UFL DBL)") return "D2 F2 D B2 D\' F2 D B2 D";
            if (caz == "(ULB URF UFL)") return "F2 L2 F\' R\' F L2 F\' R F\'";
            if (caz == "(ULB URF UBR)") return "B L\' B R2 B\' L B R2 B2";
            if (caz == "(ULB URF RUB)") return "F2 L2 F\' R2 F L2 F\' R2 F\'";
            if (caz == "(ULB URF RDF)") return "U2 R\' D\' R U2 R\' D R";
            if (caz == "(ULB URF RBD)") return "L\' D L U2 L\' D\' L U2";
            if (caz == "(ULB URF BRU)") return "L\' B2 L\' F2 L B2 L\' F2 L2";
            if (caz == "(ULB URF BDR)") return "U2 F D\' F\' U2 F D F\'";
            if (caz == "(ULB URF BLD)") return "U2 R\' D2 R U2 R\' D2 R";
            if (caz == "(ULB URF LUF)") return "B L2 B R2 B\' L2 B R2 B2";
            if (caz == "(ULB URF LDB)") return "U2 F D2 F\' U2 F D2 F\'";
            if (caz == "(ULB URF LFD)") return "U2 R\' D R U2 R\' D\' R";
            if (caz == "(ULB URF FLU)") return "R2 B2 R F2 R\' B2 R F2 R";
            if (caz == "(ULB URF FDL)") return "B D\' B\' U2 B D B\' U2";
            if (caz == "(ULB URF FRD)") return "U2 F D F\' U2 F D\' F\'";
            if (caz == "(ULB URF DFR)") return "B U2 B\' U\' F2 U B U\' F2 U\' B\'";
            if (caz == "(ULB URF DRB)") return "U R2 U\' R2 U\' B2 U R2 U R2 U\' B2";
            if (caz == "(ULB URF DLF)") return "U2 R U2 R\' F2 R\' F2 R F2 L F2 L\'";
            if (caz == "(ULB URF DBL)") return "B L B2 L F\' L\' B2 L F L2 B\'";
            if (caz == "(ULB UBR UFL)") return "B\' R B\' L2 B R\' B\' L2 B2";
            if (caz == "(ULB UBR URF)") return "B2 R2 B\' L\' B R2 B\' L B\'";
            if (caz == "(ULB UBR RFU)") return "B2 D B\' U2 B D\' B\' U2 B\'";
            if (caz == "(ULB UBR RDF)") return "B D2 B\' U\' B D2 B\' U";
            if (caz == "(ULB UBR RBD)") return "U R D R\' U\' R D\' R\'";
            if (caz == "(ULB UBR BDR)") return "U B\' D\' B U\' B\' D B";
            if (caz == "(ULB UBR BLD)") return "B D B\' U\' B D\' B\' U";
            if (caz == "(ULB UBR LUF)") return "B\' U2 B\' D\' B U2 B\' D B2";
            if (caz == "(ULB UBR LDB)") return "U R D\' R\' U\' R D R\'";
            if (caz == "(ULB UBR LFD)") return "U B\' D2 B U\' B\' D2 B";
            if (caz == "(ULB UBR FLU)") return "F R\' F\' L F R F\' L\'";
            if (caz == "(ULB UBR FUR)") return "B L B\' R B L\' B\' R\'";
            if (caz == "(ULB UBR FDL)") return "U R D2 R\' U\' R D2 R\'";
            if (caz == "(ULB UBR FRD)") return "U B\' D B U\' B\' D\' B";
            if (caz == "(ULB UBR DFR)") return "B L B\' R2 B L\' B\' R2";
            if (caz == "(ULB UBR DRB)") return "D\' R2 D\' L2 D R2 D\' L2 D2";
            if (caz == "(ULB UBR DLF)") return "D R2 D\' L2 D R2 D\' L2";
            if (caz == "(ULB UBR DBL)") return "D2 R2 D\' L2 D R2 D\' L2 D\'";
            if (caz == "(ULB RFU UFL)") return "R B\' R\' F R B R\' F\'";
            if (caz == "(ULB RFU UBR)") return "B U2 B D B\' U2 B D\' B2";
            if (caz == "(ULB RFU RUB)") return "U F U\' B\' U F\' U\' B";
            if (caz == "(ULB RFU RDF)") return "R\' F L2 F\' R F L2 F\'";
            if (caz == "(ULB RFU RBD)") return "R2 D L\' D\' R2 D L D\'";
            if (caz == "(ULB RFU BRU)") return "R2 D\' R U2 R\' D R U2 R";
            if (caz == "(ULB RFU BDR)") return "U B\' L2 B R B\' L2 B R\' U\'";
            if (caz == "(ULB RFU BLD)") return "D\' R2 D L\' D\' R2 D L";
            if (caz == "(ULB RFU LUF)") return "R B\' U2 B R\' B\' R U2 R\' B";
            if (caz == "(ULB RFU LDB)") return "L\' F2 L B L\' F2 L B\'";
            if (caz == "(ULB RFU LFD)") return "R B\' R\' F2 R B R\' F2";
            if (caz == "(ULB RFU FLU)") return "F R F\' L F R\' F\' L\'";
            if (caz == "(ULB RFU FDL)") return "U F\' L B2 L\' F L B2 L\' U\'";
            if (caz == "(ULB RFU FRD)") return "D R2 D L\' D\' R2 D L D2";
            if (caz == "(ULB RFU DFR)") return "R B\' R\' F\' R B R\' F";
            if (caz == "(ULB RFU DRB)") return "D\' F D B2 D\' F\' D B2";
            if (caz == "(ULB RFU DLF)") return "L2 B\' R B L2 B\' R\' B";
            if (caz == "(ULB RFU DBL)") return "D2 F D B2 D\' F\' D B2 D";
            if (caz == "(ULB RUB UFL)") return "B\' R\' F R B R\' F\' R";
            if (caz == "(ULB RUB URF)") return "F R2 F L2 F\' R2 F L2 F2";
            if (caz == "(ULB RUB RFU)") return "U\' L\' U R U\' L U R\'";
            if (caz == "(ULB RUB RDF)") return "U\' L\' U R2 U\' L U R2";
            if (caz == "(ULB RUB RBD)") return "U\' L\' U R\' U\' L U R";
            if (caz == "(ULB RUB BDR)") return "U2 F\' U B2 U\' F U B2 U";
            if (caz == "(ULB RUB BLD)") return "D\' R D L\' D\' R\' D L";
            if (caz == "(ULB RUB LUF)") return "F2 R2 F L2 F\' R2 F L2 F";
            if (caz == "(ULB RUB LDB)") return "U R\' U L2 U\' R U L2 U2";
            if (caz == "(ULB RUB LFD)") return "U\' F U B U\' F\' U B\'";
            if (caz == "(ULB RUB FLU)") return "U\' F\' U B U\' F U B\'";
            if (caz == "(ULB RUB FUR)") return "B\' R\' F\' R B R\' F R";
            if (caz == "(ULB RUB FDL)") return "B\' U F2 U\' B U F2 U\'";
            if (caz == "(ULB RUB FRD)") return "B\' U F\' U\' B U F U\'";
            if (caz == "(ULB RUB DFR)") return "U\' F2 U B U\' F2 U B\'";
            if (caz == "(ULB RUB DRB)") return "U2 R2 U L U\' R2 U L\' U";
            if (caz == "(ULB RUB DLF)") return "F\' R2 F L2 F\' R2 F L2";
            if (caz == "(ULB RUB DBL)") return "U B2 U F U\' B2 U F\' U2";
            if (caz == "(ULB RDF UFL)") return "U B D2 B\' U\' B D2 B\'";
            if (caz == "(ULB RDF URF)") return "U2 B D2 B\' U2 B D2 B\'";
            if (caz == "(ULB RDF UBR)") return "U\' B D2 B\' U B D2 B\'";
            if (caz == "(ULB RDF RFU)") return "F L2 F\' R\' F L2 F\' R";
            if (caz == "(ULB RDF RUB)") return "R2 U\' L\' U R2 U\' L U";
            if (caz == "(ULB RDF RBD)") return "R\' D L\' D\' R D L D\'";
            if (caz == "(ULB RDF BRU)") return "L\' B2 L\' F L B2 L\' F\' L2";
            if (caz == "(ULB RDF BDR)") return "D R\' U R D\' R\' U\' R";
            if (caz == "(ULB RDF BLD)") return "L B\' R2 B L\' B\' R2 B";
            if (caz == "(ULB RDF LUF)") return "U L2 D\' L\' U2 L D L\' U2 L\' U\'";
            if (caz == "(ULB RDF LDB)") return "D2 L U L\' D2 L U\' L\'";
            if (caz == "(ULB RDF LFD)") return "R2 B\' R\' F2 R B R\' F2 R\'";
            if (caz == "(ULB RDF FLU)") return "U\' R U L U\' R\' U L\'";
            if (caz == "(ULB RDF FUR)") return "D2 B D F2 D\' B\' D F2 D";
            if (caz == "(ULB RDF FDL)") return "F\' U2 F D F\' U2 F D\'";
            if (caz == "(ULB RDF DRB)") return "U F\' U R2 U\' F U F\' R2 F U2";
            if (caz == "(ULB RDF DLF)") return "D R D\' L2 D R\' D\' L2";
            if (caz == "(ULB RDF DBL)") return "U B2 U F\' U\' B2 U F U2";
            if (caz == "(ULB RBD UFL)") return "U L\' D L U\' L\' D\' L";
            if (caz == "(ULB RBD URF)") return "U2 L\' D L U2 L\' D\' L";
            if (caz == "(ULB RBD UBR)") return "U\' L\' D L U L\' D\' L";
            if (caz == "(ULB RBD RFU)") return "D L\' D\' R2 D L D\' R2";
            if (caz == "(ULB RBD RUB)") return "R\' U\' L\' U R U\' L U";
            if (caz == "(ULB RBD RDF)") return "R F L2 F\' R\' F L2 F\'";
            if (caz == "(ULB RBD BRU)") return "D L\' B2 L D\' L\' D B2 D\' L";
            if (caz == "(ULB RBD BLD)") return "B R B\' L\' B R\' B\' L";
            if (caz == "(ULB RBD LUF)") return "F2 R F L2 F\' R\' F L2 F";
            if (caz == "(ULB RBD LDB)") return "L B2 L F L\' B2 L F\' L2";
            if (caz == "(ULB RBD LFD)") return "D2 F U\' F\' D2 F U F\'";
            if (caz == "(ULB RBD FLU)") return "U\' R2 U L U\' R2 U L\'";
            if (caz == "(ULB RBD FUR)") return "U B\' R F2 R\' B R F2 R\' U\'";
            if (caz == "(ULB RBD FDL)") return "U R\' F2 R B R\' F2 R B\' U\'";
            if (caz == "(ULB RBD FRD)") return "D\' R U2 R\' D R U2 R\'";
            if (caz == "(ULB RBD DFR)") return "F D2 F U\' F\' D2 F U F2";
            if (caz == "(ULB RBD DLF)") return "F\' R F L2 F\' R\' F L2";
            if (caz == "(ULB RBD DBL)") return "D2 F\' D B2 D\' F D B2 D";
            if (caz == "(ULB BRU UFL)") return "L2 D L\' U2 L D\' L\' U2 L\'";
            if (caz == "(ULB BRU URF)") return "L2 F2 L B2 L\' F2 L B2 L";
            if (caz == "(ULB BRU RFU)") return "R\' U2 R\' D\' R U2 R\' D R2";
            if (caz == "(ULB BRU RDF)") return "L2 F L B2 L\' F\' L B2 L";
            if (caz == "(ULB BRU RBD)") return "L\' D B2 D\' L D L\' B2 L D\'";
            if (caz == "(ULB BRU BDR)") return "R B2 R F R\' B2 R F\' R2";
            if (caz == "(ULB BRU BLD)") return "R\' U2 R\' D2 R U2 R\' D2 R2";
            if (caz == "(ULB BRU LUF)") return "F R\' U2 R F\' R\' F U2 F\' R";
            if (caz == "(ULB BRU LDB)") return "L2 F\' L B2 L\' F L B2 L";
            if (caz == "(ULB BRU LFD)") return "R\' U2 R\' D R U2 R\' D\' R2";
            if (caz == "(ULB BRU FLU)") return "R B2 R F2 R\' B2 R F2 R2";
            if (caz == "(ULB BRU FUR)") return "L F\' U2 F L\' F\' L U2 L\' F";
            if (caz == "(ULB BRU FDL)") return "R B2 R F\' R\' B2 R F R2";
            if (caz == "(ULB BRU FRD)") return "U2 L\' U2 L\' D2 L U2 L\' D2 L2 U2";
            if (caz == "(ULB BRU DFR)") return "L2 D\' L\' U2 L D L\' U2 L\'";
            if (caz == "(ULB BRU DRB)") return "L2 D2 L\' U2 L D2 L\' U2 L\'";
            if (caz == "(ULB BRU DLF)") return "U B2 D\' B\' U2 B D B\' U2 B\' U\'";
            if (caz == "(ULB BRU DBL)") return "D\' R B2 R\' D R D\' B2 D R\'";
            if (caz == "(ULB BDR UFL)") return "L D2 L\' U\' L D2 L\' U";
            if (caz == "(ULB BDR URF)") return "F D\' F\' U2 F D F\' U2";
            if (caz == "(ULB BDR UBR)") return "B\' D\' B U B\' D B U\'";
            if (caz == "(ULB BDR RFU)") return "U R B\' L2 B R\' B\' L2 B U\'";
            if (caz == "(ULB BDR RUB)") return "U\' B2 U\' F\' U B2 U\' F U2";
            if (caz == "(ULB BDR RDF)") return "R\' U R D R\' U\' R D\'";
            if (caz == "(ULB BDR BRU)") return "R2 F R\' B2 R F\' R\' B2 R\'";
            if (caz == "(ULB BDR BLD)") return "D\' R\' D L\' D\' R D L";
            if (caz == "(ULB BDR LUF)") return "F2 D\' F\' U2 F D F\' U2 F\'";
            if (caz == "(ULB BDR LDB)") return "R\' U R D\' R\' U\' R D";
            if (caz == "(ULB BDR LFD)") return "U B L2 B\' R\' B L2 B\' R U\'";
            if (caz == "(ULB BDR FLU)") return "F R2 F\' L F R2 F\' L\'";
            if (caz == "(ULB BDR FUR)") return "R2 F\' L F R2 F\' L\' F";
            if (caz == "(ULB BDR FDL)") return "R\' U R D2 R\' U\' R D2";
            if (caz == "(ULB BDR FRD)") return "F\' R2 F\' L F R2 F\' L\' F2";
            if (caz == "(ULB BDR DFR)") return "R D\' L2 D R\' D\' L2 D";
            if (caz == "(ULB BDR DLF)") return "L2 U R\' U\' L2 U R U\'";
            if (caz == "(ULB BDR DBL)") return "R\' U B2 U\' R U R\' B2 R U\'";
            if (caz == "(ULB BLD UFL)") return "U B D B\' U\' B D\' B\'";
            if (caz == "(ULB BLD URF)") return "U2 B D B\' U2 B D\' B\'";
            if (caz == "(ULB BLD UBR)") return "U\' B D B\' U B D\' B\'";
            if (caz == "(ULB BLD RFU)") return "B\' R B L B\' R\' B L\'";
            if (caz == "(ULB BLD RUB)") return "U R U\' L U R\' U\' L\'";
            if (caz == "(ULB BLD RDF)") return "B\' R2 B L B\' R2 B L\'";
            if (caz == "(ULB BLD RBD)") return "B\' U\' B D B\' U B D\'";
            if (caz == "(ULB BLD BRU)") return "R2 D2 R U2 R\' D2 R U2 R";
            if (caz == "(ULB BLD BDR)") return "U R\' U\' L U R U\' L\'";
            if (caz == "(ULB BLD LUF)") return "B L2 B R B\' L2 B R\' B2";
            if (caz == "(ULB BLD LFD)") return "B\' U\' B D\' B\' U B D";
            if (caz == "(ULB BLD FLU)") return "U2 B2 U F U\' B2 U F\' U";
            if (caz == "(ULB BLD FUR)") return "L\' B R2 B\' L B R2 B\'";
            if (caz == "(ULB BLD FDL)") return "B2 R\' B L2 B\' R B L2 B";
            if (caz == "(ULB BLD FRD)") return "B\' U\' B D2 B\' U B D2";
            if (caz == "(ULB BLD DFR)") return "U R2 U\' L U R2 U\' L\'";
            if (caz == "(ULB BLD DRB)") return "R D2 R U2 R\' D2 R U2 R2";
            if (caz == "(ULB BLD DLF)") return "B2 D2 B U B\' D2 B U\' B";
            if (caz == "(ULB LUF URF)") return "B2 R2 B\' L2 B R2 B\' L2 B\'";
            if (caz == "(ULB LUF UBR)") return "B2 D\' B U2 B\' D B U2 B";
            if (caz == "(ULB LUF RFU)") return "B\' R U2 R\' B R B\' U2 B R\'";
            if (caz == "(ULB LUF RUB)") return "F\' L2 F\' R2 F L2 F\' R2 F2";
            if (caz == "(ULB LUF RDF)") return "U L U2 L D\' L\' U2 L D L2 U\'";
            if (caz == "(ULB LUF RBD)") return "F\' L2 F\' R F L2 F\' R\' F2";
            if (caz == "(ULB LUF BRU)") return "R\' F U2 F\' R F R\' U2 R F\'";
            if (caz == "(ULB LUF BDR)") return "F U2 F D\' F\' U2 F D F2";
            if (caz == "(ULB LUF BLD)") return "B2 R B\' L2 B R\' B\' L2 B\'";
            if (caz == "(ULB LUF LDB)") return "F U2 F D2 F\' U2 F D2 F2";
            if (caz == "(ULB LUF LFD)") return "F\' L2 F\' R\' F L2 F\' R F2";
            if (caz == "(ULB LUF FUR)") return "F U2 F D F\' U2 F D\' F2";
            if (caz == "(ULB LUF FDL)") return "B D\' L2 D B\' D\' B L2 B\' D";
            if (caz == "(ULB LUF FRD)") return "B2 R\' B\' L2 B R B\' L2 B\'";
            if (caz == "(ULB LUF DFR)") return "B2 D B U2 B\' D\' B U2 B";
            if (caz == "(ULB LUF DRB)") return "U2 F2 D2 F U2 F\' D2 F U2 F U2";
            if (caz == "(ULB LUF DLF)") return "B2 D2 B U2 B\' D2 B U2 B";
            if (caz == "(ULB LUF DBL)") return "D F\' L2 F D\' F\' D L2 D\' F";
            if (caz == "(ULB LDB UFL)") return "U L\' D\' L U\' L\' D L";
            if (caz == "(ULB LDB URF)") return "U2 L\' D\' L U2 L\' D L";
            if (caz == "(ULB LDB UBR)") return "U\' L\' D\' L U L\' D L";
            if (caz == "(ULB LDB RFU)") return "B L\' F2 L B\' L\' F2 L";
            if (caz == "(ULB LDB RUB)") return "U2 L2 U\' R\' U L2 U\' R U\'";
            if (caz == "(ULB LDB RDF)") return "D2 F\' U2 F D2 F\' U2 F";
            if (caz == "(ULB LDB RBD)") return "L2 F L\' B2 L F\' L\' B2 L\'";
            if (caz == "(ULB LDB BRU)") return "L\' B2 L\' F\' L B2 L\' F L2";
            if (caz == "(ULB LDB BDR)") return "D\' R\' U R D R\' U\' R";
            if (caz == "(ULB LDB LUF)") return "F2 D2 F\' U2 F D2 F\' U2 F\'";
            if (caz == "(ULB LDB LFD)") return "U\' F U B\' U\' F\' U B";
            if (caz == "(ULB LDB FLU)") return "U\' F\' U B\' U\' F U B";
            if (caz == "(ULB LDB FUR)") return "B D F2 D\' B\' D F2 D\'";
            if (caz == "(ULB LDB FDL)") return "B L\' F\' L B\' L\' F L";
            if (caz == "(ULB LDB FRD)") return "L F2 L\' B\' L F2 L\' B";
            if (caz == "(ULB LDB DFR)") return "U\' F2 U B\' U\' F2 U B";
            if (caz == "(ULB LDB DRB)") return "D\' R D\' L2 D R\' D\' L2 D2";
            if (caz == "(ULB LDB DLF)") return "F\' D2 F\' U2 F D2 F\' U2 F2";
            if (caz == "(ULB LFD UFL)") return "F L\' B\' L F\' L\' B L";
            if (caz == "(ULB LFD URF)") return "R\' D R U2 R\' D\' R U2";
            if (caz == "(ULB LFD UBR)") return "B\' D2 B U B\' D2 B U\'";
            if (caz == "(ULB LFD RFU)") return "F2 R B\' R\' F2 R B R\'";
            if (caz == "(ULB LFD RUB)") return "B U\' F U B\' U\' F\' U";
            if (caz == "(ULB LFD RDF)") return "R F2 R B\' R\' F2 R B R2";
            if (caz == "(ULB LFD RBD)") return "F U\' F\' D2 F U F\' D2";
            if (caz == "(ULB LFD BRU)") return "R2 D R U2 R\' D\' R U2 R";
            if (caz == "(ULB LFD BDR)") return "U R\' B L2 B\' R B L2 B\' U\'";
            if (caz == "(ULB LFD BLD)") return "D\' B\' U\' B D B\' U B";
            if (caz == "(ULB LFD LUF)") return "F2 R\' F L2 F\' R F L2 F";
            if (caz == "(ULB LFD LDB)") return "B\' U\' F U B U\' F\' U";
            if (caz == "(ULB LFD FLU)") return "U L2 U R U\' L2 U R\' U2";
            if (caz == "(ULB LFD FUR)") return "U F\' R2 F L F\' R2 F L\' U\'";
            if (caz == "(ULB LFD FRD)") return "D R U2 R\' D\' R U2 R\'";
            if (caz == "(ULB LFD DFR)") return "F\' D B2 D\' F D B2 D\'";
            if (caz == "(ULB LFD DRB)") return "B2 U\' F U B2 U\' F\' U";
            if (caz == "(ULB LFD DBL)") return "F U\' L2 U F\' U\' F L2 F\' U";
            if (caz == "(ULB FLU URF)") return "R\' F2 R\' B2 R F2 R\' B2 R2";
            if (caz == "(ULB FLU UBR)") return "B\' R\' B L\' B\' R B L";
            if (caz == "(ULB FLU RFU)") return "B\' R B L\' B\' R\' B L";
            if (caz == "(ULB FLU RUB)") return "U R U\' L\' U R\' U\' L";
            if (caz == "(ULB FLU RDF)") return "B\' R2 B L\' B\' R2 B L";
            if (caz == "(ULB FLU RBD)") return "L U\' R2 U L\' U\' R2 U";
            if (caz == "(ULB FLU BRU)") return "R2 F2 R\' B2 R F2 R\' B2 R\'";
            if (caz == "(ULB FLU BDR)") return "U R\' U\' L\' U R U\' L";
            if (caz == "(ULB FLU BLD)") return "U\' F U\' B2 U F\' U\' B2 U2";
            if (caz == "(ULB FLU LDB)") return "B\' U\' F\' U B U\' F U";
            if (caz == "(ULB FLU LFD)") return "U2 R U\' L2 U R\' U\' L2 U\'";
            if (caz == "(ULB FLU FUR)") return "U B U\' F\' U B\' U\' F";
            if (caz == "(ULB FLU FDL)") return "U B U\' F U B\' U\' F\'";
            if (caz == "(ULB FLU FRD)") return "U B U\' F2 U B\' U\' F2";
            if (caz == "(ULB FLU DFR)") return "U R2 U\' L\' U R2 U\' L";
            if (caz == "(ULB FLU DRB)") return "B2 U\' F\' U B2 U\' F U";
            if (caz == "(ULB FLU DLF)") return "U2 F2 U\' B\' U F2 U\' B U\'";
            if (caz == "(ULB FLU DBL)") return "U\' L2 U\' R\' U L2 U\' R U2";
            if (caz == "(ULB FUR UFL)") return "L\' U2 L\' D\' L U2 L\' D L2";
            if (caz == "(ULB FUR UBR)") return "R B L B\' R\' B L\' B\'";
            if (caz == "(ULB FUR RUB)") return "B L F\' L\' B\' L F L\'";
            if (caz == "(ULB FUR RDF)") return "D\' F2 D\' B D F2 D\' B\' D2";
            if (caz == "(ULB FUR RBD)") return "U R F2 R\' B\' R F2 R\' B U\'";
            if (caz == "(ULB FUR BRU)") return "F\' L U2 L\' F L F\' U2 F L\'";
            if (caz == "(ULB FUR BDR)") return "F\' L F R2 F\' L\' F R2";
            if (caz == "(ULB FUR BLD)") return "B R2 B\' L\' B R2 B\' L";
            if (caz == "(ULB FUR LUF)") return "F2 D F\' U2 F D\' F\' U2 F\'";
            if (caz == "(ULB FUR LDB)") return "B\' L F\' L\' B L F L\'";
            if (caz == "(ULB FUR LFD)") return "U L F\' R2 F L\' F\' R2 F U\'";
            if (caz == "(ULB FUR FLU)") return "U\' R\' U L U\' R U L\'";
            if (caz == "(ULB FUR FDL)") return "F2 D\' B D F2 D\' B\' D";
            if (caz == "(ULB FUR FRD)") return "F R\' B2 R F\' R\' B2 R";
            if (caz == "(ULB FUR DFR)") return "R\' D\' L2 D R D\' L2 D";
            if (caz == "(ULB FUR DRB)") return "B2 L F\' L\' B2 L F L\'";
            if (caz == "(ULB FUR DLF)") return "D R\' D\' L2 D R D\' L2";
            if (caz == "(ULB FUR DBL)") return "D2 R\' D\' L2 D R D\' L2 D\'";
            if (caz == "(ULB FDL UFL)") return "U B D\' B\' U\' B D B\'";
            if (caz == "(ULB FDL URF)") return "U2 B D\' B\' U2 B D B\'";
            if (caz == "(ULB FDL UBR)") return "U\' B D\' B\' U B D B\'";
            if (caz == "(ULB FDL RFU)") return "U L B2 L\' F\' L B2 L\' F U\'";
            if (caz == "(ULB FDL RUB)") return "U F2 U\' B\' U F2 U\' B";
            if (caz == "(ULB FDL RDF)") return "D F\' U2 F D\' F\' U2 F";
            if (caz == "(ULB FDL RBD)") return "U B R\' F2 R B\' R\' F2 R U\'";
            if (caz == "(ULB FDL BRU)") return "R2 F\' R\' B2 R F R\' B2 R\'";
            if (caz == "(ULB FDL BDR)") return "D2 R\' U R D2 R\' U\' R";
            if (caz == "(ULB FDL BLD)") return "B\' L2 B\' R\' B L2 B\' R B2";
            if (caz == "(ULB FDL LUF)") return "D\' B L2 B\' D B D\' L2 D B\'";
            if (caz == "(ULB FDL LDB)") return "D\' L U L\' D L U\' L\'";
            if (caz == "(ULB FDL FLU)") return "D\' B D F\' D\' B\' D F";
            if (caz == "(ULB FDL FUR)") return "D\' B D F2 D\' B\' D F2";
            if (caz == "(ULB FDL FRD)") return "D\' B D F D\' B\' D F\'";
            if (caz == "(ULB FDL DFR)") return "R\' D2 R\' U R D2 R\' U\' R2";
            if (caz == "(ULB FDL DRB)") return "R F\' R\' B2 R F R\' B2";
            if (caz == "(ULB FDL DBL)") return "D2 R D\' L2 D R\' D\' L2 D\'";
            if (caz == "(ULB FRD UFL)") return "U L\' D2 L U\' L\' D2 L";
            if (caz == "(ULB FRD URF)") return "U2 L\' D2 L U2 L\' D2 L";
            if (caz == "(ULB FRD UBR)") return "U\' L\' D2 L U L\' D2 L";
            if (caz == "(ULB FRD RFU)") return "D2 L\' D\' R2 D L D\' R2 D\'";
            if (caz == "(ULB FRD RUB)") return "U F\' U\' B\' U F U\' B";
            if (caz == "(ULB FRD RBD)") return "R U2 R\' D\' R U2 R\' D";
            if (caz == "(ULB FRD BRU)") return "U2 L2 D2 L U2 L\' D2 L U2 L U2";
            if (caz == "(ULB FRD BDR)") return "F2 L F R2 F\' L\' F R2 F";
            if (caz == "(ULB FRD BLD)") return "R U2 R\' D2 R U2 R\' D2";
            if (caz == "(ULB FRD LUF)") return "B L2 B R\' B\' L2 B R B2";
            if (caz == "(ULB FRD LDB)") return "B\' L F2 L\' B L F2 L\'";
            if (caz == "(ULB FRD LFD)") return "R U2 R\' D R U2 R\' D\'";
            if (caz == "(ULB FRD FLU)") return "R\' B2 R F2 R\' B2 R F2";
            if (caz == "(ULB FRD FUR)") return "R\' B2 R F R\' B2 R F\'";
            if (caz == "(ULB FRD FDL)") return "R\' B2 R F\' R\' B2 R F";
            if (caz == "(ULB FRD DRB)") return "B2 L F2 L\' B2 L F2 L\'";
            if (caz == "(ULB FRD DLF)") return "U L D2 L U\' L\' D2 L U L2 U\'";
            if (caz == "(ULB FRD DBL)") return "U\' L2 U\' R U L2 U\' R\' U2";
            if (caz == "(ULB DFR UFL)") return "D B2 D\' F2 D B2 D\' F2";
            if (caz == "(ULB DFR URF)") return "B U F2 U B\' U\' F2 U B U2 B\'";
            if (caz == "(ULB DFR UBR)") return "R2 B L B\' R2 B L\' B\'";
            if (caz == "(ULB DFR RFU)") return "D B2 D\' F D B2 D\' F\'";
            if (caz == "(ULB DFR RUB)") return "B U\' F2 U B\' U\' F2 U";
            if (caz == "(ULB DFR RBD)") return "F2 U\' F\' D2 F U F\' D2 F\'";
            if (caz == "(ULB DFR BRU)") return "L U2 L D\' L\' U2 L D L2";
            if (caz == "(ULB DFR BDR)") return "D\' L2 D R D\' L2 D R\'";
            if (caz == "(ULB DFR BLD)") return "B R\' B\' L\' B R B\' L";
            if (caz == "(ULB DFR LUF)") return "B\' U2 B\' D B U2 B\' D\' B2";
            if (caz == "(ULB DFR LDB)") return "B\' U\' F2 U B U\' F2 U";
            if (caz == "(ULB DFR LFD)") return "D B2 D\' F\' D B2 D\' F";
            if (caz == "(ULB DFR FLU)") return "L\' U R2 U\' L U R2 U\'";
            if (caz == "(ULB DFR FUR)") return "R F\' L F R\' F\' L\' F";
            if (caz == "(ULB DFR FDL)") return "R2 U R D2 R\' U\' R D2 R";
            if (caz == "(ULB DFR DRB)") return "B2 U\' F2 U B2 U\' F2 U";
            if (caz == "(ULB DFR DLF)") return "F\' R\' F L2 F\' R F L2";
            if (caz == "(ULB DFR DBL)") return "B2 U2 F2 U B2 U\' F2 U B2 U B2";
            if (caz == "(ULB DRB UFL)") return "B2 D\' F2 D B2 D\'F2 D";
            if (caz == "(ULB DRB URF)") return "U2 R B2 R\' B2 R\' U2 R B2 R B2 R\'";
            if (caz == "(ULB DRB UBR)") return "D2 L2 D R2 D\' L2 D R2 D";
            if (caz == "(ULB DRB RFU)") return "B2 D\' F D B2 D\' F\' D";
            if (caz == "(ULB DRB RUB)") return "U\' L U\' R2 U L\' U\' R2 U2";
            if (caz == "(ULB DRB RDF)") return "U2 B2 U2 B D2 B\' U2 B D2 B U2";
            if (caz == "(ULB DRB BRU)") return "L U2 L D2 L\' U2 L D2 L2";
            if (caz == "(ULB DRB BLD)") return "R2 U2 R\' D2 R U2 R\' D2 R\'";
            if (caz == "(ULB DRB LUF)") return "U2 F\' U2 F\' D2 F U2 F\' D2 F2 U2";
            if (caz == "(ULB DRB LDB)") return "D2 L2 D R D\' L2 D R\' D";
            if (caz == "(ULB DRB LFD)") return "U\' F U B2 U\' F\' U B2";
            if (caz == "(ULB DRB FLU)") return "U\' F\' U B2 U\' F U B2";
            if (caz == "(ULB DRB FUR)") return "L F\' L\' B2 L F L\' B2";
            if (caz == "(ULB DRB FDL)") return "B2 R F\' R\' B2 R F R\'";
            if (caz == "(ULB DRB FRD)") return "B2 D\' F\' D B2 D\' F D";
            if (caz == "(ULB DRB DFR)") return "U\' F2 U B2 U\' F2 U B2";
            if (caz == "(ULB DRB DLF)") return "U B2 U\' L2 U\' L2 U L2 D L2 D\' B2";
            if (caz == "(ULB DRB DBL)") return "U\' L2 U\' R2 U L2 U\' R2 U2";
            if (caz == "(ULB DLF UFL)") return "D2 B2 D\' F2 D B2 D\' F2 D\'";
            if (caz == "(ULB DLF URF)") return "U L2 U\' L2 U\' F2 U L2 U L2 U\' F2";
            if (caz == "(ULB DLF UBR)") return "B\' R\' B L2 B\' R B L2";
            if (caz == "(ULB DLF RFU)") return "B\' R B L2 B\' R\' B L2";
            if (caz == "(ULB DLF RUB)") return "U R U\' L2 U R\' U\' L2";
            if (caz == "(ULB DLF RDF)") return "B\' R2 B L2 B\' R2 B L2";
            if (caz == "(ULB DLF RBD)") return "L2 F\' R F L2 F\' R\' F";
            if (caz == "(ULB DLF BRU)") return "U B U2 B D\' B\' U2 B D B2 U\'";
            if (caz == "(ULB DLF BDR)") return "U R\' U\' L2 U R U\' L2";
            if (caz == "(ULB DLF BLD)") return "B\' U B\' D2 B U\' B\' D2 B2";
            if (caz == "(ULB DLF LUF)") return "B\' U2 B\' D2 B U2 B\' D2 B2";
            if (caz == "(ULB DLF LDB)") return "F2 U2 F D2 F\' U2 F D2 F";
            if (caz == "(ULB DLF FLU)") return "U B\' U F2 U\' B U F2 U2";
            if (caz == "(ULB DLF FUR)") return "L2 D R\' D\' L2 D R D\'";
            if (caz == "(ULB DLF FRD)") return "U L2 U\' L\' D2 L U L\' D2 L\' U\'";
            if (caz == "(ULB DLF DFR)") return "U R2 U\' L2 U R2 U\' L2";
            if (caz == "(ULB DLF DRB)") return "U\' L2 U B2 U B2 U\' B2 D\' B2 D L2";
            if (caz == "(ULB DLF DBL)") return "U B2 U F2 U\' B2 U F2 U2";
            if (caz == "(ULB DBL UFL)") return "D\' B2 D\' F2 D B2 D\' F2 D2";
            if (caz == "(ULB DBL URF)") return "B L2 F\' L\' B2 L F L\' B2 L\' B\'";
            if (caz == "(ULB DBL UBR)") return "D L2 D R2 D\' L2 D R2 D2";
            if (caz == "(ULB DBL RFU)") return "D\' B2 D\' F D B2 D\' F\' D2";
            if (caz == "(ULB DBL RUB)") return "U2 F U\' B2 U F\' U\' B2 U\'";
            if (caz == "(ULB DBL RDF)") return "U2 F\' U\' B2 U F U\' B2 U\'";
            if (caz == "(ULB DBL RBD)") return "D\' B2 D\' F\' D B2 D\' F D2";
            if (caz == "(ULB DBL BRU)") return "R D\' B2 D R\' D\' R B2 R\' D";
            if (caz == "(ULB DBL BDR)") return "U R\' B2 R U\' R\' U B2 U\' R";
            if (caz == "(ULB DBL LUF)") return "F\' D L2 D\' F D F\' L2 F D\'";
            if (caz == "(ULB DBL LFD)") return "U\' F L2 F\' U F U\' L2 U F\'";
            if (caz == "(ULB DBL FLU)") return "U2 R\' U L2 U\' R U L2 U";
            if (caz == "(ULB DBL FUR)") return "D L2 D R\' D\' L2 D R D2";
            if (caz == "(ULB DBL FDL)") return "D L2 D R D\' L2 D R\' D2";
            if (caz == "(ULB DBL FRD)") return "U2 R U L2 U\' R\' U L2 U";
            if (caz == "(ULB DBL DFR)") return "B2 U\' B2 U\' F2 U B2 U\' F2 U2 B2";
            if (caz == "(ULB DBL DRB)") return "U2 R2 U L2 U\' R2 U L2 U";
            if (caz == "(ULB DBL DLF)") return "U2 F2 U\' B2 U F2 U\' B2 U\'";

            return "nimic";
        }

        string ComutatorMuchii(string caz)//am scris un program in C++ care sa scrie toate aceste if-uri in C# in locul meu
        {

            if (caz == "(UR UB UL)") return "M2 U M\' U2 M U M2";
            if (caz == "(UR UB UF)") return "y\' M2 U M\' U2 M U M2 y";
            if (caz == "(UR UB FU)") return "M F R\' F\' M\' F R F\'";
            if (caz == "(UR UB FL)") return "R E2 R\' U R E2 R\' U\'";
            if (caz == "(UR UB FR)") return "U\' B\' E B U B\' E\' B";
            if (caz == "(UR UB FD)") return "S D S\' U S D\' S\' U\'";
            if (caz == "(UR UB RF)") return "U\' B E2 B\' U B E2 B\'";
            if (caz == "(UR UB RB)") return "U F\' E2 F U F\' E2 F U2";
            if (caz == "(UR UB RD)") return "U\' M\' D M U M\' D\' M";
            if (caz == "(UR UB BR)") return "U F E\' F\' U F E F\' U2";
            if (caz == "(UR UB BL)") return "R\' E2 R U R\' E2 R U\'";
            if (caz == "(UR UB BD)") return "U\' B E B\' U B E\' B\'";
            if (caz == "(UR UB LU)") return "L\' B L S\' L\' B\' L S";
            if (caz == "(UR UB LB)") return "B L S\' L\' B\' L S L\'";
            if (caz == "(UR UB LF)") return "U\' B\' E2 B U B\' E2 B";
            if (caz == "(UR UB LD)") return "U\' M\' D\' M U M\' D M";
            if (caz == "(UR UB DF)") return "M2 D R2 D\' M2 D R2 D\'";
            if (caz == "(UR UB DL)") return "D\' B2 D S2 D\' B2 D S2";
            if (caz == "(UR UB DR)") return "D\' M2 D R2 D\' M2 D R2";
            if (caz == "(UR UB DB)") return "B2 D S2 D\' B2 D S2 D\'";
            if (caz == "(UR UL UB)") return "B R E\' R\' U2 R E R\' U2 B\'";
            if (caz == "(UR UL UF)") return "F\' R\' E R U2 R\' E\' R U2 F";
            if (caz == "(UR UL FU)") return "R\' F R\' S2 R F\' R\' S2 R2";
            if (caz == "(UR UL FL)") return "R E2 R\' U2 R E2 R\' U2";
            if (caz == "(UR UL FR)") return "U2 L\' E2 L U2 L\' E2 L";
            if (caz == "(UR UL FD)") return "S D S\' U2 S D\' S\' U2";
            if (caz == "(UR UL RF)") return "U2 L E\' L\' U2 L E L\'";
            if (caz == "(UR UL RB)") return "U2 L\' E L U2 L\' E\' L";
            if (caz == "(UR UL RD)") return "R\' E\' R U2 R\' E R U2";
            if (caz == "(UR UL BU)") return "R B\' R S2 R\' B R S2 R2";
            if (caz == "(UR UL BR)") return "U2 L E2 L\' U2 L E2 L\'";
            if (caz == "(UR UL BL)") return "R\' E2 R U2 R\' E2 R U2";
            if (caz == "(UR UL BD)") return "S D\' S\' U2 S D S\' U2";
            if (caz == "(UR UL LB)") return "R E\' R\' U2 R E R\' U2";
            if (caz == "(UR UL LF)") return "R\' E R U2 R\' E\' R U2";
            if (caz == "(UR UL LD)") return "U2 L E L\' U2 L E\' L\'";
            if (caz == "(UR UL DF)") return "U M\' U2 M U2 M\' U2 M U";
            if (caz == "(UR UL DL)") return "S U2 S\' U2";
            if (caz == "(UR UL DR)") return "y U2 M\' U2 M y\'";
            if (caz == "(UR UL DB)") return "U M U2 M\' U";
            if (caz == "(UR UF UB)") return "F\' U R\' E R U\' R\' E\' R F";
            if (caz == "(UR UF UL)") return "F L E\' L\' U2 L E L\' U2 F\'";
            if (caz == "(UR UF FL)") return "R E2 R\' U\' R E2 R\' U";
            if (caz == "(UR UF FR)") return "U2 L\' E2 L U\' L\' E2 L U\'";
            if (caz == "(UR UF FD)") return "U F E F\' U\' F E\' F\'";
            if (caz == "(UR UF RF)") return "U2 L E\' L\' U\' L E L\' U\'";
            if (caz == "(UR UF RB)") return "U F\' E2 F U\' F\' E2 F";
            if (caz == "(UR UF RD)") return "R\' E\' R U\' R\' E R U";
            if (caz == "(UR UF BU)") return "M\' B\' R B M B\' R\' B";
            if (caz == "(UR UF BR)") return "U F E\' F\' U\' F E F\'";
            if (caz == "(UR UF BL)") return "R\' E2 R U\' R\' E2 R U";
            if (caz == "(UR UF BD)") return "S D\' S U\' S D S\' U";
            if (caz == "(UR UF LU)") return "L F\' L\' S\' L F L\' S";
            if (caz == "(UR UF LB)") return "R E\' R\' U\' R E R\' U";
            if (caz == "(UR UF LF)") return "R\' E R U\' R\' E\' R U";
            if (caz == "(UR UF LD)") return "U M D M\' U\' M D\' M\'";
            if (caz == "(UR UF DF)") return "U M U2 M U\' M\' U2 M\'";
            if (caz == "(UR UF DL)") return "D F2 D\' S2 D F2 D\' S2";
            if (caz == "(UR UF DR)") return "D M2 D\' R2 D M2 D\' R2";
            if (caz == "(UR UF DB)") return "D\' R2 D M2 D\' R2 D M2";
            if (caz == "(UR FU UB)") return "F R\' F\' M F R F\' M\'";
            if (caz == "(UR FU UL)") return "R2 S2 R F R\' S2 R F\' R";
            if (caz == "(UR FU FL)") return "F\' L S2 L\' F L S2 L\'";
            if (caz == "(UR FU FR)") return "R B\' M2 B R B\' M2 B R2";
            if (caz == "(UR FU FD)") return "F2 D S D\' F2 D S\' D\'";
            if (caz == "(UR FU RF)") return "F U\' M2 U R U\' M2 U R\' F\'";
            if (caz == "(UR FU RB)") return "B F R\' F\' M F R F\' M\' B\'";
            if (caz == "(UR FU RD)") return "D\' F2 D S D\' F2 D S\'";
            if (caz == "(UR FU BU)") return "B2 M2 B R B\' M2 B R\' B";
            if (caz == "(UR FU BR)") return "B\' M2 B R B\' M2 B R\'";
            if (caz == "(UR FU BL)") return "L2 F\' L S2 L\' F L S2 L";
            if (caz == "(UR FU BD)") return "F R\' F\' M2 F R F\' M2";
            if (caz == "(UR FU LU)") return "S R\' F R S\' R\' F\' R";
            if (caz == "(UR FU LB)") return "r E\' R\' U R E R\' U\' M";
            if (caz == "(UR FU LF)") return "M\' R\' E R U R\' E\' R U\' M";
            if (caz == "(UR FU LD)") return "D F2 D S D\' F2 D S\' D2";
            if (caz == "(UR FU DF)") return "F R\' F\' M\' F R F\' M";
            if (caz == "(UR FU DL)") return "S2 R\' F R S2 R\'F\' R";
            if (caz == "(UR FU DR)") return "D\' M D R2 D\' M\' D R2";
            if (caz == "(UR FU DB)") return "D2 M D R2 D\' M\' D R2 D";
            if (caz == "(UR FL UB)") return "U R E2 R\' U\' R E2 R\'";
            if (caz == "(UR FL UL)") return "U2 R E2 R\' U2 R E2 R\'";
            if (caz == "(UR FL UF)") return "U\' R E2 R\' U R E2 R\'";
            if (caz == "(UR FL FU)") return "L S2 L\' F\' L S2 L\' F";
            if (caz == "(UR FL FR)") return "E\' L2 E R\' E\' L2 E R";
            if (caz == "(UR FL FD)") return "L S2 L\' F L S2 L\' F\'";
            if (caz == "(UR FL RF)") return "L\' U2 L E\' L\' U2 L E";
            if (caz == "(UR FL RB)") return "L\' U2 L\' E L U2 L\' E\' L2";
            if (caz == "(UR FL RD)") return "S\' R\' F2 R S R\' F2 R";
            if (caz == "(UR FL BU)") return "F2 R\' F M2 F\' R F M2 F";
            if (caz == "(UR FL BR)") return "L\' U2 L E2 L\' U2 L E2";
            if (caz == "(UR FL BL)") return "L\' S\' L2 S L2 S\' L2 S L\'";
            if (caz == "(UR FL BD)") return "U\' M U\' L\' U M\' U\' L U2";
            if (caz == "(UR FL LU)") return "S R\' F2 R S\' R\' F2 R";
            if (caz == "(UR FL LB)") return "L\' U2 L E L\' U2 L E\'";
            if (caz == "(UR FL LD)") return "E2 R E L E\' R\' E L\' E";
            if (caz == "(UR FL DF)") return "U\' M2 U\' L\' U M2 U\' L U2";
            if (caz == "(UR FL DL)") return "S2 R\' F2 R S2 R\' F2 R";
            if (caz == "(UR FL DR)") return "R E R2 E\' R2 E R2 E\' R";
            if (caz == "(UR FL DB)") return "U M2 U L\' U\' M2 U L U2";
            if (caz == "(UR FR UB)") return "R\' F\' M F R F\' M\' F";
            if (caz == "(UR FR UL)") return "L\' E2 L U2 L\' E2 L U2";
            if (caz == "(UR FR UF)") return "U L\' E2 L U L\' E2 L U2";
            if (caz == "(UR FR FU)") return "R B\' M2 B R B\' M2 B R2";
            if (caz == "(UR FR FL)") return "F2 L S2 L\' F2 L S2 L\'";
            if (caz == "(UR FR FD)") return "U M\' U\' R U M U\' R\'";
            if (caz == "(UR FR RB)") return "E B U\' B\' E\' B U B\'";
            if (caz == "(UR FR RD)") return "D\' F D S D\' F\' D S\'";
            if (caz == "(UR FR BU)") return "R\' F M2 F\' R F M2 F\'";
            if (caz == "(UR FR BR)") return "U R2 B M\' B\' R2 B M B\' U\'";
            if (caz == "(UR FR BL)") return "E2 L U2 L\' E2 L U2 L\'";
            if (caz == "(UR FR BD)") return "U\' M U R U\' M\' U R\'";
            if (caz == "(UR FR LU)") return "R\' E\' L E R E\' L \' E";
            if (caz == "(UR FR LB)") return "S\' L S R S\' L\' S R\'";
            if (caz == "(UR FR LF)") return "E\' F U F\' E F U\' F\'";
            if (caz == "(UR FR LD)") return "R\' E\' L\' E R E\' L E";
            if (caz == "(UR FR DF)") return "U\' M2 U R U\' M2 U R\'";
            if (caz == "(UR FR DL)") return "L\' F2 L S2 L\' F2 L S2";
            if (caz == "(UR FR DR)") return "F R2 U M\' U\' R2 U M U\' F\'";
            if (caz == "(UR FR DB)") return "U M2 U\' R U M2 U\' R\'";
            if (caz == "(UR FD UB)") return "U S D S\' U\' S D\' S\'";
            if (caz == "(UR FD UL)") return "S\' D\' S U2 S\' D S U2";
            if (caz == "(UR FD UF)") return "F E F\' U F E\' F\' U\'";
            if (caz == "(UR FD FU)") return "D S D\' F2 D S\' D\' F2";
            if (caz == "(UR FD FL)") return "F L S2 L\' F\' L S2 L\'";
            if (caz == "(UR FD FR)") return "R U M\' U\' R\' U M U\'";
            if (caz == "(UR FD RF)") return "u U L\' U\' M\' U L U\' M u\'";
            if (caz == "(UR FD RB)") return "E\' R U M\' U\' R\' U M u\'";
            if (caz == "(UR FD RD)") return "M\' U M D\' M\' U\' M D";
            if (caz == "(UR FD BU)") return "M2 B\' R B M2 B\' R\' B";
            if (caz == "(UR FD BR)") return "R\' U M\' U\' R U M U\'";
            if (caz == "(UR FD BL)") return "U2 L U\' M\' U L\' U\' M U\'";
            if (caz == "(UR FD BD)") return "M\' U M D2 M\' U M D2";
            if (caz == "(UR FD LU)") return "S R\' F\' R S\' R\' F R";
            if (caz == "(UR FD LB)") return "r E\' R\' U\' R E R\' U M";
            if (caz == "(UR FD LF)") return "d S D\' F D S\' D\' F\' E\'";
            if (caz == "(UR FD LD)") return "M\' U M D M\' U\' M D\'";
            if (caz == "(UR FD DL)") return "S2 R\' F\' R S2 R\' F R";
            if (caz == "(UR FD DR)") return "R2 U M\' U\' R2 U M U\'";
            if (caz == "(UR FD DB)") return "M D\' R2 D M\' D\' R2 D";
            if (caz == "(UR RF UB)") return "B E2 B\' U\' B E2 B\' U";
            if (caz == "(UR RF UL)") return "L E\' L\' U2 L E L\' U2";
            if (caz == "(UR RF UF)") return "U L E\' L\' U L E L\' U2";
            if (caz == "(UR RF FU)") return "F\' R E2 R\' U\' R E2 R\' U F";
            if (caz == "(UR RF FL)") return "F\' U F E F\' U\' F E\'";
            if (caz == "(UR RF FD)") return "D S\' U F\' U\' S U F U\' D\'";
            if (caz == "(UR RF RB)") return "F\' U F\' E2 F U\' F\' E2 F2";
            if (caz == "(UR RF RD)") return "S\' U F\' U\' S U F U\'";
            if (caz == "(UR RF BU)") return "S F\' M F R F\' M\' F R\' S\'";
            if (caz == "(UR RF BR)") return "F\' U F E\' F\' U\' F E";
            if (caz == "(UR RF BL)") return "L2 E\' L\' U2 L E L\' U2 L\'";
            if (caz == "(UR RF BD)") return "S R F M\' F\' R\' F M f\'";
            if (caz == "(UR RF LU)") return "S U F\' U\' S\' U F U\'";
            if (caz == "(UR RF LB)") return "F\' U F E2 F\' U\' F E2";
            if (caz == "(UR RF LF)") return "F2 L\' S\' L F2 L\' S L";
            if (caz == "(UR RF LD)") return "L\' F2 L\' S\' L F2 L\' S L2";
            if (caz == "(UR RF DF)") return "F D\' S2 D F\' D\' S2 D";
            if (caz == "(UR RF DL)") return "D F D\' S2 D F\' D\' S2";
            if (caz == "(UR RF DR)") return "U2 S2 U\' F\' U S2 U\' F U\'";
            if (caz == "(UR RF DB)") return "D2 F D\' S2 D F\' D\' S2 D\'";
            if (caz == "(UR RB UB)") return "U\' L\' E L U\' L\' E\' L U2";
            if (caz == "(UR RB UL)") return "L\' E L U2 L\' E\' L U2";
            if (caz == "(UR RB UF)") return "F\' E2 F U F\' E2 F U\'";
            if (caz == "(UR RB FU)") return "S R\' B M\' B\' R B M B' S'";
            if (caz == "(UR RB FL)") return "L2 E L U2 L\' E\' L U2 L";
            if (caz == "(UR RB FR)") return "B U\' B E B U B E\'";
            if (caz == "(UR RB FD)") return "u M\' U\' R U M U\' R\' E";
            if (caz == "(UR RB RF)") return "F2 E2 F U F\' E2 F U\' F";
            if (caz == "(UR RB RD)") return "S\' U\' B U S U\' B\' U";
            if (caz == "(UR RB BU)") return "E\' R\' F M2 F\' R F M2 F\' E";
            if (caz == "(UR RB BL)") return "E L U2 L\' E\' L U2 L\'";
            if (caz == "(UR RB BD)") return "D\' S\' U\' B U S U\' B\' U D";
            if (caz == "(UR RB LU)") return "S U\' B U S\' U\' B\' U";
            if (caz == "(UR RB LB)") return "B2 L S\' L\' B2 L S L\'";
            if (caz == "(UR RB LF)") return "E2 F U F\' E2 F U\' F\'";
            if (caz == "(UR RB LD)") return "L B2 L S\' L\' B2 L S L2";
            if (caz == "(UR RB DF)") return "D2 B\' D S2 D\' B D S2 D";
            if (caz == "(UR RB DL)") return "D\' B\' D S2 D\' B D S2";
            if (caz == "(UR RB DR)") return "D B\' D S2 D\' B D S2 D2";
            if (caz == "(UR RB DB)") return "B\' D S2 D\' B D S2 D\'";
            if (caz == "(UR RD UB)") return "M\' D M U\' M\' D\' M U";
            if (caz == "(UR RD UL)") return "U2 R\' E\' R U2 R\' E R";
            if (caz == "(UR RD UF)") return "M D\' M\' U M D M\' U\'";
            if (caz == "(UR RD FU)") return "S D\' F2 D S\' D\' F2 D";
            if (caz == "(UR RD FL)") return "R\' F2 R S\' R\' F2 R S";
            if (caz == "(UR RD FR)") return "S D\' F D S\' D\' F\' D";
            if (caz == "(UR RD FD)") return "D\' M\' U M D M\' U\' M";
            if (caz == "(UR RD RF)") return "U F\' U\' S\' U F U\' S";
            if (caz == "(UR RD RB)") return "U\' B U S\' U\' B\' U S";
            if (caz == "(UR RD BU)") return "R B\' R\' S\' R B R\' S";
            if (caz == "(UR RD BR)") return "S D B\' D\' S\' D B D\'";
            if (caz == "(UR RD BL)") return "R B2 R\' S\' R B2 R\' S";
            if (caz == "(UR RD BD)") return "D M U\' M\' D\' M U M\'";
            if (caz == "(UR RD LU)") return "U2 R2 U M U\' R2 U M\' U";
            if (caz == "(UR RD LB)") return "U\' B\' U S\' U\' B U S";
            if (caz == "(UR RD LF)") return "U F U\' S\' U F\' U\' S";
            if (caz == "(UR RD LD)") return "R\' E R D2 R\' E\' R D2";
            if (caz == "(UR RD DF)") return "U F2 U\' S\' U F2 U\' S";
            if (caz == "(UR RD DL)") return "D\' M D\' R2 D M\' D\' R2 D2";
            if (caz == "(UR RD DB)") return "U\' B2 U S\' U\' B2 U S";
            if (caz == "(UR BU UL)") return "L\' B L\' S2 L B\' L\' S2 L2";
            if (caz == "(UR BU UF)") return "B\' R B M\' B\' R\' B M";
            if (caz == "(UR BU FU)") return "B\' R B\' M2 B R\' B\' M2 B2";
            if (caz == "(UR BU FL)") return "L2 B L\' S2 L B\' L\' S2 L\'";
            if (caz == "(UR BU FR)") return "F M2 F\' R\' F M2 F\' R";
            if (caz == "(UR BU FD)") return "B\' R B M2 B\' R\' B M2";
            if (caz == "(UR BU RF)") return "S F\' M F R F\' M\' F R\' S\'";
            if (caz == "(UR BU RB)") return "U R B\' M2 B R\' B\' M2 B U\'";
            if (caz == "(UR BU RD)") return "D B2 D\' S D B2 D\' S\'";
            if (caz == "(UR BU BR)") return "R2 F M2 F\' R F M2 F\' R";
            if (caz == "(UR BU BL)") return "B L\' S2 L B\' L\' S2 L";
            if (caz == "(UR BU BD)") return "B2 D\' S D B2 D\' S\' D";
            if (caz == "(UR BU LU)") return "S R B\' R\' S\' R B R\'";
            if (caz == "(UR BU LB)") return "R L U\' L\' E L U L\' E\' R\'";
            if (caz == "(UR BU LF)") return "r\' E R U\' R\' E\' R U M\'";
            if (caz == "(UR BU LD)") return "M U M D M\' U\' M D\' M2";
            if (caz == "(UR BU DF)") return "B\' R B\' M B R\' B\' M\' B2";
            if (caz == "(UR BU DL)") return "L B L\' S2 L B\' L\' S2";
            if (caz == "(UR BU DR)") return "D M\' D\' R2 D M D\' R2";
            if (caz == "(UR BU DB)") return "M\' D\' R2 D M D\' R2 D";
            if (caz == "(UR BR UB)") return "U2 F E\' F\' U\' F E F\' U\'";
            if (caz == "(UR BR UL)") return "L E2 L\' U2 L E2 L\' U2";
            if (caz == "(UR BR UF)") return "F E\' F\' U F E F\' U\'";
            if (caz == "(UR BR FU)") return "R B\' M2 B R\' B\' M2 B";
            if (caz == "(UR BR FL)") return "E2 L\' U2 L E2 L\' U2 L";
            if (caz == "(UR BR FR)") return "S2 R\' S\' R2 S R\' S2";
            if (caz == "(UR BR FD)") return "U M\' U\' R\' U M U\' R";
            if (caz == "(UR BR RF)") return "E\' F\' U F E F\' U\' F";
            if (caz == "(UR BR RD)") return "D B\' D\' S D B D\' S\'";
            if (caz == "(UR BR BU)") return "R\' F M2 F\' R\' F M2 F\' R2";
            if (caz == "(UR BR BL)") return "R E L2 E\' R\' E L2 E\'";
            if (caz == "(UR BR BD)") return "U\' M U R\' U\' M\' U R";
            if (caz == "(UR BR LU)") return "U\' M\' U R\' U\' M U R";
            if (caz == "(UR BR LB)") return "R B M B\' R\' B M\' B\'";
            if (caz == "(UR BR LF)") return "S\' L\' S R\' S\' L S R";
            if (caz == "(UR BR LD)") return "R E L E\' R\' E L\' E\'";
            if (caz == "(UR BR DF)") return "U\' M2 U R\' U\' M2 U R";
            if (caz == "(UR BR DL)") return "L B2 L\' S2 L B2 L\' S2";
            if (caz == "(UR BR DR)") return "D\' U\' M2 U R\' U\' M2 U R D";
            if (caz == "(UR BR DB)") return "U M2 U\' R\' U M2 U\' R";
            if (caz == "(UR BL UB)") return "U R\' E2 R U\' R\' E2 R";
            if (caz == "(UR BL UL)") return "U2 R\' E2 R U2 R\' E2 R";
            if (caz == "(UR BL UF)") return "U\' R\' E2 R U R\' E2 R";
            if (caz == "(UR BL FU)") return "L\' S2 L\' F\' L S2 L\' F L2";
            if (caz == "(UR BL FL)") return "L S\' L2 S L2 S\' L2 S L";
            if (caz == "(UR BL FR)") return "L U2 L\' E2 L U2 L\' E2";
            if (caz == "(UR BL FD)") return "U M\' U L U\' M U L\' U2";
            if (caz == "(UR BL RF)") return "F2 E F U F\' E\' F U\' F";
            if (caz == "(UR BL RB)") return "L U2 L\' E L U2 L\' E\'";
            if (caz == "(UR BL RD)") return "S\' R B2 R\' S R B2 R\'";
            if (caz == "(UR BL BU)") return "L\' S2 L B L\' S2 L B\'";
            if (caz == "(UR BL BR)") return "L\' S2 L B2 L\' S2 L B2";
            if (caz == "(UR BL BD)") return "L\' S2 L B\' L\' S2 L B";
            if (caz == "(UR BL LU)") return "S R B2 R\' S\' R B2 R\'";
            if (caz == "(UR BL LF)") return "L U2 L\' E\' L U2 L\' E";
            if (caz == "(UR BL LD)") return "E2 R\' E\' L\' E R E\' L E\'";
            if (caz == "(UR BL DF)") return "U\' M2 U\' L U M2 U\' L\' U2";
            if (caz == "(UR BL DL)") return "S2 R B2 R\' S2 R B2 R\'";
            if (caz == "(UR BL DR)") return "R\' E\' R2 E R2 E\' R2 E R\'";
            if (caz == "(UR BL DB)") return "U M2 U L U\' M2 U L\' U2";
            if (caz == "(UR BD UB)") return "B\' E\' B U\' B\' E B U";
            if (caz == "(UR BD UL)") return "S\' D S U2 S\' D\' S U2";
            if (caz == "(UR BD UF)") return "U\' S D\' S\' U S D S\'";
            if (caz == "(UR BD FU)") return "M2 F R\' F\' M2 F R F\'";
            if (caz == "(UR BD FL)") return "U2 L\' U M U\' L U M\' U";
            if (caz == "(UR BD FR)") return "F\' M2 F R\' F\' M2 F R";
            if (caz == "(UR BD FD)") return "D2 M\' U M D2 M\' U\' M";
            if (caz == "(UR BD RF)") return "f M\' F\' R F M F\' R\' S\'";
            if (caz == "(UR BD RB)") return "u\' U\' L U M U\' L\' U M\' u";
            if (caz == "(UR BD RD)") return "M U\' M\' D M U M\' D\'";
            if (caz == "(UR BD BU)") return "D\' S D B2 D\' S\' D B2";
            if (caz == "(UR BD BR)") return "R\' U\' M U R U\' M\' U";
            if (caz == "(UR BD BL)") return "B\' L\' S2 L B L\' S2 L";
            if (caz == "(UR BD LU)") return "S R B R\' S\' R B\' R\'";
            if (caz == "(UR BD LB)") return "d\' S D B\' D\' S\' D B E";
            if (caz == "(UR BD LF)") return "r\' E R U R\' E\' R U\' M\'";
            if (caz == "(UR BD LD)") return "M U\' M\' D\' M U M\' D";
            if (caz == "(UR BD DF)") return "M\' D R2 D\' M D R2 D\'";
            if (caz == "(UR BD DL)") return "S2 R B R\' S2 R B\' R\'";
            if (caz == "(UR BD DR)") return "R2 U\' M U R2 U\' M\' U";
            if (caz == "(UR LU UB)") return "S\' L\' B L S L\' B\' L";
            if (caz == "(UR LU UF)") return "S\' L F\' L\' S L F L\'";
            if (caz == "(UR LU FU)") return "R\' F R S R\' F\' R S\'";
            if (caz == "(UR LU FL)") return "R\' F2 R S R\' F2 R S\'";
            if (caz == "(UR LU FR)") return "S\' U\' F\' U S U\' F U";
            if (caz == "(UR LU FD)") return "R\' F\' R S R\' F R S\'";
            if (caz == "(UR LU RF)") return "S\' L F2 L\' S L F2 L\'";
            if (caz == "(UR LU RB)") return "S\' L\' B2 L S L\' B2 L";
            if (caz == "(UR LU RD)") return "U M\' U R2 U\' M U R2 U2";
            if (caz == "(UR LU BU)") return "R B\' R\' S R B R\' S\'";
            if (caz == "(UR LU BR)") return "R\' U\' M\' U R U\' M U";
            if (caz == "(UR LU BL)") return "R B2 R\' S R B2 R\' S\'";
            if (caz == "(UR LU BD)") return "R B R\' S R B\' R\' S\'";
            if (caz == "(UR LU LB)") return "U M\' U\' L U M U\' L\'";
            if (caz == "(UR LU LF)") return "U M\' U\' L\' U M U\' L";
            if (caz == "(UR LU LD)") return "U M\' U\' L2 U M U\' L2";
            if (caz == "(UR LU DF)") return "S\' L F L\' S L F\' L\'";
            if (caz == "(UR LU DL)") return "L\' E L\' U2 L E\' L\' U2 L2";
            if (caz == "(UR LU DR)") return "R2 U\' M\' U R2 U\' M U";
            if (caz == "(UR LU DB)") return "S\' L\' B\' L S L\' B L";
            if (caz == "(UR LB UB)") return "U R E\' R\' U\' R E R\'";
            if (caz == "(UR LB UL)") return "U2 R E\' R\' U2 R E R\'";
            if (caz == "(UR LB UF)") return "U\' R E\' R\' U R E R\'";
            if (caz == "(UR LB FU)") return "M\' U R E\' R\' U\' R E r\'";
            if (caz == "(UR LB FL)") return "E L\' U2 L E\' L\' U2 L";
            if (caz == "(UR LB FR)") return "R S\' L S R\' S\' L\' S";
            if (caz == "(UR LB FD)") return "M\' U\' R E\' R\' U R E r\'";
            if (caz == "(UR LB RF)") return "E2 F\' U F E2 F\' U\' F";
            if (caz == "(UR LB RB)") return "L S\' L\' B2 L S L\' B2";
            if (caz == "(UR LB RD)") return "R\' B2 R S R\' B2 R S\'";
            if (caz == "(UR LB BU)") return "R E L U\' L\' E\' L U L\' R\'";
            if (caz == "(UR LB BR)") return "B\' M\' B R B\' M B R\'";
            if (caz == "(UR LB BD)") return "E\' B\' D\' S D B D\' S\' d";
            if (caz == "(UR LB LU)") return "L U M\' U\' L\' U M U";
            if (caz == "(UR LB LF)") return "E\' R E L2 E\' R\' E L2";
            if (caz == "(UR LB LD)") return "E\' R E L E\' R\' E L\'";
            if (caz == "(UR LB DF)") return "F\' E2 F\' U F E2 F\' U\' F2";
            if (caz == "(UR LB DL)") return "S2 U\' B\' U S2 U\' B U";
            if (caz == "(UR LB DR)") return "R2 S\' L S R2 S\' L\' S";
            if (caz == "(UR LB DB)") return "L S\' L\' B\' L S L\' B";
            if (caz == "(UR LF UB)") return "U R\' E R U\' R\' E\' R";
            if (caz == "(UR LF UL)") return "U2 R\' E R U2 R\' E\' R";
            if (caz == "(UR LF UF)") return "U\' R\' E R U R\' E\' R";
            if (caz == "(UR LF FU)") return "R\' E\' L\' U L E L\' U\' L R";
            if (caz == "(UR LF FR)") return "F M F\' R\' F M\' F\' R";
            if (caz == "(UR LF FD)") return "E F D S D\' F\' D S\' d\'";
            if (caz == "(UR LF RF)") return "L\' S\' L B2 L\' S L F2";
            if (caz == "(UR LF RB)") return "E2 B U\' B\' E2 B U B\'";
            if (caz == "(UR LF RD)") return "R F2 R\' S R F2 R\' S\'";
            if (caz == "(UR LF BU)") return "M U\' R\' E R U R\' E\' r";
            if (caz == "(UR LF BR)") return "R\' S\' L\' S R S\' L S";
            if (caz == "(UR LF BL)") return "E\' L U2 L\' E L U2 L\'";
            if (caz == "(UR LF BD)") return "M U R\' E R U\' R\' E\' r";
            if (caz == "(UR LF LU)") return "L\' U\' M U L U\' M\' U";
            if (caz == "(UR LF LB)") return "E R\' E\' L2 E R E\' L2";
            if (caz == "(UR LF LD)") return "E R\' E\' L\' E R E\' L";
            if (caz == "(UR LF DF)") return "L\' S\' L F L\' S L F\'";
            if (caz == "(UR LF DL)") return "S2 U F U\' S2 U F\' U\'";
            if (caz == "(UR LF DR)") return "R2 S\' L\' S R2 S\' L S";
            if (caz == "(UR LF DB)") return "B E2 B U\' B\' E2 B U B2";
            if (caz == "(UR LD UB)") return "M\' D\' M U\' M\' D M U";
            if (caz == "(UR LD UL)") return "L E L\' U2 L E\' L\' U2";
            if (caz == "(UR LD UF)") return "M D M\' U M D\' M\' U\'";
            if (caz == "(UR LD FU)") return "R\' F R\' S\' R F\' R\' S R2";
            if (caz == "(UR LD FL)") return "R\' F2 R\' S\' R F2 R\' S R2";
            if (caz == "(UR LD FR)") return "E\' L\' E R\' E\' L E R";
            if (caz == "(UR LD FD)") return "D M\' U M D\' M\' U\' M";
            if (caz == "(UR LD RF)") return "U F\' U S U\' F U S\' U2";
            if (caz == "(UR LD RB)") return "U\' B U\' S U B\' U\' S\' U2";
            if (caz == "(UR LD RD)") return "D2 R\' E R D2 R\' E\' R";
            if (caz == "(UR LD BU)") return "R B\' R S\' R\' B R S R2";
            if (caz == "(UR LD BR)") return "E L E\' R E L\' E\' R\'";
            if (caz == "(UR LD BL)") return "R B2 R S\' R\' B2 R S R2";
            if (caz == "(UR LD BD)") return "D\' M U\' M\' D M U M\'";
            if (caz == "(UR LD LU)") return "L2 U M\' U\' L2 U M U\'";
            if (caz == "(UR LD LB)") return "L E\' R E L\' E\' R\' E";
            if (caz == "(UR LD LF)") return "L\' E R\' E\' L E R E\'";
            if (caz == "(UR LD DF)") return "M D\' M U\' M\' D M U M2";
            if (caz == "(UR LD DR)") return "D\' M\' D R2 D\' M D R2";
            if (caz == "(UR LD DB)") return "M\' D M\' U M D\' M\' U\' M2";
            if (caz == "(UR DF UB)") return "D R2 D\' M2 D R2 D\' M2";
            if (caz == "(UR DF UL)") return "U M\' U2 M U";
            if (caz == "(UR DF UF)") return "D\' S2 D F2 D\' S2 D F2";
            if (caz == "(UR DF FU)") return "F2 U F E F\' U\' F E\' F";
            if (caz == "(UR DF FL)") return "U2 L\' U M2 U\' L U M2 U";
            if (caz == "(UR DF FR)") return "R U\' M2 U R\' U\' M2 U";
            if (caz == "(UR DF RF)") return "E R\' U\' M2 U R U\' M2 u";
            if (caz == "(UR DF RB)") return "D\' S2 D\' B\' D S2 D\' B D2";
            if (caz == "(UR DF RD)") return "M2 U\' M\' D M U M\' D\' M\'";
            if (caz == "(UR DF BU)") return "B2 M B R B\' M\' B R\' B";
            if (caz == "(UR DF BR)") return "R\' U\' M2 U R U\' M2 U";
            if (caz == "(UR DF BL)") return "R\' E2 R\' D R E2 R D\' R2";
            if (caz == "(UR DF BD)") return "D R2 D\' M\' D R2 D\' M";
            if (caz == "(UR DF LU)") return "L F L\' S\' L F\' L\' S";
            if (caz == "(UR DF LB)") return "D\' S2 D\' B D S2 D\' B\' D2";
            if (caz == "(UR DF LF)") return "F L\' S\' L F\' L\' S L";
            if (caz == "(UR DF LD)") return "M2 U\' M\' D\' M U M\' D M\'";
            if (caz == "(UR DF DL)") return "S2 U F2 U\' S2 U F2 U\'";
            if (caz == "(UR DF DR)") return "R2 U\' M2 U R2 U\' M2 U";
            if (caz == "(UR DF DB)") return "D S\' U2 S D2 S\' U2 S D";
            if (caz == "(UR DL UB)") return "S2 D\' B2 D S2 D\' B2 D";
            if (caz == "(UR DL UL)") return "U2 S U2 S\'";
            if (caz == "(UR DL UF)") return "S2 D F2 D\' S2 D F2 D\'";
            if (caz == "(UR DL FU)") return "R\' F R S2 R\' F\' R S2";
            if (caz == "(UR DL FL)") return "R\' F2 R S2 R\' F2 R S2";
            if (caz == "(UR DL FR)") return "S2 L\' F2 L S2 L\' F2 L";
            if (caz == "(UR DL FD)") return "R\' F\' R S2 R\' F R S2";
            if (caz == "(UR DL RF)") return "U F\' U\' S2 U F U\' S2";
            if (caz == "(UR DL RB)") return "U\' B U S2 U\' B\' U S2";
            if (caz == "(UR DL RD)") return "D2 R2 D\' M\' D R2 D\' M D\'";
            if (caz == "(UR DL BU)") return "R B\' R\' S2 R B R\' S2";
            if (caz == "(UR DL BR)") return "S2 L B2 L\' S2 L B2 L\'";
            if (caz == "(UR DL BL)") return "R B2 R\' S2 R B2 R\' S2";
            if (caz == "(UR DL BD)") return "R B R\' S2 R B\' R\' S2";
            if (caz == "(UR DL LU)") return "L2 U2 L E L\' U2 L E\' L";
            if (caz == "(UR DL LB)") return "U\' B\' U S2 \'U B U S2";
            if (caz == "(UR DL LF)") return "U F U\' S2 U F\' U\' S2";
            if (caz == "(UR DL DF)") return "U F2 U\' S2 U F2 U\' S2";
            if (caz == "(UR DL DR)") return "R2 S\' R2 S";
            if (caz == "(UR DL DB)") return "U\' B2 U S2 U\' B2 U S2";
            if (caz == "(UR DR UB)") return "R2 D\' M2 D R2 D\' M2 D";
            if (caz == "(UR DR UL)") return "R2 S R2 S\'";
            if (caz == "(UR DR UF)") return "R2 D M2 D\' R2 D M2 D\'";
            if (caz == "(UR DR FU)") return "R2 D\' M D R2 D\' M\' D";
            if (caz == "(UR DR FL)") return "R\' E R2 E\' R2 E R2 E\' R\'";
            if (caz == "(UR DR FR)") return "F U M\' U\' R2 U M U\' R2 F\'";
            if (caz == "(UR DR FD)") return "U M\' U\' R2 U M U\' R2";
            if (caz == "(UR DR RF)") return "U F\' U S2 U\' F U S2 U2";
            if (caz == "(UR DR RB)") return "U\' B U\' S2 U B\' U\' S2 U2";
            if (caz == "(UR DR BU)") return "R2 D M\' D\' R2 D M D\'";
            if (caz == "(UR DR BR)") return "B\' U\' M U R2 U\' M\' U R2 B";
            if (caz == "(UR DR BL)") return "R E\' R2 E R2 E\' R2 E R";
            if (caz == "(UR DR BD)") return "U\' M U R2 U\' M\' U R2";
            if (caz == "(UR DR LU)") return "U\' M\' U R2 U\' M U R2";
            if (caz == "(UR DR LB)") return "S\' L S R2 S\' L\' S R2";
            if (caz == "(UR DR LF)") return "S\' L\' S R2 S\' L S R2";
            if (caz == "(UR DR LD)") return "R2 D\' M\' D R2 D\' M D";
            if (caz == "(UR DR DF)") return "U\' M2 U R2 U\' M2 U R2";
            if (caz == "(UR DR DL)") return "S\' R2 S R2";
            if (caz == "(UR DR DB)") return "U M2 U\' R2 U M2 U\' R2";
            if (caz == "(UR DB UB)") return "D S2 D\' B2 D S2 D\' B2";
            if (caz == "(UR DB UL)") return "U M U2 M\' U2 M U2 M\' U";
            if (caz == "(UR DB UF)") return "D\' R2 D M2 D\' R2 D M2";
            if (caz == "(UR DB FU)") return "F2 M\' F\' R\' F M F\' R F\'";
            if (caz == "(UR DB FL)") return "U2 L\' U\' M2 U L U\' M2 U\'";
            if (caz == "(UR DB FR)") return "R U M2 U\' R\' U M2 U\'";
            if (caz == "(UR DB FD)") return "D\' R2 D M D\' R2 D M\'";
            if (caz == "(UR DB RF)") return "D S2 D F D\' S2 D F\' D2";
            if (caz == "(UR DB RB)") return "D S2 D\' B\' D S2 D\' B";
            if (caz == "(UR DB RD)") return "R\' B R S R\' B\' R S\'";
            if (caz == "(UR DB BU)") return "D\' R2 D M\' D\' R2 D M";
            if (caz == "(UR DB BR)") return "R\' U M2 U\' R U M2 U\'";
            if (caz == "(UR DB BL)") return "U2 L U\' M2 U L\' U\' M2 U\'";
            if (caz == "(UR DB LU)") return "L\' B\' L S\' L\' B L S";
            if (caz == "(UR DB LB)") return "B\' L S\' L\' B L S L\'";
            if (caz == "(UR DB LF)") return "D S2 D F\' D\' S2 D F D2";
            if (caz == "(UR DB LD)") return "M2 U M D M\' U\' M D\' M";
            if (caz == "(UR DB DF)") return "D\' S D2 S\' D2 S D2 S\' D\'";
            if (caz == "(UR DB DL)") return "S2 U\' B2 U S2 U\' B2 U";
            if (caz == "(UR DB DR)") return "R2 U M2 U\' R2 U M2 U\'";
            return "nimic: " + caz;
        }

        void RezolvareComutatoriColturi()
        {
            int i;
            string caz="";
            for (i = 1; i < sirColturiSize; i += 2)
            {
                caz = "(ULB ";
                caz += (sirColturi[i] + " ");
                caz += sirColturi[i + 1];
                caz += ")";
                SolutieColturi += (ComutatorColturi(caz) + "\n");
            }

        }

        void RezolvareComutatoriMuchii()
        {
            int i;
            string caz = "";
            for (i = 1; i < sirMuchiiSize; i += 2)
            {
                caz = "(UR ";
                caz += (sirMuchii[i] + " ");
                caz += sirMuchii[i + 1];
                caz += ")";
                SolutieMuchii += (ComutatorMuchii(caz) + "\n");
            }
        }

        private void buttonRezolva_Click(object sender, EventArgs e)
        {
            int i;
            string sticker = "";
            SolutiaCubului = "";
            SolutieColturi = SolutieColturiSucite = SolutieMuchii = SolutieMuchiiSucite = "";
            {
                PreaMultCeva();//verific daca am prea mult dintr-o culoare
                if (!PreaMult)
                {
                    InitMat();
                    FormezColturi();
                    FormezMuchii();
                    //Rubik.Mutarea_F();
                    PotRezolva = true;
                    VerificAmbiguitati();
                    if (PotRezolva)
                    {
                        FormezSirColturi();//daca sirul va fi impar, voi face swap intre muchiile 1 si 4(cand rezolv in realitate, se face automat, dar acum..)
                        FormezSirMuchii();//astfel, sirul de muchii va fi sigur par( vreau sa evit paritatea)
                           
                        
                        
                        if (sirMuchiiSize % 2 == 1)
                                MessageBox.Show("Cub imposibil de rezolvat.");
                        else
                        {
                                RezolvareComutatoriColturi();
                                if (sirColturiSize % 2 == 1)
                                    SolutieColturi += (RezolvareOldPochman(sirColturi[sirColturiSize]) + "\n");
                                for (i = 1; i <= sirColturiSuciteSize; ++i)
                                    SolutieColturiSucite += (RezolvareOldPochman(sirColturiSucite[i]) + "\n");

                                RezolvareComutatoriMuchii();
                                for (i = 1; i <= sirMuchiiSuciteSize; ++i)//schimb in caz ca am centrele schimbate
                                {
                                    sticker = sirMuchiiSucite[i];
                                    if (i % 2 == 0)
                                    {
                                        if (sticker == "DR") sticker = "UL";
                                        else
                                        {
                                            if (sticker == "UL") sticker = "DR";
                                            else
                                            {
                                                if (sticker == "RD") sticker = "LU";
                                                else
                                                    if (sticker == "LU") sticker = "RD";
                                            }
                                        }
                                    }
                                    SolutieMuchiiSucite += (RezolvareM2(sticker) + "\n");
                                }

                                //FORMEZ SOLUTIAAA!!--------------*************************************************************************************
                                SolutiaCubului += ("Sulotie colturi:\n" + SolutieColturi + "\n\n");
                                if (sirColturiSuciteSize > 0)
                                    SolutiaCubului += ("Solutie colturi sucite:\n" + SolutieColturiSucite + "\n\n\n");
                                else
                                    SolutiaCubului += ("Nu exista colturi sucite.\n\n\n");

                                SolutiaCubului += ("Sulotie muchii:\n" + SolutieMuchii + "\n\n");
                                if (sirMuchiiSuciteSize > 0)
                                    SolutiaCubului += ("Solutie muchii sucite:\ny\' x2\n" + SolutieMuchiiSucite + "\n\n");
                                else
                                    SolutiaCubului += ("Nu exista muchii sucite.\n\n");
                                MessageBox.Show("                                                                             \n" + SolutiaCubului);
                            }
                    }
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)//reseteaza culorile butoanelor
        {
            cntAlb = 9;
            cntGalben = 9;
            cntAlbastru = 9;
            cntVerde = 9;
            cntRosu = 9;
            cntPortocaliu = 9;
            labelRosu.Text = "Rosu:" + (cntRosu);
            labelPortocaliu.Text = "Portocaliu:" + (cntPortocaliu);
            labelAlbastru.Text = "Albastru:" + (cntAlbastru);
            labelVerde.Text = "Verde:" + (cntVerde);
            labelAlb.Text = "Alb:" + (cntAlb);
            labelGalben.Text = "Galben:" + (cntGalben);
            /*----------------------------------------------------Schimb Culorile---------------------------------------------------------------*/
            //Verde
            button1.BackColor = Color.Lime;
            button2.BackColor = Color.Lime;
            button3.BackColor = Color.Lime;
            button4.BackColor = Color.Lime;
            button5.BackColor = Color.Lime;
            button6.BackColor = Color.Lime;
            button7.BackColor = Color.Lime;
            button8.BackColor = Color.Lime;
            button9.BackColor = Color.Lime;
            //Rosu
            button10.BackColor = Color.Red;
            button11.BackColor = Color.Red;
            button12.BackColor = Color.Red;
            button13.BackColor = Color.Red;
            button14.BackColor = Color.Red;
            button15.BackColor = Color.Red;
            button16.BackColor = Color.Red;
            button17.BackColor = Color.Red;
            button18.BackColor = Color.Red;
            //Alb
            button19.BackColor = Color.White;
            button20.BackColor = Color.White;
            button21.BackColor = Color.White;
            button22.BackColor = Color.White;
            button23.BackColor = Color.White;
            button24.BackColor = Color.White;
            button25.BackColor = Color.White;
            button26.BackColor = Color.White;
            button27.BackColor = Color.White;
            //Albastru
            button28.BackColor = Color.Blue;
            button29.BackColor = Color.Blue;
            button30.BackColor = Color.Blue;
            button31.BackColor = Color.Blue;
            button32.BackColor = Color.Blue;
            button33.BackColor = Color.Blue;
            button34.BackColor = Color.Blue;
            button35.BackColor = Color.Blue;
            button36.BackColor = Color.Blue;
            //Galben
            button37.BackColor = Color.Yellow;
            button38.BackColor = Color.Yellow;
            button39.BackColor = Color.Yellow;
            button40.BackColor = Color.Yellow;
            button41.BackColor = Color.Yellow;
            button42.BackColor = Color.Yellow;
            button43.BackColor = Color.Yellow;
            button44.BackColor = Color.Yellow;
            button45.BackColor = Color.Yellow;
            //Portocaliu
            button46.BackColor = Color.Orange;
            button47.BackColor = Color.Orange;
            button48.BackColor = Color.Orange;
            button49.BackColor = Color.Orange;
            button50.BackColor = Color.Orange;
            button51.BackColor = Color.Orange;
            button52.BackColor = Color.Orange;
            button53.BackColor = Color.Orange;
            button54.BackColor = Color.Orange;
        }
    }
}