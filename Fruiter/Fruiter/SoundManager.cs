using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Fruiter
{
    public class SoundManager
    {
        public SoundEffect BasketShootSound, explodeSound, GameOverSound, Advancementsound;
        public Song bgMusic, MenuThemeSong;

        // Constructor
        public SoundManager()
        {
            BasketShootSound = null;
            explodeSound = null;
            bgMusic = null;
            GameOverSound = null;
            Advancementsound = null;
            MenuThemeSong = null; 
        }

        public void LoadContent(ContentManager Content)
        {
            BasketShootSound = Content.Load<SoundEffect>("playershoot");
            explodeSound = Content.Load<SoundEffect>("explode");
            bgMusic = Content.Load<Song>("theme");
            GameOverSound = Content.Load<SoundEffect>("GameOverSnd");
            Advancementsound = Content.Load<SoundEffect>("Advance");
            MenuThemeSong = Content.Load<Song>("menutheme");
        }
    }
}
