using Microsoft.AspNetCore.Razor.TagHelpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing.Common;
using SixLabors.ImageSharp.Formats.Png;

namespace AlzaBox.API.WebExample.Core.Extensions;

[HtmlTargetElement("barcode")] 
public class BarcodeHelper: TagHelper { 
    public override void Process(TagHelperContext context, TagHelperOutput output) { 
         
        var content = context.AllAttributes["content"]?.Value.ToString();
        var alt = context.AllAttributes["alt"]?.Value.ToString(); 
        var width = 200;  
        var height = 100;
        var margin = 0; 
        var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32> { 
            Format = ZXing.BarcodeFormat.CODE_128, 
            Options = new EncodingOptions { 
                Height = height, Width = width, Margin = margin 
            }
        }; 

        using (var image = barcodeWriter.Write(content))
        {
            output.TagName = "img"; 
            output.Attributes.Clear(); 
            output.Attributes.Add("width", image.Width); 
            output.Attributes.Add("height", image.Height); 
            output.Attributes.Add("alt", alt);
            output.Attributes.Add("src", string.Format(image.ToBase64String(PngFormat.Instance)));
            
        }
    } 
} 