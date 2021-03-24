using System;
using System.Collections.Generic;

using InsanityBot.Util.FileMeta.Reference;

namespace InsanityBot.Util.FileMeta
{
    public class User
    {
        public String Username { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays
        public List<ModlogEntry> Modlog { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        public Int32 ModlogCount { get; set; }

        public User()
        {
            Modlog = new List<ModlogEntry>();
            ModlogCount = 0;
        }

        public void AddModlogEntry(ModlogType type, String reason)
        {
            ModlogCount++;
            Modlog.Add(ModlogEntry.CreateNew(type, reason));
        }
    }
}
