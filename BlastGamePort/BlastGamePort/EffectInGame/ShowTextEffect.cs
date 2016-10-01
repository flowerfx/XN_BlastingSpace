using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace BlastGamePort
{
    static class ShowTextEffect
    {
        static List<TextEffect> listText = new List<TextEffect>();

        public static void OnPushText(string str, Vector2 pos, int state)
        {
            TextEffect t = new TextEffect(str, pos, state);
            listText.Add(t);
        }

        public static void OnClearText()
        {
            listText.Clear();
        }

        public static void OnDrawListText(SpriteBatch spriteBatch)
        {
            if (listText.Count() <= 0)
                return;
            spriteBatch.Begin();
            for (int i = 0; i < listText.Count; i++)
            {
                listText[i].DrawTextEffect(spriteBatch);
            }
            spriteBatch.End();
        }
        public static void OnUpdateListText()
        {
            if (listText.Count() <= 0)
                return;
            for (int i = 0; i < listText.Count; i++)
            {
                listText[i].UpdateTextEffect();
                if (listText[i].timeLive <= 0)
                    listText.RemoveAt(i);
            }
        }
    }
    class TextEffect
    {
        string strText;
        Vector2 Position;
        int state;
        public int timeLive {get;set;}
        Color tint;
        Random rand = new Random();
        int speed;
        float Scale;
        public TextEffect(string str, Vector2 pos, int t)
        {
            strText = str;
            Position = new Vector2(pos.X + rand.Next(-5,5) * 3,pos.Y + rand.Next(-5,5) * 3);
            state = t;
            timeLive = 50 + rand.Next(-10, 10);
            if (t == 0)
            {
                strText = "+" + strText;
                tint = new Color(255, rand.Next(0, 255), 0);
                Scale = (float)rand.Next(3, 10) / (float)5;
                speed = rand.Next(-3, 5);
            }
            else if (t == 1)
            {
                strText = "Score X" + strText;
                tint = new Color(0, rand.Next(0, 255), 255);
                Scale = (float)rand.Next(4, 7) / (float)3;
                speed = rand.Next(2, 4);
                timeLive = 30;
            }
            else if (t == 2)
            {
                tint = Color.White;
                Scale = (float)rand.Next(2, 5) / (float)3;
                speed = rand.Next(-3, 3);
            }
            else if (t == 3)
            {
                tint = Color.Red;
                Scale = (float)rand.Next(4, 5) / (float)3;
                speed = rand.Next(1, 4);
            }
            else if (t == 4)
            {
                strText = "Multiplier Score X" + strText;
                tint = new Color(0, rand.Next(0, 255), 255);
                Scale = (float)rand.Next(4, 8) / (float)3;
                speed = 2;
                Position = new Vector2(625, 155);
                timeLive = 30;
            }
        }
        public void DrawTextEffect(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Art.Font, strText, Position, tint, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 0);
        }
        public void UpdateTextEffect()
        {
            timeLive--;
            if (timeLive < 0)
                return;
            if (state == 0)
            {
                Position.Y -= speed;
            }
            else if (state == 1)
            {
               // Position.X -= speed;
                Position.Y -= speed;
            }
            else if (state == 2 )
            {
                Position.Y -= speed;
            }

        }

    }
}
