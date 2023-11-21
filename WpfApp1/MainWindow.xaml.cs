using MyGameEngine.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            // Очікування та обробка подій
            bool quit = false;
            MyGameEngine.SDL.SDL_Event e;

            while (!quit)
            {
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

            // Закриття вікна та завершення роботи з SDL
            SDLWrapper.SDL_DestroyWindow(window);
            SDLWrapper.SDL_Quit();
        }
    }
}
