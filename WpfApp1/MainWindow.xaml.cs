using MyGameEngine.SDL;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private const int ProgressBarHeight = 20;

        public MainWindow()
        {
            InitializeComponent();

            // Ініціалізація SDL
            if (SDLWrapper.SDL_Init(0) != 0)
            {
                return;
            }

            // Отримання розмірів екрану
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            // Створення вікна
            IntPtr window = SDLWrapper.SDL_CreateWindow("My Game", SDLWrapper.SDL_WINDOWPOS_CENTERED, SDLWrapper.SDL_WINDOWPOS_CENTERED, (int)Width, (int)Height, SDLWrapper.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine("Window creation failed!");
                SDLWrapper.SDL_Quit();
                return;
            }

            // Створення рендерера
            uint rendererFlags = SDLWrapper.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDLWrapper.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
            IntPtr renderer = SDLTexture.SDL_CreateRenderer(window, -1, rendererFlags);

            // Створення текстури для прогрес-бару
            IntPtr progressBarTexture = CreateProgressBarTexture(renderer);

            // Очікування та обробка подій
            bool quit = false;
            MyGameEngine.SDL.SDL_Event e;

            while (!quit)
            {
                // Оновлення та відображення прогрес-бару
                UpdateAndRenderProgressBar(progressBarTexture, renderer);

                // Перевірка на тип події SDL_QUIT
                while (SDLWrapper.SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDLWrapper.SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;

                        case SDLWrapper.SDL_EventType.SDL_WINDOWEVENT:
                            if (e.window.windowEvent == SDLWrapper.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                            {
                                quit = true;
                            }
                            break;
                    }
                }
            }

            // Звільнення ресурсів текстури та закриття вікна
            SDLTexture.SDL_DestroyTexture(progressBarTexture);
            SDLWrapper.SDL_DestroyWindow(window);
            SDLWrapper.SDL_Quit();
        }

        private IntPtr CreateProgressBarTexture(IntPtr renderer)
        {
            // Тут ви можете використовувати SDL для створення текстури прогрес-бару
            // Наприклад, створіть прямокутник зеленого кольору для початкового стану прогресу
            IntPtr progressBarTexture = SDLTexture.SDL_CreateTexture(renderer, SDLTexture.SDL_PixelFormat.SDL_PIXELFORMAT_RGBA8888, (int)SDLTexture.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, (int)Width, ProgressBarHeight);

            return progressBarTexture;
        }

        private void UpdateAndRenderProgressBar(IntPtr progressBarTexture, IntPtr renderer)
        {
            // Оновлення значення прогрес-бару (змінюйте це значення під час завантаження гри)
            int progressValue = CalculateProgressValue();

            // Оновлення текстури прогрес-бару (змінюйте це значення під час завантаження гри)
            UpdateProgressBarTexture(progressBarTexture, progressValue);

            // Відображення прогрес-бару на екрані
            SDLTexture.SDL_RenderClear(renderer);
            SDLTexture.SDL_RenderCopy(renderer, progressBarTexture, IntPtr.Zero, new SDLTexture.SDL_Rect { x = 0, y = (int)(Height - ProgressBarHeight), w = (int)Width, h = ProgressBarHeight });
            SDLTexture.SDL_RenderPresent(renderer);
        }

        private int CalculateProgressValue()
        {
            // Тут ви повинні визначити поточне значення прогресу на основі завантаження ресурсів
            // Повертається значення від 0 до 100
            return 50;
        }

        private void UpdateProgressBarTexture(IntPtr progressBarTexture, int progressValue)
        {
            // Отримати ширину та висоту текстури
            int textureWidth, textureHeight;
            SDLTexture.SDL_QueryTexture(progressBarTexture, out _, out _, out textureWidth, out textureHeight);

            // Створити буфер для пікселів
            byte[] pixels = new byte[textureWidth * textureHeight * 4];

            // Заповнити буфер залежно від значення прогресу
            int progressWidth = (int)(textureWidth * (progressValue / 100.0));
            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    int index = (y * textureWidth + x) * 4;
                    if (x < progressWidth)
                    {
                        // Зелений колір для частини прогрес-бару
                        pixels[index] = 0;     // R
                        pixels[index + 1] = 255; // G
                        pixels[index + 2] = 0;   // B
                        pixels[index + 3] = 255; // A
                    }
                    else
                    {
                        // Чорний колір для решти прогрес-бару
                        pixels[index] = 0;   // R
                        pixels[index + 1] = 0; // G
                        pixels[index + 2] = 0; // B
                        pixels[index + 3] = 255; // A
                    }
                }
            }

            // Оновити текстуру прогрес-бару
            GCHandle pinnedPixels = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            IntPtr pixelsPtr = pinnedPixels.AddrOfPinnedObject();
            SDLTexture.SDL_UpdateTexture(progressBarTexture, IntPtr.Zero, pixelsPtr, textureWidth * 4);
            pinnedPixels.Free();
        }
    }
}
