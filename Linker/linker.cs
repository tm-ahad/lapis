﻿using lapis.Helpers;

namespace lapis.Link
{
    public class Linker
    {
        public Fetcher fetcher;

        public Linker() 
        {
            fetcher = new Fetcher();
        }

        public void Link(string name) 
        {
            string linkCommand = $"ld {name}.o -o {name}";
            fetcher.ExecuteCommand(linkCommand);
        }
    }
}