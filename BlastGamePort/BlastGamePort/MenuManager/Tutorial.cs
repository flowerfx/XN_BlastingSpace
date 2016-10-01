using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace BlastGamePort
{
    enum TUTORIAL_STEP
    {
        NONE = 0,
        POPUP_INTRO,
        POPUP_MOVE,
        HAND_TOUCH,
        POP_UP
    }
    class Tutorial : EntityMenu
    {
        static Vector3 State;

        static string Str;

        static public bool IsShow = false;       

        private BackgroundState BackgroundMain;
        private BackgroundState BackgroundPoint;
        private Vector2 PosString;
        private static Vector2 PosPopUp = new Vector2(0, 0);
        private static int StateHavePoint = 0;

        private static Tutorial instance;
        public static Tutorial Instance
        {
            get
            {
                if (instance == null)
                    instance = new Tutorial();

                return instance;
            }
        }

        public Tutorial()
        {
            BackgroundMain = new BackgroundState(Art.PopUpTutorial, PosPopUp, new Vector2(308, 181));
            if (StateHavePoint == 1)
                BackgroundPoint = new BackgroundState(Art.HandPoint, PlayerShip.Instance.Position, new Vector2(60, 93));
            else if (StateHavePoint == 2)
                BackgroundPoint = new BackgroundState(Art.HandPoint, new Vector2(800,480) - PlayerShip.Instance.Position, new Vector2(60, 93));
            else if (StateHavePoint == 3)
                BackgroundPoint = new BackgroundState(Art.ArrowPoint, new Vector2(440, 60), new Vector2(60, 60));
            else if (StateHavePoint == 4)
                BackgroundPoint = new BackgroundState(Art.ArrowPoint, new Vector2(700, 40), new Vector2(60, 60));


            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp = new ButtonState(Art.Btn_OK_DF, Art.Btn_OK_HL, new Vector2(BackgroundMain.Position.X + 103, BackgroundMain.Position.Y + 139), new Vector2(102, 30));

            Buttons.Add(buttonStateTemp);
            IsOnTheShow = true;
            PosString = new Vector2(BackgroundMain.Position.X + 32, BackgroundMain.Position.Y + 14);
        }

        public static void OnShow(Vector3 state, int numberButton)
        {
            IsShow = true;
            OnSetString(state, true);
            State = state;
            instance = new Tutorial();
        }

        private static void OnSetString(Vector3 st , bool IsShow) // X is chapter | Y is Mission | Y Is Step
        {
            if (st.X == 0)
            {
                if (st.Y == 0)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "Hello, Krazer ! \nYou need to find way \nback to our earth";
                            PosPopUp = new Vector2(174, 101);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                        }
                    }
                    else if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "First!\nYou need to learn \nhow to control our ship:\nTouch on ship and drag its \non screen";
                            PosPopUp = new Vector2(9, 219);
                            StateHavePoint = 1;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                        }
                    }
                    else if (st.Z == 2)
                    {
                        if (IsShow)
                        {
                            Str = "Good job !\nWhen you're moving \nyou also fire the bullet!";
                            PosPopUp = new Vector2(174, 101);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 0.5f;
                        }
                    }
                    else if (st.Z == 3)
                    {
                        if (IsShow)
                        {
                            Str = "\nTouch anywhere on screen \nto make your ship fire \nthat point";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 2;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                        }
                    }
                    else if (st.Z == 4)
                    {
                        if (IsShow)
                        {
                            Str = "Good job ! \n\nYou must finish before time-out\nOr survive in time-survival";
                            PosPopUp = new Vector2(150, 80);
                            StateHavePoint = 3;
                        }
                        else
                            HUDMenu.TimeDelayTutorial = 2.5f;
                    }
                    else if (st.Z == 5)
                    {
                        if (IsShow)
                        {
                            Str = "\nNote: !\nThis is target You must gain \n to complete the mission !";
                            PosPopUp = new Vector2(400, 80);
                            StateHavePoint = 4;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                        }
                    }
                    else if (st.Z == 6)
                    {
                        if (IsShow)
                        {
                            Str = "\nGood job ! \nNow begin your adventure !";
                            PosPopUp = new Vector2(174, 101);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                           // Game1.StepFinishTutorial = 1;
                        }
                    }
                }
                else if (st.Y == 1)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "Carefully !\nThe Hole will pull anything \naround it and push your \nbullet....";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 0.5f;
                        }
                    }
                    else if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "\n\n...But you can attack the hole!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                            //Game1.StepFinishTutorial = 2;
                        }
                    }
                }
                else if (st.Y == 2)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "\n\nHey \nTake care of \nIncomming Missile ... !";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 1.5f;
                        }
                    }
                    else if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "\nKrazer! \nWhen have alert \ntry avoiding atomic-missile....!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            //Game1.StepFinishTutorial = 3;
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                        }
                    }
                }
                else if (st.Y == 3)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "\nIf You have problem in battle\nJust upgrade your weapon...!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                        }
                    }                  
                }
                else if (st.Y == 8)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "\nKrazer !\nTake care of Enemy Battle-\nShip, They are the nightmare \n of space";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 1f;
                        }
                    }
                    else if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "\nYou need to know !\nThe High-Ship has top-Power!\nPrepare yourself to duel it";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                            //Game1.StepFinishTutorial = 4;
                        }
                    }
                }
                else if (st.Y == 11)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "\nKrazer!\nThe Enternal Battle of \nthis Chapter is opened !";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 1.0f;
                        }
                    }
                    if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "\nAnd....!\n...You can find more \n coin when fight in its";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                            //Game1.StepFinishTutorial = 5;
                        }
                    }
                }
                else if (st.Y == 19)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "\nWell Krazer ! \nYou've reached end of \nBlasting Rock\n";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 1.5f;
                        }

                    }
                    if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "\nJust survive after the last point \nAnd you will come to new zone";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                            //Game1.StepFinishTutorial = 6;
                        }

                    }
                }
            }
            else if (st.X == 1)
            {
                if (st.Y == 0)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "Krazer !\nYou've come Blasting Machine\nThis Zone is full of..\n...enemy-machine";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 1.5f;

                        }
                    }
                    if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "\nRemember This !\nYou need at least lightning-Shield\n to survive in Chapter.!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 1.5f;
                        }
                    }
                    if (st.Z == 2)
                    {
                        if (IsShow)
                        {
                            Str = "\nIf you dont have shield! \nYou can purchase its \nin upgrade-menu!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                            //Game1.StepFinishTutorial = 7;
                        }
                    }
                }           
                else if (st.Y == 19)
                {
                    if (st.Z == 0)
                    {
                        if (IsShow)
                        {
                            Str = "Hey Krazer !\nThis is the last mission of us! \nThe Hardest mission ever ....!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                        }
                    }
                    if (st.Z == 1)
                    {
                        if (IsShow)
                        {
                            Str = "CHIEN.TQ: \nThis is the last mission of \n this version! And the next mission \nwill come soon !!!";
                            PosPopUp = new Vector2(368, 72);
                            StateHavePoint = 0;
                        }
                        else
                        {
                            HUDMenu.TimeDelayTutorial = 2.5f;
                            HUDMenu.Instance.IsFinishThisChapterTutorial = true;
                            // Game1.StepFinishTutorial = 8;
                        }
                    }
                }
            }
        }

        public static void OnClose()
        {
           IsShow = false;
           OnSetString(State, false);
           instance = null;
        }       

        public override void Update()
        {
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                OnCallFunctionAtButtonIdx(0);

            }
            //
                int HRESULT = Input.IsThisButtonPress(Buttons[0]);
                if (HRESULT > 0)
                {
                    if (HRESULT == 2) // release the button
                    {
                        OnCallFunctionAtButtonIdx(0);
                    }
                    Buttons[0].IsPress = true;
                }
                else
                {
                    Buttons[0].IsPress = false;
                }
            //
            
        }

        public void DrawPopUp(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BackgroundMain.DrawBackground(spriteBatch, Color.White, 1);
            if (BackgroundPoint != null)
                BackgroundPoint.DrawBackground(spriteBatch, Color.White, 1);
            spriteBatch.DrawString(BackgroundTexturePopUp.Font, new StringBuilder(Str), PosString, Color.White,0f, new Vector2(0,0),0.8f,0,0);
            Draw(spriteBatch);
            spriteBatch.End();
        }

        private void OnCallFunctionAtButtonIdx(int i)
        {
            Sound.OkButton.Play(1.0f,0.0f,0.0f);         
            OnClose();
        }
    }
}
