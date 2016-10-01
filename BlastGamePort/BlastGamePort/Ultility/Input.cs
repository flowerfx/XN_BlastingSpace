using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    static class Input
    {
        private static KeyboardState keyboardState, lastKeyboardState;
        private static MouseState mouseState, lastMouseState;
        private static GamePadState gamepadState, lastGamepadState;
        private static TouchCollection ListTouchState;

        private static bool isAimingWithMouse = false;

        public static Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public static int IsThisButtonPress(ButtonState t)
        { 
            if(ListTouchState.Count <= 0)
                return 0;
            //
            for (int i = 0; i < ListTouchState.Count; i++ )
            {
                if (IsTheTouchOnTheButton(t , ListTouchState[i].Position))
                {
                    if (ListTouchState[i].State == TouchLocationState.Pressed || ListTouchState[i].State == TouchLocationState.Moved)
                    {
                        return 1;
                    }
                    if (ListTouchState[i].State == TouchLocationState.Released)
                    {
                        return 2;
                    }
                }
            }
            return 0;
            ////               
        }
        public static int IsThisTouchMoveControl(ButtonState t, int id)
        {
            if (ListTouchState.Count <= 0 || id < 0)
            {
                return 0;
            }
            if(id >= ListTouchState.Count)
                return 0;
            if (IsTheTouchOnTheButton(t, ListTouchState[id].Position))
            {
                if (ListTouchState[id].State == TouchLocationState.Pressed || ListTouchState[id].State == TouchLocationState.Moved)
                {
                        return 1;
                }
                if (ListTouchState[id].State == TouchLocationState.Released)
                {
                        return 2;
                }
             }
             return 0;           
        }
        public static int IsThisDynamicButtonSlide(BackgroundState t, int maxIdx)
        {
            for (int i = 0; i < ListTouchState.Count; i++)
            {
                Rectangle t1 = new Rectangle((int)t.Position.X, (int)t.Position.Y, (int)t.Size.X, (int)t.Size.Y);
                if (IsTheTouchOnCustomButton(t1, ListTouchState[i].Position) && (ListTouchState[i].State == TouchLocationState.Moved || ListTouchState[i].State == TouchLocationState.Pressed))
                {
                    for(int j = -1 ;j<= maxIdx + 1; j ++)
                    {
                        if (ListTouchState[i].Position.X <= t1.X + (j * t1.Width / maxIdx) && 
                            ListTouchState[i].Position.X >= t1.X + ((j - 1) * t1.Width / maxIdx))
                        {
                            return j;
                        }
                    }                  
                }
            }
            return 0;
        }

        private static bool IsTheTouchOnTheButton(ButtonState B , Vector2 TouchPos)
        {
            if (TouchPos.X >= B.Position.X && TouchPos.X <= B.Position.X + B.Size.X && TouchPos.Y >= B.Position.Y && TouchPos.Y <= B.Position.Y + B.Size.Y)
                return true;
            return false;
        }
        public static bool IsTheTouchOnCustomButton(Rectangle B, Vector2 TouchPos)
        {
            if (TouchPos.X >= B.X && TouchPos.X <= B.X + B.Width && TouchPos.Y >= B.Y && TouchPos.Y <= B.Y + B.Height)
                return true;
            return false;
        }
        public static void Update()  
        {
#if WINDOWS || XBOX
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            lastGamepadState = gamepadState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
            ListTouchState = TouchPanel.GetState();


            // If the player pressed one of the arrow keys or is using a gamepad to aim, we want to disable mouse aiming. Otherwise,
            // if the player moves the mouse, enable mouse aiming.
            if (new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down }.Any(x => keyboardState.IsKeyDown(x)) || gamepadState.ThumbSticks.Right != Vector2.Zero)
                isAimingWithMouse = false;
            else if (MousePosition != new Vector2(lastMouseState.X, lastMouseState.Y))
                isAimingWithMouse = true;
#else
            ListTouchState = TouchPanel.GetState();
#endif
        }

        public static Vector2 ProcessControlMC(Rectangle Object, Vector2 curState)
        {
            if (ListTouchState.Count == 1)
            {
                if (IsTheTouchOnCustomButton(Object, ListTouchState[0].Position))
                    return new Vector2(-1,0);
                else
                    return new Vector2(0, -1);
            }
            else if (ListTouchState.Count == 2)
            {
                if (IsTheTouchOnCustomButton(Object, ListTouchState[0].Position))
                {
                    return new Vector2(1, 0);
                }
                else if (IsTheTouchOnCustomButton(Object, ListTouchState[1].Position))
                {
                    return new Vector2(0, 1);
                }
                return new Vector2(0, -1);
            }
            return new Vector2(-1, -1);
        } // use to move object MC
        public static Vector2 ProcessControlMCByAnalog(Rectangle Object1, Rectangle Object2) // 1 is move 2 is attack
        {
            if (ListTouchState.Count == 1)
            {
                if (IsTheTouchOnCustomButton(Object1, ListTouchState[0].Position))
                    return new Vector2(-1, 0);
                else if (IsTheTouchOnCustomButton(Object2, ListTouchState[0].Position))
                    return new Vector2(0, -1);
                else
                    return new Vector2(-1, -1);
            }
            else if (ListTouchState.Count == 2)
            {
                if (IsTheTouchOnCustomButton(Object1, ListTouchState[0].Position))
                {
                    if (IsTheTouchOnCustomButton(Object2, ListTouchState[1].Position))
                        return new Vector2(1, 0);
                    else
                        return new Vector2(-1, 0);
                }
                else if (IsTheTouchOnCustomButton(Object1, ListTouchState[1].Position))
                {
                    if (IsTheTouchOnCustomButton(Object2, ListTouchState[0].Position))
                        return new Vector2(0, 1);
                    else
                        return new Vector2(0, -1);
                }
                return new Vector2(-1, -1);
            }
            return new Vector2(-1, -1);
        }
        // Checks if a key was just pressed down
        public static bool WasKeyPressed(Keys key)
        {
            return lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }
        public static bool WasButtonPressed(Buttons button)
        {
            return lastGamepadState.IsButtonUp(button) && gamepadState.IsButtonDown(button);
        }

        public static Vector2 GetMovementDirection()
        {

            Vector2 direction = gamepadState.ThumbSticks.Left;
            direction.Y *= -1;	// invert the y-axis

            if (keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;

            // Clamp the length of the vector to a maximum of 1.
            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        public static Vector2 GetAimDirection(int idx)
        {           

#if WINDOWS || XBOX
             if (isAimingWithMouse)
                return GetMouseAimDirection();
            Vector2 direction = gamepadState.ThumbSticks.Right;
            direction.Y *= -1;

            if (keyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;

            // If there's no aim input, return zero. Otherwise normalize the direction to have a length of 1.
            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);

#else
            return GetMouseAimDirection(idx);
#endif

        }

        private static Vector2 GetMouseAimDirection(int idx)
        {
#if WINDOWS || XBOX
            Vector2 direction = MousePosition - PlayerShip.Instance.Position;
#else
            Vector2 direction = new Vector2(0, 0);
            if (idx > -1)
                direction = ListTouchState[idx].Position - PlayerShip.Instance.Position;
            else
                direction = MousePosition - PlayerShip.Instance.Position;

#endif

            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }
        public static int IsPressOnScreen(int idx)
        {
             if(ListTouchState.Count <= 0 || idx < 0)
             {
                return 0;
             }
            //
             if (idx == -1 || idx >= ListTouchState.Count)
                 return 0;
             if (ListTouchState[idx].State == TouchLocationState.Pressed || ListTouchState[idx].State == TouchLocationState.Moved)
             {
                 return 1;
             }
             if (ListTouchState[idx].State == TouchLocationState.Released)
             {
                 return 2;
             }
                
             return 0;
        }
        public static Vector2 GetTouchPos(int id)
        {
#if WINDOWS || XBOX
            return MousePosition;
#else
            if (id > -1 && id < ListTouchState.Count)
                return ListTouchState[id].Position;
            return MousePosition;
#endif
        }
        //
        private static Vector2 StatPos;
        public static Vector3 GetDirectTouchMove()
        {
            Vector3 res = new Vector3(0, 0, 0);
            if (ListTouchState.Count == 1)
            {
                if (ListTouchState[0].State == TouchLocationState.Pressed)
                {
                    StatPos = ListTouchState[0].Position;
                    res.Z = 0;
                }
                else if (ListTouchState[0].State == TouchLocationState.Moved)
                {
                    res = new Vector3(ListTouchState[0].Position.X - StatPos.X,ListTouchState[0].Position.Y - StatPos.Y,1);
                    StatPos = ListTouchState[0].Position;
                }
                else if (ListTouchState[0].State == TouchLocationState.Released)
                {
                    StatPos = new Vector2(0, 0);
                    res.Z = 2;
                }
            }
            return res;
        }

        public static bool WasBombButtonPressed()
        {
            return WasButtonPressed(Buttons.LeftTrigger) || WasButtonPressed(Buttons.RightTrigger) || WasKeyPressed(Keys.Space);
        }
    }
}
