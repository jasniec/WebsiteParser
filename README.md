# Website parser

Website was created to make web scraping / web harvesing easier. \
Instead of creating methods / classes with extracting logic, you can describe your properties using attributes.\
\
Extracting is done mostly using CSS selectors so this library is not resistant to website changes.

# Nuget
`
PM> Install-Package WebsiteParser -Version 1.1.1
`\
\
[https://www.nuget.org/packages/WebsiteParser/1.1.1](https://www.nuget.org/packages/WebsiteParser/1.1.1)

## Patch notes 1.1.1

- Added [CompareValueAttribute](https://github.com/jasniec/WebsiteParser/wiki/CompareValueAttribute)

# Usage

Parsing:
```csharp
string html;
// ...
UserInfo johnInfo = WebContentParser.Parse<UserInfo>(html);
```

Model class:
```csharp
class UserInfo
{
    [Selector("#personName")]
    public string Name { get; set; }

    [Selector("#personBirdthDate")]
    [Converter(typeof(DateTimeConverter))]
    public DateTime BirdthDate { get; set; }

    [Selector("#personAbout")]
    [Regex("about me: (.*)")]
    public string Description { get; set; }
}
```

Input data:
```html
<html>
    <body>
        <div id="personName">John</div>
        <div id="personBirdthDate">23-10-2000</div>
        <div id="personAbout">about me: lorem ipsum dolor sit amet</div>
    </body>
</html>
```

## Lists
For parsing lists use `ListSelectorAttribute` which should be used on model class. \
Constructor's argument "selector" is parent of children which should be parsed. \
`ChildSelector` property is used to choose which try to parse.

### Example:

Parsing:
```csharp
string html;
// ...
IEnumerable<UserInfo> people = WebContentParser.ParseList<UserInfo>(html);
```

Model class:
```csharp
[ListSelector("people", ChildSelector = ".person")]
class UserInfo
{
    [Selector("#personName")]
    public string Name { get; set; }

    [Selector("#personBirdthDate")]
    [Converter(typeof(DateTimeConverter))]
    public DateTime BirdthDate { get; set; }

    [Selector("#personAbout")]
    [Regex("about me: (.*)")]
    public string Description { get; set; }
}
```

Input data:
```html
<html>
    <body>
        <div id="people">
            <div class="person">
                <div id="personName">John</div>
                <div id="personBirdthDate">19-01-2038</div>
                <div id="personAbout">about me: lorem ipsum dolor sit amet</div>
            </div>
            <div class="person">
                <div id="personName">Joe</div>
                <div id="personBirdthDate">01-01-1970</div>
                <div id="personAbout">about me: hello world</div>
            </div>
        </div>
    </body>
</html>
```



# Attributes

Attributes are handled in same order they were added e.g.
```csharp
[Selector("#addDate")]
[Remove("Add date: ", RemoverValueType.Text)]
[Converter(typeof(DateTimeConverter))]
public DateTime AddDate { get; set; }
```
`Selector` will extract markup's text as string, `Remove` will change received string and pass it to `Converter`, which will change string's type to DateTime. At the end value of type DateTime will be set in the `AddDate` property.


There are two types of attributes:
- Start attributes, which can be only added at the beginning.
- Parser attributes, which will modify and pass result to next attribute or set to property.

Read more about attributes in [Wiki](https://github.com/jasniec/WebsiteParser/wiki)

## Creating custom attribute
You can create your own parser attribute simply implementing `WebsiteParser.Attributes.Abstract.IParserAttribute` interface. Keep in mind that input type depends on above attribute's output.

## License
[MIT](https://choosealicense.com/licenses/mit/)
