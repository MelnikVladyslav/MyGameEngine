using MyGameEngine.TestItems;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGameEngine
{
    public class Game : GameWindow
    {
        int VertexBufferObject;
        Triangle testTr = new Triangle();
        Shader shader;
        private int VertexArrayObject;
        private int _elementBufferObject;
        int Width, Height;

        // For documentation on this, check Texture.cs.
        private Texture texture;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { Width = width; Height = height; }

        //Start(Initilization)
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //Texture
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, testTr.vertices.Length * sizeof(float), testTr.vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, testTr.indices.Length * sizeof(uint), testTr.indices, BufferUsageHint.StaticDraw);

            // Генерація та компіляція вершинного та фрагментного шейдерів
            shader = new Shader("Files/Shader/shader.vert", "Files/Shader/shader.frag");
            shader.Use();

            //Texture
            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            texture = Texture.LoadFromFile("Img/container.png");
            texture.Use(TextureUnit.Texture0);

            //Vector
            //// Генерація та зв'язування Vertex Buffer Object (VBO)
            //VertexBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, testTr.vertices.Length * sizeof(float), testTr.vertices, BufferUsageHint.StaticDraw);

            //// Генерація та зв'язування Vertex Array Object (VAO)
            //VertexArrayObject = GL.GenVertexArray();
            //GL.BindVertexArray(VertexArrayObject);

            //// Встановлення вказівників атрибутів вершин
            ////Vector
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //GL.EnableVertexAttribArray(0);
        }

        //Renderer
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Texture
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(VertexArrayObject);

            texture.Use(TextureUnit.Texture0);
            shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, testTr.indices.Length, DrawElementsType.UnsignedInt, 0);

            //Vector
            //GL.Clear(ClearBufferMask.ColorBufferBit);

            //shader.Use();

            //GL.UseProgram(shader.Handle);
            //GL.BindVertexArray(VertexArrayObject);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        //Update size window
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        //Update frame
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //На Escape -> Exit
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        //Unload
        protected override void OnUnload()
        {
            base.OnUnload();

            shader.Dispose();
        }
    }
}
