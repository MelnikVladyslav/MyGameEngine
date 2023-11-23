using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static MyGameEngine.SDL.SDLWrapper;

namespace MyGameEngine.SDL
{
    public class SDLTexture
    {
        // Завантаження бібліотеки SDL
        private const string SDL2Library = "SDL2.dll";

        // Створення текстури
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

        // Знищення текстури
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyTexture(IntPtr texture);

        // Створення рендерера
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRenderer(IntPtr window, int index, uint flags);

        // Очистка екрану
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderClear(IntPtr renderer);

        // Копіювання текстури на екран
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr src, SDL_Rect dst);

        // Оновлення текстури
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateTexture(IntPtr texture, IntPtr rect, IntPtr pixels, int pitch);

        // Оголосіть SDL_QueryTexture в SDLWrapper
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_QueryTexture(IntPtr texture, out uint format, out int access, out int w, out int h);

        // Оновлення екрану
        [DllImport(SDL2Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderPresent(IntPtr renderer);

        // Визначення SDL_TextureAccess
        public enum SDL_TextureAccess : int
        {
            SDL_TEXTUREACCESS_STATIC = 0,
            SDL_TEXTUREACCESS_STREAMING = 1,
            SDL_TEXTUREACCESS_TARGET = 2
        }

        // Додайте визначення для форматів текстур
        public static class SDL_PixelFormat
        {
            public const uint SDL_PIXELFORMAT_RGBA8888 = 0x16462004; // Це значення може змінюватися в залежності від вашої версії SDL
                                                                     // Додайте інші формати, якщо необхідно
        }

        // Додайте визначення для прапорів рендерера
        public static class SDL_RendererFlags
        {
            public const uint SDL_RENDERER_ACCELERATED = 0x00000002;
            public const uint SDL_RENDERER_PRESENTVSYNC = 0x00000004;
        }

        // Додайте визначення для структури SDL_Rect
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Rect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }
    }
}
