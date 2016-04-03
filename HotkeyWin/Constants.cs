namespace GlobalHotkey
{
    public static class Constants
    {
        //modifiers
        public const int NOMOD = 0x0000;

        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        //windows message id for hotkey
        public const int WM_HOTKEY_MSG_ID = 0x0312;

        // lparams IDs for keys
        #region intPtr values
        //public const intPtr HK_1 = 0x310003;
        //public const intPtr HK_2 = 0x320003;
        //public const intPtr HK_3 = 0x330003;
        //public const intPtr HK_4 = 0x340003;
        //public const intPtr HK_5 = 0x350003;
        //public const intPtr HK_6 = 0x360003;
        //public const intPtr HK_7 = 0x370003;
        //public const intPtr HK_8 = 0x380003;
        //public const intPtr HK_9 = 0x390003;
        #endregion
        public const string HK_1 = "3211267";
        public const string HK_2 = "3276803";
        public const string HK_3 = "3342339";
        public const string HK_4 = "3407875";
        public const string HK_5 = "3473411";
        public const string HK_6 = "3538947";
        public const string HK_7 = "3604483";
        public const string HK_8 = "3670019";
        public const string HK_9 = "3735555";
    }
}