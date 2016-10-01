using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    enum IteamDropStyle
    {
        ADD200HP,
        ADD800HP,
        ADD15SEC,
        ADD4ARMOR,
        ADD1GUN,
        ADD50DAMAGE,
        ADDCASH
    }
    class ItemDropManger
    {
        static int[] ChangeDrops = { 3, 7, 11, 16, 20, 25 , 34 };
        static List<ItemDrop> ListItemTyles = new List<ItemDrop>();
        static Random rand = new Random();

        private static void OnAddNewItemDrop(ItemDrop style)
        {
            ListItemTyles.Add(style);
        }
        private static void OnRemoveItemDrop(ItemDrop style)
        {
            ListItemTyles.Remove(style);
        }

        public static void OnUpdateItemDrop()
        {
            if (ListItemTyles == null)
                ListItemTyles = new List<ItemDrop>();
            if(ListItemTyles.Count <= 0 )
                return;
            for (int i = 0; i < ListItemTyles.Count;i++ )
            {
                if (ListItemTyles.Count <= 0 || i >= ListItemTyles.Count)
                    return;
                ListItemTyles[i].OnUpdate();
                if (ListItemTyles[i].OnColision(PlayerShip.Instance.Rect))
                {
                    OnProcessDropItem(ListItemTyles[i], ListItemTyles[i].Pos);
                    OnRemoveItemDrop(ListItemTyles[i]);
                    Sound.PickUp.Play(0.8f, 0.0f, 0.0f);
                    continue;
                }
                if (ListItemTyles[i].IsExpire)
                {
                    ListItemTyles.Remove(ListItemTyles[i]);
                    continue;
                }
            }
        }
        public static void OnDrawItemDrop(SpriteBatch spriteBatch)
        {
            if (ListItemTyles == null)
                ListItemTyles = new List<ItemDrop>();
            if (ListItemTyles.Count <= 0)
                return;
            spriteBatch.Begin();
            foreach (ItemDrop i in ListItemTyles)
            {
                i.OnDraw(spriteBatch);
            }
            spriteBatch.End();
        }

        public static void OnSetItemDrop(int state, Vector2 Pos)
        {
            if (rand == null)
                rand = new Random();
            int numberItemDrop = 1;
            int ChangeDrop = 0;
            //if (state == 0)// kill big meteor
            //{
            //    numberItemDrop = rand.Next(0, 10) / 5;                
            //}
            //else if (state == 1) // kill missile
            //{
            //    numberItemDrop = rand.Next(0, 10) / 7;
            //}
            //else if (state == 2) // kill black hole
            //{
            //    numberItemDrop = rand.Next(0, 10) / 9;
            //}
            //else if (state == 3) // kill ship enemy
            //{
            //    numberItemDrop = rand.Next(0, 10) / 6;

            //}
            //else if (state == 4) // kill H ship enemy
            //{
            //    numberItemDrop = rand.Next(0, 10) / 4;
            //}
            for (int i = 0; i < numberItemDrop; i++)
            {
                ChangeDrop = rand.Next(1, 100);
                for (int j = 0; j < ChangeDrops.Count(); j++)
                {
                    if (ChangeDrop % ChangeDrops[j] == 0)
                    {
                        OnGenChange(ChangeDrops[j], Pos);
                    }
                }
            }
        }

        public static void OnReleaseMem()
        {
            ListItemTyles.Clear();
            ListItemTyles = null;
            rand = null;
        }


        private static void OnGenChange(int t , Vector2 Pos)
        {
            if (t == 3)
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADD200HP,Pos));
            }
            else if (t == 7 )
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADD15SEC, Pos));
            }
            else if (t == 11)
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADD50DAMAGE, Pos));
            }
            else if (t == 16)
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADD1GUN, Pos));
            }
            else if (t == 20 && Game1.gIsPurchaseLightningArmor == 1)
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADD4ARMOR, Pos));
            }
            else if (t == 25 )
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADD800HP, Pos));
            }
            else if (t == 34)
            {
                OnAddNewItemDrop(new ItemDrop(IteamDropStyle.ADDCASH, Pos));
            }

        }

        private static void OnProcessDropItem(ItemDrop i, Vector2 Pos)
        {
            Pos = Vector2.Clamp(Pos, new Vector2(100, 100), new Vector2(700, 480));
            if (i.tyle == IteamDropStyle.ADD200HP)
            {
                PlayerShip.Instance.CharacterManager.HitPoint += 200;
                ShowTextEffect.OnPushText("Add 200 HP", Pos, 2);
            }
            else if (i.tyle == IteamDropStyle.ADD800HP)
            {
                PlayerShip.Instance.CharacterManager.HitPoint += 800;
                ShowTextEffect.OnPushText("Add 800 HP", Pos, 2);
            }
            else if (i.tyle == IteamDropStyle.ADD4ARMOR)
            {
                PlayerShip.Instance.CharacterManager.NumberLightningArmor += 4;
                ShowTextEffect.OnPushText("Add 4 lighting Armor", Pos, 2);
            }
            else if (i.tyle == IteamDropStyle.ADD15SEC)
            {
                HUDMenu.Instance.AddTime(15);
                ShowTextEffect.OnPushText("Add 15 Seconds", Pos, 2);
            }
            else if (i.tyle == IteamDropStyle.ADD50DAMAGE)
            {
                PlayerShip.Instance.CharacterManager.DamageBullet += 50;
                ShowTextEffect.OnPushText("Add 50 Damage Bullet", Pos, 2);
            }
            else if (i.tyle == IteamDropStyle.ADD1GUN)
            {
                PlayerShip.Instance.CharacterManager.NumberBullet += 1;
                if (PlayerShip.Instance.CharacterManager.NumberBullet > 18)
                    PlayerShip.Instance.CharacterManager.NumberBullet = 18;
                PlayerShip.Instance.OnResetGun();
                ShowTextEffect.OnPushText("Increase Your Gun", Pos, 2);
            }    
            else
            {
                int temp = rand.Next(1, 3);
                Game1.GlobalCash += temp;
                //SaveLoadManager.SaveAppSettingValue("GlobalCash", Game1.GlobalCash.ToString());
                ShowTextEffect.OnPushText("Add " + temp.ToString() + " Coin", Pos, 2);
            }

        }


    }
    class ItemDrop
    {
        public IteamDropStyle tyle;
        public bool IsExpire = false;
        Texture2D mTex;
        Texture2D HightLight;
        public Vector2 Pos;
        public Vector2 Speed;
        Vector2 Size;
        Random rand;
        public ItemDrop(IteamDropStyle t,Vector2 Position)
        {
            OnGenTexture(t);
            HightLight = Art.HightLight;
            tyle = t;
            Pos = Position;
            Size = new Vector2(36, 36);
            rand = new Random();
            Speed = new Vector2(rand.Next(-2,2),rand.Next(-2,2));
        }
        public void OnUpdate()
        {
            Pos += Speed;
            if (Pos.X >= Game1.GAMEWIDTH || Pos.X < 0 || Pos.Y >= Game1.GAMEHEIGH || Pos.Y < 0)
                IsExpire = true;
        }

        public bool OnColision(Rectangle rec)
        {
            bool val = false;
            if (IsCollision(rec))
                val = true;
            return val;
        }

        private bool IsCollision(Rectangle rec)
        {
            bool val = false;

            float radius = (float)(Math.Sqrt((double)(Size.X * Size.X + Size.Y * Size.Y)) / 2) + PlayerShip.Instance.Radius;
            if (Vector2.Distance(Pos + Size / 2f, PlayerShip.Instance.PosCenter) <= radius)
                val = true;
            return val;
        }


        public void OnDraw(SpriteBatch spriteBatch)
        {
            float scale = 1 + 0.1f * (float)Math.Sin(10 * Game1.GameTime.TotalGameTime.TotalSeconds);
            //spriteBatch.Draw(HightLight, Pos, null, Color.White, 0f, new Vector2(0,0), scale, 0, 0);
            spriteBatch.Draw(mTex, Pos,null, Color.White, 0f, new Vector2(0,0), scale, 0, 0);
        }

        private void OnGenTexture(IteamDropStyle t)
        {
            if (t == IteamDropStyle.ADD200HP)
            {
                mTex = Art.ADD100HP;
            }
            else if (t == IteamDropStyle.ADD800HP)
            {
                mTex = Art.ADD500HP;
            }
            else if (t == IteamDropStyle.ADD15SEC)
            {
                mTex = Art.ADD10SEC;
            }
            else if (t == IteamDropStyle.ADD4ARMOR)
            {
                mTex = Art.ADD3ARMOR;
            }
            else if (t == IteamDropStyle.ADD1GUN)
            {
                mTex = Art.ADD1GUN;
            }
            else if (t == IteamDropStyle.ADD50DAMAGE)
            {
                mTex = Art.ADD50DAMAGE;
            }
            else if (t == IteamDropStyle.ADDCASH)
            {
                mTex = Art.ADDCASH;
            }
        }
    }

}
