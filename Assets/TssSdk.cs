using System;
using System.Runtime.InteropServices;

public static class TssSdk{

	public enum EUINTYPE
	{
		UIN_TYPE_INT = 1, // integer format
		UIN_TYPE_STR = 2  // string format
	}
	
	public enum EAPPIDTYPE
	{
		APP_ID_TYPE_INT = 1, // integer format
		APP_ID_TYPE_STR = 2  // string format
	}
	
	public enum EENTRYID
	{
		ENTRY_ID_QZONE = 1, // QZone
		ENTRY_ID_MM = 2,    // wechat
		ENTRY_ID_OTHERS = 3 // other platform
	}
	
	public enum EGAMESTATUS
	{
		GAME_STATUS_FRONTEND = 1,  // running in front-end
		GAME_STATUS_BACKEND = 2    // running in back-end
	}
	
	// init info
	[StructLayout(LayoutKind.Sequential)]
	public class InitInfo
	{
		public uint size_;           // struct size
		public uint game_id_;        // game id
	}
	
	// game status
	[StructLayout(LayoutKind.Sequential)]
	public class GameStatusInfo
	{
		public uint size_;			// struct size
		public uint game_status_;   // running in front-end or back-end
	}
	

	
	// user info
	[StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct UinInfoInt
    {
        [FieldOffset(0)]
        public uint uin;
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct UinInfoStr
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)]
        public string uin;
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct AppIdInfoInt
    {
        [FieldOffset(0)]
        public uint app_id;
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct AppIdInfoStr
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UIN_INT
    {
        public uint type;
        public UinInfoInt uin;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UIN_STR
    {
        public uint type;
        public UinInfoStr uin;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class APPID_INT
    {        
        public uint type;        
        public AppIdInfoInt app_id;       
    }

    [StructLayout(LayoutKind.Sequential)]
    public class APPID_STR
    {
        public uint type;
        public AppIdInfoStr app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfoIntInt
    {
        public uint size;
        public uint entrance_id;
        public  UIN_INT uin;
        public APPID_INT app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfoStrStr
    {
        public uint size;
        public uint entrance_id;
        public UIN_STR uin;
        public APPID_STR app_id;
    }
	
	[StructLayout(LayoutKind.Sequential)]
    public class UserInfoIntStr
    {
        public uint size;
        public uint entrance_id;
        public UIN_INT uin;
        public APPID_STR app_id;
    }
	
	[StructLayout(LayoutKind.Sequential)]
    public class UserInfoStrInt
    {
        public uint size;
        public uint entrance_id;
        public UIN_STR uin;
        public APPID_INT app_id;
    }
	
	
	/// <summary>
	/// Tsses the sdk init.
	/// </summary>
	/// <param name='gameId'>
	/// game id provided by sdk provider
	/// </param>
	public static void TssSdkInit(uint gameId)
	{
		InitInfo info = new InitInfo();
		info.size_ = (uint)Marshal.SizeOf (info);
		info.game_id_ = gameId;
		tss_sdk_init (info);
	}
	
	/// <summary>
	/// Tsses the sdk set game status.
	/// </summary>
	/// <param name='gameStatus'>
	/// back-end or front-end
	/// </param>
	public static void TssSdkSetGameStatus(EGAMESTATUS gameStatus)
	{
		GameStatusInfo info = new GameStatusInfo();
		info.size_ = (uint)Marshal.SizeOf (info);
		info.game_status_ = (uint)gameStatus;
		tss_sdk_setgamestatus (info);
	}
	
	/// <summary>
	/// Tsses the sdk set user info.
	/// </summary>
	/// <param name='entryId'>
	/// EENTRYID defined.
	/// </param>
	/// <param name='uin'>
	/// uin or openid contains integer format and string format.
	/// </param>
	/// <param name='appId'>
	/// appid contains integer format and string format.
	/// </param>
	public static void TssSdkSetUserInfo(EENTRYID entryId,
		uint uin,
		uint appId)
	{
		UserInfoIntInt info = new UserInfoIntInt();
		info.size = (uint)Marshal.SizeOf(info);
		info.entrance_id = (uint)entryId;
		info.uin = new UIN_INT();
		info.uin.type = (uint)EUINTYPE.UIN_TYPE_INT;
		info.uin.uin.uin = uin;
		info.app_id = new APPID_INT();
		info.app_id.type = (uint)EAPPIDTYPE.APP_ID_TYPE_INT;
		info.app_id.app_id.app_id = appId;
		tss_sdk_setuserinfo(info);		
	}
	
	public static void TssSdkSetUserInfo(EENTRYID entryId,
		string uin,
		uint appId)
	{
		UserInfoStrInt info = new UserInfoStrInt();
		info.size = (uint)Marshal.SizeOf(info);
		info.entrance_id = (uint)entryId;
		info.uin = new UIN_STR();
		info.uin.type = (uint)EUINTYPE.UIN_TYPE_STR;
		info.uin.uin.uin = uin;
		info.app_id = new APPID_INT();
		info.app_id.type = (uint)EAPPIDTYPE.APP_ID_TYPE_INT;
		info.app_id.app_id.app_id = appId;
		tss_sdk_setuserinfo(info);		
	}
	
	public static void TssSdkSetUserInfo(EENTRYID entryId,
		uint uin,
		string appId)
	{
		UserInfoIntStr info = new UserInfoIntStr();
		info.size = (uint)Marshal.SizeOf(info);
		info.entrance_id = (uint)entryId;
		info.uin = new UIN_INT();
		info.uin.type = (uint)EUINTYPE.UIN_TYPE_INT;
		info.uin.uin.uin = uin;
		info.app_id = new APPID_STR();
		info.app_id.type = (uint)EAPPIDTYPE.APP_ID_TYPE_STR;
		info.app_id.app_id.app_id = appId;
		tss_sdk_setuserinfo(info);		
	}
	
	public static void TssSdkSetUserInfo(EENTRYID entryId,
		string uin,
		string appId)
	{
		UserInfoStrStr info = new UserInfoStrStr();
		info.size = (uint)Marshal.SizeOf(info);
		info.entrance_id = (uint)entryId;
		info.uin = new UIN_STR();
		info.uin.type = (uint)EUINTYPE.UIN_TYPE_STR;
		info.uin.uin.uin = uin;
		info.app_id = new APPID_STR();
		info.app_id.type = (uint)EAPPIDTYPE.APP_ID_TYPE_STR;
		info.app_id.app_id.app_id = appId;
		tss_sdk_setuserinfo(info);		
	}
	
	
	// game id provided by sdk provider
	private const uint GAME_ID = 9999u; // test 

	/// <summary>
	/// game client calls this interface to set initialize information when game starts
	/// </summary>
	/// <param name='info'>
	/// data of initialization information ,defined in class InitInfo
	/// </param>
	[DllImport("tersafe")]
	private static extern void tss_sdk_init(InitInfo info);
	

	/// <summary>
	/// game client calls this interface to set user info when login in game
	/// </summary>
	/// <param name='info'>
	/// data of user information,defined in class UserInfo
	/// </param>
	[DllImport("tersafe")]
	private static extern void tss_sdk_setuserinfo(UserInfoIntInt info);
	[DllImport("tersafe")]
	private static extern void tss_sdk_setuserinfo(UserInfoStrInt info);
	[DllImport("tersafe")]
	private static extern void tss_sdk_setuserinfo(UserInfoIntStr info);
	[DllImport("tersafe")]
	private static extern void tss_sdk_setuserinfo(UserInfoStrStr info);
	
	/// <summary>
	/// game client calls this interface to notify security component's game current status.
	/// </summary>
	/// <param name='info'>
	/// game status if game running in front-end,set it to EGAMESTATUS.GAME_STATUS_FRONTEND
	/// game status if game running in back-end, set it to EGAMESTATUS.GAME_STATUS_BACKEND
	/// </param>
	[DllImport("tersafe")]
	private static extern void tss_sdk_setgamestatus(GameStatusInfo info);
}
