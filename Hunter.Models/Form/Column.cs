using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.Form
{
    public class Column
    {
        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("align")]
        public string Align { get; set; }

        [JsonProperty("valign")]
        public string VAlign { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("timespan")]
        public long TimeSpan { get; set; }

    }

}
