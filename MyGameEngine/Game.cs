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

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

        //Start(Initilization)
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Генерація та компіляція вершинного та фрагментного шейдерів
            shader = new Shader("shader.vert", "shader.frag");

            // Генерація та зв'язування Vertex Buffer Object (VBO)
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, testTr.vertices.Length * sizeof(float), testTr.vertices, BufferUsageHint.StaticDraw);

            // Генерація та зв'язування Vertex Array Object (VAO)
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // Встановлення вказівників атрибутів вершин
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        //Renderer
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();

            GL.UseProgram(shader.Handle);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
