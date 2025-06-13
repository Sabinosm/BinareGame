using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace binare_game
{
    public partial class Form1 : Form
    {
        Button[] butao_1;
        Dictionary<GroupBox, TextBox> grupoResposta = new Dictionary<GroupBox, TextBox>();
        GroupBox[] grupoboxes;
        int[] acumuladores = new int[7];



        //variaveis
        bool invertido = false;
        int acumulador = 0;
        int acumulador2 = 0;
        int acumulador3 = 0;
        int acumulador4 = 0;
        int acumulador5 = 0;
        int acumulador6 = 0;
        int acumulador7 = 0;
        int linhas_faltando = 0;
        int level = 1;
        int tentativas = 0;
        int pontuacao = 0;
        Stopwatch cronometro = new Stopwatch();
        int linhasPassadas = 0;
        int numerosClear = 0;
        bool jogo_parado = false;
        bool jogoPausado = false;
        List<GroupBox> gpVoltar = new List<GroupBox>();
        List<Button[]> allButons = new List<Button[]>();
        bool emAndamento = false;



        public Form1()
        {
            InitializeComponent();

        }
        private async void button1_Click(object sender, EventArgs e)
        {



            btn_start.Text = "START";
            pan_end.Visible = false;
            btn_desistir.Enabled = true;
            btn_pausar.Enabled = true;

            cronometro.Start();

            btn_start.Enabled = false;

            linhas_faltando = 10;
            lbl_linhas_faltando.Text = linhas_faltando.ToString();

            gp_1.Visible = true;
            gp_2.Visible = true;
            gp_3.Visible = true;

            lbl_level.Text = level.ToString();


            butao_1 = new Button[]
           {
                btn_128_rum, btn_32_rum, btn_16_rum, btn_64_rum, btn_8_rum, btn_4_rum, btn_2_rum, btn_1_rum,
                btn_128_rdois, btn_32_rdois, btn_16_rdois, btn_64_rdois, btn_8_rdois, btn_4_rdois, btn_2_rdois, btn_1_rdois,
                btn_128_rtres, btn_32_rtres, btn_16_rtres, btn_64_rtres, btn_8_rtres, btn_4_rtres, btn_2_rtres, btn_1_rtres,
                btn_128_rquatro, btn_32_rquatro, btn_16_rquatro, btn_64_rquatro, btn_8_rquatro, btn_4_rquatro, btn_2_rquatro, btn_1_rquatro,
                btn_128_rcinco, btn_32_rcinco, btn_16_rcinco, btn_64_rcinco, btn_8_rcinco, btn_4_rcinco, btn_2_rcinco, btn_1_rcinco,
                btn_128_rseis, btn_32_rseis, btn_16_rseis, btn_64_rseis, btn_8_rseis, btn_4_rseis, btn_2_rseis, btn_1_rseis,
                btn_128_rsete, btn_32_rsete, btn_16_rsete, btn_64_rsete, btn_8_rsete, btn_4_rsete, btn_2_rsete, btn_1_rsete,

           };
            acumuladores= new int[7] { acumulador, acumulador2, acumulador3, acumulador4, acumulador5, acumulador6, acumulador7 };
            grupoboxes = new GroupBox[]
            {
                gp_1, gp_2, gp_3, gp_4, gp_5, gp_6, gp_7
            };  




            FuncaoBasica();

            txt_resposta1.Text = r.Next(0, 129).ToString();
            txt_resposta2.Text = r.Next(0, 129).ToString();
            txt_resposta3.Text = r.Next(0, 129).ToString();

            if (tentativas == 0)
            {
                foreach (Button botoes in butao_1)
                {
                    botoes.Click += Botao_Click;
                }
                grupoResposta.Add(gp_1, txt_resposta1);
                grupoResposta.Add(gp_2, txt_resposta2);
                grupoResposta.Add(gp_3, txt_resposta3);
                grupoResposta.Add(gp_4, txt_resposta4);
                grupoResposta.Add(gp_5, txt_resposta5);
                grupoResposta.Add(gp_6, txt_resposta6);
                grupoResposta.Add(gp_7, txt_resposta7);
            }


            //ID_CONTINUIDADE


            await Dificuldade(10000, 129);


        }


        Random r = new Random();




        public async void VerificarResultado(GroupBox z, int acumulador, TextBox resposta)
        {
            if (acumulador.ToString() == resposta.Text)
            {
                z.Visible = false;
                pontuacao += 100;
                lbl_pontuacao.Text = pontuacao.ToString();
                linhas_faltando--;
                lbl_linhas_faltando.Text = linhas_faltando.ToString();
                linhasPassadas++;

                if (lbl_linhas_faltando.Text == "0" && level != 10)
                {
                    level++;
                    lbl_level.Text = level.ToString();

                    foreach (GroupBox gp in grupoboxes)
                    {
                        if (gp.Visible)
                        {
                            gp.Visible = false;
                        }
                    }
                    pan_nextLevel.Visible = true;
                }

                var proximo = grupoboxes.FirstOrDefault(gp => gp.Visible);
                if (proximo == null && linhas_faltando != 0)
                {
                    await ProcessarClear();
                }
            }
        }

        private async Task ProcessarClear()
        {
          

            if (level < 5 )
            {
                if (linhas_faltando > 2)
                {
                    await MostrarClear("+200 pontos", "- 1 linha");
                    linhas_faltando -= 1;
                    pontuacao += 200;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
                else
                {
                    await MostrarClear("+200 pontos", "- total de linha");
                    linhas_faltando = 0;
                    pontuacao += 200;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
            }
            else if (level < 7)
            {
                if (linhas_faltando > 4)
                {
                    await MostrarClear("+400 pontos", "- 3 linhas");
                    linhas_faltando -= 3;
                    pontuacao += 400;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
                else
                {
                    await MostrarClear("+400 pontos", "-total de linhas");
                    linhas_faltando = 0;
                    pontuacao += 400; 
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
            }
            else if (level < 9)
            {
                if (linhas_faltando > 5)
                {
                    await MostrarClear("+600 pontos", "- 4 linhas");
                    linhas_faltando -= 4;
                    pontuacao += 600;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
                else
                {
                    await MostrarClear("+600 pontos", "- total de linhas");
                    linhas_faltando = 0;
                    pontuacao += 600;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
            }
            else
            {
               
                if(linhas_faltando > 7)
                {
                    await MostrarClear("+1000 pontos", "- 6 linhas");
                    linhas_faltando -= 6;
                    pontuacao += 1000;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
                else 
                {
                    await MostrarClear("+1000 pontos", "- total de linhas");
                    linhas_faltando = 0;
                    pontuacao += 1000;
                    lbl_pontuacao.Text = pontuacao.ToString();
                }
                    

                pontuacao += 1000;
            }

            numerosClear++;
            lbl_pontuacao.Text = pontuacao.ToString();

            gp_1.Visible = true;
            gp_2.Visible = true;
            gp_3.Visible = true;

            txt_resposta1.Text = r.Next(0, 129).ToString();
            txt_resposta2.Text = r.Next(0, 129).ToString();
            txt_resposta3.Text = r.Next(0, 129).ToString();

           
        }

        private async Task MostrarClear(string pontos, string linhas)
        {
           
            pan_clear.Visible = true;
            await Task.Delay(1000);
            lbl_campoLimpo.Text = pontos;
            await Task.Delay(1000);
            lbl_campoLimpo.Text = linhas;
            await Task.Delay(800);
            pan_clear.Visible = false;
            lbl_campoLimpo.Text = "Campo Limpo";
        }
        private int IdBotao(Button botao)
        {
            if (botao == null || botao.Name == null) return -1;

            if (botao.Name.Contains("rum")) return 0;
            else if (botao.Name.Contains("rdois")) return 1;
            else if (botao.Name.Contains("rtres")) return 2;
            else if (botao.Name.Contains("rquatro")) return 3;
            else if (botao.Name.Contains("rcinco")) return 4;
            else if (botao.Name.Contains("rseis")) return 5;
            else if (botao.Name.Contains("rsete")) return 6;
            else return -1;
        }


        private void AtualizarAcumulador(int acm, Button botaoClicado = null, GroupBox grupo = null, TextBox resposta = null)
        {

            if (botaoClicado != null)
            {
                int idb = IdBotao(botaoClicado);
                if (idb >= 0)
                {
                    for (int potencia = 0; potencia <= 7; potencia++)
                    {
                        if (botaoClicado.Name.Contains($"_{1 << potencia}_"))
                        {
                            int valor = 1 << potencia;
                            if (botaoClicado.Text == "1")
                                acumuladores[idb] += valor;
                            else if (botaoClicado.Text == "0" && acumuladores[idb] > 0)
                                acumuladores[idb] -= valor;
                        }
                    }
                }
            }
            else if (grupo != null && grupo.Visible && resposta != null && !resposta.ReadOnly)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (grupo.Name.Contains($"{i + 1}"))
                    {
                        acumuladores[i] = 0;

                        foreach (Button b in butao_1)
                        {
                            for (int potencia = 0; potencia <= 7; potencia++)
                            {
                                if (b.Name.Contains($"_{1 << potencia}_") && IdBotao(b) == i)
                                {
                                    int valor = 1 << potencia;
                                    if (b.Text == "1")
                                        acumuladores[i] += valor;
                                }
                            }
                        }
                    }
                }
            }

            int idBotao = IdBotao(botaoClicado);

            if (grupo != null && resposta != null)
            {
                if (idBotao >= 0)
                {
                    
                    VerificarResultado(grupo, acumuladores[idBotao], resposta);
                }
                else if (acm >= 0)
                {
                    
                    VerificarResultado(grupo, acumuladores[acm], resposta);
                }
            }
        }

        private void Botao_Click(object sender, EventArgs e)
        {


            Button botaoClicado = sender as Button;

            if (botaoClicado.Text == "0")
            {
                botaoClicado.Text = "1";
                botaoClicado.ForeColor = Color.Green;
            }
            else if (botaoClicado.Text == "1")
            {
                botaoClicado.Text = "0";
                botaoClicado.ForeColor = Color.Red;

            }


            for(int i = 0; i < 7; i++)
            {
                if (IdBotao(botaoClicado) == i)
                {
                    
                     AtualizarAcumulador(-1, botaoClicado, grupoboxes[i], grupoResposta[grupoboxes[i]]);
                }
            }
           
        }

        private async Task<bool> EndGame(int tempo)
        {
            emAndamento = false;


            for (int timer = 0; timer <= tempo / 2; timer += 500)
            {
                if (grupoboxes.Any(gp => !gp.Visible))
                {
                    jogo_parado = false;
                    
                    return false;
                }
                btn_pausar.Enabled = false;

                panel1.BackColor = Color.FromKnownColor(KnownColor.Red);
                await Task.Delay(500);

                panel1.BackColor = SystemColors.ActiveCaption;
                await Task.Delay(500);

                jogo_parado = true;

            }

            foreach (GroupBox gp in grupoboxes)
            {
                gp.Visible = false;
            }

                pan_nextLevel.Visible = true;
                btn_nextLevel.Visible = false;
                btn_restart.Visible = true;
                lbl_nextLevel.Text = "Você perdeu";
                



            return true;
        }

        public void FuncaoBasica()
        {
            foreach (Button btoes in butao_1)
            {
                btoes.Text = r.Next(0, 2).ToString();

                if (btoes.Text == "1")
                {
                    btoes.ForeColor = Color.Green;
                }
                else
                {
                    btoes.ForeColor = Color.Red;
                }
                

                for(int i = 0; i < 7; i++)
                {

                        if (IdBotao(btoes) == i && btoes.Text == $"1")
                        {
                            for (int potencia = 0; potencia <= 7; potencia++)
                            {
                                if (btoes.Name.Contains($"_{Math.Pow(2, potencia)}_"))
                                {
                                    acumuladores[i] += Convert.ToInt32(Math.Pow(2, potencia));
                                }

                            }

                        }
                }
             


            }
        }

        void TextoMudado( object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            string nome;
            nome = txt.Name;

            
            for(int i = 1; i <=7; i++)
            {
                if (nome.Contains($"resposta{i}"))

                {
                    txt.Text = txt.Text;
                    AtualizarAcumulador(i - 1, null, grupoboxes[i - 1], grupoResposta[grupoboxes[i-1]]);
                    if (grupoboxes[i - 1].Visible == false)
                    {
                        txt.Text = "0";
                        txt.ReadOnly = true;
                        txt.ForeColor = Color.Black;
                        txt.TextChanged -= TextoMudado;

                        foreach (Button but in butao_1)
                        {
                            if (IdBotao(but) == i-1)
                            {
                                but.Enabled = true;
                                
                            }

                        }   
                    }
                }
            
            }
          
            
        }

        public async Task Dificuldade(int tempo, int random)
        {
            if (emAndamento)
            { return; }

            emAndamento = true;

            try
            {
               
                while (linhas_faltando != 0)
                {

                    var proximo = grupoboxes.FirstOrDefault(gp => !gp.Visible);

                    if (proximo != null && linhas_faltando != 0)
                    {
                        await Task.Delay(tempo/2);
                        
                            int ale = 1;

                        if (linhas_faltando != 0 && jogo_parado == false)
                        {
                            await Task.Delay(tempo/2);

                            if (linhas_faltando != 0 && jogo_parado == false)
                            {
                                if (ale == r.Next(1, 6))
                                {

                                    proximo.Visible = true;
                                    grupoResposta[proximo].ReadOnly = false;
                                    grupoResposta[proximo].Text = "0";
                                    grupoResposta[proximo].ForeColor = Color.IndianRed;
                                    grupoResposta[proximo].TextChanged += TextoMudado;

                                    for (int i = 1; i <= 7; i++)
                                    {
                                        if (proximo.Name.Contains($"{i}"))
                                        {
                                            foreach (Button but in butao_1)
                                            {
                                                if (IdBotao(but) == i - 1)
                                                {
                                                    but.Enabled = false;
                                                }

                                            }

                                        }

                                    }

                                }
                                else if (ale != r.Next(1, 6) && jogo_parado == false)
                                {
                                    proximo.Visible = true;
                                    grupoResposta[proximo].Text = r.Next(0, random).ToString();
                                }

                            }

                        }
                        else if (linhas_faltando != 0 && jogo_parado == false)
                        {
                            proximo.Visible = true;

                            grupoResposta[proximo].Text = r.Next(0, random).ToString();
                        }

                    }

                    else if (proximo == null)
                    {
                        if (!await EndGame(tempo))
                        {
                            break;
                        }
                        else
                        {
                            panel1.BackColor = SystemColors.ActiveCaption;
                            emAndamento = true;
                            btn_pausar.Enabled = true;
                        }

                    }
                }
            }
            finally 
            { 
                emAndamento = false; 
            }

        }
        public void AuxiliarNext()
        {
            lbl_nextLevel.Text = ($"Level {level} completo");
            lbl_linhas_faltando.Text = linhas_faltando.ToString();

            gp_1.Visible = true;
            gp_2.Visible = true;
            gp_3.Visible = true;

            ZerarAcumulador();
        }
        private async void btn_nextLevel_Click(object sender, EventArgs e)
        {
            pan_nextLevel.Visible = false;
            linhas_faltando = 15;

            if (level == 2)
            {

                AuxiliarNext();

                await Dificuldade(10000, 201);
                FuncaoBasica();



            }
            else if (level == 3)
            {

                linhas_faltando = 20;

                AuxiliarNext();

                await Dificuldade(10000, 255);
                FuncaoBasica();

               
            }
            else if (level == 4)
            {
              

                linhas_faltando = 25;

                AuxiliarNext();

                await Dificuldade(9000, 255);
                FuncaoBasica();

              
            }
            else if (level == 5)
            {

                gp_consulta.Visible = false;

                pan_mensagem.Visible= true;
                await Task.Delay(2000);
                pan_mensagem.Visible = false;

                linhas_faltando = 30;

                AuxiliarNext();

                await Dificuldade(9000, 255);
                FuncaoBasica();

                
            }
            else if (level == 6)
            {
            
                linhas_faltando = 35;

                AuxiliarNext();


                await Dificuldade(9000, 255);
                FuncaoBasica();

              
            }
            else if (level == 7)
            {

                linhas_faltando = 35;

                AuxiliarNext();

                await Dificuldade(8000, 255);
                FuncaoBasica();

                
            }
            else if (level == 8)
            { 

                gp_consulta.Visible = false;

                pan_mensagem.Visible = true;
                lbl_mensagem.Text = "Apartir de agora, a posição dos números estarão invertidos, mas a consulta aparece";
                await Task.Delay(2000);
                pan_mensagem.Visible = false;

                gp_consulta.Visible = true;
                linhas_faltando = 30;

                AuxiliarNext();

                Inversao();

                await Dificuldade(10000, 255);
                FuncaoBasica();

                
            }
            else if (level == 9)
            {

                linhas_faltando = 35;
                
                AuxiliarNext();

                await Dificuldade(10000, 255);
                FuncaoBasica();

                
            }
            else if(level == 10) 
            {
                linhas_faltando = 40;

                AuxiliarNext();

                await Dificuldade(8000, 255);
                FuncaoBasica();

                
            }



        }
        private void ZerarAcumulador()
        {
           for(int i = 0; i < 7;i++)
            {
                acumuladores[i] = 0;
            }
        }

        private void btn_restart_Click(object sender, EventArgs e)
        {

            ZerarAcumulador();
            level = 1;
            linhas_faltando = 10;
            pontuacao = 0;
            btn_start.Enabled = true;
            btn_desistir.Enabled = true;
            btn_pausar.Enabled = true;
            lbl_mensagem.Text = "Apartir do level 5 não apareceram mais números para consulta";

            tentativas++;

            pan_nextLevel.Visible = false;
            btn_nextLevel.Visible = true;
            btn_restart.Visible = false;
            lbl_nextLevel.Text = $"Level {level} completo";
            gp_consulta.Visible = true;
            numerosClear = 0;
            linhasPassadas = 0;
            numerosClear = 0;
            cronometro.Reset();
            pan_end.Visible = false;
            lbl_pontuacao.Text = pontuacao.ToString();
            emAndamento = false;
            jogoPausado = false;
            btn_pausar.Enabled = true;


            if (invertido == true)
            {
                invertido = false;
                Inversao();
            }

            foreach (GroupBox gp in grupoboxes)
            {
                gp.Enabled = true;
            }

            btn_start.PerformClick();
           
        }

        private void Inversao()
        {
            
            invertido = true;

            Button x;
            x = new Button();

            if(allButons.Count == 0)
            {
                allButons.Add(new Button[] { btn_1_rum, btn_2_rum, btn_4_rum, btn_8_rum, btn_16_rum, btn_32_rum, btn_64_rum, btn_128_rum });
                allButons.Add(new Button[] { btn_1_rum, btn_2_rum, btn_4_rum, btn_8_rum, btn_16_rum, btn_32_rum, btn_64_rum, btn_128_rum });
                allButons.Add(new Button[] { btn_1_rdois, btn_2_rdois, btn_4_rdois, btn_8_rdois, btn_16_rdois, btn_32_rdois, btn_64_rdois, btn_128_rdois });
                allButons.Add(new Button[] { btn_1_rtres, btn_2_rtres, btn_4_rtres, btn_8_rtres, btn_16_rtres, btn_32_rtres, btn_64_rtres, btn_128_rtres });
                allButons.Add(new Button[] { btn_1_rquatro, btn_2_rquatro, btn_4_rquatro, btn_8_rquatro, btn_16_rquatro, btn_32_rquatro, btn_64_rquatro, btn_128_rquatro });
                allButons.Add(new Button[] { btn_1_rcinco, btn_2_rcinco, btn_4_rcinco, btn_8_rcinco, btn_16_rcinco, btn_32_rcinco, btn_64_rcinco, btn_128_rcinco });
                allButons.Add(new Button[] { btn_1_rseis, btn_2_rseis, btn_4_rseis, btn_8_rseis, btn_16_rseis, btn_32_rseis, btn_64_rseis, btn_128_rseis });
                allButons.Add(new Button[] { btn_1_rsete, btn_2_rsete, btn_4_rsete, btn_8_rsete, btn_16_rsete, btn_32_rsete, btn_64_rsete, btn_128_rsete });
                
            }
            foreach (Button[] buton in allButons)
            {
                inversaoGp(buton);
            }

            TextBox[] texts = new TextBox[]
            {
                txt_g1,txt_g2,txt_g3,txt_g4,txt_g5,txt_g6,txt_g7,txt_g8
            };

            int n = texts.Length;

            for (int i = 0; i < n / 2; i++)
            {

                Point temp = texts[i].Location;
                texts[i].Location = texts[n - 1 - i].Location;
                texts[n - 1 - i].Location = temp;
            }

            void inversaoGp(Button[] g)
            {
                int l = g.Length;

                for (int i = 0; i < l / 2; i++)
                {
                    Point temp = g[i].Location;
                    g[i].Location = g[l - 1 - i].Location;
                    g[l - 1 - i].Location = temp;
                }
            }


        }

        private void btn_desistir_Click(object sender, EventArgs e)
        {
            foreach (GroupBox gp in grupoboxes)
            {
                gp.Visible = false;
            }
            
            jogo_parado = true;
            pan_nextLevel.Visible = true;
            btn_nextLevel.Visible = false;
            btn_restart.Visible = true;
            btn_pausar.Enabled = false;
            lbl_nextLevel.Text = "Você desistiu";
        }

        private void btn_pausar_Click(object sender, EventArgs e)
        {
            btn_pausar.Text = "VOLTAR";
            btn_desistir.Enabled = false;


            if (jogoPausado == false)
            {
                foreach (GroupBox gp in grupoboxes)
                {
                    if (gp.Visible == true)
                    {
                        gp.Visible = false;

                        gpVoltar.Add(gp);
                    }


                }
                jogoPausado = true;
                jogo_parado = true;
                pan_mensagem.Visible = true;
                lbl_mensagem.Text = "Jogo pausado, clique em voltar para continuar";
            }
            else if (jogoPausado == true)
            {
                btn_pausar.Text = "PAUSAR";
                btn_desistir.Enabled = true;

                foreach (GroupBox gp in grupoboxes)
                {
                    if (gpVoltar.Contains(gp))
                    {
                        gp.Visible = true;

                        gpVoltar.Remove(gp);
                    }


                }

                jogoPausado = false;
                jogo_parado = false;
                pan_mensagem.Visible = false;
                lbl_mensagem.Text = "Apartir do level 5 não apareceram mais números para consulta";

            }
        }

       
    }
        
}
