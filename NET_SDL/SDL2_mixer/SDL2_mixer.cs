#region License
/* SDL2# - C# Wrapper for SDL2
 *
 * Copyright (c) 2013-2024 Ethan Lee.
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */
#endregion

#region Using Statements
using System;
using System.Runtime.InteropServices;
#endregion

namespace SDL2
{
	public static partial class SDL_mixer
	{
		#region SDL2# Variables

		/* Used by DllImport to load the native library. */
		private const string nativeLibName = "SDL2_mixer";

		#endregion

		#region SDL_mixer.h

		/* Similar to the headers, this is the version we're expecting to be
		 * running with. You will likely want to check this somewhere in your
		 * program!
		 */
		public const int SDL_MIXER_MAJOR_VERSION =	2;
		public const int SDL_MIXER_MINOR_VERSION =	0;
		public const int SDL_MIXER_PATCHLEVEL =		5;

		/* In C, you can redefine this value before including SDL_mixer.h.
		 * We're not going to allow this in SDL2#, since the value of this
		 * variable is persistent and not dependent on preprocessor ordering.
		 */
		public const int MIX_CHANNELS = 8;

		public static readonly int MIX_DEFAULT_FREQUENCY = 44100;
		public static readonly ushort MIX_DEFAULT_FORMAT =
			BitConverter.IsLittleEndian ? SDL.AUDIO_S16LSB : SDL.AUDIO_S16MSB;
		public static readonly int MIX_DEFAULT_CHANNELS = 2;
		public static readonly byte MIX_MAX_VOLUME = 128;

		[Flags]
		public enum MIX_InitFlags
		{
			MIX_INIT_FLAC =		0x00000001,
			MIX_INIT_MOD =		0x00000002,
			MIX_INIT_MP3 =		0x00000008,
			MIX_INIT_OGG =		0x00000010,
			MIX_INIT_MID =		0x00000020,
			MIX_INIT_OPUS =		0x00000040
		}

		public struct MIX_Chunk
		{
			public int allocated;
			public IntPtr abuf; /* Uint8* */
			public uint alen;
			public byte volume;
		}

		public enum Mix_Fading
		{
			MIX_NO_FADING,
			MIX_FADING_OUT,
			MIX_FADING_IN
		}

		public enum Mix_MusicType
		{
			MUS_NONE,
			MUS_CMD,
			MUS_WAV,
			MUS_MOD,
			MUS_MID,
			MUS_OGG,
			MUS_MP3,
			MUS_MP3_MAD_UNUSED,
			MUS_FLAC,
			MUS_MODPLUG_UNUSED,
			MUS_OPUS
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MixFuncDelegate(
			IntPtr udata, // void*
			IntPtr stream, // Uint8*
			int len
		);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Mix_EffectFunc_t(
			int chan,
			IntPtr stream, // void*
			int len,
			IntPtr udata // void*
		);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Mix_EffectDone_t(
			int chan,
			IntPtr udata // void*
		);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MusicFinishedDelegate();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ChannelFinishedDelegate(int channel);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int SoundFontDelegate(
			IntPtr a, // const char*
			IntPtr b // void*
		);

		public static void SDL_MIXER_VERSION(out SDL.SDL_version X)
		{
			X.major = SDL_MIXER_MAJOR_VERSION;
			X.minor = SDL_MIXER_MINOR_VERSION;
			X.patch = SDL_MIXER_PATCHLEVEL;
		}

		[LibraryImport(nativeLibName, EntryPoint = "MIX_Linked_Version")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static partial IntPtr INTERNAL_MIX_Linked_Version();
		public static SDL.SDL_version MIX_Linked_Version()
		{
			SDL.SDL_version result;
			IntPtr result_ptr = INTERNAL_MIX_Linked_Version();
			result = SDL.PtrToStructure<SDL.SDL_version>(
				result_ptr
			);
			return result;
		}

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_Init(MIX_InitFlags flags);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_Quit();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_OpenAudio(
			int frequency,
			ushort format,
			int channels,
			int chunksize
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_AllocateChannels(int numchans);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_QuerySpec(
			out int frequency,
			out ushort format,
			out int channels
		);

		/* src refers to an SDL_RWops*, IntPtr to a Mix_Chunk* */
		/* THIS IS A PUBLIC RWops FUNCTION! */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr Mix_LoadWAV_RW(
			IntPtr src,
			int freesrc
		);
		
		/* IntPtr refers to a Mix_Chunk* */
		/* This is an RWops macro in the C header. */
		public static IntPtr Mix_LoadWAV(string file)
		{
			IntPtr rwops = SDL.SDL_RWFromFile(file, "rb");
			return Mix_LoadWAV_RW(rwops, 1);
		}

		/* IntPtr refers to a Mix_Music* */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_LoadMUS")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static unsafe partial IntPtr INTERNAL_Mix_LoadMUS(
			byte* file
		);
		public static unsafe IntPtr Mix_LoadMUS(string file)
		{
			byte* utf8File = SDL.Utf8EncodeHeap(file);
			IntPtr handle = INTERNAL_Mix_LoadMUS(
				utf8File
			);
			Marshal.FreeHGlobal((IntPtr) utf8File);
			return handle;
		}

		/* IntPtr refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr Mix_QuickLoad_WAV(
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)]
				byte[] mem
		);

		/* IntPtr refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr Mix_QuickLoad_RAW(
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)]
				byte[] mem,
			uint len
		);

		/* chunk refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_FreeChunk(IntPtr chunk);

		/* music refers to a Mix_Music* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_FreeMusic(IntPtr music);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GetNumChunkDecoders();

		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetChunkDecoder")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static partial IntPtr INTERNAL_Mix_GetChunkDecoder(int index);
		public static string Mix_GetChunkDecoder(int index)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetChunkDecoder(index)
			);
		}

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GetNumMusicDecoders();

		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetMusicDecoder")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static partial IntPtr INTERNAL_Mix_GetMusicDecoder(int index);
		public static string Mix_GetMusicDecoder(int index)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetMusicDecoder(index)
			);
		}

		/* music refers to a Mix_Music* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial Mix_MusicType Mix_GetMusicType(IntPtr music);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetMusicTitle")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr INTERNAL_Mix_GetMusicTitle(IntPtr music);
		public static string Mix_GetMusicTitle(IntPtr music)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetMusicTitle(music)
			);
		}

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetMusicTitleTag")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr INTERNAL_Mix_GetMusicTitleTag(IntPtr music);
		public static string Mix_GetMusicTitleTag(IntPtr music)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetMusicTitleTag(music)
			);
		}

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetMusicArtistTag")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr INTERNAL_Mix_GetMusicArtistTag(IntPtr music);
		public static string Mix_GetMusicArtistTag(IntPtr music)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetMusicArtistTag(music)
			);
		}

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetMusicAlbumTag")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr INTERNAL_Mix_GetMusicAlbumTag(IntPtr music);
		public static string Mix_GetMusicAlbumTag(IntPtr music)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetMusicAlbumTag(music)
			);
		}

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetMusicCopyrightTag")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr INTERNAL_Mix_GetMusicCopyrightTag(IntPtr music);
		public static string Mix_GetMusicCopyrightTag(IntPtr music)
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetMusicCopyrightTag(music)
			);
		}

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_SetPostMix(
			MixFuncDelegate mix_func,
			IntPtr arg // void*
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_HookMusic(
			MixFuncDelegate mix_func,
			IntPtr arg // void*
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_HookMusicFinished(
			MusicFinishedDelegate music_finished
		);

		/* IntPtr refers to a void* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr Mix_GetMusicHookData();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_ChannelFinished(
			ChannelFinishedDelegate channel_finished
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_RegisterEffect(
			int chan,
			Mix_EffectFunc_t f,
			Mix_EffectDone_t d,
			IntPtr arg // void*
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_UnregisterEffect(
			int channel,
			Mix_EffectFunc_t f
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_UnregisterAllEffects(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_SetPanning(
			int channel,
			byte left,
			byte right
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_SetPosition(
			int channel,
			short angle,
			byte distance
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_SetDistance(int channel, byte distance);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_SetReverseStereo(int channel, int flip);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_ReserveChannels(int num);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GroupChannel(int which, int tag);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GroupChannels(int from, int to, int tag);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GroupAvailable(int tag);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GroupCount(int tag);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GroupOldest(int tag);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GroupNewer(int tag);

		/* chunk refers to a Mix_Chunk* */
		public static int Mix_PlayChannel(
			int channel,
			IntPtr chunk,
			int loops
		) {
			return Mix_PlayChannelTimed(channel, chunk, loops, -1);
		}

		/* chunk refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_PlayChannelTimed(
			int channel,
			IntPtr chunk,
			int loops,
			int ticks
		);

		/* music refers to a Mix_Music* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_PlayMusic(IntPtr music, int loops);

		/* music refers to a Mix_Music* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_FadeInMusic(
			IntPtr music,
			int loops,
			int ms
		);

		/* music refers to a Mix_Music* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_FadeInMusicPos(
			IntPtr music,
			int loops,
			int ms,
			double position
		);

		/* chunk refers to a Mix_Chunk* */
		public static int Mix_FadeInChannel(
			int channel,
			IntPtr chunk,
			int loops,
			int ms
		) {
			return Mix_FadeInChannelTimed(channel, chunk, loops, ms, -1);
		}

		/* chunk refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_FadeInChannelTimed(
			int channel,
			IntPtr chunk,
			int loops,
			int ms,
			int ticks
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_Volume(int channel, int volume);

		/* chunk refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_VolumeChunk(
			IntPtr chunk,
			int volume
		);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_VolumeMusic(int volume);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GetVolumeMusicStream(IntPtr music);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_HaltChannel(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_HaltGroup(int tag);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_HaltMusic();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_ExpireChannel(int channel, int ticks);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_FadeOutChannel(int which, int ms);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_FadeOutGroup(int tag, int ms);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_FadeOutMusic(int ms);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial Mix_Fading Mix_FadingMusic();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial Mix_Fading Mix_FadingChannel(int which);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_Pause(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_Resume(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_Paused(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_PauseMusic();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_ResumeMusic();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_RewindMusic();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_PausedMusic();

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_SetMusicPosition(double position);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial double Mix_GetMusicPosition(IntPtr music);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial double Mix_MusicDuration(IntPtr music);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial double Mix_GetMusicLoopStartTime(IntPtr music);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial double Mix_GetMusicLoopEndTime(IntPtr music);

		/* music refers to a Mix_Music*
		 * Only available in 2.0.5 or higher.
		 */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial double Mix_GetMusicLoopLengthTime(IntPtr music);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_Playing(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_PlayingMusic();

		[LibraryImport(nativeLibName, EntryPoint = "Mix_SetMusicCMD")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static unsafe partial int INTERNAL_Mix_SetMusicCMD(
			byte* command
		);
		public static unsafe int Mix_SetMusicCMD(string command)
		{
			byte* utf8Cmd = SDL.Utf8EncodeHeap(command);
			int result = INTERNAL_Mix_SetMusicCMD(
				utf8Cmd
			);
			Marshal.FreeHGlobal((IntPtr) utf8Cmd);
			return result;
		}

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_SetSynchroValue(int value);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_GetSynchroValue();

		[LibraryImport(nativeLibName, EntryPoint = "Mix_SetSoundFonts")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static unsafe partial int INTERNAL_Mix_SetSoundFonts(
			byte* paths
		);
		public static unsafe int Mix_SetSoundFonts(string paths)
		{
			byte* utf8Paths = SDL.Utf8EncodeHeap(paths);
			int result = INTERNAL_Mix_SetSoundFonts(
				utf8Paths
			);
			Marshal.FreeHGlobal((IntPtr) utf8Paths);
			return result;
		}

		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetSoundFonts")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static partial IntPtr INTERNAL_Mix_GetSoundFonts();
		public static string Mix_GetSoundFonts()
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetSoundFonts()
			);
		}

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial int Mix_EachSoundFont(
			SoundFontDelegate function,
			IntPtr data // void*
		);

		/* Only available in 2.0.5 or later. */
		[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Mix_SetTimidityCfg(
			[In()] [MarshalAs(UnmanagedType.LPStr)]
				string path
		);

		/* Only available in 2.0.5 or later. */
		[LibraryImport(nativeLibName, EntryPoint = "Mix_GetTimidityCfg")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr INTERNAL_Mix_GetTimidityCfg();
		public static string Mix_GetTimidityCfg()
		{
			return SDL.UTF8_ToManaged(
				INTERNAL_Mix_GetTimidityCfg()
			);
		}

		/* IntPtr refers to a Mix_Chunk* */
		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial IntPtr Mix_GetChunk(int channel);

		[LibraryImport(nativeLibName)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        public static partial void Mix_CloseAudio();

		public static string Mix_GetError()
		{
			return SDL.SDL_GetError();
		}

		public static void Mix_SetError(string fmtAndArglist)
		{
			SDL.SDL_SetError(fmtAndArglist);
		}
		
		public static void Mix_ClearError()
		{
			SDL.SDL_ClearError();
		}
		
		#endregion
	}
}
