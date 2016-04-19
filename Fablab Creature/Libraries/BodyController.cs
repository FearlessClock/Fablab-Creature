using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Fablab_Creature
{
    class BodyController
    {

        Dictionary<string, Texture2D> BodyParts;
        BodyPart handLeft;
        BodyPart footRight;
        BodyPart handRight;
        BodyPart footLeft;
        BodyPart body;
        BodyPart head;
        BodyPart eyeLeft;
        BodyPart eyeRight;
        BodyPart mouth;

        BodyPart CENTER;

        Random rand = new Random();

        Vector3 placement = new Vector3(400, 200, 0);

        TextWriter textWriter = new TextWriter("Alphabet/");

        public BodyController()
        {
            BodyParts = new Dictionary<string, Texture2D>();
            BodyParts.Add("Hand", ContentPipe.LoadTexture("BodyParts/Hand.png"));
            BodyParts.Add("FootLeft", ContentPipe.LoadTexture("BodyParts/FootLeft.png"));
            BodyParts.Add("FootRight", ContentPipe.LoadTexture("BodyParts/FootRight.png"));
            BodyParts.Add("Head", ContentPipe.LoadTexture("BodyParts/Head.png"));
            BodyParts.Add("Body", ContentPipe.LoadTexture("BodyParts/Body.png"));
            BodyParts.Add("Mouth", ContentPipe.LoadTexture("BodyParts/Mouth.png"));
            BodyParts.Add("Eye", ContentPipe.LoadTexture("BodyParts/Eye.png"));

            CENTER = new BodyPart(new Vector3(100, 100, 0), new Vector3(0, 0, 0), new Vector3(10, 10, 0), ContentPipe.LoadTexture("placeholder.png"), 0, 0, rand, false);

            body = new BodyPart(placement, new Vector3(0, 0, 0), new Vector3(60, 60, 0), BodyParts["Body"], 0, 5, rand, false);
            handLeft = new BodyPart(placement, new Vector3(-30,0,0), new Vector3(30, 30, 0), BodyParts["Hand"], 0, 5, rand);
            handRight = new BodyPart(placement, new Vector3(30, 0, 0), new Vector3(30, 30, 0), BodyParts["Hand"], 0, 5, rand);
            footRight = new BodyPart(placement, new Vector3(25, 50, 0), new Vector3(30, 30, 0), BodyParts["FootRight"], 0, 5, rand);
            footLeft = new BodyPart(placement, new Vector3(-25, 50, 0), new Vector3(30, 30, 0), BodyParts["FootLeft"], 0, 5, rand);
            head = new BodyPart(placement, new Vector3(0, -50, 0), new Vector3(60, 60, 0), BodyParts["Head"], 0, 1, rand);
            eyeLeft = new BodyPart(placement, new Vector3(-10, -55, 0), new Vector3(20, 20, 0), BodyParts["Eye"], 0, 2, rand);
            eyeRight = new BodyPart(placement, new Vector3(10, -55, 0), new Vector3(20, 20, 0), BodyParts["Eye"], 0, 2, rand);
            mouth = new BodyPart(placement, new Vector3(0, -30, 0), new Vector3(15, 15, 0), BodyParts["Mouth"], 0, 3, rand);

            //What I could do to place the body parts is pick a circle where the different parts must be and then just constreint the movement to those circles. 

        }
        
        public void Update()
        {
            body.Update(new Vector3(Camera.cursorPos.X, Camera.cursorPos.Y, 0));
            handLeft.Update(body.centerPoint);
            footRight.Update(body.centerPoint);
            handRight.Update(body.centerPoint);
            footLeft.Update(body.centerPoint);
            head.Update(body.centerPoint);
            eyeLeft.Update(body.centerPoint);
            eyeRight.Update(body.centerPoint);
            mouth.Update(body.centerPoint);
        }

        public void Talk(string text)
        {
            textWriter.WriteToScreen(mouth.final.Xy + new Vector2(20, -20), text, 100, 10, true);
        }

        public void Draw(GraphicsBuffer square)
        {
            body.Draw(square);
            handLeft.Draw(square);
            footRight.Draw(square);
            handRight.Draw(square);
            footLeft.Draw(square);
            head.Draw(square);
            eyeLeft.Draw(square);
            eyeRight.Draw(square);
            mouth.Draw(square);
            CENTER.Draw(square);
            Talk("Hello Dylan!!");
        }

        public override string ToString()
        {
            return "Hand:" + handLeft.ToString() + " Foot:" + footRight.ToString();
        }

    }
}
