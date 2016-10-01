using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    class LoadingMenu
    {
        private static LoadingMenu instance;
        public static LoadingMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoadingMenu();

                return instance;
            }
        }
        float rotate;
        public LoadingMenu()
        {
            rotate = 0;
        }
        public void Update()
        {
            rotate += (float)(Math.PI / 180f);
            if(rotate > 2 * Math.PI)
                rotate = 0;
        }
        public void DrawMainMenu(SpriteBatch spriteBatch, bool IsDrawLogo)
        {
            spriteBatch.Begin();
            if (IsDrawLogo)
                spriteBatch.Draw(LoadingData.Logo, new Vector2(0, 0), Color.White);
            else
            {
                spriteBatch.Draw(LoadingData.MainLoading, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(LoadingData.Rotate1, new Rectangle(482, 369, LoadingData.Rotate1.Width, LoadingData.Rotate1.Height), null, Color.White, rotate, new Vector2(LoadingData.Rotate1.Width / 2, LoadingData.Rotate1.Height / 2), 0, 0);
                spriteBatch.Draw(LoadingData.Rotate2, new Rectangle(457, 408, LoadingData.Rotate2.Width, LoadingData.Rotate2.Height), null, Color.White, rotate, new Vector2(LoadingData.Rotate2.Width / 2, LoadingData.Rotate2.Height / 2), 0, 0);
                spriteBatch.Draw(LoadingData.Rotate3, new Rectangle(504, 419, LoadingData.Rotate3.Width, LoadingData.Rotate3.Height), null, Color.White, rotate * -1, new Vector2(LoadingData.Rotate3.Width / 2, LoadingData.Rotate3.Height / 2), 0, 0);
            }
            spriteBatch.End();
        }

        public static void OnShow()
        {
            instance = new LoadingMenu();
        }
    }
}
