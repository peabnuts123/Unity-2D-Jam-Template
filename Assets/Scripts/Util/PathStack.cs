using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class PathStack
{
  /* PURPOSE:
      Represent platform agnostic paths as an array of strings,
      avoiding buggery around trailing/leading slashes, etc.

      PathStacks can be converted to/from string paths.
   */

  private PathType type;
  private IEnumerable<string> tokens;


  public int Length
  {
    get
    {
      return this.tokens.Count();
    }
  }

  public PathStack(PathType type, IEnumerable<string> initialPath)
  {
    this.type = type;
    this.tokens = initialPath;
  }

  public PathStack(IEnumerable<string> initialPath)
  {
    this.type = PathType.Relative;
    this.tokens = initialPath;
  }

  public PathStack(PathType type, params string[] initialPath)
  {
    this.type = type;
    this.tokens = initialPath;
  }

  public PathStack(params string[] initialPath)
  {
    this.type = PathType.Relative;
    this.tokens = initialPath;
  }

  public static PathStack FromPathString(string pathString, char? pathDelimeter = null)
  {
    // Default path delimeter to system path string
    if (!pathDelimeter.HasValue)
    {
      pathDelimeter = Path.DirectorySeparatorChar;
    }

    // Split by delimeter, remove any tokens that are just whitespace
    IEnumerable<string> tokens = pathString
      .Split(pathDelimeter.Value)
      .Where(token => token != null && token.Trim().Length > 0);

    // Test for leading `/`, signalling an Absolute path
    
    bool isAbsolute = pathString.Length > 0 && Regex.IsMatch(pathString, "^(" + pathDelimeter + "|\\w:)");

    // Resolve PathType based on `isAbsolute` 
    PathType type;
    if (isAbsolute)
    {
      type = PathType.Absolute;
    }
    else
    {
      type = PathType.Relative;
    }

    // Construct
    return new PathStack(type, tokens);
  }

  public PathStack Push(params string[] newTokens)
  {
    return new PathStack(this.type, tokens.Concat(newTokens));
  }

  public PathStack Pop(int number = 1)
  {
    return new PathStack(this.type, tokens.Reverse().Skip(number).Reverse());
  }

  public string Peek()
  {
    return tokens.Last();
  }

  public bool IsEmpty()
  {
    return this.Length == 0;
  }

  public override string ToString()
  {
    string result = "";
    if (this.type == PathType.Absolute)
    {
      result += Path.DirectorySeparatorChar;
    }
    result += string.Join(Path.DirectorySeparatorChar.ToString(), tokens.ToArray());
    return result;
  }

  public static PathStack operator +(PathStack self, string s)
  {
    return self.Push(s);
  }

  // @TODO test
  public static PathStack operator +(PathStack self, PathStack other)
  {
    // Cannot add Absolute paths to anything
    if (other.type == PathType.Absolute)
    {
      throw new InvalidOperationException("Cannot add absolute path to another path, you can only add ONTO Absolute Path Stacks");
    }

    return new PathStack(self.type, self.tokens.Concat(other.tokens));
  }
}

public enum PathType
{
  Absolute,
  Relative,
}