using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static MyGameEngine.SDL.SDLWrapper;

namespace MyGameEngine.SDL
{
    public class SDLWrapper
    {
        // Завантаження бібліотеки SDL
        private const string SDL2Library = "SDL2.dll";

        // Ініціалізація SDL
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_Init(uint flags);

        // Створення вікна
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, uint flags);

        // Очікування на події та обробка
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PollEvent(out SDL_Event e);

        // Закриття вікна
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyWindow(IntPtr window);

        // Завершення роботи з SDL
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();

        // Додайте визначення для SDL_WINDOWPOS_CENTERED
        public const int SDL_WINDOWPOS_CENTERED = 0x1FFF0000;

        // Додайте визначення для прапорів вікна (перелік необхідно додати відповідно до вашого випадку)
        public static class SDL_WindowFlags
        {
            public const uint SDL_WINDOW_SHOWN = 0x00000004;
            public const uint SDL_WINDOW_FULLSCREEN = 0x00000001;
        }

        // Додайте визначення для типу подій
        public enum SDL_EventType : uint
        {
            SDL_QUIT = 0x100,
            SDL_WINDOWEVENT = 0x200,
            // Додайте інші типи подій за потребою
        }

        // Додайте визначення для ідентифікаторів подій вікна
        public enum SDL_WindowEventID : byte
        {
            SDL_WINDOWEVENT_CLOSE = 0,
            // Додайте інші ідентифікатори подій вікна за потребою
        }
    }

    // Оголошення структури для подій SDL
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Event
    {
        public SDL_EventType type;
        public SDL_WindowEvent window; // Додайте це поле для доступу до інформації про події вікна
                                       // Інші поля можуть бути додані згідно потреб
    }

    // Оголошення структури для подій вікна SDL
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_WindowEvent
    {
        public SDL_WindowEventID windowEvent;
        // Інші поля можуть бути додані згідно потреб
    }
}
