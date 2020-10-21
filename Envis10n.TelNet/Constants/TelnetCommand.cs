namespace Envis10n.TelNet.Constants
{
    public struct TelnetCommand
    {
        public const byte
            /** Mark the start of a negotiation sequence. */
            IAC = 255,
            /** Confirm  */
            WILL = 251,
            /** Tell the other side that we refuse to use an option. */
            WONT = 252,
            /** Request that the other side begin using an option. */
            DO = 253,
            /**  */
            DONT = 254,
            NOP = 241,
            /** Subnegotiation used for sending out-of-band data. */
            SB = 250,
            /** Marks the end of a subnegotiation sequence. */
            SE = 240,
            IS = 0,
            SEND = 1,
            /** Go Ahead */
            GA = 249;
    }
}