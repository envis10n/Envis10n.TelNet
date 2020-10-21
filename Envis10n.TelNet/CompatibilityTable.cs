using System.Collections.Generic;
using Envis10n.TelNet.Constants;

namespace Envis10n.TelNet
{
    public struct CompabilityConstants
    {
        public const byte
            EnabledLocal = 1,
            EnabledRemote = 1 << 1,
            LocalState = 1 << 2,
            RemoteState = 1 << 3;
    }
    public struct CompatibilityEntry
    {
        public bool Local;
        public bool Remote;
        public bool LocalState;
        public bool RemoteState;
        public static implicit operator byte(CompatibilityEntry entry)
        {
            byte res = 0;
            if (entry.Local) res |= CompabilityConstants.EnabledLocal;
            if (entry.Remote) res |= CompabilityConstants.EnabledRemote;
            if (entry.LocalState) res |= CompabilityConstants.LocalState;
            if (entry.RemoteState) res |= CompabilityConstants.RemoteState;
            return res;
        }
        public static implicit operator CompatibilityEntry(byte mask)
        {
            return new CompatibilityEntry()
            {
                Local = (mask & CompabilityConstants.EnabledLocal) == CompabilityConstants.EnabledLocal,
                Remote = (mask & CompabilityConstants.EnabledRemote) == CompabilityConstants.EnabledRemote,
                LocalState = (mask & CompabilityConstants.LocalState) == CompabilityConstants.LocalState,
                RemoteState = (mask & CompabilityConstants.RemoteState) == CompabilityConstants.RemoteState,
            };
        }
    }

    public sealed class CompatibilityTable
    {
        private readonly byte[] _table = new byte[256];

        public void SupportLocal(byte option)
        {
            CompatibilityEntry entry = _table[option];
            entry.Local = true;
            _table[option] = entry;
        }
        public void SupportRemote(byte option)
        {
            CompatibilityEntry entry = _table[option];
            entry.Remote = true;
            _table[option] = entry;
        }
        public void Support(byte option)
        {
            CompatibilityEntry entry = _table[option];
            entry.Local = true;
            entry.Remote = true;
            _table[option] = entry;
        }

        public CompatibilityEntry GetOption(byte option)
        {
            return _table[option];
        }

        public void SetOption(byte option, CompatibilityEntry entry)
        {
            _table[option] = entry;
        }
    }
}