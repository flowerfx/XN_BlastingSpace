using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

namespace WaterTest
{
    public sealed class Camera
    {
        public static Vector3 myPosition;
        public static Vector3 myTarget;
        public static Quaternion myRotation;

        private static Matrix myWorld;
        public static Matrix myView;
        public static Matrix myProjection;
        public static Viewport myViewport;

        private Camera()
        { }

        public static void Initialize()
        {
            myTarget = new Vector3();
            myPosition = new Vector3(0, 0, 0);
            myRotation = new Quaternion(0, 0, 0, 1);
        }

        public static void Update()
        {
            myWorld = Matrix.Identity;

            myView = Matrix.Invert(Matrix.CreateFromQuaternion(myRotation) *
                                    Matrix.CreateTranslation(myPosition));

            myProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3.0f, (float)myViewport.Width / (float)myViewport.Height, myViewport.MinDepth, myViewport.MaxDepth);
        }
        public static void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(myRotation));
            myRotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * myRotation);

            Update();
        }
        public static void Translate(Vector3 distance)
        {
            myPosition += Vector3.Transform(distance, Matrix.CreateFromQuaternion(myRotation));
            Update();
        }

        public static void Revolve(Vector3 target, Vector3 axis, float angle)
        {
            Rotate(axis, angle);
            Vector3 revolveAxis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(myRotation));
            Quaternion rotate = Quaternion.CreateFromAxisAngle(revolveAxis, angle);
            myPosition = Vector3.Transform(target - myPosition, Matrix.CreateFromQuaternion(rotate));

            Update();
        }
    }
}
