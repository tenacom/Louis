# Fluency

The `Louis.Fluency` namespace contains extension methods that let you incorporate conditional or other logic in a fluent call chain.

## Invoke

The `Invoke` fluent extension method makes a void-returning method or lambda fluent.

  ```csharp
  sb = sb.Append(SomeValue)
         .Invoke(x => { intermediateResult = x.ToString(); })
         .Append(SomeOtherValue);
  ```

## Chain

The `Chain` fluent extension method lets you use a same-type-returning method or lambda as if it were an extension method.

  ```csharp
  sb = sb.Append(SomeValue)
         .Chain(AppendSeparator)
         .Append(SomeOtherValue);

	// This local function cannot be an extension method.
	static StringBuilder AppendSeparator(StringBuilder sb) => sb.Length > 0 ? sb.Append(',') : sb;
  ```

## If

The `If` fluent extension method executes a fluent method according to a boolean condition.

  ```csharp
  sb = sb.Append("Hello, ")
         .If(IsCloseFriend, static x => x.Append("my dear "))
         .Append(Name)
         .Append('.');
  ```

## IfElse

The `IfElse` fluent extension method executes one of two different fluent methods according to a boolean condition.

  ```csharp
  sb = sb
      .IfElse(
          isFirstElement,
          static x => x.Clear().Append('['),
          static x => x.Append(", "))
      .Append(element.Value);
  ```

## InvokeIf

The `InvokeIf` fluent extension method executes a void-returning method in a fluent fashion, according to a boolean condition.

  ```csharp
  sb = sb.Append("Hello, ")
         .InvokeIf(IsCloseFriend, AppendMyDear)
         .Append(Name)
         .Append('.');

	// This is a silly example, in that you may just have AppendMyDear return sb.
	// But what if this were a 3rd-party method that you cannot modify?
	static void AppendMyDear(StringBuilder sb) => _ = sb.Append("my dear ");
  ```

## Switch

The `Switch` fluent extension method selects a fluent method to invoke according to a given value.

  ```csharp
  sb = sb.Switch(
      element.Value,
      x => x.Append("<unknown>"), // Default action (optional)
      (0, x => x.Append("zero")),
      (1, x => x.Append("one")),
      (2, x => x.Append("two")));
  ```

## ForEach

The `ForEach` fluent extension method invokes a fluent method passing each element of a sequence or span as a parameter.

  ```csharp
  private const string HexDigits = "0123456789abcdef";

  public string HexDump(ReadOnlySpan<byte> bytes)
      => new StringBuilder(2 * bytes.Length + 2)
          .Append('[')
          .ForEach(bytes, static (x, b) => x.Append(HexDigits[b >> 4]).Append(HexDigits[b & 0x0F]))
          .Append(']')
          .ToString();

  public string HexDumpWithSeparator(ReadOnlySpan<byte> bytes)
      => new StringBuilder(3 * bytes.Length + 1)
          .ForEach(bytes, static (x, b, i) => x.Append(i == 0 ? '[' : ',')
                                               .Append(HexDigits[b >> 4])
                                               .Append(HexDigits[b & 0x0F]))
          .Append(']')
          .ToString();
  ```
