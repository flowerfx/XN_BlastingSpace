using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    class ButtonState
    {
        public Texture2D Button_DF { get; set; }
        public Texture2D Button_HL { get; set; }
        public bool IsPress = false;
        public Vector2 Position;
        public Vector2 Size;
        public Color Tint{ get; set; }
        //
        //if size have 0 value, it will be set as the size of the texture
        public ButtonState(Texture2D button1, Texture2D button2, Vector2 pos, Vector2 size)
        {
            Button_DF = button1;
            Button_HL = button2;
            Position = pos;
            Size = size;
            Tint = Color.White;
        }
        public void DrawButton(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle((int)Position.X,(int)Position.Y,(int)Size.X,(int)Size.Y);
            if (IsPress)
                spriteBatch.Draw(Button_HL, rect, Tint);
            else
                spriteBatch.Draw(Button_DF, rect, Tint);
        }
    }

    class DynamicButonState
    {
        public Texture2D Button_DF { get; set; }
        public bool IsPress = false;
        public Vector2 Position;
        public Vector2 Size;
        public DynamicButonState(Texture2D button1, Vector2 pos, Vector2 size)
        {
            Button_DF = button1;
            Position = pos;
            Size = size;
        }
        public void DrawDynamicButton(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle((int)Position.X,(int)Position.Y,(int)Size.X,(int)Size.Y);
            spriteBatch.Draw(Button_DF, rect, Color.White);
        }
        public void OnSetStateDynamic(int idx, int value)
        {
            if (idx == 0) // for the volume state
            {
                int tValue = value;
                if (tValue > 5)
                    tValue = 5;
                if (tValue < 1)
                    tValue = 1;
                //////////////
                if (tValue == 1)
                    Button_DF = ButtonTextureOptions.VolumeState_1;
                else if (tValue == 2)
                    Button_DF = ButtonTextureOptions.VolumeState_2;
                else if (tValue == 3)
                    Button_DF = ButtonTextureOptions.VolumeState_3;
                else if (tValue == 4)
                    Button_DF = ButtonTextureOptions.VolumeState_4;
                else if (tValue == 5)
                    Button_DF = ButtonTextureOptions.VolumeState_5;
            }
            else if (idx == 1) // for the volume state
            {
                int tValue = value;
                if (tValue > 4)
                    tValue = 4;
                if (tValue < 1)
                    tValue = 1;
                //////////////
                if (tValue == 1)
                    Button_DF = ButtonTextureOptions.GameLevelState_1;
                else if (tValue == 2)
                    Button_DF = ButtonTextureOptions.GameLevelState_2;
                else if (tValue == 3)
                    Button_DF = ButtonTextureOptions.GameLevelState_3;
                else if (tValue == 4)
                    Button_DF = ButtonTextureOptions.GameLevelState_4;
            }

        }
    }
    class BackgroundState
    {
        public Texture2D Background;
        public Vector2 Position;
        public Vector2 Size;
        public float rotate;
        public bool IsExpire { get; set; }
        public BackgroundState(Texture2D background, Vector2 pos, Vector2 size)
        {
            Background = background;
            Position = pos;
            Size = size;
            if (Size.X == 0 || Size.Y == 0)
            {
                if (Size.X == 0)
                    Size.X = background.Width;
                if (Size.Y == 0)
                    Size.Y = background.Height;
            }
            rotate = 0f;
            IsExpire = false;
        }
        public void UpdateBackground(int state)
        {
            if (state == 0)
            {
                rotate = rotate + (float)(Math.PI / 180f);
                if (rotate > 2 * Math.PI)
                {
                    rotate = 0;
                }
            }
            else if (state == 2)
            {
                Position.X += 1;
                if (Position.X >= Game1.GAMEWIDTH + 10)
                {
                    IsExpire = true;
                }

            }
            else if (state == 3)
            {
                Position.X += 3;
                if (Position.X >= Game1.GAMEWIDTH + 10)
                {
                    IsExpire = true;
                }

            }
        }

        public void UpdateSpeedBackground(int speed)
        {
            Position.X += speed;
            if (Position.X >= Game1.GAMEWIDTH + 5 * speed)
            {
                    IsExpire = true;
            }           
        }
        public void DrawBackground(SpriteBatch spriteBatch,Color tint, int state)
        {
            Rectangle rect = new Rectangle((int)Position.X,(int)Position.Y,(int)Size.X,(int)Size.Y);
            if(state == 0)
                spriteBatch.Draw(Background, rect, null, tint, rotate, new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2)), SpriteEffects.None, 0);
            else if (state == 1 || state == 2)
                spriteBatch.Draw(Background, rect, tint);

        }
    }

    class stateBackgroundRotate
    {
        Texture2D texture;
        public Texture2D Texture { get { return texture; } }
        float speedRotate;
        public float SpeedRotate { get { return speedRotate; } }
        float rotateAngle;
        public float RotateAngle { get { return rotateAngle; } set { rotateAngle = value; } }
        public Vector2 SizeCustom { get; set; }
        public Vector2 RealSize { get; set; }
        public stateBackgroundRotate(Texture2D tex, float speed, Vector2 Size)
        {
            texture = tex;
            speedRotate = speed;
            rotateAngle = 0f;
            if (Size.X == 0 || Size.Y == 0)
            {
                if (Size.X == 0)
                    Size.X = Texture.Width;
                if (Size.Y == 0)
                    Size.Y = Texture.Height;
            }
            RealSize = new Vector2(Texture.Width, Texture.Height);
            SizeCustom = Size;
        }
    }

    class RotateObjectMenu
    {
        public List<stateBackgroundRotate> Backgrounds;
        public Vector2 Position;
        public bool IsExpire { get; set; }
        private int curState;
        public RotateObjectMenu(Vector2 pos, int state)
        {
            Backgrounds = new List<stateBackgroundRotate>();
            OnGenRotateTexture(state);
            curState = state;
            Position = pos;           
            IsExpire = false;
        }
        public void UpdateRotateBackground(int state)
        {
            for (int i = 0; i < Backgrounds.Count; i++)
            {
                Backgrounds[i].RotateAngle = Backgrounds[i].RotateAngle + Backgrounds[i].SpeedRotate * (float)(Math.PI / 180f);
                if (Backgrounds[i].RotateAngle > 2 * Math.PI)
                {
                    Backgrounds[i].RotateAngle = 0;
                }
            }
           
        }
        public void DrawRotateBackground(SpriteBatch spriteBatch, Color tint, int state)
        {
            for (int i = 0; i < Backgrounds.Count; i++)
            {
                Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Backgrounds[i].SizeCustom.X, (int)Backgrounds[i].SizeCustom.Y);
                spriteBatch.Draw(Backgrounds[i].Texture, rect, null, tint, Backgrounds[i].RotateAngle, Backgrounds[i].RealSize / 2f, SpriteEffects.None, 0);
            }
        }

        public void SetSize(Vector2 size)
        {
            if (curState >= 7 && curState <= 10 )
            {
                Backgrounds[0].SizeCustom = size;
            }
        }

        private void OnGenRotateTexture(int idx)
        {
            if (idx == 0)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_1, 1 , new Vector2(182,180)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_2, 2, new Vector2(182, 180)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_3, -1, new Vector2(182, 180)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_4, -2, new Vector2(182, 180)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_5, 1, new Vector2(182, 180)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_6, -1, new Vector2(182, 180)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIGM.Rotate1_7, 1, new Vector2(182, 180)));
            }
            else if (idx == 1)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Radar1_1, 0, new Vector2(129, 129)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Radar1_2, 1.5f, new Vector2(129, 129)));
            }
            else if (idx == 2)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_1, 1, new Vector2(381, 381)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_2, 1, new Vector2(381, 381)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_3, 1.5f, new Vector2(381, 381)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_4, 0.5f, new Vector2(416, 416)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_5, 1, new Vector2(381, 381)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_6, 0.5f, new Vector2(438, 438)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate2_7, 1, new Vector2(381, 381)));
            }
            else if (idx == 3)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateST_1, 1, new Vector2(111, 113)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateST_2, -1.5f, new Vector2(111, 113)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateST_3, 1.5f, new Vector2(111, 113)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateST_4, -1f, new Vector2(111, 113)));
            }
            else if (idx == 4)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateRoundButtons_1, -2, new Vector2(150, 150)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateRoundButtons_2, 2, new Vector2(150, 150)));
            }
            else if (idx == 5)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureOption.Rotate1_1, 1, new Vector2(442, 442)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureOption.Rotate1_2, 0, new Vector2(359, 383)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureOption.Rotate1_3, -1, new Vector2(339, 339)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureOption.Rotate1_4, 0, new Vector2(269, 299)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureOption.Rotate1_5, 1, new Vector2(243, 243)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureOption.Rotate1_6, 0, new Vector2(49, 49)));
            }
            else if (idx == 6)
            {
                Backgrounds.Add(new stateBackgroundRotate(TextureResultMenu.Rotate3_1, 1, new Vector2(111, 113)));
                Backgrounds.Add(new stateBackgroundRotate(TextureResultMenu.Rotate3_2, 0, new Vector2(111, 113)));
                Backgrounds.Add(new stateBackgroundRotate(TextureResultMenu.Rotate3_3, -1, new Vector2(111, 113)));
                Backgrounds.Add(new stateBackgroundRotate(TextureResultMenu.Rotate3_4, 0, new Vector2(111, 113)));
            }
            else if (idx == 7)
            {
                Backgrounds.Add(new stateBackgroundRotate(TextureChapterMenu.MissionRotateButton, 1, new Vector2(240, 240)));
            }
            else if (idx == 8)
            {
                Backgrounds.Add(new stateBackgroundRotate(TextureChapterMenu.MissionRotateButton, 1, new Vector2(240 / 2, 240 / 2)));
            }
            else if (idx == 9)
            {
                Backgrounds.Add(new stateBackgroundRotate(TextureChapterMenu.ChapterRotateButton, 1, new Vector2(381, 381)));
            }
            else if (idx == 10)
            {
                Backgrounds.Add(new stateBackgroundRotate(TextureChapterMenu.ChapterRotateButton, 1, new Vector2(381 * 2 / 3, 381 * 2 / 3)));
            }
            else if (idx == 11)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateUD_1, 1.5f, new Vector2(189, 189)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateUD_2, -1.5f, new Vector2(189, 189)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateUD_3, 1.6f, new Vector2(189, 189)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.RotateUD_4, -1.6f, new Vector2(189, 189)));
            }
            else if (idx == 12)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate4_1, 1, new Vector2(282, 282)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate4_2, -1, new Vector2(282, 282)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate4_3, 1.5f, new Vector2(282, 282)));
            }
            else if (idx == 13)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTextureIAP.Rotate, 1, new Vector2(282, 261)));
            }
            else if (idx == 14)
            {
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate4_1, 1, new Vector2(110, 110)));
                Backgrounds.Add(new stateBackgroundRotate(BackgroundTexture.Rotate4_2, -1, new Vector2(110, 110)));
            }
        }
    }

    class ControlMoveObject
    {
        public Texture2D ObjectTexture;
        public Vector2 Position;
        public Vector2 Size;
        private Vector2 CenterPoint;
        private float Radius;
        public Vector2 MoveDirection { get; set; }
        public ControlMoveObject(Vector2 pos, Vector2 size, int state)
        {
            OnGenTexture(state);
            Position = pos;
            Size = size;
            CenterPoint = Position + (Size / 2f);
            Radius = (Size.X / 2f);
        }
        public void UpdateControlObject(int state, float radius, int id)
        {
            Vector2 MoveDir = new Vector2(0, 0);
            if (state == 0 || state == 2)
            {
                Position = new Vector2(CenterPoint.X - Radius, CenterPoint.Y - Radius);
                id = -1 ;
            }
            else if (state == 1)
            {
                Vector2 Dir = Input.GetTouchPos(id) - CenterPoint;
                float Lengh = Dir.Length();
                Vector2 DirTemp = Dir;
                DirTemp.Normalize();
                if (Lengh > Radius)
                    Position = CenterPoint + new Vector2((DirTemp.X * radius ) - Radius, (DirTemp.Y * radius ) - Radius);
                else
                    Position = Input.GetTouchPos(id) - new Vector2(Radius);
                MoveDir = Dir;
            }
            MoveDirection = MoveDir;
        }
        public void DrawControlObject(SpriteBatch spriteBatch, Color tint, int state)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            spriteBatch.Draw(ObjectTexture,rect,tint);
        }
        private void OnGenTexture(int idx)
        {
            if (idx == 0)
                ObjectTexture = TextureHUD.MoveStick;

        }
    }

    abstract class EntityMenu
    {
        protected List<ButtonState> Buttons;
       // protected Texture2D Background;
        protected int deltaTimeChange = 10;
        protected int stepProcess = 10;
        protected bool IsOnTheShow = false;
        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //draw the button
            for (int i = 0; i < Buttons.Count(); i++)
            {
                Buttons[i].DrawButton(spriteBatch);
            }
            //draw the background
            //spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
        }
    }
}
