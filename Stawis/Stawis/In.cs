/*-------------------------------------------------------------------------
In.cs -- Formatted Input in C#,
Copyright (c) 2005 Hanspeter Moessenboeck, University of Linz

This class is free software; you can redistribute it and/or modify it 
under the terms of the GNU General Public License as published by the 
Free Software Foundation; either version 2, or (at your option) any 
later version.

This class is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License 
for more details.
-------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;

namespace Stawis {
  public class In {
    public const char eof = '\uffff';   // signals end of file
    private const char empty = '\ufffe';   // signals: no lookahead character available

    private static bool done = true;       // success of most recent operation
    private static TextReader r = null;  // input stream
    private static char ch = ' ';          // auxiliary for reading
    private static char buf = empty;       // the lookahead character
    private static NumberFormatInfo provider = null; // for conversions between string and float
    private static Stack readerStack = new Stack();  // for nested calls of Open()
    private static Stack bufStack = new Stack();     // for nested calls of Open()


    public static bool Done {
      get {
        return done;
      }
    }

    public static void Open(string path) {
      try {
        Open(new FileStream(path, FileMode.Open));
      } catch {
        done = false;
      }
    }

    public static void Open(Stream s) {
      StreamReader rNew = new StreamReader(s);
      readerStack.Push(r);
      bufStack.Push(buf);
      r = rNew;
      buf = empty;
      done = true;
    }

    public static void OpenString(String s) {
      StringReader sr = new StringReader(s);
      readerStack.Push(r);
      bufStack.Push(buf);
      r = sr;
      buf = empty;
      done = true;
    }

    public static void Close() {
      if (r != null)
        r.Close();
      if (readerStack.Count > 0) {
        r = (StreamReader)readerStack.Pop();
        buf = (char)bufStack.Pop();
      } else {
        r = null;
        buf = empty;
      }
    }

    private static char CharAfterWhiteSpace() {
      char ch;
      do
        ch = Read();
      while (ch <= ' ');
      return ch;
    }

    private static string ReadDigits() {
      StringBuilder b = new StringBuilder();
      char ch = CharAfterWhiteSpace();
      if (ch == '-') {
        b.Append(ch);
        ch = Read();
      }
      while (Char.IsDigit(ch)) {
        b.Append(ch);
        ch = Read();
      }
      buf = ch;
      return b.ToString();
    }

    private static string ReadFloatDigits() {
      StringBuilder b = new StringBuilder();
      char ch = CharAfterWhiteSpace();
      if (ch == '+' || ch == '-') {
        b.Append(ch);
        ch = Read();
      }
      while (Char.IsDigit(ch)) {
        b.Append(ch);
        ch = Read();
      }
      if (ch == '.') {
        b.Append(ch);
        ch = Read();
        while (Char.IsDigit(ch)) {
          b.Append(ch);
          ch = Read();
        }
      }
      if (ch == 'e' || ch == 'E') {
        b.Append(ch);
        ch = Read();
        if (ch == '+' || ch == '-') {
          b.Append(ch);
          ch = Read();
        }
        while (Char.IsDigit(ch)) {
          b.Append(ch);
          ch = Read();
        }
      }
      buf = ch;
      return b.ToString();
    }

    public static char Read() {
      if (buf != empty) {
        ch = buf;
        if (buf != eof)
          buf = empty;
      } else {
        int x;
        if (r == null)
          x = Console.Read();
        else
          x = r.Read();
        if (x < 0) {
          ch = eof;
          buf = eof;
          done = false;
        } else {
          ch = (char)x;
        }
      }
      return ch;
    }

    public static int ReadInt() {
      string s = ReadDigits();
      try {
        done = true;
        return Convert.ToInt32(s);
      } catch {
        done = false;
        return 0;
      }
    }

    public static long ReadLong() {
      string s = ReadDigits();
      try {
        done = true;
        return Convert.ToInt64(s);
      } catch {
        done = false;
        return 0;
      }
    }

    public static float ReadFloat() {
      string s = ReadFloatDigits();
      try {
        done = true;
        return Convert.ToSingle(s, provider);
      } catch {
        done = false;
        return 0f;
      }
    }

    public static double ReadDouble() {
      string s = ReadFloatDigits();
      try {
        done = true;
        return Convert.ToDouble(s, provider);
      } catch {
        done = false;
        return 0.0;
      }
    }

    public static bool ReadBool() {
      string s = ReadIdent();
      done = true;
      if (s == "true")
        return true;
      else if (s == "false")
        return false;
      else {
        done = false;
        return false;
      }
    }

    public static string ReadIdent() {
      StringBuilder b = new StringBuilder();
      char ch = CharAfterWhiteSpace();
      if (Char.IsLetter(ch) || ch == '_') {
        b.Append(ch);
        ch = Read();
        while (Char.IsLetterOrDigit(ch) || ch == '_') {
          b.Append(ch);
          ch = Read();
        }
      }
      buf = ch;
      done = b.Length > 0;
      return b.ToString();
    }

    public static string ReadString() {
      StringBuilder b = new StringBuilder();
      char ch = CharAfterWhiteSpace();
      if (ch == '"') {
        ch = Read();
        while (ch != eof && ch != '"') {
          b.Append(ch);
          ch = In.Read();
        }
        if (ch == '"') {
          done = true;
          ch = Read();
        } else
          done = false;
      } else
        done = false;
      buf = ch;
      return b.ToString();
    }

    public static string ReadWord() {
      StringBuilder b = new StringBuilder();
      char ch = CharAfterWhiteSpace();
      while (ch > ' ' && ch != eof) {
        b.Append(ch);
        ch = Read();
      }
      buf = ch;
      done = b.Length > 0;
      return b.ToString();
    }

    public static string ReadLine() {
      StringBuilder b = new StringBuilder();
      char ch = Read();
      done = ch != eof;
      while (ch != eof && ch != '\r') {
        b.Append(ch);
        ch = Read();
      }
      Read(); // skip '\n'
      buf = empty;
      return b.ToString();
    }

    public static string ReadFile() {
      StringBuilder b = new StringBuilder();
      char ch = Read();
      while (done) {
        b.Append(ch);
        ch = Read();
      }
      buf = eof;
      done = true;
      return b.ToString();
    }

    public static char Peek() {
      char ch = CharAfterWhiteSpace();
      buf = ch;
      return ch;
    }

    static In() {
      provider = new CultureInfo("en-US").NumberFormat;
    }
  }
}