//#define My_debug
using Mole_Shooter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Mole_Shooter
{
    public partial class MoleShooter : Form
    {
        const int FrameNum = 12;
        const int SplatNum = 3;
        bool splat = false;
        int _gameFrame = 0;
        int _splatTime = 0;
        int _hits = 0;
        int _misses = 0;
        int _totalShots = 0;
        double _averageHits = 0;
#if My_debug
        int _cursX = 0;
        int _cursY = 0;
#endif
        CMole _mole;
        CSplat _splat;
        CSign _sign;
        CScoreFrame _scoreFrame;
        Random rnd = new Random();

        public MoleShooter()
        {
            InitializeComponent();

            Bitmap b = new Bitmap(Resources.sight);
            // this.Cursor = CustomCursor.CreateCursor(b, b.Height / 2, b.Width / 2);
            Graphics g = Graphics.FromImage(b);
            IntPtr ptr = b.GetHicon();
            Cursor = new System.Windows.Forms.Cursor(ptr);
            _mole = new CMole() {Left = 10, Top = 200 };
            _sign = new CSign() { Left = 320, Top = 10 };
            _splat = new CSplat();
            _scoreFrame = new CScoreFrame() { Left = 10, Top = 10 };
            
        }

        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            if (_gameFrame >= FrameNum)
            {
                UpdateMole();
                _gameFrame = 0;
            }

            if (splat)
            {
                
                if (_splatTime >= SplatNum)
                {
                    splat = false;
                    _splatTime = 0;
                    UpdateMole();
                }
                _splatTime++;
            }
            _gameFrame++;
            this.Refresh();
        }
        private void UpdateMole()
        {
            _mole.Update(
                rnd.Next(Resources.Mole.Width, this.Width - Resources.Mole.Width),
                rnd.Next(this.Height / 2, this.Height - Resources.Mole.Height * 2)
                );
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if (splat == true)
            {
                
                _splat.DrawImage(dc);
                

            }
            else
            {
                _mole.DrawImage(dc);
            }

            _sign.DrawImage(dc);
            _scoreFrame.DrawImage(dc);

#if My_debug
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Stencil",12, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, "x=" + _cursX.ToString() + ":" + "Y=" + _cursY.ToString(), _font,
                new Rectangle(0, 0, 120, 20), SystemColors.ControlText, flags);
#endif
            // put scores on the screen
            TextFormatFlags flags2 = TextFormatFlags.Left;
            Font _font2 = new System.Drawing.Font("Stencil", 10, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, "Shots: " + _totalShots.ToString(), _font2, new Rectangle(44, 53, 120, 20), SystemColors.ControlText, flags2);
            TextRenderer.DrawText(e.Graphics, "Hits: " + _hits.ToString(), _font2, new Rectangle(44, 73, 120, 20), SystemColors.ControlText, flags2);
            TextRenderer.DrawText(e.Graphics, "Misses: " + _misses.ToString(), _font2, new Rectangle(44, 93, 120, 20), SystemColors.ControlText, flags2);
            TextRenderer.DrawText(e.Graphics, "Avg: " + _averageHits.ToString("F0") + "%", _font2, new Rectangle(44, 113, 120, 20), SystemColors.ControlText, flags2);


            base.OnPaint(e);

        }

        private void MoleShooter_MouseMove(object sender, MouseEventArgs e)
        {
#if My_debug
            _cursX = e.X;
            _cursY = e.Y;
#endif
            this.Refresh();
        }

        private void MoleShooter_MouseClick(object sender, MouseEventArgs e)
        {

            if(e.X> 426 && e.X<466 && e.Y >97 && e.Y<106) // Start hot spot
            {
                timerGameLoop.Start();

            }

          else  if (e.X > 426 && e.X < 460 && e.Y > 115 && e.Y < 125) // Stop hot spot
            {
                timerGameLoop.Stop();
            }

         else   if (e.X > 426 && e.X < 466 && e.Y > 132 && e.Y < 142) // Reset hot spot
            {
                timerGameLoop.Stop();
            }

         else   if (e.X > 426 && e.X < 458 && e.Y > 151 && e.Y < 161) // Quit hot spot
            {
                timerGameLoop.Stop();
            }
            else
            {
                if (_mole.Hit(e.X, e.Y))
                    
                {
                    splat = true;
                    _splat.Left = _mole.Left - Resources.splatter.Width / 3;
                    _splat.Top = _mole.Top - Resources.splatter.Height / 3;

                    _hits++;

                }
                else
                {
                    _misses++;
                }
                _totalShots = _hits + _misses;
                _averageHits = (double)_hits / (double)_totalShots * 100;

                 
                
                

            }


            FireGun();
            


        }

       private void SoundTrack()
       {
           SoundPlayer simpleSound = new SoundPlayer(Resources.FirstTrack);
            
           
            simpleSound.PlayLooping();
       }

        private void FireGun()
        {
            //gun fire
            SoundPlayer simpleSound = new SoundPlayer(Resources.Shotgun);
                simpleSound.Play();
        }


        //game soundtrack: needs full path for audio file
        /* private void MoleShooter_Load(object sender, EventArgs e)
         {
             //SoundTrack
             // WMP.URL = @"D:\Ennio Morricone - Il Buono, Il Cattivo, Il Brutto (The Good, The Bad & The Ugly) (Main Title).mp3";          
             //  WMP.controls.play();                        
             }*/


        //game soundtrack 2-d variant
        private void MoleShooter_Load(object sender, EventArgs e)
         {
           SoundTrack();
          }
    }
}
