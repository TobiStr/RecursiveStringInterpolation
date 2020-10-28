# RecursiveStringInterpolation

![Tests](https://github.com/TobiStr/RecursiveStringInterpolation/workflows/.NET%20Core/badge.svg)

Recursively interpolate strings with a given collection of key-value-pairs in .NET with configureable start and end tags.

## Information

The package uses regex, to match the opening and closing tags and replaces the matched variables with the values in the dictionary.

## Add Nuget-Package

```cli
dotnet add package RecursiveStringInterpolation
```

## Usage

### Import Namespace:
```csharp
  using String.Interpolation.Recursive;
```

### Initialize:
```csharp
    //Here nearly everything, even longer strings and special characters are supported.
    //See tests for examples.
    var openingTag = "{";
    var closingTag = "}";

    var stringInterpolator = new RecursiveStringInterpolator(openingTag, closingTag);
```

### Interpolate a string:
```csharp
    //Provide a string, you want to interpolate
    const string testString = "{a}{b}{c}";

    //Provide a dictionary, with the key-value-pairs, you want to use for interpolation
    var keyValues = new Dictionary<string, string>();
    keyValues.Add("a", "a");
    keyValues.Add("b", "{a}");
    keyValues.Add("c", "{b}");

    //Use Interpolator to interpolate the string
    var result = stringInterpolator.Interpolate(testString, keyValues);
    //result = "aaa"
```

### P.S.:
Let me know, if you like to have more features. 