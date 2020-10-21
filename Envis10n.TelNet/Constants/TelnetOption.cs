namespace Envis10n.TelNet.Constants
{
    public struct TelnetOption
    {
        public const byte
        /** Whether the other side should interpret data as 8-bit characters instead of standard NVT ASCII.  */
        BINARY_TRANSMISSION = 0,
        /** Whether the other side should continue to echo characters. */
        ECHO = 1,
        RECONNECTION = 2,
        SUPPRESS_GO_AHEAD = 3,
        APPROX_MESSAGE_SIZE_NEGOTIATION = 4,
        STATUS = 5,
        TIMING_MARK = 6,
        REMOTE_CONTROLLED_TRANS_ECHO = 7,
        OUTPUT_LINE_WIDTH = 8,
        OUTPUT_PAGE_SIZE = 9,
        OUTPUT_CR_DISPOSITION = 10,
        OUTPUT_HORIZONTAL_TAB_STOPS = 11,
        OUTPUT_HORIZONTAL_TAB_DISPOSITION = 12,
        OUTPUT_FORMFEED_DISPOSITION = 13,
        OUTPUT_VERTICAL_TAB_STOPS = 14,
        OUTPUT_VERTICAL_TAB_DISPOSITION = 15,
        OUTPUT_LINEFEED_DISPOSITION = 16,
        EXTENDED_ASCII = 17,
        LOGOUT = 18,
        BYTE_MACRO = 19,
        DATA_ENTRY_TERMINAL = 20,
        SUPDUP = 21,
        SUPDUP_OUTPUT = 22,
        SEND_LOCATION = 23,
        TERMINAL_TYPE = 24,
        END_OF_RECORD = 25,
        TACACS_USER_IDENTIFICATION = 26,
        OUTPUT_MARKING = 27,
        TERMINAL_LOCATION_NUMBER = 28,
        TELNET_3270_REGIME = 29,
        X3_PAD = 30,
        /**
     * Whether to negotiate about window size (client).
     * @example
     * [IAC, SB, NAWS, WIDTH[1], WIDTH[0], HEIGHT[1], HEIGHT[0], IAC, SE]
     */
        NEGOTIATE_ABOUT_WINDOW_SIZE = 31,
        TERMINAL_SPEED = 32,
        REMOTE_FLOW_CONTROL = 33,
        LINEMODE = 34,
        X_DISPLAY_LOCATION = 35,
        ENVIRONMENT = 36,
        AUTHENTICATION = 37,
        ENCRYPTION = 38,
        NEW_ENVIRONMENT = 39,
        TN3270E = 40,
        XAUTH = 41,
        CHARSET = 42,
        TELNET_REMOTE_SERIAL_PORT = 43,
        COM_PORT_CONTROL = 44,
        TELNET_SUPPRESS_LOCAL_ECHO = 45,
        TELNET_START_TLS = 46,
        KERMIT = 47,
        SEND_URL = 48,
        FORWARD_X = 49,
        TELOPT_PRAGMA_LOGON = 138,
        TELOPT_SSPI_LOGON = 139,
        TELOPT_PRAGMA_HEARTBEAT = 140,
        /** Generic MUD Communication Protocol option.
     * @example
     * [IAC, SB, GMCP, "Package.SubPackage", "JSON", IAC, SE]
     */
        GMCP = 201,
        EXTENDED_OPTIONS_LIST = 255;
    }
}