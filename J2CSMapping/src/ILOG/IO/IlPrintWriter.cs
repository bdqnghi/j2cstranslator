//========================================================== -*- Java -*- ===
// Defines a Java-like PrintWriter to ease the translation, especially
// when PrintWriter is derived.
//===========================================================================

namespace ILOG.J2CsMapping.IO
{
  using System;
  using System.IO;

#if UNIT_TESTS
	public
#else
	internal
#endif
		class IlPrintWriter
  {
    TextWriter writer;

    //------------------------------------------------------------
    // Constructors
    //------------------------------------------------------------

    public IlPrintWriter(TextWriter writer)
    {
      this.writer = writer;
    }

    //------------------------------------------------------------
    // Properties
    //------------------------------------------------------------

    protected TextWriter Out
    {
      get { return writer; }
      set { writer = value; }
    }

    //------------------------------------------------------------
    // Write without EOL
    //------------------------------------------------------------

    public void Write(bool arg)
    {
      writer.Write(arg);
    }

    public void Write(char arg)
    {
      writer.Write(arg);
    }

    public void Write(char[] arg)
    {
      writer.Write(arg);
    }

    public void Write(int arg)
    {
      writer.Write(arg);
    }

    public void Write(long arg)
    {
      writer.Write(arg);
    }

    public void Write(float arg)
    {
      writer.Write(arg);
    }

    public void Write(double arg)
    {
      writer.Write(arg);
    }

    public void Write(string arg)
    {
      writer.Write(arg);
    }

    public void Write(object arg)
    {
      writer.Write(arg);
    }

    //------------------------------------------------------------
    // Write with EOL
    //------------------------------------------------------------

    public void WriteLine(bool arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(char arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(char[] arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(int arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(long arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(float arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(double arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(string arg)
    {
      writer.WriteLine(arg);
    }

    public void WriteLine(object arg)
    {
      writer.WriteLine(arg);
    }

    //------------------------------------------------------------
    // Other
    //------------------------------------------------------------

    public void WriteLine()
    {
      writer.WriteLine();
    }

    public void Flush()
    {
      writer.Flush();
    }

    public void Close()
    {
      writer.Close();
    }
  };
};
