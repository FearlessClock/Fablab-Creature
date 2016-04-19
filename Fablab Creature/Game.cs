using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Fablab_Creature
{
    struct Vertex
    {
        public Vector2 position;
        public Vector2 texCoord;
        public Vector4 color;

        public Color Color
        {
            get
            {
                return Color.FromArgb((int)(255 * color.W), (int)(255 * color.X), (int)(255 * color.Y), (int)(255 * color.Z));
            }
            set
            {
                this.color = new Vector4(value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f);
            }

        }
        static public int SizeInBytes
        {
            get { return Vector2.SizeInBytes * 2 + Vector4.SizeInBytes; }
        }

        public Vertex(Vector2 position, Vector2 texCoord)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = new Vector4(1, 1, 1, 1);
        }


    }
    class Game
    {
        public GameWindow window;
        Texture2D texture;

        BodyController bodyController;
        TextWriter textWriter;

        //Start of the vertex buffer
        GraphicsBuffer buffer = new GraphicsBuffer();
        GraphicsBuffer[] CursorBuf;


        public Game(GameWindow windowInput)
        {
            window = windowInput;

            window.Load += Window_Load;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.Closing += Window_Closing;
            Camera.SetupCamera(window, 30);
            textWriter = new TextWriter("Alphabet/");
            bodyController = new BodyController();
            
            window.CursorVisible = false;
        }


        private void Window_Load(object sender, EventArgs e)
        {
            texture = ContentPipe.LoadTexture("placeholder.png");

            buffer.vertBuffer = new Vertex[4]
            {
                new Vertex(new Vector2(0, 0), new Vector2(0, 0)),
                new Vertex(new Vector2(0, 1), new Vector2(0, 1)),
                new Vertex(new Vector2(1, 1), new Vector2(1, 1)),
                new Vertex(new Vector2(1, 0), new Vector2(1, 0))
            };


            buffer.VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.VBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * buffer.vertBuffer.Length), buffer.vertBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            buffer.indexBuffer = new uint[4]
            {
                0,1,2,3
            };

            buffer.IBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * (buffer.indexBuffer.Length)), buffer.indexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void BufferFill(GraphicsBuffer buf)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buf.VBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * buf.vertBuffer.Length), buf.vertBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buf.IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * (buf.indexBuffer.Length)), buf.indexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        int counter = 0;
        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            CursorBuf = Camera.CameraUpdate();
            foreach (GraphicsBuffer b in CursorBuf)
            {
                BufferFill(b);
            }
            bodyController.Update();
            counter++;
        }

        System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            //Clear screen color
            GL.ClearColor(Color.Wheat);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Enable color blending, which allows transparency
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            //Blending everything for transparency
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            //Create the projection matrix for the scene
            Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref proj);



            //Enable all the different arrays
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);


            //textWriter.WriteToScreen(new Vector2(0, 30), bodyController.ToString(), window.Width, 20);

            //Bind the texture that will be used
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            //Load the vert and index buffers
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.VBO);
            GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, (IntPtr)0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector2.SizeInBytes));
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector2.SizeInBytes * 2));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.IBO);


            
            bodyController.Draw(buffer);
            //Flush everything 
            GL.Flush();
            //Write the new buffer to the screen
            long time = 0;
            stop.Start();
            time = stop.ElapsedMilliseconds;
            Console.Error.WriteLine(stop.ElapsedMilliseconds);
            window.SwapBuffers();
            Console.Error.WriteLine(stop.ElapsedMilliseconds + " ! " + (stop.ElapsedMilliseconds - time));

        }
    }
}
