using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Fablab_Creature
{
    class BodyPart
    {
        public Vector3 centerPoint;
        Vector3 absolutePoint;
        Vector3 position;
        Vector3 scale;
        Vector3 rampent;
        public Vector3 final;
        Texture2D texture;

        double rotation;

        double distanceFromCenter;

        Random rand;
        bool randomMovement;
        int counter = 0;

        Stopwatch stop = new Stopwatch();

        public BodyPart(Vector3 cP, Vector3 posInRapport, Vector3 sca, Texture2D tex, double rot, double dis, Random r, bool moveAround = true)
        {
            centerPoint = cP;
            absolutePoint = posInRapport;
            position = new Vector3(0,0,0);
            scale = sca;
            randomMovement = moveAround;
            rand = r;
           //rampent = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), 0);
            texture = tex;

            rotation = rot;
            final = centerPoint + absolutePoint;

            distanceFromCenter = dis;

        }

        public void Update(Vector3 cp)
        {
            //When I try and make position x and y 0, the program speed drops drastically but not when I do one or the other.

            if (randomMovement)
            {
                centerPoint = cp;
                counter++;
                if (counter > 10)
                {
                    counter = 0;
                    rampent.X += (float)(rand.NextDouble() * 2 - 1) / 5;
                    rampent.Y += (float)(rand.NextDouble() * 2 - 1) / 5;
                }
                if (DistanceBetween(final + position + rampent, final) < distanceFromCenter)
                {
                    position += rampent;
                }
                else
                {
                    rampent = 0 * (absolutePoint - position).Normalized() / 5;
                    counter = 8;
                }
            }
            else
            {
                centerPoint.Y += (float)9.81 * 1F;
                if(centerPoint.Y >= 538)
                {
                    centerPoint.Y = 538;
                }
            }
            final = centerPoint + absolutePoint;

        }

        public double DistanceBetween(Vector3 a, Vector3 b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        //private double Rotate(double angle)
        //{

        //}

        public void Draw(GraphicsBuffer square)
        {
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            Matrix4 mat = Matrix4.CreateTranslation(final + position-scale/2);      //Create a translation matrix
            GL.MatrixMode(MatrixMode.Modelview);                    //Load the modelview matrix, last in the chain of view matrices
            GL.LoadMatrix(ref mat);                                 //Load the translation matrix into the modelView matrix
            mat = Matrix4.CreateScale(scale);                       //Create a scale matrix
            GL.MultMatrix(ref mat);                                 //Multiply the scale matrix with the modelview matrix
            mat = Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(rotation));
            GL.MultMatrix(ref mat);
            GL.PushMatrix();
            GL.DrawElements(PrimitiveType.Quads, square.indexBuffer.Length, DrawElementsType.UnsignedInt, 0);
            GL.PopMatrix();
        }

        public override string ToString()
        {
            return (centerPoint + absolutePoint + position).ToString();
        }
    }
}
